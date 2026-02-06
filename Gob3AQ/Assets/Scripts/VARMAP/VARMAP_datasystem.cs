using Gob3AQ.VARMAP.Enum;
using Gob3AQ.VARMAP.Variable;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.Types.Parsers;
using UnityEngine;
using Gob3AQ.VARMAP.Safe;
using Gob3AQ.VARMAP.SaveData;
using Gob3AQ.VARMAP.DefaultValues;
using Gob3AQ.VARMAP.Config;
using Gob3AQ.Libs.CRC32;
using Gob3AQ.VARMAP.GameMaster;
using Gob3AQ.VARMAP.LevelMaster;
using Gob3AQ.VARMAP.InputMaster;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.VARMAP.GameMenu;
using Gob3AQ.VARMAP.PlayerMaster;
using Gob3AQ.VARMAP.ItemMaster;
using Gob3AQ.VARMAP.GameEventMaster;
using System.IO;
using Gob3AQ.FixedConfig;
using System;

namespace Gob3AQ.VARMAP.Initialization
{
    public sealed class VARMAP_DataSystem : VARMAP
    {
        /// <summary>
        /// Initializes all VARMAP Data with DefaultValues. Must be called only once in Program Execution.
        /// 
        /// TIP: This should be the very first function called in Program Execution
        /// </summary>
        public static void InitializeVARMAP()
        {
            /* Initialize Libraries */
            CRC32.Initialize();

            /* First allocate array length for DATA and EVENTS */
            AllocateDataSystem();

            /* Initialize safety system (before InitializeDataSystem) */
            VARMAP_Safe.InitializeSafety();

            /* Initialize commit queue */
            VARMAP_Variable_Indexable.Initialize();

            /* Initialize DATA System by creating every VARMAP Variable of its purpose type */
            InitializeDataSystem();

            /* Update delegate with recently created VARMAP Variable instances */
            VARMAP_Initialization.UpdateDelegates();
            VARMAP_GameMaster.UpdateDelegates();
            VARMAP_LevelMaster.UpdateDelegates();
            VARMAP_InputMaster.UpdateDelegates();
            VARMAP_GraphicsMaster.UpdateDelegates();
            VARMAP_GameMenu.UpdateDelegates();
            VARMAP_PlayerMaster.UpdateDelegates();
            VARMAP_ItemMaster.UpdateDelegates();
            VARMAP_GameEventMaster.UpdateDelegates();


            /* BONUS: Set VARMAP data with default values */
            ResetVARMAP();
        }



        /// <summary>
        /// Resets Events and VARMAP Data to defaults
        /// </summary>
        public static void ResetVARMAP()
        {
            ClearVARMAPChangeEvents();
            VARMAP_DefaultValues.SetDefaultValues();
            VARMAP_Variable_Indexable.CommitPending();
        }


        public static void SaveVARMAPData()
        {
            Digest digest = CRC32.CreateDigest;

            using (FileStream fstream = File.Open(GameFixedConfig.LOADSAVE_FILEPATH, FileMode.Create))
            {
                using (BinaryWriter bwriter = new BinaryWriter(fstream))
                {
                    bwriter.Write(GameFixedConfig.LOAD_SAVE_FILE_FORMAT_VERSION);

                    for (int i = 0; i < VARMAP_savedata.SAVE_IDS.Length; i++)
                    {
                        VARMAP_Variable_Indexable indexable = DATA[(int)VARMAP_savedata.SAVE_IDS[i]];

                        int variable_length = indexable.GetElemSize();

                        Span<byte> variable_bytes = stackalloc byte[variable_length];
                        ReadOnlySpan<byte> bytes_read = variable_bytes;
                        indexable.ParseToBytes(ref variable_bytes);

                        CRC32.UpdateDigest(ref digest, ref bytes_read, variable_length);

                        bwriter.Write(variable_bytes);
                    }

                    bwriter.Write(digest.CRC32_Result);
                }
            }
        }

        public static void LoadVARMAPData()
        {
            Digest digest = CRC32.CreateDigest;

            using (FileStream fstream = File.Open(GameFixedConfig.LOADSAVE_FILEPATH, FileMode.Open))
            {
                using (BinaryReader breader = new BinaryReader(fstream))
                {

                    Span<byte> version = stackalloc byte[GameFixedConfig.LOAD_SAVE_FILE_FORMAT_VERSION.Length];
                    breader.Read(version);

                    if ((version[0] != GameFixedConfig.LOAD_SAVE_FILE_FORMAT_VERSION[0]) || (version[1] != GameFixedConfig.LOAD_SAVE_FILE_FORMAT_VERSION[1]) || (version[2] != GameFixedConfig.LOAD_SAVE_FILE_FORMAT_VERSION[2]))
                    {
                        throw new Exception("Versions mismatch");
                    }


                    for (int i = 0; i < VARMAP_savedata.SAVE_IDS.Length; i++)
                    {
                        VARMAP_Variable_Indexable indexable = DATA[(int)VARMAP_savedata.SAVE_IDS[i]];

                        int variable_length = indexable.GetElemSize();

                        Span<byte> variable_bytes = stackalloc byte[variable_length];
                        ReadOnlySpan<byte> read_bytes = variable_bytes;
                        breader.Read(variable_bytes);

                        indexable.ParseFromBytes(ref read_bytes);

                        CRC32.UpdateDigest(ref digest, ref read_bytes, variable_length);
                    }

                    uint readCRC32 = breader.ReadUInt32();

                    if (digest.CRC32_Result != readCRC32)
                    {
                        VARMAP_DefaultValues.SetDefaultValues();
                        throw new Exception("Corrupted load file");
                    }
                }
            }

            ClearVARMAPChangeEvents();
            VARMAP_Variable_Indexable.CommitPending();
        }

        /// <summary>
        /// Allocates DATA and EVENTS arrays to total VARMAP elements size. Must only be used by Initialization process
        /// </summary>
        private static void AllocateDataSystem()
        {
            DATA = new VARMAP_Variable_Indexable[(int)VARMAP_Variable_ID.VARMAP_ID_TOTAL];
            RUBISH_BIN = new uint[VARMAP_Config.VARMAP_SAFE_RUBISH_BIN_SIZE];
        }



        public static void ClearVARMAPChangeEvents()
        {
            for (int i = 0; i < (int)VARMAP_Variable_ID.VARMAP_ID_TOTAL; i++)
            {
                DATA[i].ClearChangeEvent();
            }
        }

        /// <summary>
        /// Should only be called once in Program execution, at Start.
        /// Creates every VARMAP Variable instance according to architecture type
        /// </summary>
        private static void InitializeDataSystem()
        {
            /* > ATG 1 START < */
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS] = new VARMAP_Variable<GameOptionsStruct>(VARMAP_Variable_ID.VARMAP_ID_GAME_OPTIONS, GameOptionsStruct.StaticParseFromBytes, GameOptionsStruct.StaticParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS] = new VARMAP_SafeVariable<ulong>(VARMAP_Variable_ID.VARMAP_ID_ELAPSED_TIME_MS, true, VARMAP_parsers.ulong_ParseFromBytes, VARMAP_parsers.ulong_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM] = new VARMAP_SafeVariable<Room>(VARMAP_Variable_ID.VARMAP_ID_ACTUAL_ROOM, true, VARMAP_parsers.Room_ParseFromBytes, VARMAP_parsers.Room_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED] = new VARMAP_SafeArray<MultiBitFieldStruct>(VARMAP_Variable_ID.VARMAP_ID_EVENTS_OCCURRED, 1, true, MultiBitFieldStruct.StaticParseFromBytes, MultiBitFieldStruct.StaticParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_UNLOCKED_MEMENTO] = new VARMAP_SafeArray<MultiBitFieldStruct>(VARMAP_Variable_ID.VARMAP_ID_UNLOCKED_MEMENTO, 1, true, MultiBitFieldStruct.StaticParseFromBytes, MultiBitFieldStruct.StaticParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_UNWATCHED_PARENT_MEMENTO] = new VARMAP_Array<MultiBitFieldStruct>(VARMAP_Variable_ID.VARMAP_ID_UNWATCHED_PARENT_MEMENTO, 1, MultiBitFieldStruct.StaticParseFromBytes, MultiBitFieldStruct.StaticParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER] = new VARMAP_SafeArray<CharacterType>(VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_OWNER, 3, true, VARMAP_parsers.CharacterType_ParseFromBytes, VARMAP_parsers.CharacterType_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT] = new VARMAP_SafeArray<int>(VARMAP_Variable_ID.VARMAP_ID_PLAYER_ACTUAL_WAYPOINT, 3, true, VARMAP_parsers.int_ParseFromBytes, VARMAP_parsers.int_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_CAMERA_DISPOSITION] = new VARMAP_Variable<CameraDispositionStruct>(VARMAP_Variable_ID.VARMAP_ID_CAMERA_DISPOSITION, CameraDispositionStruct.StaticParseFromBytes, CameraDispositionStruct.StaticParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_HOVER] = new VARMAP_Variable<GameItem>(VARMAP_Variable_ID.VARMAP_ID_ITEM_MENU_HOVER, VARMAP_parsers.GameItem_ParseFromBytes, VARMAP_parsers.GameItem_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS] = new VARMAP_Variable<Game_Status>(VARMAP_Variable_ID.VARMAP_ID_GAMESTATUS, VARMAP_parsers.Game_Status_ParseFromBytes, VARMAP_parsers.Game_Status_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS] = new VARMAP_Variable<KeyStruct>(VARMAP_Variable_ID.VARMAP_ID_PRESSED_KEYS, KeyStruct.StaticParseFromBytes, KeyStruct.StaticParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES] = new VARMAP_Variable<MousePropertiesStruct>(VARMAP_Variable_ID.VARMAP_ID_MOUSE_PROPERTIES, MousePropertiesStruct.StaticParseFromBytes, MousePropertiesStruct.StaticParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED] = new VARMAP_Variable<CharacterType>(VARMAP_Variable_ID.VARMAP_ID_PLAYER_SELECTED, VARMAP_parsers.CharacterType_ParseFromBytes, VARMAP_parsers.CharacterType_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN] = new VARMAP_SafeVariable<GameItem>(VARMAP_Variable_ID.VARMAP_ID_PICKABLE_ITEM_CHOSEN, true, VARMAP_parsers.GameItem_ParseFromBytes, VARMAP_parsers.GameItem_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_ITEM_HOVER] = new VARMAP_Variable<GameItem>(VARMAP_Variable_ID.VARMAP_ID_ITEM_HOVER, VARMAP_parsers.GameItem_ParseFromBytes, VARMAP_parsers.GameItem_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_USER_INPUT_INTERACTION] = new VARMAP_Variable<UserInputInteraction>(VARMAP_Variable_ID.VARMAP_ID_USER_INPUT_INTERACTION, VARMAP_parsers.UserInputInteraction_ParseFromBytes, VARMAP_parsers.UserInputInteraction_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_EVENTS_BEING_PROCESSED] = new VARMAP_Variable<bool>(VARMAP_Variable_ID.VARMAP_ID_EVENTS_BEING_PROCESSED, VARMAP_parsers.bool_ParseFromBytes, VARMAP_parsers.bool_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_DAY_MOMENT] = new VARMAP_Variable<MomentType>(VARMAP_Variable_ID.VARMAP_ID_DAY_MOMENT, VARMAP_parsers.MomentType_ParseFromBytes, VARMAP_parsers.MomentType_ParseToBytes);
            DATA[(int)VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL] = new VARMAP_Variable<bool>(VARMAP_Variable_ID.VARMAP_ID_LAST_VARMAP_VAL, VARMAP_parsers.bool_ParseFromBytes, VARMAP_parsers.bool_ParseToBytes);
            /* > ATG 1 END < */

        }
    }
}
