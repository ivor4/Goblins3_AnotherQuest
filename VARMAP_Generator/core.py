import sys
import config

class ATGFile:
    def __init__(self, path: str, max_atg_zones: int):
        self.path = path
        self.max_atg_zones = max_atg_zones
        self.writing_lines = []
        self.indents = []
        self.filelines = []
        
        if max_atg_zones > 0:
            with open(self.path, 'r', encoding='utf-8') as f:
                self.filelines = f.readlines()
            self._detect_and_clear_atg_zones()

    def _detect_and_clear_atg_zones(self):
        last_inspected = 0
        for atg_index in range(1, self.max_atg_zones + 1):
            startline, endline, indent = None, None, ""
            
            for i in range(last_inspected, len(self.filelines)):
                line = self.filelines[i]
                if f'ATG {atg_index} START' in line:
                    if startline is not None:
                        sys.exit(f"Error: ATG {atg_index} START duplicated in {self.path}")
                    startline = i
                    indent = line[:line.index("/*")]
                
                if f'ATG {atg_index} END' in line:
                    if endline is not None:
                        sys.exit(f"Error: ATG {atg_index} END duplicated in {self.path}")
                    if startline is None:
                        sys.exit(f"Error: ATG END without START in {self.path}")
                    endline = i
                    break
            
            if startline is None or endline is None:
                sys.exit(f"Error: Start/End not found for index {atg_index} in {self.path}")
        
            if (endline - startline) > 1:
                del self.filelines[startline + 1 : endline]

            last_inspected = startline + 2
            self.writing_lines.append(startline + 1)
            self.indents.append(indent)

    def insert_line(self, atg_index: int, line_str: str):
        if self.max_atg_zones > 0:
            idx = self.writing_lines[atg_index - 1]
            self.filelines.insert(idx, self.indents[atg_index - 1] + line_str)
            for i in range(atg_index - 1, self.max_atg_zones):
                self.writing_lines[i] += 1
        else:
            self.filelines.append(line_str)

    def save(self):
        with open(self.path, "w", encoding='utf-8') as f:
            f.writelines(self.filelines)


class CodeGenContext:
    """Mantiene todas las instancias de archivos ATG para acceso global fácil."""
    def __init__(self):
        self.proto = ATGFile(config.PROTO_PATH, 2)
        self.init = ATGFile(config.INITIALIZATION_PATH, 1)
        self.default = ATGFile(config.DEFAULTVALUES_PATH, 1)
        self.enum = ATGFile(config.ENUM_PATH, 1)
        self.delegate_update = ATGFile(config.DELEGATEUPDATE_PATH, 2)
        self.savedata = ATGFile(config.SAVEDATA_PATH, 1)
        
        self.auto_types = ATGFile(config.AUTO_TYPES_PATH, 7)
        self.dialog_types = ATGFile(config.DIALOG_TYPES_PATH, 3)
        self.decision_types = ATGFile(config.DECISION_TYPES_PATH, 2)
        self.rooms_types = ATGFile(config.ROOMS_TYPES_PATH, 1)
        self.names_types = ATGFile(config.NAMES_TYPES_PATH, 1)
        self.sprite_types = ATGFile(config.SPRITE_TYPES_PATH, 1)
        self.sound_types = ATGFile(config.SOUND_TYPES_PATH, 1)
        self.event_types = ATGFile(config.EVENT_TYPES_PATH, 4)
        self.card_types = ATGFile(config.CARD_TYPES_PATH, 1)
        self.anim_types = ATGFile(config.ANIMATION_TYPES_PATH, 1)
        self.modules_types = ATGFile(config.MODULES_TYPES_PATH, 1)
        self.items_types = ATGFile(config.ITEMS_TYPES_PATH, 6)
        
        self.phrases_text = ATGFile(config.PHRASES_TEXT_PATH, 0)
        self.names_text = ATGFile(config.NAMES_TEXT_PATH, 0)
        
        self.items_interact = ATGFile(config.ITEMS_INTERACTION_PATH, 10)
        self.dialog_atlas = ATGFile(config.DIALOG_ATLAS_PATH, 3)
        self.decision_atlas = ATGFile(config.DECISION_ATLAS_PATH, 2)
        self.sprite_atlas = ATGFile(config.SPRITE_ATLAS_PATH, 1)
        self.sound_atlas = ATGFile(config.SOUND_ATLAS_PATH, 1)
        self.room_atlas = ATGFile(config.ROOM_ATLAS_PATH, 1)
        
        self.modules = []
        self.module_lines = []

    def save_all(self):
        for attr in self.__dict__.values():
            if isinstance(attr, ATGFile):
                attr.save()
            elif isinstance(attr, list) and len(attr) > 0 and isinstance(attr[0], ATGFile):
                for atg in attr:
                    atg.save()