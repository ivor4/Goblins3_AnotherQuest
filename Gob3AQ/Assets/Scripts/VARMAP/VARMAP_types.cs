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

    public enum KeyFunctionsIndex
    {
        KEYFUNC_INDEX_CHANGEACTION = 0,
        KEYFUNC_INDEX_SELECT = 1,
        KEYFUNC_INDEX_INVENTORY = 2,
        KEYFUNC_INDEX_ZOOM_UP = 3,
        KEYFUNC_INDEX_ZOOM_DOWN = 4,
        KEYFUNC_INDEX_DRAG = 5,
        KEYFUNC_INDEX_PAUSE = 6,

        KEYFUNC_INDEX_TOTAL
    }

    [System.Flags]
    public enum KeyFunctions
    {
        KEYFUNC_NONE = 0,
        KEYFUNC_CHANGEACTION = 1 << KeyFunctionsIndex.KEYFUNC_INDEX_CHANGEACTION,
        KEYFUNC_SELECT = 1 << KeyFunctionsIndex.KEYFUNC_INDEX_SELECT,
        KEYFUNC_INVENTORY = 1 << KeyFunctionsIndex.KEYFUNC_INDEX_INVENTORY,
        KEYFUNC_ZOOM_UP = 1 << KeyFunctionsIndex.KEYFUNC_INDEX_ZOOM_UP,
        KEYFUNC_ZOOM_DOWN = 1 << KeyFunctionsIndex.KEYFUNC_INDEX_ZOOM_DOWN,
        KEYFUNC_DRAG = 1 << KeyFunctionsIndex.KEYFUNC_INDEX_DRAG,
        KEYFUNC_PAUSE = 1 << KeyFunctionsIndex.KEYFUNC_INDEX_PAUSE
    }

    public enum UserInputInteraction
    {
        INPUT_INTERACTION_TAKE,
        INPUT_INTERACTION_TALK,
        INPUT_INTERACTION_OBSERVE,

        INPUT_INTERACTION_TOTAL
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




    
    public readonly struct LevelElemInfo
    {
        public readonly GameItem item;
        public readonly GameItemFamily family;
        public readonly WaypointClass waypoint;
        public readonly bool active;

        public static readonly LevelElemInfo EMPTY = new(GameItem.ITEM_NONE, GameItemFamily.ITEM_FAMILY_TYPE_NONE, null, false);
        public static readonly LevelElemInfo DEACTIVATOR = new(GameItem.ITEM_NONE, GameItemFamily.ITEM_FAMILY_TYPE_NONE, null, true);

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
        private readonly GameItem[] items;

        public ReadOnlySpan<GameSprite> Sprites => sprites;
        public ReadOnlySpan<DialogPhrase> Phrases => phrases;
        public ReadOnlySpan<GameItem> Items => items;


        public static readonly RoomInfo EMPTY = new(new GameSprite[0], new DialogPhrase[0], new GameItem[0]);

        public RoomInfo(GameSprite[] sprites, DialogPhrase[] phrases, GameItem[] items)
        {
            this.sprites = sprites;
            this.phrases = phrases;
            this.items = items;
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
        private readonly SpawnConditions[] spawnConditions;


        public ReadOnlySpan<ActionConditions> Conditions => conditions;
        public ReadOnlySpan<SpawnConditions> SpawnConditions => spawnConditions;
        public ReadOnlySpan<GameSprite> Sprites => sprites;

        public static readonly ItemInfo EMPTY = new(NameType.NAME_NONE,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new GameSprite[0],false,
            GameSprite.SPRITE_NONE, GamePickableItem.ITEM_PICK_NONE, new ActionConditions[0], new SpawnConditions[0]);

        public ItemInfo(NameType name, GameItemFamily family, GameSprite[] sprites, bool isPickable, GameSprite pickableSprite, GamePickableItem pickableItem,
            ActionConditions[] conditions, SpawnConditions[] spawnConditions)
        {
            this.name = name;
            this.family = family;
            this.sprites = sprites;
            this.isPickable = isPickable;
            this.pickableSprite = pickableSprite;
            this.pickableItem = pickableItem;
            this.conditions = conditions;
            this.spawnConditions = spawnConditions;
        }
    }

    public readonly struct SpawnConditionInfo
    {
        public readonly bool spawn;
        public readonly bool despawn;
        public readonly bool changeSprite;
        private readonly GameEventCombi[] neededEvents;
        public readonly GameSprite targetSprite;


        public ReadOnlySpan<GameEventCombi> Events => neededEvents;

        public static readonly SpawnConditionInfo EMPTY = new(false, false, false, new GameEventCombi[0], GameSprite.SPRITE_NONE);

        public SpawnConditionInfo(bool spawn, bool despawn, bool changeSprite, GameEventCombi[] events, GameSprite targetSprite)
        {
            this.spawn = spawn;
            this.despawn = despawn;
            this.changeSprite = changeSprite;
            this.neededEvents = events;
            this.targetSprite = targetSprite;
        }

    }


    public readonly struct ActionConditionsInfo
    {
        private readonly GameEventCombi[] neededEvents;
        public readonly CharacterType srcChar;
        public readonly GameItem srcItem;
        public readonly ItemInteractionType actionOK;
        public readonly CharacterAnimation animationOK;
        public readonly DialogType dialogOK;
        public readonly DialogPhrase phraseOK;
        public readonly GameEvent unchainEvent;
        public readonly bool consumes;

        public ReadOnlySpan<GameEventCombi> Events => neededEvents;

        public static readonly ActionConditionsInfo EMPTY = new(new GameEventCombi[0], CharacterType.CHARACTER_NONE,
            GameItem.ITEM_NONE, ItemInteractionType.INTERACTION_NONE, CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_NONE, DialogPhrase.PHRASE_NONE, GameEvent.EVENT_NONE, false);

        public ActionConditionsInfo(GameEventCombi[] events, CharacterType srcChar, GameItem srcItem,
            ItemInteractionType actionOK, CharacterAnimation animationOK,DialogType dialogOK, DialogPhrase phraseOK,
            GameEvent unchainEvent, bool consumes)
        {
            this.neededEvents = events;
            this.srcChar = srcChar;
            this.srcItem = srcItem;
            this.actionOK = actionOK;
            this.animationOK = animationOK;
            this.dialogOK = dialogOK;
            this.phraseOK = phraseOK;
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
        public InteractionUsageOutcome(CharacterAnimation animation, DialogType dialogType,
            DialogPhrase dialogPhrase, GameEvent outEvent, bool ok, bool consumes)
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
        public readonly ItemInteractionType type;
        public readonly CharacterType playerSource;
        public readonly GameItem itemSource;
        public readonly CharacterType playerDest;
        public readonly GameItem itemDest;
        public readonly int destListIndex;
        public readonly WaypointClass destWaypoint;

        public static InteractionUsage CreatePlayerMove(CharacterType playerSource, WaypointClass destWp)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_MOVE, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, GameItem.ITEM_NONE, -1, destWp);
        }

        public static InteractionUsage CreateTakeItem(CharacterType playerSource, GameItem itemDest, WaypointClass destWp)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_TAKE, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp);
        }

        public static InteractionUsage CreateUseItemWithItem(CharacterType playerSource, GameItem itemSource,
            GameItem itemDest, WaypointClass destWp)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_USE, playerSource, itemSource,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp);
        }

        public static InteractionUsage CreateObserveItem(CharacterType playerSource, GameItem itemDest, WaypointClass destWp)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_OBSERVE, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp);
        }

        public static InteractionUsage CreateTalkItem(CharacterType playerSource, GameItem itemDest, WaypointClass destWp)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_TALK, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp);
        }



        public InteractionUsage(ItemInteractionType type, CharacterType playerSource, GameItem itemSource,
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
        public const int STRUCT_SIZE = 2*2*sizeof(float) + 1*2 * sizeof(int);

        public Vector2 pos1;
        public Vector2 pos2;
        public Vector2Int posPixels;


        public static void StaticParseFromBytes(ref MousePropertiesStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new(reader);

            gstruct.pos1 = new Vector2(BitConverter.ToSingle(readZone.ReadNext(sizeof(float))), BitConverter.ToSingle(readZone.ReadNext(sizeof(float))));
            gstruct.pos2 = new Vector2(BitConverter.ToSingle(readZone.ReadNext(sizeof(float))), BitConverter.ToSingle(readZone.ReadNext(sizeof(float))));
            gstruct.posPixels = new Vector2Int(BitConverter.ToInt32(readZone.ReadNext(sizeof(int))), BitConverter.ToInt32(readZone.ReadNext(sizeof(int))));
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
        }

        public static IStreamable CreateNewInstance()
        {
            return new MousePropertiesStruct();
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

        public readonly int GetElemSize()
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
    }

    public struct KeyOptions : IStreamable
    {
        public const int STRUCT_SIZE = 7 * sizeof(ushort);
        public KeyCode changeActionKey;
        public KeyCode selectKey;
        public KeyCode inventoryKey;
        public KeyCode zoomUpKey;
        public KeyCode zoomDownKey;
        public KeyCode dragKey;
        public KeyCode pauseKey;
            
            

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new KeyOptions();
        }

        public readonly int GetElemSize()
        {
            return STRUCT_SIZE;
        }

        public static void StaticParseFromBytes(ref KeyOptions gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.changeActionKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.selectKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.inventoryKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.zoomUpKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.zoomDownKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.dragKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.pauseKey = (KeyCode)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
        }

        public static void StaticParseToBytes(in KeyOptions gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.changeActionKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.selectKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.inventoryKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.zoomUpKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.zoomDownKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.dragKey);
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
        public const int STRUCT_SIZE = 3 * sizeof(uint);

        public KeyFunctions pressedKeys;
        public KeyFunctions cyclepressedKeys;
        public KeyFunctions cyclereleasedKeys;

        public static readonly KeyStruct KEY_EMPTY = default;

        public readonly int GetElemSize()
        {
            return STRUCT_SIZE;
        }

        public readonly bool isKeyBeingPressed(KeyFunctions key)
        {
            bool retVal = ((int)pressedKeys & (int)key) != 0;
            return retVal;
        }

        public readonly bool isKeyCyclePressed(KeyFunctions key)
        {
            bool retVal = ((int)cyclepressedKeys & (int)key) != 0;
            return retVal;
        }

        public readonly bool isKeyCycleReleased(KeyFunctions key)
        {
            bool retVal = ((int)cyclereleasedKeys & (int)key) != 0;
            return retVal;
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
