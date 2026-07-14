import sys
from config import MODULES_START_COLUMN, ENUM_NAME, ENUM_PREFIX, ATG_PATH, \
    SERVICE_MODULES_START_COLUMN, PREFIXES
from core import CodeGenContext, ATGFile

def read_csv(filepath: str, replace_commas=False) -> list:
    """Lee un CSV, limpia retornos de carro, comillas y devuelve las columnas."""
    lines = []
    with open(filepath, 'r', encoding='utf-8') as f:
        for line in f:
            clean = line.strip('\n\r').replace('"', '')
            if replace_commas:
                clean = clean.replace(', ', '|')
            lines.append(clean.split(','))
    return lines

def build_array_str(options: list, prefix: str) -> str:
    """Helper para crear strings de arrays en C# rápidamente"""
    if not options or options == ['']: return ""
    return ", ".join([f"{prefix}{opt}" for opt in options])

def parse_events(events_str: str) -> str:
    """Helper para formatear combinaciones de eventos lógicos (NOT)"""
    if not events_str: return ""
    options = events_str.split('|')
    events = []
    for opt in options:
        is_not = 'true' if '(NOT)' in opt else 'false'
        clean_opt = opt.replace('(NOT)', '')
        events.append(f"new({PREFIXES['event']}{clean_opt}, {is_not})")
    return ", ".join(events)


def process_varmap(ctx: CodeGenContext):
    rows = read_csv("VARMAP.csv")
    for idx, cols in enumerate(rows):
        if idx == 0:
            for i in range(MODULES_START_COLUMN, len(cols)):
                mod = cols[i]
                ctx.modules.append(mod)
                ctx.module_lines.append(ATGFile(f"{ATG_PATH}VARMAP_{mod}.cs", 3))
            continue
        
        v_name, v_type, v_safe, v_arr, v_def, v_save = cols[1], cols[2], int(cols[3]), int(cols[4]), cols[5], cols[6]
        is_struct = "Struct" in v_type
        enum_str = f"{ENUM_NAME}.{ENUM_PREFIX}{v_name}"
        var_in_array = f"DATA[(int){enum_str}]"
        
        # Initialization
        safe_prefix = "Safe" if v_safe != 0 else ""
        class_type = f"VARMAP_{safe_prefix}Array" if v_arr != 0 else f"VARMAP_{safe_prefix}Variable"
        arr_str = f"{v_arr}, " if v_arr != 0 else ""
        safe_str = "false, " if v_safe == 1 else ("true, " if v_safe == 2 else "")
        parser = f"{v_type}.StaticParse" if is_struct else f"VARMAP_parsers.{v_type}_Parse"
        
        init_line = f"{var_in_array} = new {class_type}<{v_type}>({enum_str}, {arr_str}{safe_str}{parser}FromBytes, {parser}ToBytes);\n"
        ctx.init.insert_line(1, init_line)
        
        # Default Values
        cast_var = f"((VARMAP_Variable_Interface<{v_type}>){var_in_array})"
        def_line = f"{cast_var}.InitializeListElems({v_def});\n" if v_arr != 0 else f"{cast_var}.SetValue({v_def});\n"
        ctx.default.insert_line(1, def_line)
        
        # Enums & Savedata
        ctx.enum.insert_line(1, f"{ENUM_PREFIX}{v_name},\n")
        if v_save == "Y":
            ctx.savedata.insert_line(1, f"{ENUM_NAME}.{ENUM_PREFIX}{v_name},\n")
            
        # Protos & Delegates
        if v_arr == 0:
            ctx.proto.insert_line(1, f"protected static GetVARMAPValueDelegate<{v_type}> _GET_{v_name};\n")
            ctx.proto.insert_line(1, f"protected static GetVARMAPValueDelegate<{v_type}> _GET_SHADOW_{v_name};\n")
            ctx.proto.insert_line(1, f"protected static SetVARMAPValueDelegate<{v_type}> _SET_{v_name};\n")
            
            ctx.delegate_update.insert_line(1, f"_GET_{v_name} = {cast_var}.GetValue;\n")
            ctx.delegate_update.insert_line(1, f"_GET_SHADOW_{v_name} = {cast_var}.GetShadowValue;\n")
            ctx.delegate_update.insert_line(1, f"_SET_{v_name} = {cast_var}.SetValue;\n")
        else:
            ctx.proto.insert_line(1, f"protected static GetVARMAPArrayElemValueDelegate<{v_type}> _GET_ELEM_{v_name};\n")
            ctx.proto.insert_line(1, f"protected static GetVARMAPArrayElemValueDelegate<{v_type}> _GET_SHADOW_ELEM_{v_name};\n")
            ctx.proto.insert_line(1, f"protected static SetVARMAPArrayElemValueDelegate<{v_type}> _SET_ELEM_{v_name};\n")
            ctx.proto.insert_line(1, f"protected static GetVARMAPArraySizeDelegate _GET_SIZE_{v_name};\n")
            ctx.proto.insert_line(1, f"protected static GetVARMAPArrayDelegate<{v_type}> _GET_ARRAY_{v_name};\n")
            ctx.proto.insert_line(1, f"protected static GetVARMAPArrayDelegate<{v_type}> _GET_SHADOW_ARRAY_{v_name};\n")
            ctx.proto.insert_line(1, f"protected static SetVARMAPArrayDelegate<{v_type}> _SET_ARRAY_{v_name};\n")
            
            ctx.delegate_update.insert_line(1, f"_GET_ELEM_{v_name} = {cast_var}.GetListElem;\n")
            ctx.delegate_update.insert_line(1, f"_GET_SHADOW_ELEM_{v_name} = {cast_var}.GetShadowListElem;\n")
            ctx.delegate_update.insert_line(1, f"_SET_ELEM_{v_name} = {cast_var}.SetListElem;\n")
            ctx.delegate_update.insert_line(1, f"_GET_SIZE_{v_name} = {cast_var}.GetListSize;\n")
            ctx.delegate_update.insert_line(1, f"_GET_ARRAY_{v_name} = {cast_var}.GetListCopy;\n")
            ctx.delegate_update.insert_line(1, f"_GET_SHADOW_ARRAY_{v_name} = {cast_var}.GetShadowListCopy;\n")
            ctx.delegate_update.insert_line(1, f"_SET_ARRAY_{v_name} = {cast_var}.SetListValues;\n")

        ctx.proto.insert_line(1, f"protected static ReUnRegisterVARMAPValueChangeEventDelegate<{v_type}> _REG_{v_name};\n")
        ctx.proto.insert_line(1, f"protected static ReUnRegisterVARMAPValueChangeEventDelegate<{v_type}> _UNREG_{v_name};\n")
        ctx.delegate_update.insert_line(1, f"_REG_{v_name} = {cast_var}.RegisterChangeEvent;\n")
        ctx.delegate_update.insert_line(1, f"_UNREG_{v_name} = {cast_var}.UnregisterChangeEvent;\n")

        # Module Permissions
        writers = 0
        for i in range(MODULES_START_COLUMN, len(cols)):
            idx_use = i - MODULES_START_COLUMN
            mod_atg = ctx.module_lines[idx_use]
            perms = cols[i]
            
            has_access = False
            if "W" in perms:
                writers += 1
                has_access = True
                if v_arr == 0:
                    mod_atg.insert_line(2, f"public static GetVARMAPValueDelegate<{v_type}> GET_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_{v_name} = _GET_{v_name};\n")
                    mod_atg.insert_line(2, f"public static GetVARMAPValueDelegate<{v_type}> GET_SHADOW_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_SHADOW_{v_name} = _GET_SHADOW_{v_name};\n")
                    mod_atg.insert_line(2, f"public static SetVARMAPValueDelegate<{v_type}> SET_{v_name};\n")
                    mod_atg.insert_line(1, f"SET_{v_name} = _SET_{v_name};\n")
                else:
                    mod_atg.insert_line(2, f"public static GetVARMAPArrayElemValueDelegate<{v_type}> GET_ELEM_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_ELEM_{v_name} = _GET_ELEM_{v_name};\n")
                    mod_atg.insert_line(2, f"public static GetVARMAPArrayElemValueDelegate<{v_type}> GET_SHADOW_ELEM_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_SHADOW_ELEM_{v_name} = _GET_SHADOW_ELEM_{v_name};\n")
                    mod_atg.insert_line(2, f"public static SetVARMAPArrayElemValueDelegate<{v_type}> SET_ELEM_{v_name};\n")
                    mod_atg.insert_line(1, f"SET_ELEM_{v_name} = _SET_ELEM_{v_name};\n")
                    mod_atg.insert_line(2, f"public static GetVARMAPArraySizeDelegate GET_SIZE_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_SIZE_{v_name} = _GET_SIZE_{v_name};\n")
                    mod_atg.insert_line(2, f"public static GetVARMAPArrayDelegate<{v_type}> GET_ARRAY_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_ARRAY_{v_name} = _GET_ARRAY_{v_name};\n")
                    mod_atg.insert_line(2, f"public static GetVARMAPArrayDelegate<{v_type}> GET_SHADOW_ARRAY_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_SHADOW_ARRAY_{v_name} = _GET_SHADOW_ARRAY_{v_name};\n")
                    mod_atg.insert_line(2, f"public static SetVARMAPArrayDelegate<{v_type}> SET_ARRAY_{v_name};\n")
                    mod_atg.insert_line(1, f"SET_ARRAY_{v_name} = _SET_ARRAY_{v_name};\n")
            
            elif "R" in perms:
                has_access = True
                if v_arr == 0:
                    mod_atg.insert_line(2, f"public static GetVARMAPValueDelegate<{v_type}> GET_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_{v_name} = _GET_{v_name};\n")
                else:
                    mod_atg.insert_line(2, f"public static GetVARMAPArrayElemValueDelegate<{v_type}> GET_ELEM_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_ELEM_{v_name} = _GET_ELEM_{v_name};\n")
                    mod_atg.insert_line(2, f"public static GetVARMAPArraySizeDelegate GET_SIZE_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_SIZE_{v_name} = _GET_SIZE_{v_name};\n")
                    mod_atg.insert_line(2, f"public static GetVARMAPArrayDelegate<{v_type}> GET_ARRAY_{v_name};\n")
                    mod_atg.insert_line(1, f"GET_ARRAY_{v_name} = _GET_ARRAY_{v_name};\n")

            if "E" in perms and has_access:
                mod_atg.insert_line(2, f"public static ReUnRegisterVARMAPValueChangeEventDelegate<{v_type}> REG_{v_name};\n")
                mod_atg.insert_line(1, f"REG_{v_name} = _REG_{v_name};\n")
                mod_atg.insert_line(2, f"public static ReUnRegisterVARMAPValueChangeEventDelegate<{v_type}> UNREG_{v_name};\n")
                mod_atg.insert_line(1, f"UNREG_{v_name} = _UNREG_{v_name};\n")
                
        if writers != 1:
            sys.exit(f"VARMAP var {v_name} has {writers} writers (must be 1).")
            
    ctx.enum.insert_line(1, "VARMAP_ID_TOTAL\n")


def process_services(ctx: CodeGenContext):
    rows = read_csv("SERVICES.csv")
    for idx, cols in enumerate(rows):
        if idx == 0: continue
        name, delegate, route, desc = cols[1], cols[2], cols[3], cols[4]
        delegate = delegate or f"{name}_DELEGATE"
        
        accessors = [ctx.modules[i] for i, p in enumerate(cols[SERVICE_MODULES_START_COLUMN:]) if p == 'X']
        owner = next((ctx.modules[i] for i, p in enumerate(cols[SERVICE_MODULES_START_COLUMN:]) if p == 'W'), "")
        
        doc = f"/// <summary> \n/// {desc} \n/// <para> Owner: {owner} </para> \n/// <para> Accessors: {', '.join(accessors)} </para> \n/// <para> Method: <see cref=\"{route}\"/> </para> \n/// </summary>\n"
        
        ctx.proto.insert_line(2, doc + f"protected static {delegate} _{name};\n")
        ctx.delegate_update.insert_line(2, f"_{name} = {route};\n")
        
        writers = sum(1 for p in cols[SERVICE_MODULES_START_COLUMN:] if "W" in p)
        if writers != 1: sys.exit(f"Service {name} has {writers} writers.")
        
        for i in range(SERVICE_MODULES_START_COLUMN, len(cols)):
            if "W" in cols[i] or "X" in cols[i]:
                mod_atg = ctx.module_lines[i - SERVICE_MODULES_START_COLUMN]
                mod_atg.insert_line(3, doc + f"public static {delegate} {name};\n")
                mod_atg.insert_line(1, f"{name} = _{name};\n")


def process_dialogs(ctx: CodeGenContext):
    rows = read_csv("DIALOGS.csv", replace_commas=True)
    zone = 1
    ignoreNext = True
    for idx, cols in enumerate(rows):
        if ignoreNext:
            ignoreNext = False
            continue
        if cols[0] == '':
            ctx.dialog_types.insert_line(zone, '\n' + ('DIALOG_TOTAL\n' if zone == 1 else ''))
            zone += 1
            ignoreNext = True
            continue
            
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.dialog_types.insert_line(zone, f"{name}, \n")
        
        if 'NONE' not in cols[1]:
            if zone == 1:
                items = build_array_str(cols[2].split('|'), PREFIXES['item'])
                opts = build_array_str(cols[3].split('|'), PREFIXES['dialog_opt'])
                ctx.dialog_atlas.insert_line(1, f"new( /* {cols[1]} */\n")
                ctx.dialog_atlas.insert_line(1, f"new GameItem[{len(cols[2].split('|'))}]{{{items}}},\n")
                ctx.dialog_atlas.insert_line(1, f"new DialogOption[{len(cols[3].split('|'))}]{{{opts}}}\n")
                ctx.dialog_atlas.insert_line(1, "),\n\n")
            elif zone == 2:
                events = parse_events(cols[2])
                actions = build_array_str(cols[4].split('|'), PREFIXES['action'])
                phrases = build_array_str(cols[7].split('|'), PREFIXES['phrase'])
                ctx.dialog_atlas.insert_line(2, f"new( /* {cols[1]} */\n")
                ctx.dialog_atlas.insert_line(2, f"new GameEventCombi[{len(cols[2].split('|'))}]{{{events}}},\n")
                ctx.dialog_atlas.insert_line(2, f"{PREFIXES['moment']}{cols[3]},\n")
                ctx.dialog_atlas.insert_line(2, f"new GameAction[{len(cols[4].split('|'))}]{{{actions}}},\n")
                ctx.dialog_atlas.insert_line(2, f"{PREFIXES['dialog']}{cols[5]},{cols[6].lower()},\n")
                ctx.dialog_atlas.insert_line(2, f"new DialogPhrase[{len(cols[7].split('|'))}]{{{phrases}}}\n")
                ctx.dialog_atlas.insert_line(2, "),\n")
                
    ctx.dialog_types.insert_line(2, '\nDIALOG_OPTION_TOTAL\n')


def process_generic_atlas(filename: str, type_atg: ATGFile, atlas_atg: ATGFile, prefix_key: str, builder_func):
    rows = read_csv(filename, replace_commas=True)
    zone = 1
    ignoreNext = True
    for idx, cols in enumerate(rows):
        if ignoreNext:
            ignoreNext = False
            continue
        if cols[0] == '':
            type_atg.insert_line(zone, '\n')
            if zone == 1 and prefix_key == 'decision':
                type_atg.insert_line(zone, 'DECISION_TOTAL\n')
            zone += 1
            ignoreNext = True
            continue
        
        name_val = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        type_atg.insert_line(zone, f"{name_val}, \n")
        if 'NONE' not in cols[1] and builder_func != None:
            builder_func(cols, zone, type_atg, atlas_atg)
            
    if prefix_key == 'decision':
        type_atg.insert_line(2, '\n')
        type_atg.insert_line(2, 'DECISION_OPTION_TOTAL\n')


def builder_decisions(cols, zone, t_atg, a_atg):
    if zone == 1:
        opts = build_array_str(cols[2].split('|'), PREFIXES['decision_opt'])
        a_atg.insert_line(1, f"new( /* {cols[1]} */\n")
        a_atg.insert_line(1, f"new DecisionOption[{len(cols[2].split('|'))}]{{{opts}}}),\n")
    elif zone == 2:
        acts = build_array_str(cols[3].split('|'), PREFIXES['action'])
        a_atg.insert_line(2, f"new( /* {cols[1]} */\n")
        a_atg.insert_line(2, f"{PREFIXES['phrase']}{cols[2]},\n")
        a_atg.insert_line(2, f"new GameAction[{len(cols[3].split('|'))}]{{{acts}}}),\n")


def process_phrases(ctx: CodeGenContext):
    rows = read_csv("PHRASES.csv")
    for idx, cols in enumerate(rows):
        if idx == 0: continue
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.dialog_types.insert_line(3, f"{name}, \n")
        
        if 'NONE' not in cols[1]:
            ctx.phrases_text.insert_line(0, ",".join(cols[7:]) + '\n')
            anim_trigs = f"{PREFIXES['anim_trigger']}{cols[4]},{PREFIXES['anim_trigger']}{cols[5]},{PREFIXES['anim_trigger']}{cols[6]}"
            ctx.dialog_atlas.insert_line(3, f"new({cols[2]},{PREFIXES['sound']}{cols[3]}, new AnimationTrigger[3]{{{anim_trigs}}}), /* {cols[1]} */ \n")
            
    ctx.dialog_types.insert_line(3, '\nPHRASE_TOTAL\n')


def process_rooms(ctx: CodeGenContext):
    rows = read_csv("ROOMS.csv", replace_commas=True)
    for idx, cols in enumerate(rows):
        if idx == 0: continue
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.rooms_types.insert_line(1, f"{name}, \n")
        
        if 'NONE' not in cols[1]:
            ctx.room_atlas.insert_line(1, f"new( /* {cols[1]} */\n")
            
            sprites = build_array_str(cols[2].split('|'), PREFIXES['sprite'])
            ctx.room_atlas.insert_line(1, f"new GameSprite[{len(cols[2].split('|'))}]{{{sprites}}},\n")
            
            sounds = build_array_str(cols[3].split('|'), PREFIXES['sound'])
            ctx.room_atlas.insert_line(1, f"new GameSound[{len(cols[3].split('|'))}]{{{sounds}}},\n")
            
            ctx.room_atlas.insert_line(1, f"new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>({len(cols[4].split('|'))}){{{build_array_str(cols[4].split('|'), PREFIXES['sprite'])}}}), \n")
            ctx.room_atlas.insert_line(1, f"new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>({len(cols[5].split('|'))}){{{build_array_str(cols[5].split('|'), PREFIXES['item'])}}}), \n")
            ctx.room_atlas.insert_line(1, f"new ReadOnlyHashSet<NameType>(new HashSet<NameType>({len(cols[6].split('|'))}){{{build_array_str(cols[6].split('|'), PREFIXES['name'])}}}), \n")
            ctx.room_atlas.insert_line(1, f"new ReadOnlyHashSet<GameSound>(new HashSet<GameSound>({len(cols[7].split('|'))}){{{build_array_str(cols[7].split('|'), PREFIXES['sound'])}}}), \n")
            ctx.room_atlas.insert_line(1, f"new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>({len(cols[8].split('|'))}){{{build_array_str(cols[8].split('|'), PREFIXES['unchain_cond'])}}}), \n")
            ctx.room_atlas.insert_line(1, f"new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>({len(cols[9].split('|'))}){{{build_array_str(cols[9].split('|'), PREFIXES['unchain_cond'])}}}), \n")
            ctx.room_atlas.insert_line(1, f"new ReadOnlyHashSet<UnchainConditions>(new HashSet<UnchainConditions>({len(cols[10].split('|'))}){{{build_array_str(cols[10].split('|'), PREFIXES['unchain_cond'])}}}) \n")
            
            ctx.room_atlas.insert_line(1, "),\n\n")
            
    ctx.rooms_types.insert_line(1, '\nROOMS_TOTAL \n')


def process_auto_types(ctx: CodeGenContext):
    rows = read_csv("TYPES.csv")
    zone = 1
    ignoreNext = True
    for idx, cols in enumerate(rows):
        if ignoreNext:
            ignoreNext = False
            continue
        if cols[0] == '':
            ctx.auto_types.insert_line(zone, '\n')
            if zone == 1: ctx.auto_types.insert_line(zone, 'CHARACTER_TOTAL\n')
            elif zone == 2: ctx.auto_types.insert_line(zone, 'ITEM_USE_ANIMATION_TOTAL\n')
            elif zone == 3: ctx.auto_types.insert_line(zone, 'INTERACTION_TOTAL\n')
            elif zone == 4: ctx.auto_types.insert_line(zone, 'ACTION_TYPE_TOTAL\n')
            elif zone == 5: ctx.auto_types.insert_line(zone, 'DIALOG_ANIMATION_TOTAL\n')
            elif zone == 6: ctx.auto_types.insert_line(zone, 'ITEM_FAMILY_TYPE_TOTAL\n')
            zone += 1
            ignoreNext = True
            continue
            
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.auto_types.insert_line(zone, f"{name}, \n")
        
    ctx.auto_types.insert_line(7, '\nMOMENT_TOTAL\n')


def process_action_conds(ctx: CodeGenContext):
    rows = read_csv("ACTION_CONDS.csv", replace_commas=True)
    zone = 1
    ignoreNext = True
    for idx, cols in enumerate(rows):
        if ignoreNext:
            ignoreNext = False
            continue
        if cols[0] == '':
            if zone == 1:
                ctx.items_types.insert_line(3, '\nCOND_TOTAL\n')
            elif zone == 2:
                ctx.items_types.insert_line(4, '\nUNCHAIN_TOTAL\n')
            zone += 1
            ignoreNext = True
            continue
            
        name = cols[1]
        if zone == 1:
            ctx.items_types.insert_line(3, f"{name}, \n")
            ctx.items_interact.insert_line(2, f"new( /* {name} */\n")
            
            events = parse_events(cols[2])
            ctx.items_interact.insert_line(2, f"new GameEventCombi[{len(cols[2].split('|'))}]{{{events}}}, \n")
            ctx.items_interact.insert_line(2, f"{PREFIXES['moment']}{cols[3]},{PREFIXES['char']}{cols[4]},{PREFIXES['item']}{cols[5]},{PREFIXES['interaction']}{cols[6]},\n")
            
            actions = build_array_str(cols[7].split('|'), PREFIXES['action'])
            ctx.items_interact.insert_line(2, f"new GameAction[{len(cols[7].split('|'))}]{{{actions}}}), \n\n")
            
        elif zone == 2:
            ctx.items_types.insert_line(4, f"{name}, \n")
            ctx.items_interact.insert_line(1, f"new( /* {name} */\n")
            
            ignore_not = 'true' if '(NOT)' in cols[5] else 'false'
            ignore_clean = cols[5].replace('(NOT)', '')
            ctx.items_interact.insert_line(1, f"{cols[2].lower()},{cols[3].lower()},{cols[4].lower()},new({PREFIXES['event']}{ignore_clean}, {ignore_not}), \n")
            
            events = parse_events(cols[6])
            ctx.items_interact.insert_line(1, f"new GameEventCombi[{len(cols[6].split('|'))}]{{{events}}}, \n")
            ctx.items_interact.insert_line(1, f"{PREFIXES['moment']}{cols[7]}, \n")
            
            actions = build_array_str(cols[8].split('|'), PREFIXES['action'])
            ctx.items_interact.insert_line(1, f"new GameAction[{len(cols[8].split('|'))}]{{{actions}}}), \n\n")

        elif zone == 3:
            ctx.items_types.insert_line(5, f"{name}, \n")
            ctx.items_interact.insert_line(10, f"new( /* {name} */\n")
            
            ctx.items_interact.insert_line(10, f"{cols[2].lower()},{PREFIXES['unchain']}{cols[3]},{PREFIXES['item']}{cols[4]},{PREFIXES['sprite']}{cols[5]},\n")
            ctx.items_interact.insert_line(10, f"{PREFIXES['char']}{cols[6]},{PREFIXES['memento']}{cols[7]},\n")
            
            events = parse_events(cols[8])
            ctx.items_interact.insert_line(10, f"new GameEventCombi[{len(cols[8].split('|'))}]{{{events}}}, \n")
            
            ctx.items_interact.insert_line(10, f"{PREFIXES['decision']}{cols[9]},{PREFIXES['moment']}{cols[10]},{PREFIXES['dialog']}{cols[11]},{PREFIXES['phrase']}{cols[12]},{PREFIXES['anim_trigger']}{cols[13]},{PREFIXES['animation']}{cols[14]},{PREFIXES['sound']}{cols[15]},{PREFIXES['room']}{cols[16]},\"{cols[17]}\",{cols[18].lower().replace('none','null')},{cols[19].lower().replace('none','null')},{cols[20]},{PREFIXES['cardgame']}{cols[21]}), \n\n")

    ctx.items_types.insert_line(5, '\nACTION_TOTAL\n')


def process_items(ctx: CodeGenContext):
    ctx.items_types.insert_line(2, 'ITEM_PICK_NONE = -1,\n')
    rows = read_csv("ITEMS.csv", replace_commas=True)
    
    for idx, cols in enumerate(rows):
        if idx == 0: continue
        
        name = cols[1]
        enum_name = f"{name} = -1" if 'NONE' in name else name
        ctx.items_types.insert_line(1, f"{enum_name}, \n")
        
        if 'NONE' not in name:
            is_pickable = 'true' in cols[6].lower()
            pickname = name.replace('ITEM_', 'ITEM_PICK_') if is_pickable else 'ITEM_PICK_NONE'
            
            if is_pickable:
                ctx.items_types.insert_line(2, f"{pickname}, \n")
                ctx.items_interact.insert_line(4, f"{PREFIXES['item']}{name},\t/* {pickname} */\n")
                ctx.items_interact.insert_line(5, f"{PREFIXES['sprite']}{cols[7]},\t/* {pickname} */\n")
            
            ctx.items_interact.insert_line(3, f"new ( /* {name} */\n")
            
            sprites = build_array_str(cols[4].split('|'), PREFIXES['sprite'])
            ctx.items_interact.insert_line(3, f"{PREFIXES['name']}{cols[2]},{PREFIXES['family']}{cols[3]},new(new HashSet<GameSprite>({len(cols[4].split('|'))}){{{sprites}}}),\n")
            
            ctx.items_interact.insert_line(3, f"{PREFIXES['sprite']}{cols[5]},{str(is_pickable).lower()},{PREFIXES['sprite']}{cols[7]},{PREFIXES['pickable']}{pickname},{PREFIXES['detail']}{cols[8]},\n")
            
            conds = build_array_str(cols[9].split('|'), PREFIXES['cond'])
            ctx.items_interact.insert_line(3, f"new(new HashSet<ActionConditions>({len(cols[9].split('|'))}){{{conds}}})),\n\n")
            
    ctx.items_types.insert_line(1, "\nITEM_TOTAL\n")
    ctx.items_types.insert_line(2, "\nITEM_PICK_TOTAL\n")


def process_names(ctx: CodeGenContext):
    rows = read_csv("NAMES.csv")
    for idx, cols in enumerate(rows):
        if idx == 0: continue
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.names_types.insert_line(1, f"{name}, \n")
        if 'NONE' not in cols[1]:
            ctx.names_text.insert_line(0, f"{cols[2]},{cols[3]}\n")
    ctx.names_types.insert_line(1, '\nNAME_TOTAL\n')


def process_animations(ctx: CodeGenContext):
    rows = read_csv("ANIMATIONS.csv")
    zone = 1
    ignoreNext = True
    for idx, cols in enumerate(rows):
        if ignoreNext:
            ignoreNext = False
            continue
        if cols[0] == '':
            if zone == 1:
                ctx.anim_types.insert_line(1, '\nANIMATION_TOTAL\n')
            zone += 1
            ignoreNext = True
            continue
            
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        if zone == 1:
            ctx.anim_types.insert_line(1, f"{name}, \n")
        elif zone == 2:
            ctx.card_types.insert_line(1, f"{name}, \n")
            
    ctx.card_types.insert_line(1, '\nCARD_GAME_TOTAL\n')


def process_sprites(ctx: CodeGenContext):
    rows = read_csv("SPRITES.csv")
    for idx, cols in enumerate(rows):
        if idx == 0: continue
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.sprite_types.insert_line(1, f"{name}, \n")
        if 'NONE' not in cols[1]:
            ctx.sprite_atlas.insert_line(1, f'new("{cols[2]}", "{cols[3]}"), /* {cols[1]} */ \n')
    ctx.sprite_types.insert_line(1, '\nSPRITE_TOTAL\n')


def process_sounds(ctx: CodeGenContext):
    rows = read_csv("SOUNDS.csv")
    for idx, cols in enumerate(rows):
        if idx == 0: continue
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.sound_types.insert_line(1, f"{name}, \n")
        if 'NONE' not in cols[1]:
            ctx.sound_atlas.insert_line(1, f'new({PREFIXES["sound_effect"]}{cols[2]},"{cols[3]}"), /* {cols[1]} */ \n')
    ctx.sound_types.insert_line(1, '\nSOUND_TOTAL\n')


def process_events(ctx: CodeGenContext):
    rows = read_csv("EVENTS.csv", replace_commas=True)
    zone = 1
    ignoreNext = True
    for idx, cols in enumerate(rows):
        if ignoreNext:
            ignoreNext = False
            continue
        if cols[0] == '':
            if zone == 1: ctx.event_types.insert_line(1, '\nEVENT_TOTAL\n')
            elif zone == 2: ctx.event_types.insert_line(2, '\nMEMENTO_PARENT_TOTAL\n')
            elif zone == 3: ctx.event_types.insert_line(3, '\nMEMENTO_TOTAL\n')
            elif zone == 4: break
            zone += 1
            ignoreNext = True
            continue
            
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.event_types.insert_line(zone, f"{name}, \n")
        
        if 'NONE' not in cols[1]:
            if zone == 2:
                ctx.items_interact.insert_line(6, f"/* {cols[1]} */\n")
                ctx.items_interact.insert_line(6, f"new(\n")
                ctx.items_interact.insert_line(6, f"{PREFIXES['name']}{cols[2]},{PREFIXES['sprite']}{cols[3]},\n")
                mementos = build_array_str(cols[4].split('|'), PREFIXES['memento'])
                ctx.items_interact.insert_line(6, f"new Memento[{len(cols[4].split('|'))}]{{{mementos}}}\n")
                ctx.items_interact.insert_line(6, f"),\n\n")
            elif zone == 3:
                ctx.items_interact.insert_line(7, f"/* {cols[1]} */\n")
                ctx.items_interact.insert_line(7, f"new({PREFIXES['memento_parent']}{cols[2]},{PREFIXES['phrase']}{cols[3]},\n")
                combis = build_array_str(cols[4].split('|'), PREFIXES['memento_combi'])
                ctx.items_interact.insert_line(7, f"new(new HashSet<MementoCombi>({len(cols[4].split('|'))}){{{combis}}}),{cols[5].lower()}),\n")
            elif zone == 4:
                ctx.items_interact.insert_line(8, f"/* {cols[1]} */\n")
                ctx.items_interact.insert_line(8, f"new(\n{PREFIXES['event']}{cols[2]}\n),\n")
                
    ctx.event_types.insert_line(4, '\nMEMENTO_COMBI_TOTAL\n')


def process_details(ctx: CodeGenContext):
    rows = read_csv("DETAILS.csv", replace_commas=True)
    for idx, cols in enumerate(rows):
        if idx == 0: continue
        name = f"{cols[1]} = -1" if 'NONE' in cols[1] else cols[1]
        ctx.items_types.insert_line(6, f"{name}, \n")
        
        if 'NONE' not in cols[1]:
            ctx.items_interact.insert_line(9, f"new({cols[2]}, /* {cols[1]} */ \n")
            
            names = build_array_str(cols[3].split('|'), PREFIXES['name'])
            ctx.items_interact.insert_line(9, f"new(new HashSet<NameType>({len(cols[3].split('|'))}){{{names}}}),\n")
            
            phrases = build_array_str(cols[4].split('|'), PREFIXES['phrase'])
            ctx.items_interact.insert_line(9, f"new(new HashSet<DialogPhrase>({len(cols[4].split('|'))}){{{phrases}}})),\n\n")
            
    ctx.items_types.insert_line(6, '\nDETAIL_TOTAL\n')