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
        ITEM_WITH_ITEM,
        PLAYER_WITH_ITEM
    }

    
    public readonly struct LevelElemInfo
    {
        public readonly GameItem item;
        public readonly GameItemFamily family;
        public readonly WaypointClass waypoint;
        public readonly bool active;

        public static readonly LevelElemInfo EMPTY = new(GameItem.ITEM_NONE, GameItemFamily.ITEM_FAMILY_TYPE_NONE, null, false);

        public LevelElemInfo(GameItem item, GameItemFamily family, WaypointClass waypoint, bool active)
        {
            this.item = item;
            this.family = family;
            this.waypoint = waypoint;
            this.active = active;
        }
    }

    public readonly struct RoomInfo
    {
        private readonly GameSprite[] sprites;
        private readonly DialogPhrase[] phrases;
        private readonly NameType[] names;

        public ReadOnlySpan<GameSprite> Sprites => sprites;
        public ReadOnlySpan<DialogPhrase> Phrases => phrases;
        public ReadOnlySpan<NameType> Names => names;


        public static readonly RoomInfo EMPTY = new(new GameSprite[0], new DialogPhrase[0], new NameType[0]);

        public RoomInfo(GameSprite[] sprites, DialogPhrase[] phrases, NameType[] names)
        {
            this.sprites = sprites;
            this.phrases = phrases;
            this.names = names;
        }
    }

    public readonly struct SpriteConfig
    {
        public readonly string path;

        public static readonly SpriteConfig EMPTY = new(string.Empty);

        public SpriteConfig(string path)
        {
            this.path = path;
        }
    }

    public readonly struct DialogConfig
    {
        public ReadOnlySpan<GameItem> Talkers => talkers;
        public ReadOnlySpan<DialogOption> Options => options;

        private readonly GameItem[] talkers;
        private readonly DialogOption[] options;

        public static readonly DialogConfig EMPTY = new(new GameItem[0],new DialogOption[0]);

        public DialogConfig(GameItem[] talkers, DialogOption[] options)
        {
            this.talkers = talkers;
            this.options = options;
        }
    }

    public readonly struct DialogOptionConfig
    {
        public readonly ReadOnlySpan<GameEventCombi> ConditionEvents => conditionEvents;
        public readonly ReadOnlySpan<DialogPhrase> Phrases => phrases;

        private readonly GameEventCombi[] conditionEvents;
        public readonly GameEvent triggeredEvent;
        public readonly DialogType dialogTriggered;
        private readonly DialogPhrase[] phrases;

        public static readonly DialogOptionConfig EMPTY = new(new GameEventCombi[0], GameEvent.EVENT_NONE, DialogType.DIALOG_NONE, new DialogPhrase[0]);

        public DialogOptionConfig(GameEventCombi[] conditionEvents, GameEvent triggeredEvent, DialogType dialogTriggered, DialogPhrase[] phrases)
        {
            this.conditionEvents = conditionEvents;
            this.triggeredEvent = triggeredEvent;
            this.dialogTriggered = dialogTriggered;
            this.phrases = phrases;
        }
    }


    public readonly struct PhraseConfig
    {
        public readonly int talkerIndex;
        public readonly int sound;
        public readonly DialogAnimation animation;

        public static readonly PhraseConfig EMPTY = new(0,0, DialogAnimation.DIALOG_ANIMATION_NONE);

        public PhraseConfig(int talkerIndex, int sound, DialogAnimation animation)
        {
            this.talkerIndex = talkerIndex;
            this.sound = sound;
            this.animation = animation;
        }
    }

    public readonly struct PhraseContent
    {
        public readonly PhraseConfig config;
        public readonly string message;

        public static readonly PhraseContent EMPTY = new(in PhraseConfig.EMPTY, string.Empty);

        public PhraseContent(in PhraseConfig config, string message)
        {
            this.config = config;
            this.message = message;
        }
    }



    public readonly struct ItemInfo
    {
        public readonly NameType name;
        public readonly GameItemFamily family;
        private readonly GameSprite[] sprites;
        public readonly bool isPickable;
        public readonly GameSprite pickableSprite;
        public readonly GamePickableItem pickableItem;
        private readonly ActionConditions[] conditions;

        public ReadOnlySpan<ActionConditions> Conditions => conditions;
        public ReadOnlySpan<GameSprite> Sprites => sprites;

        public static readonly ItemInfo EMPTY = new(NameType.NAME_NONE,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new GameSprite[0],false,
            GameSprite.SPRITE_NONE, GamePickableItem.ITEM_PICK_NONE, new ActionConditions[0]);

        public ItemInfo(NameType name, GameItemFamily family, GameSprite[] sprites, bool isPickable, GameSprite pickableSprite, GamePickableItem pickableItem,
            ActionConditions[] conditions)
        {
            this.name = name;
            this.family = family;
            this.sprites = sprites;
            this.isPickable = isPickable;
            this.pickableSprite = pickableSprite;
            this.pickableItem = pickableItem;
            this.conditions = conditions;
        }
    }


    public readonly struct ActionConditionsInfo
    {
        private readonly GameEventCombi[] neededEvents;
        public readonly CharacterType srcChar;
        public readonly GameItem srcItem;
        public readonly ItemInteractionType actionOK;
        public readonly CharacterAnimation animationOK;
        public readonly CharacterAnimation animationNOK_Event;
        public readonly DialogType dialogOK;
        public readonly DialogPhrase phraseOK;
        public readonly DialogType dialogNOK_Event;
        public readonly DialogPhrase phraseNOK_Event;
        public readonly GameEvent unchainEvent;
        public readonly bool consumes;

        public ReadOnlySpan<GameEventCombi> Events => neededEvents;

        public static readonly ActionConditionsInfo EMPTY = new(new GameEventCombi[0], CharacterType.CHARACTER_NONE,
            GameItem.ITEM_NONE, ItemInteractionType.INTERACTION_NONE, CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            CharacterAnimation.ITEM_USE_ANIMATION_NONE, DialogType.DIALOG_NONE, DialogPhrase.PHRASE_NONE,
            DialogType.DIALOG_NONE, DialogPhrase.PHRASE_NONE, GameEvent.EVENT_NONE, false);

        public ActionConditionsInfo(GameEventCombi[] events, CharacterType srcChar, GameItem srcItem,
            ItemInteractionType actionOK, CharacterAnimation animationOK, CharacterAnimation animationNOK_Event,
            DialogType dialogOK, DialogPhrase phraseOK, DialogType dialogNOK_Event, DialogPhrase phraseNOK_Event,
            GameEvent unchainEvent, bool consumes)
        {
            this.neededEvents = events;
            this.srcChar = srcChar;
            this.srcItem = srcItem;
            this.actionOK = actionOK;
            this.animationOK = animationOK;
            this.animationNOK_Event = animationNOK_Event;
            this.dialogOK = dialogOK;
            this.phraseOK = phraseOK;
            this.dialogNOK_Event = dialogNOK_Event;
            this.phraseNOK_Event = phraseNOK_Event;
            this.unchainEvent = unchainEvent;
            this.consumes = consumes;
        }
    }


    public readonly ref struct InteractionUsageOutcome
    {
        public readonly CharacterAnimation animation;
        public readonly DialogType dialogType;
        public readonly DialogPhrase dialogPhrase;
        public readonly GameEvent outEvent;
        public readonly bool ok;
        public readonly bool consumes;
        public InteractionUsageOutcome(CharacterAnimation animation, DialogType dialogType, DialogPhrase dialogPhrase,
            GameEvent outEvent, bool ok, bool consumes)
        {
            this.animation = animation;
            this.dialogType = dialogType;
            this.dialogPhrase = dialogPhrase;
            this.outEvent = outEvent;
            this.ok = ok;
            this.consumes = consumes;
        }
    }

    public readonly struct InteractionUsage
    {
        public readonly InteractionType type;
        public readonly CharacterType playerSource;
        public readonly GameItem itemSource;
        public readonly CharacterType playerDest;
        public readonly GameItem itemDest;
        public readonly int destListIndex;
        public readonly WaypointClass destWaypoint;

        public static InteractionUsage CreatePlayerMove(CharacterType playerSource, WaypointClass destWp)
        {
            return new InteractionUsage(InteractionType.PLAYER_MOVE, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, GameItem.ITEM_NONE, -1, destWp);
        }

        public static InteractionUsage CreatePlayerWithItem(CharacterType playerSource, GameItem itemDest, WaypointClass destWp)
        {
            return new InteractionUsage(InteractionType.PLAYER_WITH_ITEM, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp);
        }

        public static InteractionUsage CreatePlayerUseItemWithItem(CharacterType playerSource, GameItem itemSource,
            GameItem itemDest, WaypointClass destWp)
        {
            return new InteractionUsage(InteractionType.ITEM_WITH_ITEM, playerSource, itemSource,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp);
        }



        public InteractionUsage(InteractionType type, CharacterType playerSource, GameItem itemSource,
            CharacterType playerDest, GameItem itemDest, int doorIndex, WaypointClass destWaypoint)
        {
            this.type = type;
            this.playerSource = playerSource;
            this.itemSource = itemSource;
            this.playerDest = playerDest;
            this.itemDest = itemDest;
            this.destListIndex = doorIndex;
            this.destWaypoint = destWaypoint;
        }
    }

    public readonly struct GameEventCombi
    {
        public readonly GameEvent eventType;
        public readonly bool eventNOT;

        public static readonly GameEventCombi EMPTY = new(GameEvent.EVENT_NONE, false);

        public GameEventCombi(GameEvent eventType, bool eventNOT)
        {
            this.eventType = eventType;
            this.eventNOT = eventNOT;
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
