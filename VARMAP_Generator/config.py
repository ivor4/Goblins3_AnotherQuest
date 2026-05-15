

# --- PATHS ---
ATG_PATH = "./../Gob3AQ/Assets/Scripts/VARMAP/"
PROTO_PATH = f"{ATG_PATH}VARMAP.cs"
INITIALIZATION_PATH = f"{ATG_PATH}VARMAP_datasystem.cs"
DEFAULTVALUES_PATH = f"{ATG_PATH}VARMAP_defaultvalues.cs"
ENUM_PATH = f"{ATG_PATH}VARMAP_enum.cs"
DELEGATEUPDATE_PATH = f"{ATG_PATH}VARMAP_UpdateDelegates.cs"
SAVEDATA_PATH = f"{ATG_PATH}VARMAP_savedata.cs"
PHRASES_TEXT_PATH = "./../Gob3AQ/Assets/Dialogs/PHRASES_CSV.csv"
NAMES_TEXT_PATH = "./../Gob3AQ/Assets/Dialogs/NAMES_CSV.csv"

AUTO_TYPES_PATH = f"{ATG_PATH}VARMAP_types_auto.cs"
DIALOG_TYPES_PATH = f"{ATG_PATH}VARMAP_types_dialogs.cs"
DECISION_TYPES_PATH = f"{ATG_PATH}VARMAP_types_decisions.cs"
ROOMS_TYPES_PATH = f"{ATG_PATH}VARMAP_types_rooms.cs"
MODULES_TYPES_PATH = f"{ATG_PATH}VARMAP_types_modules.cs"
ITEMS_TYPES_PATH = f"{ATG_PATH}VARMAP_types_items.cs"
NAMES_TYPES_PATH = f"{ATG_PATH}VARMAP_types_names.cs"
SPRITE_TYPES_PATH = f"{ATG_PATH}VARMAP_types_sprites.cs"
SOUND_TYPES_PATH = f"{ATG_PATH}VARMAP_types_sounds.cs"
EVENT_TYPES_PATH = f"{ATG_PATH}VARMAP_types_events.cs"
CARD_TYPES_PATH = f"{ATG_PATH}VARMAP_types_cards.cs"
ANIMATION_TYPES_PATH = f"{ATG_PATH}VARMAP_types_animations.cs"

ITEMS_INTERACTION_PATH = f"{ATG_PATH}../Static/ItemsInteractionsClass.cs"
DIALOG_ATLAS_PATH = f"{ATG_PATH}../Static/ResourceDialogsAtlas.cs"
DECISION_ATLAS_PATH = f"{ATG_PATH}../Static/ResourceDecisionsAtlas.cs"
SPRITE_ATLAS_PATH = f"{ATG_PATH}../Static/ResourceSpritesAtlas.cs"
SOUND_ATLAS_PATH = f"{ATG_PATH}../Static/ResourceSoundsAtlas.cs"
ROOM_ATLAS_PATH = f"{ATG_PATH}../Static/ResourceAtlas.cs"

# --- CONSTANTS ---
MODULES_START_COLUMN = 8
SERVICE_MODULES_START_COLUMN = 6

ENUM_NAME = "VARMAP_Variable_ID"
ENUM_PREFIX = "VARMAP_ID_"

# --- PREFIXES ---
PREFIXES = {
    'char': 'CharacterType.',
    'pickable': 'GamePickableItem.',
    'interaction': 'ItemInteractionType.',
    'char_anim': 'CharacterAnimation.',
    'anim_trigger': 'AnimationTrigger.',
    'event': 'GameEvent.',
    'action': 'GameAction.',
    'cond': 'ActionConditions.',
    'spawn_cond': 'SpawnConditions.',
    'dialog': 'DialogType.',
    'dialog_opt': 'DialogOption.',
    'decision': 'DecisionType.',
    'decision_opt': 'DecisionOption.',
    'phrase': 'DialogPhrase.',
    'item': 'GameItem.',
    'room': 'Room.',
    'sprite': 'GameSprite.',
    'sound': 'GameSound.',
    'sound_effect': 'SoundEffect.',
    'name': 'NameType.',
    'family': 'GameItemFamily.',
    'unchain': 'ActionType.',
    'memento': 'Memento.',
    'memento_parent': 'MementoParent.',
    'memento_combi': 'MementoCombi.',
    'moment': 'MomentType.',
    'animation': 'GameAnimation.',
    'detail': 'DetailType.',
    'unchain_cond': 'UnchainConditions.',
    'cardgame': 'CardGameID.'
}