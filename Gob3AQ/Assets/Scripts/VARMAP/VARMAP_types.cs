using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.Variable.IstreamableNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


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
        GAME_STATUS_PLAY_DECISION,
        GAME_STATUS_PLAY_MEMENTO,
        GAME_STATUS_PLAY_ITEM_MENU,
        GAME_STATUS_PLAY_FREEZE,
        GAME_STATUS_PAUSE,
        GAME_STATUS_CHANGING_ROOM,
        GAME_STATUS_LOADING
    }

    public readonly struct DoorInfo : IEquatable<DoorInfo>
    {
        public readonly Room roomLeadTo;
        public readonly int waypointLeadTo;

        public DoorInfo(Room roomLeadTo, int waypointLeadTo)
        {
            this.roomLeadTo = roomLeadTo;
            this.waypointLeadTo = waypointLeadTo;
        }

        public readonly bool Equals(DoorInfo other)
        {
            return (roomLeadTo == other.roomLeadTo) && (waypointLeadTo == other.waypointLeadTo);
        }

        public override readonly bool Equals(object other)
        {
            return other is LevelElemInfo info && Equals(info);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(roomLeadTo, waypointLeadTo);
        }
    }


    public readonly struct LevelElemInfo : IEquatable<LevelElemInfo>
    {
        public readonly GameItem item;
        public readonly GameItemFamily family;
        public readonly int hoverPriority;
        public readonly int waypoint;
        public readonly bool active;

        public static readonly LevelElemInfo EMPTY = new(GameItem.ITEM_NONE, GameItemFamily.ITEM_FAMILY_TYPE_NONE, -1, -100, false);

        public LevelElemInfo(GameItem item, GameItemFamily family, int waypoint, int hoverPriority, bool active)
        {
            this.item = item;
            this.family = family;
            this.waypoint = waypoint;
            this.hoverPriority = hoverPriority;
            this.active = active;
        }

        public readonly bool Equals(LevelElemInfo other)
        {
            return (item == other.item) && (family == other.family);
        }

        public override readonly bool Equals(object other)
        {
            return other is LevelElemInfo info && Equals(info);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(item, family);
        }
    }

    public readonly struct RoomInfo
    {
        private readonly GameSprite[] backgrounds;
        public readonly ReadOnlyHashSet<GameSprite> sprites;
        public readonly ReadOnlyHashSet<DialogPhrase> phrases;
        public readonly ReadOnlyHashSet<GameItem> items;
        public readonly ReadOnlyHashSet<NameType> names;
        public readonly ReadOnlyHashSet<ActionConditions> actionConditions;

        public ReadOnlySpan<GameSprite> Backgrounds => backgrounds;

        public static readonly RoomInfo EMPTY = new(new GameSprite[0],new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>(0)),
            new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>(0)), new ReadOnlyHashSet<NameType>(new HashSet<NameType>(0)),
            new ReadOnlyHashSet<ActionConditions>(new HashSet<ActionConditions>(0)), new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>(0)));

        public RoomInfo(GameSprite[] backgrounds, ReadOnlyHashSet<GameSprite> sprites,
            ReadOnlyHashSet<GameItem> items, ReadOnlyHashSet<NameType> names, 
            ReadOnlyHashSet<ActionConditions> actionConditions, ReadOnlyHashSet<DialogPhrase> phrases)
        {
            this.backgrounds = backgrounds;
            this.sprites = sprites;
            this.phrases = phrases;
            this.items = items;
            this.names = names;
            this.actionConditions = actionConditions;
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
        public readonly ReadOnlySpan<GameEventCombi> TriggeredEvents => triggeredEvents;
        public readonly ReadOnlySpan<DialogPhrase> Phrases => phrases;

        private readonly GameEventCombi[] conditionEvents;
        public readonly MomentType momentType;
        private readonly GameEventCombi[] triggeredEvents;
        public readonly DialogType dialogTriggered;
        public readonly bool randomized;
        private readonly DialogPhrase[] phrases;

        public static readonly DialogOptionConfig EMPTY = new(new GameEventCombi[0], MomentType.MOMENT_ANY, new GameEventCombi[0], DialogType.DIALOG_NONE, false, new DialogPhrase[0]);

        public DialogOptionConfig(GameEventCombi[] conditionEvents, MomentType momentType, GameEventCombi[] triggeredEvents, DialogType dialogTriggered,
            bool randomized, DialogPhrase[] phrases)
        {
            this.conditionEvents = conditionEvents;
            this.momentType = momentType;
            this.triggeredEvents = triggeredEvents;
            this.dialogTriggered = dialogTriggered;
            this.randomized = randomized;
            this.phrases = phrases;
        }
    }

    public readonly struct DecisionConfig
    {
        private readonly DecisionOption[] options;

        public ReadOnlySpan<DecisionOption> Options => options;

        public static DecisionConfig EMPTY = new(new DecisionOption[0]);

        public DecisionConfig(DecisionOption[] options)
        {
            this.options = options;
        }
    }

    public readonly struct DecisionOptionConfig
    {
        public readonly DialogPhrase phrase;
        private readonly GameEventCombi[] triggeredEvents;
        public ReadOnlySpan<GameEventCombi> TriggeredEvents => triggeredEvents;

        public static DecisionOptionConfig EMPTY = new(DialogPhrase.PHRASE_NONE, new GameEventCombi[0]);

        public DecisionOptionConfig(DialogPhrase phrase, GameEventCombi[] triggeredEvents)
        {
            this.phrase = phrase;
            this.triggeredEvents = triggeredEvents;
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
        public readonly ReadOnlyHashSet<GameSprite> sprites;
        public readonly GameSprite defaultSprite;
        public readonly bool isPickable;
        public readonly GameSprite pickableSprite;
        public readonly GamePickableItem pickableItem;
        public readonly ReadOnlyHashSet<ActionConditions> conditions;



        public static readonly ItemInfo EMPTY = new(NameType.NAME_NONE,GameItemFamily.ITEM_FAMILY_TYPE_NONE,new(new HashSet<GameSprite>(0)),
            GameSprite.SPRITE_NONE,false,GameSprite.SPRITE_NONE, GamePickableItem.ITEM_PICK_NONE, new(new HashSet<ActionConditions>(0)));

        public ItemInfo(NameType name, GameItemFamily family, ReadOnlyHashSet<GameSprite> sprites,
            GameSprite defaultSprite, bool isPickable, GameSprite pickableSprite, GamePickableItem pickableItem, ReadOnlyHashSet<ActionConditions> conditions)
        {
            this.name = name;
            this.family = family;
            this.sprites = sprites;
            this.defaultSprite = defaultSprite;
            this.isPickable = isPickable;
            this.pickableSprite = pickableSprite;
            this.pickableItem = pickableItem;
            this.conditions = conditions;
        }
    }

    public readonly struct MementoParentInfo
    {
        public readonly NameType name;
        public readonly GameSprite sprite;
        private readonly Memento[] children;

        public readonly ReadOnlySpan<Memento> Children => children;

        public static readonly MementoParentInfo EMPTY = new(NameType.NAME_NONE, GameSprite.SPRITE_NONE,new Memento[0]);

        public MementoParentInfo(NameType name, GameSprite sprite, Memento[] children)
        {
            this.name = name;
            this.sprite = sprite;
            this.children = children;
        }
    }

    public readonly struct MementoInfo
    {
        public readonly MementoParent parent;
        public readonly DialogPhrase phrase;
        public readonly ReadOnlyHashSet<MementoCombi> combinations;
        public readonly bool final;

        public static readonly MementoInfo EMPTY = new(MementoParent.MEMENTO_PARENT_NONE, DialogPhrase.PHRASE_NONE,
            new(new HashSet<MementoCombi>(0)), false);

        public MementoInfo(MementoParent parent, DialogPhrase phrase, ReadOnlyHashSet<MementoCombi> combinations, bool final)
        {
            this.parent = parent;
            this.phrase = phrase;
            this.combinations = combinations;
            this.final = final;
        }
    }

    public readonly struct MementoCombiInfo
    {
        public readonly GameEvent triggeredEvent;


        public static readonly MementoCombiInfo EMPTY = new(GameEvent.EVENT_NONE);

        public MementoCombiInfo(GameEvent triggeredEvent)
        {
            this.triggeredEvent = triggeredEvent;
        }
    }

    public readonly struct UnchainInfo
    {
        public readonly bool repeat;
        public readonly UnchainType type;
        public readonly GameEventCombi ignoreif;
        private readonly GameEventCombi[] neededEvents;
        public readonly MomentType momentType;
        public readonly GameItem targetItem;
        public readonly GameSprite targetSprite;
        public readonly CharacterType targetCharacter;
        public readonly Memento targetMemento;
        private readonly GameEventCombi[] targetEvents;
        public readonly DecisionType targetDecision;
        public readonly MomentType targetMomentOfDay;

        public ReadOnlySpan<GameEventCombi> NeededEvents => neededEvents;

        public ReadOnlySpan<GameEventCombi> TargetEvents => targetEvents;

        public static readonly UnchainInfo EMPTY = new(false, UnchainType.UNCHAIN_TYPE_SET_SPRITE, GameEventCombi.EMPTY,
            new GameEventCombi[0],MomentType.MOMENT_ANY, GameItem.ITEM_NONE, GameSprite.SPRITE_NONE, CharacterType.CHARACTER_NONE,
            Memento.MEMENTO_NONE, new GameEventCombi[0], DecisionType.DECISION_NONE, MomentType.MOMENT_ANY);

        public UnchainInfo(bool repeat, UnchainType type, GameEventCombi ignoreif, GameEventCombi[] neededEvents, MomentType momentType,
            GameItem targetItem, GameSprite targetSprite, CharacterType targetCharacter, Memento targetMemento,
            GameEventCombi[] targetEvents, DecisionType targetDecision, MomentType targetMomentOfDay)
        {
            this.repeat = repeat;
            this.type = type;
            this.ignoreif = ignoreif;
            this.neededEvents = neededEvents;
            this.momentType = momentType;
            this.targetItem = targetItem;
            this.targetSprite = targetSprite;
            this.targetCharacter = targetCharacter;
            this.targetMemento = targetMemento;
            this.targetEvents = targetEvents;
            this.targetDecision = targetDecision;
            this.targetMomentOfDay = targetMomentOfDay;
        }

    }


    public readonly struct ActionConditionsInfo
    {
        private readonly GameEventCombi[] neededEvents;
        public readonly MomentType momentType;
        public readonly CharacterType srcChar;
        public readonly GameItem srcItem;
        public readonly ItemInteractionType actionOK;
        public readonly CharacterAnimation animationOK;
        public readonly DialogType dialogOK;
        public readonly DialogPhrase phraseOK;
        private readonly GameEventCombi[] unchainEvents;

        public ReadOnlySpan<GameEventCombi> NeededEvents => neededEvents;
        public ReadOnlySpan<GameEventCombi> UnchainEvents => unchainEvents;


        public static readonly ActionConditionsInfo EMPTY = new(new GameEventCombi[0], MomentType.MOMENT_ANY, CharacterType.CHARACTER_NONE,
            GameItem.ITEM_NONE, ItemInteractionType.INTERACTION_NONE, CharacterAnimation.ITEM_USE_ANIMATION_NONE,
            DialogType.DIALOG_NONE, DialogPhrase.PHRASE_NONE, new GameEventCombi[0]);

        public ActionConditionsInfo(GameEventCombi[] events, MomentType momentType, CharacterType srcChar, GameItem srcItem,
            ItemInteractionType actionOK, CharacterAnimation animationOK,DialogType dialogOK, DialogPhrase phraseOK,
            GameEventCombi[] unchainEvents)
        {
            this.neededEvents = events;
            this.momentType = momentType;
            this.srcChar = srcChar;
            this.srcItem = srcItem;
            this.actionOK = actionOK;
            this.animationOK = animationOK;
            this.dialogOK = dialogOK;
            this.phraseOK = phraseOK;
            this.unchainEvents = unchainEvents;
        }
    }


    public readonly ref struct InteractionUsageOutcome
    {
        public readonly CharacterAnimation animation;
        public readonly DialogType dialogType;
        public readonly DialogPhrase dialogPhrase;
        public readonly bool ok;
        public InteractionUsageOutcome(CharacterAnimation animation, DialogType dialogType,
            DialogPhrase dialogPhrase, bool ok)
        {
            this.animation = animation;
            this.dialogType = dialogType;
            this.dialogPhrase = dialogPhrase;
            this.ok = ok;
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
        public readonly int destWaypoint_index;

        public static InteractionUsage CreatePlayerMove(CharacterType playerSource, int destWp_index)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_MOVE, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, GameItem.ITEM_NONE, -1, destWp_index);
        }

        public static InteractionUsage CreateTakeItem(CharacterType playerSource, GameItem itemDest, int destWp_index)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_TAKE, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp_index);
        }

        public static InteractionUsage CreateCrossDoor(CharacterType playerSource, GameItem itemDest, int destWp_index)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_CROSS_DOOR, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp_index);
        }

        public static InteractionUsage CreateUseItemWithItem(CharacterType playerSource, GameItem itemSource,
            GameItem itemDest, int destWp_index)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_USE, playerSource, itemSource,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp_index);
        }

        public static InteractionUsage CreateObserveItem(CharacterType playerSource, GameItem itemDest, int destWp_index)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_OBSERVE, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp_index);
        }

        public static InteractionUsage CreateTalkItem(CharacterType playerSource, GameItem itemDest, int destWp_index)
        {
            return new InteractionUsage(ItemInteractionType.INTERACTION_TALK, playerSource, GameItem.ITEM_NONE,
                CharacterType.CHARACTER_NONE, itemDest, -1, destWp_index);
        }



        public InteractionUsage(ItemInteractionType type, CharacterType playerSource, GameItem itemSource,
            CharacterType playerDest, GameItem itemDest, int doorIndex, int destWaypoint_index)
        {
            this.type = type;
            this.playerSource = playerSource;
            this.itemSource = itemSource;
            this.playerDest = playerDest;
            this.itemDest = itemDest;
            this.destListIndex = doorIndex;
            this.destWaypoint_index = destWaypoint_index;
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

    public struct CameraDispositionStruct : IStreamable
    {
        public const int STRUCT_SIZE = 2 * sizeof(float) + sizeof(float) + sizeof(byte);
        public Vector2 position;
        public float orthoSize;
        public Room room;

        public static void StaticParseFromBytes(ref CameraDispositionStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.position.x = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.position.y = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.orthoSize = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.room = (Room)readZone.ReadNext(sizeof(byte))[0];
        }

        public static void StaticParseToBytes(in CameraDispositionStruct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.y);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.orthoSize);
            writeZone.WriteNext(sizeof(byte))[0] = (byte)gstruct.room;
        }

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new CameraDispositionStruct();
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
            StaticParseToBytes(in this, ref writer);
        }
    }

    public struct Vector2Struct : IStreamable
    {
        public const int STRUCT_SIZE =  2 * sizeof(float);
        public Vector2 position;

        public static void StaticParseFromBytes(ref Vector2Struct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.position.x = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
            gstruct.position.y = BitConverter.ToSingle(readZone.ReadNext(sizeof(float)));
        }

        public static void StaticParseToBytes(in Vector2Struct gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.x);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(float)), gstruct.position.y);
        }

        /// <summary>
        /// Normally used to give a new instance to receive from parsed Bytes
        /// </summary>
        /// <returns>new instance of struct/class</returns>
        public static IStreamable CreateNewInstance()
        {
            return new Vector2Struct();
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
            StaticParseToBytes(in this, ref writer);
        }
    }

    public struct MultiBitFieldStruct : IStreamable, IEquatable<MultiBitFieldStruct>
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

        public MultiBitFieldStruct(MultiBitFieldStruct copyFrom)
        {
            bitfield = copyFrom.bitfield;
        }

        public readonly bool GetIndividualBool(int pos)
        {
            bool retVal;
            int pos_corrected;

            pos_corrected = pos & 0x3F;

            retVal = ((bitfield >> pos_corrected) & 0x1ul) != 0x0ul;


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

        public bool Equals(MultiBitFieldStruct other)
        {
            return bitfield == other.bitfield;
        }
    }

    public struct GameOptionsStruct : IStreamable
    {
        public const int STRUCT_SIZE = KeyOptions.STRUCT_SIZE + MouseOptions.STRUCT_SIZE + sizeof(float) + 4 * sizeof(float);
        public KeyOptions keyOptions;
        public MouseOptions mouseOptions;
        public float timeMultiplier; /* From computer to game world */
        public Color rectangleSelectionColor;

        public static void StaticParseFromBytes(ref GameOptionsStruct gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            KeyOptions.StaticParseFromBytes(ref gstruct.keyOptions, ref reader);
            MouseOptions.StaticParseFromBytes(ref gstruct.mouseOptions, ref reader);

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
            MouseOptions.StaticParseToBytes(in gstruct.mouseOptions, ref writer);

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
        public const int STRUCT_SIZE = 2 * sizeof(ushort);
        public Key changeActionKey;
        public Key pauseKey;
            
            

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

            gstruct.changeActionKey = (Key)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.pauseKey = (Key)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
        }

        public static void StaticParseToBytes(in KeyOptions gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.changeActionKey);
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

    public struct MouseOptions : IStreamable
    {
        public const int STRUCT_SIZE = 5 * sizeof(ushort);
        public MouseButton selectKey;
        public MouseButton inventoryKey;
        public MouseButton zoomUpKey;
        public MouseButton zoomDownKey;
        public MouseButton dragKey;



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

        public static void StaticParseFromBytes(ref MouseOptions gstruct, ref ReadOnlySpan<byte> reader)
        {
            ReadStreamSpan<byte> readZone = new ReadStreamSpan<byte>(reader);

            gstruct.selectKey = (MouseButton)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.inventoryKey = (MouseButton)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.zoomUpKey = (MouseButton)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.zoomDownKey = (MouseButton)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
            gstruct.dragKey = (MouseButton)BitConverter.ToUInt16(readZone.ReadNext(sizeof(ushort)));
        }

        public static void StaticParseToBytes(in MouseOptions gstruct, ref Span<byte> writer)
        {
            WriteStreamSpan<byte> writeZone = new WriteStreamSpan<byte>(writer);

            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.selectKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.inventoryKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.zoomUpKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.zoomDownKey);
            BitConverter.TryWriteBytes(writeZone.WriteNext(sizeof(ushort)), (ushort)gstruct.dragKey);
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
