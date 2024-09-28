using MVerse.VARMAP.Enum;
using MVerse.VARMAP.Safe;
using MVerse.VARMAP.SaveData;
using MVerse.VARMAP.DefaultValues;
using MVerse.VARMAP.Variable;
using MVerse.VARMAP.Config;
using MVerse.Libs.CRC32;
using MVerse.VARMAP.GameMaster;
using MVerse.VARMAP.LevelMaster;
using MVerse.VARMAP.InputMaster;
using MVerse.VARMAP.PhysicsMaster;
using MVerse.VARMAP.GraphicsMaster;
using MVerse.VARMAP.GameMenu;
using MVerse.VARMAP.PlayerMaster;
using MVerse.VARMAP.EnemyMaster;
using MVerse.VARMAP.ItemMaster;
using MVerse.VARMAP.GameEventMaster;
using MVerse.VARMAP.BossMaster;
using System.IO;
using MVerse.FixedConfig;
using System;
using UnityEngine;

namespace MVerse.VARMAP.Initialization
{
    public abstract partial class VARMAP_Initialization : VARMAP
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

            /* Initialize DATA System by creating every VARMAP Variable of its purpose type */
            InitializeDataSystem();

            /* Update delegate with recently created VARMAP Variable instances */
            UpdateDelegates();
            VARMAP_GameMaster.UpdateDelegates();
            VARMAP_LevelMaster.UpdateDelegates();
            VARMAP_InputMaster.UpdateDelegates();
            VARMAP_PhysicsMaster.UpdateDelegates();
            VARMAP_GraphicsMaster.UpdateDelegates();
            VARMAP_GameMenu.UpdateDelegates();
            VARMAP_PlayerMaster.UpdateDelegates();
            VARMAP_EnemyMaster.UpdateDelegates();
            VARMAP_ItemMaster.UpdateDelegates();
            VARMAP_GameEventMaster.UpdateDelegates();
            VARMAP_BossMaster.UpdateDelegates();


            /* BONUS: Set VARMAP data with default values */
            VARMAP_DefaultValues.SetDefaultValues();
        }

        /// <summary>
        /// Resets Events and VARMAP Data to defaults
        /// </summary>
        public static void ResetVARMAP()
        {
            ClearVARMAPChangeEvents();
            VARMAP_DefaultValues.SetDefaultValues();
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
        }

        /// <summary>
        /// Allocates DATA and EVENTS arrays to total VARMAP elements size. Must only be used by Initialization process
        /// </summary>
        private static void AllocateDataSystem()
        {
            DATA = new VARMAP_Variable_Indexable[(int)VARMAP_Variable_ID.VARMAP_ID_TOTAL];
            RUBISH_BIN = new uint[VARMAP_Config.VARMAP_SAFE_RUBISH_BIN_SIZE];
        }

        

        private static void ClearVARMAPChangeEvents()
        {
            for (int i = (int)VARMAP_Variable_ID.VARMAP_ID_NONE+1; i < (int)VARMAP_Variable_ID.VARMAP_ID_TOTAL; i++)
            {
                DATA[i].ClearChangeEvent();
            }
        }

    }
    
}
