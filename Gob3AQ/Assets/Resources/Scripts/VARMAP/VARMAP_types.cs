using Gob3AQ.VARMAP.Variable.IstreamableNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Gob3AQ.VARMAP.Types;
using UnityEngine;
using Gob3AQ.Libs.Arith;
using Gob3AQ.Waypoint;

namespace Gob3AQ.VARMAP.Types
{
    public delegate void VARMAPValueChangedEvent<T>(ChangedEventType eventType, in T oldval, in T newval);
    public delegate ref readonly T GetVARMAPValueDelegate<T>();
    public delegate void SetVARMAPValueDelegate<T>(in T newValue);
    public delegate void ReUnRegisterVARMAPValueChangeEventDelegate<T>(VARMAPValueChangedEvent<T> callback);
    public delegate ref readonly T GetVARMAPArrayElemValueDelegate<T>(int pos);
    public delegate void SetVARMAPArrayElemValueDelegate<T>(int pos, in T newval);
    public delegate int GetVARMAPArraySizeDelegate();
    public delegate ReadOnlySpan<T> GetVARMAPArrayDelegate<T>();
    public delegate void SetVARMAPArrayDelegate<T>(List<T> newvals);
    public delegate void EVENT_SUBSCRIPTION_CALL_DELEGATE(bool newStatus);


    public enum ChangedEventType
    {
        CHANGED_EVENT_NONE,
        CHANGED_EVENT_SET,
        CHANGED_EVENT_SET_LIST_ELEM
    }


    public enum KeyFunctions
    {
        KEYFUNC_NONE = 0,
        KEYFUNC_UP = 1 << 0,
        KEYFUNC_DOWN = 1 << 1,
        KEYFUNC_LEFT = 1 << 2,
        KEYFUNC_RIGHT = 1 << 3,
        KEYFUNC_JUMP = 1 << 4,
        KEYFUNC_ATTACK = 1 << 5,
        KEYFUNC_SPELL = 1 << 6,
        KEYFUNC_PAUSE = 1 << 7
    }



    public enum Game_Status
    {
        GAME_STATUS_STOPPED,
        GAME_STATUS_PLAY,
        GAME_STATUS_PLAY_DIALOG,
        GAME_STATUS_PLAY_ITEM_MENU,
        GAME_STATUS_PLAY_FREEZE,
        GAME_STATUS_PAUSE,
        GAME_STATUS_CHANGING_ROOM,
        GAME_STATUS_LOADING
    }



    public enum InteractionType
    {
        PLAYER_MOVE,
        PLAYER_WITH_ITEM,
        PLAYER_WITH_DOOR,
        PLAYER_WITH_NPC,
        ITEM_WITH_ITEM,
        ITEM_WITH_PLAYER,
        ITEM_WITH_NPC
    }

    public enum GameEvent
    {
        EVENT_NONE = -1,

        EVENT_FOUNTAIN_FULL,
        EVENT_TALK_MAN,

        EVENT_TOTAL
    }

    public readonly struct SpriteConfig
    {
        public readonly string path;
        public readonly GameItem item;
        public readonly Room room;

        public static readonly SpriteConfig EMPTY = new(string.Empty, GameItem.ITEM_NONE, Room.ROOM_NONE);

        public SpriteConfig(string path, GameItem item, Room room)
        {
            this.path = path;
            this.item = item;
            this.room = room;
        }
    }

    public readonly struct DialogConfig
    {
        public ReadOnlySpan<DialogOption> Options => options;

        private readonly DialogOption[] options;

        public static readonly DialogConfig EMPTY = new(new DialogOption[0]);

        public DialogConfig(DialogOption[] options)
        {
            this.options = options;
        }
    }

    public readonly struct DialogOptionConfig
    {
        public readonly GameEvent conditionEvent;
        public readonly bool conditionNotOccurred;
        public readonly GameEvent triggeredEvent;
        public readonly DialogType dialogTriggered;
        public readonly DialogPhrase[] phrases;

        public static readonly DialogOptionConfig EMPTY = new(GameEvent.EVENT_NONE, false, GameEvent.EVENT_NONE, DialogType.DIALOG_NONE, new DialogPhrase[0]);

        public DialogOptionConfig(GameEvent conditionEvent, bool conditionNotOccurred, GameEvent triggeredEvent, DialogType dialogTriggered, DialogPhrase[] phrases)
        {
            this.conditionEvent = conditionEvent;
            this.conditionNotOccurred = conditionNotOccurred;
            this.triggeredEvent = triggeredEvent;
            this.dialogTriggered = dialogTriggered;
            this.phrases = phrases;
        }
    }


    public readonly struct PhraseConfig
    {
        public readonly Room room;
        public readonly int sound;
        public readonly DialogAnimation animation;

        public static readonly PhraseConfig EMPTY = new(Room.ROOM_NONE, 0, DialogAnimation.DIALOG_ANIMATION_NONE);

        public PhraseConfig(Room room, int sound, DialogAnimation animation)
        {
            this.room = room;
            this.sound = sound;
            this.animation = animation;
        }
    }

    public readonly struct PhraseContent
    {
        public readonly PhraseConfig config;
        public readonly string senderName;
        public readonly string message;

        public static readonly PhraseContent EMPTY = new(in PhraseConfig.EMPTY, string.Empty, string.Empty);

        public PhraseContent(in PhraseConfig config, string senderName, string message)
        {
            this.config = config;
            this.senderName = senderName;
            this.message = message;
        }
    }

    public readonly struct ItemConditions
    {
        public readonly GameEvent eventType;
        public readonly CharacterAnimation animationOK;
        public readonly CharacterAnimation animationNOK_Event;
        public readonly DialogType dialogOK;
        public readonly DialogPhrase phraseOK;
        public readonly DialogType dialogNOK_Event;
        public readonly DialogPhrase phraseNOK_Event;

        public ItemConditions(GameEvent eventType, CharacterAnimation animationOK,
            CharacterAnimation animationNOK_Event,
            DialogType dialogOK, DialogPhrase phraseOK, DialogType dialogNOK_Event, DialogPhrase phraseNOK_Event)
        {
            this.eventType = eventType;
            this.animationOK = animationOK;
            this.animationNOK_Event = animationNOK_Event;
            this.dialogOK = dialogOK;
            this.phraseOK = phraseOK;
            this.dialogNOK_Event = dialogNOK_Event;
            this.phraseNOK_Event = phraseNOK_Event;
        }
    }



    public readonly struct ItemInteractionInfo
    {
        public readonly CharacterType srcChar;
        public readonly ItemInteractionType interaction;
        public readonly GameItem srcItem;
        public readonly ItemConditionsType conditions;
        public readonly GameEvent outEvent;
        public readonly bool consumes;

        public ItemInteractionInfo(CharacterType srcChar, ItemInteractionType interaction,
            GameItem srcItem, ItemConditionsType conditions, GameEvent outEvent, bool consumes)
        {
            this.srcChar = srcChar;
            this.interaction = interaction;
            this.srcItem = srcItem;
            this.conditions = conditions;
            this.outEvent = outEvent;
            this.consumes = consumes;
        }
    }

    public readonly ref struct InteractionUsageOutcome
    {
        public readonly CharacterAnimation animation;
        public readonly DialogType dialogType;
        public readonly DialogPhrase dialogPhrase;
        public readonly GameEvent outEvent;
        public readonly bool consumes;
        public InteractionUsageOutcome(CharacterAnimation animation, DialogType dialogType, DialogPhrase dialogPhrase,
            GameEvent outEvent, bool consumes)
        {
            this.animation = animation;
            this.dialogType = dialogType;
            this.dialogPhrase = dialogPhrase;
            this.outEvent = outEvent;
            this.consumes = consumes;
        }
    }

    public readonly struct InteractionUsage
    {
        public readonly InteractionType type;
        public readonly CharacterType playerSource;
        public readonly GameItem itemSource;
        public readonly CharacterType playerDest;
        public readonly NPCType npcDest;
        public readonly GameItem itemDest;
        public readonly int destListIndex;
        public readonly WaypointClass destWaypoint;
        public readonly ulong playerTransactionId;

        public static InteractionUsage CreatePlayerMove(CharacterType playerSource, WaypointClass destWp)
        {
            return new InteractionUsage(InteractionType.PLAYER_MOVE, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, NPCType.NPC_NONE, GameItem.ITEM_NONE, -1, destWp, 0);
        }

        public static InteractionUsage CreatePlayerTakeItem(CharacterType playerSource, GameItem itemDest, WaypointClass destWp)
        {
            return new InteractionUsage(InteractionType.PLAYER_WITH_ITEM, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, NPCType.NPC_NONE, itemDest, -1, destWp, 0);
        }

        public static InteractionUsage CreatePlayerUseItemWithItem(CharacterType playerSource, GameItem itemSource,
            GameItem itemDest, WaypointClass destWp)
        {
            return new InteractionUsage(InteractionType.ITEM_WITH_ITEM, playerSource, itemSource,
                CharacterType.CHARACTER_NONE, NPCType.NPC_NONE, itemDest, -1, destWp, 0);
        }

        public static InteractionUsage CreatePlayerUseItemWithPlayer(CharacterType playerSource, GameItem itemSource,
            CharacterType playerDest, ulong playerDestTransactionId, WaypointClass destWp)
        {
            return new InteractionUsage(InteractionType.ITEM_WITH_PLAYER, playerSource, itemSource,
                playerDest, NPCType.NPC_NONE, GameItem.ITEM_NONE, -1, destWp, playerDestTransactionId);
        }

        public static InteractionUsage CreatePlayerUseDoor(CharacterType playerSource, int doorIndex, WaypointClass destWp)
        {
            return new InteractionUsage(InteractionType.PLAYER_WITH_DOOR, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, NPCType.NPC_NONE, GameItem.ITEM_NONE, doorIndex, destWp, 0);
        }

        public InteractionUsage(InteractionType type, CharacterType playerSource, GameItem itemSource,
            CharacterType playerDest, NPCType npcDest, GameItem itemDest, int doorIndex, WaypointClass destWaypoint, ulong destPlayerTransaction)
        {
            this.type = type;
            this.playerSource = playerSource;
            this.itemSource = itemSource;
            this.playerDest = playerDest;
            this.npcDest = npcDest;
            this.itemDest = itemDest;
            this.destListIndex = doorIndex;
            this.destWaypoint = destWaypoint;
            this.playerTransactionId = destPlayerTransaction;
        }
    }


    public struct MousePropertiesStruct : IStreamable
    {
        public const int STRUCT_SIZE = 2*2*sizeof(float) + 1*2 * sizeof(int) + 6*sizeof(bool);

        public Vector2 pos1;
        public Vector2 pos2;

        public Vector2Int posPixels;

        public bool primaryPressed;
        public bool primaryPressing;
        public bool primaryReleased;
        public bool secondaryPressed;
        public bool secondaryPressing;
        public bool secondaryReleased;


        public static void StaticParseFromBytes(ref MousePropertiesStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.pos1 = new Vector2(BitConverter.ToSingle(readZone.ReadNext(sizeof(float))), BitConverter.ToSingle(readZone.ReadNext(sizeof(float))));
            gstruct.pos2 = new Vector2(BitConverter.ToSingle(readZone.ReadNext(sizeof(float))), BitConverter.ToSingle(readZone.ReadNext(sizeof(float))));
            gstruct.posPixels = new Vector2Int(BitConverter.ToInt32(readZone.ReadNext(sizeof(int))), BitConverter.ToInt32(readZone.ReadNext(sizeof(int))));
            gstruct.primaryPressed = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.primaryPressing = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.primaryReleased = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.secondaryPressed = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.secondaryPressing = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
            gstruct.secondaryReleased = BitConverter.ToBoolean(readZone.ReadNext(sizeof(bool)));
        }

        public static void StaticParseToBytes(in MousePropertiesStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos1.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos1.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos2.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.pos2.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(int)), gstruct.posPixels.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(int)), gstruct.posPixels.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.primaryPressed);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.primaryPressing);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.primaryReleased);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.secondaryPressed);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.secondaryPressing);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(bool)), gstruct.secondaryReleased);
        }

        public static IStreamable CreateNewInstance()
        {
            return new MousePropertiesStruct();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }

        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public readonly void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(in this, ref writer);
        }
    }

    public struct Vector3Struct : IStreamable
    {
        public const int STRUCT_SIZE =  3 * sizeof(float);
        public Vector3 position;

        public static void StaticParseFromBytes(ref Vector3Struct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.position.x = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.position.y = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.position.z = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
        }

        public static void StaticParseToBytes(ref Vector3Struct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.z);
        }

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new Vector3Struct();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }


        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(ref this, ref writer);
        }
    }

    public struct MultiBitFieldStruct : IStreamable
    {
        public const int STRUCT_SIZE = sizeof(ulong);
        private ulong bitfield;

        public static void StaticParseFromBytes(ref MultiBitFieldStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);
            gstruct.bitfield = BitConverter.ToUInt64(readZone.ReadNext(sizeof(ulong)));
        }

        public static void StaticParseToBytes(in MultiBitFieldStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ulong)), gstruct.bitfield);
        }

        public readonly bool GetIndividualBool(int pos)
        {
            bool retVal;
            int pos_corrected;

            pos_corrected = pos & 0x3F;

            retVal = ((bitfield >> pos_corrected) & 0x1ul) != 0x0ul;


            return retVal;
        }

        public readonly ulong GetValueFromOffset(int offset)
        {
            ulong retVal;
            int offset_corrected;

            offset_corrected = offset & 0x3F;

            retVal = bitfield >> offset_corrected;


            return retVal;
        }

        public void SetIndividualBool(int pos, bool value)
        {
            int pos_corrected;
            ulong setbitval;

            pos_corrected = pos & 0x3F;

            setbitval = 0x1ul << pos_corrected;

            if (value)
            {
                bitfield |= setbitval;
            }
            else
            {
                bitfield &= ~setbitval;
            }
        }

        public void SetValueFromOffset(int offset, ulong value, ulong mask)
        {
            int offset_corrected;

            offset_corrected = offset & 0x3F;

            bitfield &= ~(mask << offset_corrected);
            bitfield |= (value & mask) << offset_corrected;
        }

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new MultiBitFieldStruct();
        }

        public readonly int GetElemSize()
        {
            return STRUCT_SIZE;
        }


        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public readonly void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(in this, ref writer);
        }

        public override readonly string ToString()
        {
            string str = bitfield.ToString("X16");
            return str;
        }
    }

    public struct GameOptionsStruct : IStreamable
    {
        public const int STRUCT_SIZE = KeyOptions.STRUCT_SIZE + sizeof(float) + 4 * sizeof(float);
        public KeyOptions keyOptions;
        public float timeMultiplier; /* From computer to game world */
        public Color rectangleSelectionColor;

        public static void StaticParseFromBytes(ref GameOptionsStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            KeyOptions.StaticParseFromBytes(ref gstruct.keyOptions, ref reader);

            _ = readZone.ReadNext(KeyOptions.STRUCT_SIZE);

            gstruct.timeMultiplier = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));

            gstruct.rectangleSelectionColor.r = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.rectangleSelectionColor.g = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.rectangleSelectionColor.b = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.rectangleSelectionColor.a = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
        }

        public static void StaticParseToBytes(in GameOptionsStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            KeyOptions.StaticParseToBytes(in gstruct.keyOptions, ref writer);

            _ = writeZone.WriteNext(KeyOptions.STRUCT_SIZE);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.timeMultiplier);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.rectangleSelectionColor.r);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.rectangleSelectionColor.g);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.rectangleSelectionColor.b);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.rectangleSelectionColor.a);
        }

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new GameOptionsStruct();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }


        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public readonly void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(in this, ref writer);
        }
    }

    public struct KeyOptions : IStreamable
    {
        public const int STRUCT_SIZE = 8 * sizeof(ushort);
        public KeyCode upKey;
        public KeyCode downKey;
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode jumpKey;
        public KeyCode attackKey;
        public KeyCode spellKey;
        public KeyCode pauseKey;
            
            

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new KeyOptions();
        }

        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }

        public static void StaticParseFromBytes(ref KeyOptions gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.upKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.downKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.leftKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.rightKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.jumpKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.attackKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.spellKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.pauseKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
        }

        public static void StaticParseToBytes(in KeyOptions gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.upKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.downKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.leftKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.rightKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.jumpKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.attackKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.spellKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.pauseKey);
        }

        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public readonly void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(in this, ref writer);
        }
    }
    

    public struct KeyStruct : IStreamable
    {
        public const int STRUCT_SIZE = 3 * sizeof(uint) + sizeof(int);

        public KeyFunctions pressedKeys;
        public KeyFunctions cyclepressedKeys;
        public KeyFunctions cyclereleasedKeys;



        public int GetElemSize()
        {
            return STRUCT_SIZE;
        }

        public static void StaticParseFromBytes(ref KeyStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.pressedKeys = (KeyFunctions)BitConverter.ToUInt32(readZone.ReadNext(sizeof(uint)));
            gstruct.cyclepressedKeys = (KeyFunctions)BitConverter.ToUInt32(readZone.ReadNext(sizeof(uint)));
            gstruct.cyclereleasedKeys = (KeyFunctions)BitConverter.ToUInt32(readZone.ReadNext(sizeof(uint)));
        }

        public static void StaticParseToBytes(in KeyStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.pressedKeys);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.cyclepressedKeys);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(uint)), (uint)gstruct.cyclereleasedKeys);
        }

        public void ParseFromBytes(ref ReadOnlySpan<byte> reader)
        {
            StaticParseFromBytes(ref this, ref reader);
        }

        public readonly void ParseToBytes(ref Span<byte> writer)
        {
            StaticParseToBytes(in this, ref writer);
        }
    }
}
