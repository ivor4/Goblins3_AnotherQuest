# -*- coding: utf-8 -*-
"""
Spyder Editor

This is not a temporary script file.
"""

#Global part
atg_path = "./../Gob3AQ/Assets/Scripts/VARMAP/"
proto_path = atg_path+"VARMAP.cs"
initialization_path = atg_path+"VARMAP_datasystem.cs"
defaultvalues_path = atg_path+"VARMAP_defaultvalues.cs"
enum_path = atg_path+"VARMAP_enum.cs"
delegateupdate_path = atg_path+"VARMAP_UpdateDelegates.cs"
savedata_path = atg_path+"VARMAP_savedata.cs"
phrases_text_path = "./../Gob3AQ/Assets/Dialogs/PHRASES_CSV.csv"
names_text_path = "./../Gob3AQ/Assets/Dialogs/NAMES_CSV.csv"

#This is Custom part
auto_types_path = atg_path + "VARMAP_types_auto.cs"
dialog_types_path = atg_path + "VARMAP_types_dialogs.cs"
decision_types_path = atg_path + "VARMAP_types_decisions.cs"
rooms_types_path = atg_path + "VARMAP_types_rooms.cs"
modules_types_path = atg_path + "VARMAP_types_modules.cs"
items_types_path = atg_path + "VARMAP_types_items.cs"
names_types_path = atg_path + "VARMAP_types_names.cs"
sprite_types_path = atg_path + "VARMAP_types_sprites.cs"
event_types_path = atg_path + "VARMAP_types_events.cs"
items_interaction_path = atg_path + "../Static/ItemsInteractionsClass.cs"
dialog_atlas_path = atg_path + "../Static/ResourceDialogsAtlas.cs"
decision_atlas_path = atg_path + "../Static/ResourceDecisionsAtlas.cs"
sprite_atlas_path = atg_path + "../Static/ResourceSpritesAtlas.cs"
room_atlas_path = atg_path + "../Static/ResourceAtlas.cs"

MODULES_START_COLUMN = 8
SERVICE_MODULES_START_COLUMN = 6

#ATG Class Definition
class ATGFile:
    def __init__(self, pathString, maxATGZones):
        self.path = pathString
        self.maxATGZones = maxATGZones
        self.ATGWritingLine = []
        self.ATGIndent = []
        
        if(maxATGZones != 0):
            readfile = open(self.path)
            self.filelines = readfile.readlines()
            readfile.close()
            self.DetectAndClearATGZones()
        else:
            self.filelines = []

    def DetectAndClearATGZones(self):
        lastInspectedLine = 0

        for atg_index in range(1,self.maxATGZones+1):
            startline = None
            endline = None
            indent = ""
            
            for i in range(lastInspectedLine,len(self.filelines)):
                line = self.filelines[i]
                if(('ATG ' + str(atg_index)+ ' START') in line):
                    if(startline != None):
                        print("ATG x START for same index found twice in "+self.path)
                        exit()
                    startline = i
                    indentindex = line.index("/*")
                    indent=line[0:indentindex]
                if(('ATG ' + str(atg_index)+ ' END') in line):
                    if(endline != None):
                        print("ATG x END for same index found twice in "+self.path)
                        exit()
                    endline = i
                    if(startline == None):
                        print("Found end of ATG without having found a START in "+self.path)
                        exit()
                    break
            if((startline == None) or (endline == None)):
                print("Start or end not found in "+self.path)
                exit()
        
            difference = endline - startline
            
            if(difference > 1):
                del self.filelines[(startline+1):endline]

            lastInspectedLine = startline + 2
            self.ATGWritingLine.append(startline+1)
            self.ATGIndent.append(indent)

    def InsertLineInATG(self, atg_index, line_str):
        if(self.maxATGZones != 0):
            self.filelines.insert(self.ATGWritingLine[atg_index-1], self.ATGIndent[atg_index-1] + line_str)
            for i in range(atg_index-1, self.maxATGZones):
                self.ATGWritingLine[i] += 1
        else:
            self.filelines.append(line_str)

    def SaveFile(self):
        writefile = open(self.path,"w")
        writefile.writelines(self.filelines)
        writefile.close()

   
            

#CLEAN FILES (Modules are at this point yet unknown)
proto_lines = ATGFile(proto_path, 2)
initialization_lines = ATGFile(initialization_path, 1)
defaultvalues_lines = ATGFile(defaultvalues_path, 1)
enum_lines = ATGFile(enum_path, 1)
delegateupdate_lines = ATGFile(delegateupdate_path, 2)
savedata_lines = ATGFile(savedata_path, 1)
auto_lines = ATGFile(auto_types_path, 7)
dialogs_types_lines = ATGFile(dialog_types_path, 3)
decisions_types_lines = ATGFile(decision_types_path, 2)
phrases_lines = ATGFile(phrases_text_path, 0)
names_lines = ATGFile(names_text_path, 0)
rooms_types_lines = ATGFile(rooms_types_path, 1)
names_types_lines = ATGFile(names_types_path, 1)
sprite_types_lines = ATGFile(sprite_types_path, 1)
event_types_lines = ATGFile(event_types_path, 4)
modules_types_lines = ATGFile(modules_types_path, 1)
items_types_lines = ATGFile(items_types_path, 5)
items_interaction_lines = ATGFile(items_interaction_path, 9)
dialog_atlas_lines = ATGFile(dialog_atlas_path, 3)
decision_atlas_lines = ATGFile(decision_atlas_path, 2)
sprite_atlas_lines = ATGFile(sprite_atlas_path, 1)
room_atlas_lines = ATGFile(room_atlas_path, 1)


added_savedata_lines = 0



enumName = "VARMAP_Variable_ID"
enumPrefix = "VARMAP_ID_"

VARMAPinputFile = open("VARMAP.csv", "r")
SERVICESinputFile = open("SERVICES.csv", "r")
ITEMSinputFile = open("ITEMS.csv", "r")
ACTIONCONDSinputFile = open("ACTION_CONDS.csv", "r")
AUTOinputFile = open("TYPES.csv", "r")
DIALOGSinputFile = open("DIALOGS.csv", "r")
DECISIONSinputFile = open("DECISIONS.csv", "r")
PHRASESinputFile = open("PHRASES.csv", "r")
ROOMSinputFile = open("ROOMS.csv", "r")
NAMESinputFile = open("NAMES.csv", "r")
SPRITESinputFile = open("SPRITES.csv", "r")
EVENTSinputFile = open("EVENTS.csv", "r")
DETAILSinputFile = open("DETAILS.csv", "r")

VARMAPPermissionFile = []



VARMAPVars = []
Modules = []
Modulelines = []
linecount = -1

for line in VARMAPinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    
    
    
    columns = line.split(',')
    print(columns)
    
    if(linecount == 0):
        for i in range(MODULES_START_COLUMN,len(columns)):
            Modules.append(columns[i])
            modulepath = atg_path+"VARMAP_"+columns[i]+".cs"
            Modulelines.append(ATGFile(modulepath, 3))
        continue
    
    VARMAPVar = {}
    
    
    VARMAPVar["index"] = int(columns[0])
    VARMAPVar["name"] = columns[1]
    VARMAPVar["type"] = columns[2]
    VARMAPVar["safety"] = int(columns[3])
    VARMAPVar["array"] = int(columns[4])
    VARMAPVar["default"] = columns[5]
    VARMAPVar["save"] = columns[6]
    VARMAPVar["writers"] = 0
    VARMAPVar["struct"] = ("Struct" in VARMAPVar["type"])
    
    #Only full lowercase types are primitives
    if(columns[2].lower() == columns[2]):
        VARMAPVar["primitive"] = True
    else:
        VARMAPVar["primitive"] = False
    
    VARMAPVars.append(VARMAPVar)
    
    
    #USEFUL PREPROCESSED STRINGS
    enumstring = enumName + "." + enumPrefix + VARMAPVar["name"]
    variableinarray = "DATA[(int)"+enumstring+"]"
    
    
    #INITIALIZE FILE
    
    stringToWrite = variableinarray
    
    if(VARMAPVar["array"] == 0):
        if(VARMAPVar["safety"] == 0):
            stringToWrite += " = new VARMAP_Variable<"+VARMAPVar["type"]+">"
        else:
            stringToWrite += " = new VARMAP_SafeVariable<"+VARMAPVar["type"]+">"
        arrayString = ""
    else:
        if(VARMAPVar["safety"] == 0):
            stringToWrite += " = new VARMAP_Array<"+VARMAPVar["type"]+">"
        else:
            stringToWrite += " = new VARMAP_SafeArray<"+VARMAPVar["type"]+">"
        arrayString = str(VARMAPVar["array"])+", "

    
    if(VARMAPVar["safety"]==1):
        safetyString = "false, "
    elif(VARMAPVar["safety"]==2):
        safetyString = "true, "
    else:
        safetyString = ""

    

    if(VARMAPVar["struct"]):
        stringToWrite += "("+enumstring+", "+arrayString+safetyString+VARMAPVar["type"]+".StaticParseFromBytes, "+VARMAPVar["type"]+".StaticParseToBytes"
    else:
        stringToWrite += "("+enumstring+", "+arrayString+safetyString+"VARMAP_parsers."+VARMAPVar["type"]+"_ParseFromBytes, "+"VARMAP_parsers."+VARMAPVar["type"]+"_ParseToBytes"
    
        

    stringToWrite += ");"


    initialization_lines.InsertLineInATG(1, stringToWrite+'\n')
    
    #DEFAULT FILE
    variableinarray = "((VARMAP_Variable_Interface<"+VARMAPVar["type"]+">)DATA[(int)"+enumstring+"])"
    if(VARMAPVar["array"] == 0):
        stringToWrite = variableinarray+".SetValue("+VARMAPVar["default"]+");"
    else:
        stringToWrite = variableinarray+".InitializeListElems("+VARMAPVar["default"]+");"

    defaultvalues_lines.InsertLineInATG(1, stringToWrite+'\n')
    
    #ENUM FILE
    enum_lines.InsertLineInATG(1, enumPrefix+VARMAPVar["name"]+",\n")

    #SAVE DATA FILE
    if(VARMAPVar["save"] == "Y"):
        savedata_lines.InsertLineInATG(1, "VARMAP_Variable_ID."+enumPrefix+VARMAPVar["name"]+",\n")
    
    #PROTO FILE    
    
    if(VARMAPVar["array"] == 0):
        stringToWrite = "protected static GetVARMAPValueDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _GET_"+VARMAPVar["name"]+";\n"

        proto_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "protected static GetVARMAPValueDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _GET_SHADOW_"+VARMAPVar["name"]+";\n"

        proto_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "protected static SetVARMAPValueDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _SET_"+VARMAPVar["name"]+";\n"

        proto_lines.InsertLineInATG(1, stringToWrite)
    
    else:
        stringToWrite = "protected static GetVARMAPArrayElemValueDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _GET_ELEM_"+VARMAPVar["name"]+";\n"
        
        proto_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "protected static GetVARMAPArrayElemValueDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _GET_SHADOW_ELEM_"+VARMAPVar["name"]+";\n"
        
        proto_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "protected static SetVARMAPArrayElemValueDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _SET_ELEM_"+VARMAPVar["name"]+";\n"
        
        proto_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "protected static GetVARMAPArraySizeDelegate"
        stringToWrite += " _GET_SIZE_"+VARMAPVar["name"]+";\n"
        
        proto_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "protected static GetVARMAPArrayDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _GET_ARRAY_"+VARMAPVar["name"]+";\n"
        
        proto_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "protected static GetVARMAPArrayDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _GET_SHADOW_ARRAY_"+VARMAPVar["name"]+";\n"
        
        proto_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "protected static SetVARMAPArrayDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _SET_ARRAY_"+VARMAPVar["name"]+";\n"
        
        proto_lines.InsertLineInATG(1, stringToWrite)
        
        
        
    stringToWrite = "protected static ReUnRegisterVARMAPValueChangeEventDelegate<"
    stringToWrite += VARMAPVar["type"]+"> _REG_"+VARMAPVar["name"]+";\n"
    
    proto_lines.InsertLineInATG(1, stringToWrite)
    
    stringToWrite = "protected static ReUnRegisterVARMAPValueChangeEventDelegate<"
    stringToWrite += VARMAPVar["type"]+"> _UNREG_"+VARMAPVar["name"]+";\n"
    
    proto_lines.InsertLineInATG(1, stringToWrite)
    
    
    #DELEGATE ASSIGN
    variableinarray = "((VARMAP_Variable_Interface<"+VARMAPVar["type"]+">)DATA[(int)"+enumstring+"])"
    
    if(VARMAPVar["array"] == 0):
        stringToWrite = "_GET_"+VARMAPVar["name"]+" = " + variableinarray + ".GetValue;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_GET_SHADOW_"+VARMAPVar["name"]+" = " + variableinarray + ".GetShadowValue;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_SET_"+VARMAPVar["name"]+" = " + variableinarray + ".SetValue;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
    else:
        stringToWrite = "_GET_ELEM_"+VARMAPVar["name"]+" = " + variableinarray + ".GetListElem;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_GET_SHADOW_ELEM_"+VARMAPVar["name"]+" = " + variableinarray + ".GetShadowListElem;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_SET_ELEM_"+VARMAPVar["name"]+" = " + variableinarray + ".SetListElem;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_GET_SIZE_"+VARMAPVar["name"]+" = " + variableinarray + ".GetListSize;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_GET_ARRAY_"+VARMAPVar["name"]+" = " + variableinarray + ".GetListCopy;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_GET_SHADOW_ARRAY_"+VARMAPVar["name"]+" = " + variableinarray + ".GetShadowListCopy;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_SET_ARRAY_"+VARMAPVar["name"]+" = " + variableinarray + ".SetListValues;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
    
    stringToWrite = "_REG_"+VARMAPVar["name"]+" = " + variableinarray + ".RegisterChangeEvent;\n"
    
    delegateupdate_lines.InsertLineInATG(1, stringToWrite)
    
    stringToWrite = "_UNREG_"+VARMAPVar["name"]+" = " + variableinarray + ".UnregisterChangeEvent;\n"
    
    delegateupdate_lines.InsertLineInATG(1, stringToWrite)
    
    
    #MODULE PERMISSIONS
    for i in range(MODULES_START_COLUMN,len(columns)):
        indextouse = i-MODULES_START_COLUMN
        hasAccess = False
        if("W" in columns[i]):
            VARMAPVar["writers"] += 1
            
            if(VARMAPVar["array"] == 0):
                stringToWrite = "public static GetVARMAPValueDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "GET_"+VARMAPVar["name"]
                stringToWrite += " = _GET_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static GetVARMAPValueDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_SHADOW_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)
                
                stringToWrite = "GET_SHADOW_"+VARMAPVar["name"]
                stringToWrite += " = _GET_SHADOW_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static SetVARMAPValueDelegate<"
                stringToWrite += VARMAPVar["type"]+"> SET_"+VARMAPVar["name"]
                stringToWrite += ";\n"

                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "SET_"+VARMAPVar["name"]
                stringToWrite += " = _SET_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                
            else:
                stringToWrite = "public static GetVARMAPArrayElemValueDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_ELEM_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "GET_ELEM_"+VARMAPVar["name"]
                stringToWrite += " = _GET_ELEM_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static GetVARMAPArrayElemValueDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_SHADOW_ELEM_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "GET_SHADOW_ELEM_"+VARMAPVar["name"]
                stringToWrite += " = _GET_SHADOW_ELEM_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static SetVARMAPArrayElemValueDelegate<"
                stringToWrite += VARMAPVar["type"]+"> SET_ELEM_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "SET_ELEM_"+VARMAPVar["name"]
                stringToWrite += " = _SET_ELEM_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static GetVARMAPArraySizeDelegate"
                stringToWrite +=" GET_SIZE_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite ="GET_SIZE_"+VARMAPVar["name"]
                stringToWrite += " = _GET_SIZE_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static GetVARMAPArrayDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_ARRAY_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "GET_ARRAY_"+VARMAPVar["name"]
                stringToWrite += " = _GET_ARRAY_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static GetVARMAPArrayDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_SHADOW_ARRAY_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "GET_SHADOW_ARRAY_"+VARMAPVar["name"]
                stringToWrite += " = _GET_SHADOW_ARRAY_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static SetVARMAPArrayDelegate<"
                stringToWrite += VARMAPVar["type"]+"> SET_ARRAY_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "SET_ARRAY_"+VARMAPVar["name"]
                stringToWrite += " = _SET_ARRAY_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
            
            hasAccess = True
        elif("R" in columns[i]):
            if(VARMAPVar["array"] == 0):
                stringToWrite = "public static GetVARMAPValueDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "GET_"+VARMAPVar["name"]
                stringToWrite += " = _GET_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
            else:
                stringToWrite = "public static GetVARMAPArrayElemValueDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_ELEM_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "GET_ELEM_"+VARMAPVar["name"]
                stringToWrite += " = _GET_ELEM_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static GetVARMAPArraySizeDelegate"
                stringToWrite +=" GET_SIZE_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite ="GET_SIZE_"+VARMAPVar["name"]
                stringToWrite += " = _GET_SIZE_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
                
                stringToWrite = "public static GetVARMAPArrayDelegate<"
                stringToWrite += VARMAPVar["type"]+"> GET_ARRAY_"+VARMAPVar["name"]
                stringToWrite += ";\n"
                
                Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

                stringToWrite = "GET_ARRAY_"+VARMAPVar["name"]
                stringToWrite += " = _GET_ARRAY_"+VARMAPVar["name"]+";\n"
                
                Modulelines[indextouse].InsertLineInATG(1, stringToWrite)

            hasAccess = True
        
        
        if(("E" in columns[i])and(hasAccess == True)):
            stringToWrite = "public static ReUnRegisterVARMAPValueChangeEventDelegate<"
            stringToWrite += VARMAPVar["type"]+"> REG_"+VARMAPVar["name"]
            stringToWrite += ";\n"
            
            Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

            stringToWrite = "REG_"+VARMAPVar["name"]
            stringToWrite += " = _REG_"+VARMAPVar["name"]+";\n"
            
            Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
            
            stringToWrite = "public static ReUnRegisterVARMAPValueChangeEventDelegate<"
            stringToWrite += VARMAPVar["type"]+"> UNREG_"+VARMAPVar["name"]
            stringToWrite += ";\n"
            
            Modulelines[indextouse].InsertLineInATG(2, stringToWrite)

            stringToWrite = "UNREG_"+VARMAPVar["name"]
            stringToWrite += " = _UNREG_"+VARMAPVar["name"]+";\n"
            
            Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
    
    if(VARMAPVar["writers"] != 1):
        print("VARMAP variable has not a producer or has more than 1 writer")
        exit()
        
            
enum_lines.InsertLineInATG(1, "VARMAP_ID_TOTAL\n")


print('\n\n------SERVICES-------\n\n')
linecount = -1

for line in SERVICESinputFile:
    ServiceVar = {}
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    
    
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue

    ServiceVar["name"] = columns[1]
    ServiceVar["delegate"] = columns[2]
    ServiceVar["route"] = columns[3]
    ServiceVar["descr"] = columns[4]
    ServiceVar["writers"] = 0

    if(ServiceVar["delegate"] == ''):
        ServiceVar["delegate"] = ServiceVar["name"]+'_DELEGATE'

    #PROTO FILE
    stringToWrite = "/// <summary> \n"
    proto_lines.InsertLineInATG(2, stringToWrite)
    stringToWrite = "/// "+ServiceVar["descr"] + "\n"
    proto_lines.InsertLineInATG(2, stringToWrite)
    _str_accessors = ""
    _str_owner = ""
    
    _perms = columns[SERVICE_MODULES_START_COLUMN:len(columns)]
    for i in range(0,len(_perms)):
        if(_perms[i] == 'X'):
            _str_accessors += Modules[i] + ", "
        elif(_perms[i] == 'W'):
            _str_owner =  Modules[i]
    
    stringToWrite = "/// <para> Owner: " + _str_owner + " </para> \n"
    proto_lines.InsertLineInATG(2, stringToWrite)
    stringToWrite = "/// <para> Accessors: " + _str_accessors + " </para> \n"
    proto_lines.InsertLineInATG(2, stringToWrite)
    stringToWrite = "/// <para> Method: <see cref=\"" + ServiceVar["route"] + "\"/> </para> \n"
    proto_lines.InsertLineInATG(2, stringToWrite)
    stringToWrite = "/// </summary>\n"
    proto_lines.InsertLineInATG(2, stringToWrite)
    stringToWrite = "protected static "+ServiceVar["delegate"]
    stringToWrite += " _"+ServiceVar["name"]+";\n"
    proto_lines.InsertLineInATG(2, stringToWrite)

    #UPDATE_DELEGATE FILE
    stringToWrite = "_"+ServiceVar["name"]+" = " + ServiceVar["route"]+";\n"    
    delegateupdate_lines.InsertLineInATG(2, stringToWrite)

    for i in range(SERVICE_MODULES_START_COLUMN,len(columns)):
        indextouse = i-SERVICE_MODULES_START_COLUMN
        hasAccess = False
        if("W" in columns[i]):
            if(ServiceVar["writers"] == 0):
                ServiceVar["writers"] = 1
            else:
                print('More than 1 writer in Services, service '+str(line))
                exit()
            hasAccess = True
        elif("X" in columns[i]):
            hasAccess = True

        if(hasAccess):
            stringToWrite = "/// <summary> \n"
            Modulelines[indextouse].InsertLineInATG(3, stringToWrite)
            stringToWrite = "/// "+ServiceVar["descr"] + "\n"
            Modulelines[indextouse].InsertLineInATG(3, stringToWrite)
            stringToWrite = "/// <para> Owner: " + _str_owner + " </para> \n"
            Modulelines[indextouse].InsertLineInATG(3, stringToWrite)
            stringToWrite = "/// <para> Accessors: " + _str_accessors + " </para> \n"
            Modulelines[indextouse].InsertLineInATG(3, stringToWrite)
            stringToWrite = "/// <para> Method: <see cref=\"" + ServiceVar["route"] + "\"/> </para> \n"
            Modulelines[indextouse].InsertLineInATG(3, stringToWrite)
            stringToWrite = "/// </summary>\n"
            Modulelines[indextouse].InsertLineInATG(3, stringToWrite)
            stringToWrite = "public static "+ServiceVar["delegate"]
            stringToWrite += " "+ServiceVar["name"]+";\n"
            Modulelines[indextouse].InsertLineInATG(3, stringToWrite)
            
            stringToWrite = ServiceVar["name"] + " = _"+ServiceVar["name"]+";\n"
            Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
    if(ServiceVar["writers"] == 0):
        print("Service "+str(linecount)+" has no writer")
        exit()
        
        
character_prefix = 'CharacterType.'
pickable_prefix = 'GamePickableItem.'
interaction_prefix = 'ItemInteractionType.'
animation_prefix = 'CharacterAnimation.'
dialoganim_prefix = 'DialogAnimation.'
event_prefix = 'GameEvent.'
conditiontype_prefix = 'ActionConditions.'
spawnconditiontype_prefix = 'SpawnConditions.'
dialog_prefix = 'DialogType.'
dialog_option_prefix = 'DialogOption.'
decision_prefix = 'DecisionType.'
decision_option_prefix = 'DecisionOption.'
phrase_prefix = 'DialogPhrase.'
item_prefix = 'GameItem.'
room_prefix = 'Room.'
sprite_prefix = 'GameSprite.'
name_prefix = 'NameType.'
family_prefix = 'GameItemFamily.'
unchain_type_prefix = 'UnchainType.'
memento_prefix = 'Memento.'
memento_parent_prefix = 'MementoParent.'
memento_combi_prefix = 'MementoCombi.'
moment_prefix = 'MomentType.'
detail_prefix = 'DetailType.'


print('\n\n------DIALOGS (Custom GOB3) -------\n\n')
linecount = -1
zone = 1
for line in DIALOGSinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    line = line.replace('"','')
    
    line = line.replace(', ','|')
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    if(columns[0] == ''):
        stringToWrite = '\n'
        dialogs_types_lines.InsertLineInATG(zone, stringToWrite)
        
        if(zone == 1):
            stringToWrite = 'DIALOG_TOTAL\n'
        
        dialogs_types_lines.InsertLineInATG(zone, stringToWrite)
        
        zone +=1
        linecount = -1
        continue
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    dialogs_types_lines.InsertLineInATG(zone, stringToWrite)
    
    if(not 'NONE' in columns[1]):
        if(zone == 1):
            stringToWrite = 'new( /* '+columns[1] + ' */\n'
            dialog_atlas_lines.InsertLineInATG(1, stringToWrite)
            
            options = columns[2].split('|')
            num_options = len(options)
            stringToWrite = 'new GameItem['+str(num_options)+']{'
            for _option in options:
                stringToWrite += item_prefix+_option+','
            stringToWrite += '},\n'
            dialog_atlas_lines.InsertLineInATG(1, stringToWrite)
            
            options = columns[3].split('|')
            num_options = len(options)
                
            stringToWrite = 'new DialogOption['+str(num_options)+']{'
            for _option in options:
                stringToWrite += dialog_option_prefix + _option + ', '
            stringToWrite += '}\n'
            dialog_atlas_lines.InsertLineInATG(1, stringToWrite)
            dialog_atlas_lines.InsertLineInATG(1, '),\n')
            dialog_atlas_lines.InsertLineInATG(1, '\n')
        elif(zone == 2):
            stringToWrite = 'new( /* '+columns[1]+' */\n'
            dialog_atlas_lines.InsertLineInATG(2, stringToWrite)
            
            options = columns[2].split('|')
            num_options = len(options)
            
            stringToWrite = 'new GameEventCombi['+str(num_options)+']{'
            
            for _option in options:
                _not = str('(NOT)' in _option).lower()
                _option = _option.replace('(NOT)','')
                stringToWrite += 'new('+event_prefix+_option+', '+_not+'), '
                
            stringToWrite += '},\n'
            dialog_atlas_lines.InsertLineInATG(2, stringToWrite)
            
            stringToWrite = moment_prefix + columns[3] + ',\n'
            dialog_atlas_lines.InsertLineInATG(2, stringToWrite)
            
            options = columns[4].split('|')
            num_options = len(options)
            
            stringToWrite = 'new GameEventCombi['+str(num_options)+']{'
            
            for _option in options:
                _not = str('(NOT)' in _option).lower()
                _option = _option.replace('(NOT)','')
                stringToWrite += 'new('+event_prefix+_option+', '+_not+'), '
                
            stringToWrite += '},\n'
            dialog_atlas_lines.InsertLineInATG(2, stringToWrite)
            
            
            
            stringToWrite = dialog_prefix+columns[5]+','+columns[6].lower()+',\n'
            dialog_atlas_lines.InsertLineInATG(2, stringToWrite)
            
            options = columns[7].split('|')
            num_options = len(options)
            
            stringToWrite = 'new DialogPhrase['+str(num_options)+']{'

            for _option in options:
                stringToWrite += phrase_prefix+_option+', '
                
            stringToWrite += '}\n'
            dialog_atlas_lines.InsertLineInATG(2, stringToWrite)
            stringToWrite = '),\n'
            dialog_atlas_lines.InsertLineInATG(2, stringToWrite)
        
        
    
stringToWrite = '\n'
dialogs_types_lines.InsertLineInATG(2, stringToWrite)
stringToWrite = 'DIALOG_OPTION_TOTAL\n'
dialogs_types_lines.InsertLineInATG(2, stringToWrite)


print('\n\n------DECISIONS (Custom GOB3) -------\n\n')
linecount = -1
zone = 1
for line in DECISIONSinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    line = line.replace('"','')
    
    line = line.replace(', ','|')
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    if(columns[0] == ''):
        stringToWrite = '\n'
        decisions_types_lines.InsertLineInATG(zone, stringToWrite)
        
        if(zone == 1):
            stringToWrite = 'DECISION_TOTAL\n'
        
        decisions_types_lines.InsertLineInATG(zone, stringToWrite)
        
        zone +=1
        linecount = -1
        continue
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    decisions_types_lines.InsertLineInATG(zone, stringToWrite)
    
    if(not 'NONE' in columns[1]):
        if(zone == 1):
            stringToWrite = 'new( /* '+columns[1] + ' */\n'
            decision_atlas_lines.InsertLineInATG(1, stringToWrite)
            
            options = columns[2].split('|')
            num_options = len(options)
            stringToWrite = 'new DecisionOption['+str(num_options)+']{'
            for _option in options:
                stringToWrite += decision_option_prefix+_option+','
            stringToWrite += '}),\n'
            decision_atlas_lines.InsertLineInATG(1, stringToWrite)
        elif(zone == 2):
            stringToWrite = 'new( /* '+columns[1]+' */\n'
            decision_atlas_lines.InsertLineInATG(2, stringToWrite)
            
            stringToWrite = phrase_prefix + columns[2] + ',\n'
            decision_atlas_lines.InsertLineInATG(2, stringToWrite)
            
            
            options = columns[3].split('|')
            num_options = len(options)
            
            stringToWrite = 'new GameEventCombi['+str(num_options)+']{'
            
            for _option in options:
                _not = str('(NOT)' in _option).lower()
                _option = _option.replace('(NOT)','')
                stringToWrite += 'new('+event_prefix+_option+', '+_not+'), '
                
            stringToWrite += '}),\n'
            decision_atlas_lines.InsertLineInATG(2, stringToWrite)
            
        
        
    
stringToWrite = '\n'
decisions_types_lines.InsertLineInATG(2, stringToWrite)
stringToWrite = 'DECISION_OPTION_TOTAL\n'
decisions_types_lines.InsertLineInATG(2, stringToWrite)


print('\n\n------PHRASES (Custom GOB3) -------\n\n')
linecount = -1
zone = 1

for line in PHRASESinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
        
    
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    dialogs_types_lines.InsertLineInATG(3, stringToWrite)
    
    if(not 'NONE' in columns[1]):
        selectedColumns = columns[5:]
        stringToWrite = ",".join(selectedColumns)+'\n'
        phrases_lines.InsertLineInATG(0, stringToWrite)
        
        stringToWrite = 'new('+columns[2]+','+columns[3]+', '+\
            dialoganim_prefix + columns[4]+'), /* '+columns[1]+' */ \n'
        dialog_atlas_lines.InsertLineInATG(3, stringToWrite)
    
stringToWrite = '\n'
dialogs_types_lines.InsertLineInATG(3, stringToWrite)
stringToWrite = 'PHRASE_TOTAL\n'
dialogs_types_lines.InsertLineInATG(3, stringToWrite)




print('\n\n------ROOMS (Custom GOB3) -------\n\n')
linecount = -1
zone = 1
for line in ROOMSinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    line = line.replace('"','')
    
    
    line = line.replace(', ','|')
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    rooms_types_lines.InsertLineInATG(1, stringToWrite)
    
    if('NONE' not in columns[1]):
        stringToWrite = 'new( /* '+columns[1] + ' */\n'
        room_atlas_lines.InsertLineInATG(1, stringToWrite)
        
        options = columns[2].split('|')
        num_options = len(options)
        
        stringToWrite = 'new GameSprite['+str(num_options)+']{'
        
        for _option in options:
            stringToWrite += sprite_prefix + _option + ', '
        
        stringToWrite += '},\n'
        room_atlas_lines.InsertLineInATG(1, stringToWrite)
        
        
        options = columns[3].split('|')
        num_options = len(options)
            
        stringToWrite = 'new ReadOnlyHashSet<GameSprite>(new HashSet<GameSprite>('+str(num_options)+'){'
        for _option in options:
            stringToWrite += sprite_prefix + _option + ', '
        stringToWrite += '}), \n'
        room_atlas_lines.InsertLineInATG(1, stringToWrite)
        
        options = columns[4].split('|')
        num_options = len(options)
            
        stringToWrite = 'new ReadOnlyHashSet<GameItem>(new HashSet<GameItem>('+str(num_options)+'){'
        for _option in options:
            stringToWrite += item_prefix + _option + ', '
        stringToWrite += '}), \n'
        room_atlas_lines.InsertLineInATG(1, stringToWrite)
        
        options = columns[5].split('|')
        num_options = len(options)
            
        stringToWrite = 'new ReadOnlyHashSet<NameType>(new HashSet<NameType>('+str(num_options)+'){'
        for _option in options:
            stringToWrite += name_prefix + _option + ', '
        stringToWrite += '}), \n'
        room_atlas_lines.InsertLineInATG(1, stringToWrite)
        
        options = columns[6].split('|')
        num_options = len(options)
            
        stringToWrite = 'new ReadOnlyHashSet<ActionConditions>(new HashSet<ActionConditions>('+str(num_options)+'){'
        for _option in options:
            stringToWrite += conditiontype_prefix + _option + ', '
        stringToWrite += '}), \n'
        room_atlas_lines.InsertLineInATG(1, stringToWrite)
        
        options = columns[7].split('|')
        num_options = len(options)
            
        stringToWrite = 'new ReadOnlyHashSet<DialogPhrase>(new HashSet<DialogPhrase>('+str(num_options)+'){'
        for _option in options:
            stringToWrite += phrase_prefix + _option + ', '
        stringToWrite += '}) \n'
        room_atlas_lines.InsertLineInATG(1, stringToWrite)
        
        
        
        stringToWrite = '),\n'
        room_atlas_lines.InsertLineInATG(1, stringToWrite)
        room_atlas_lines.InsertLineInATG(1, '\n')
    
stringToWrite = '\n'
rooms_types_lines.InsertLineInATG(1, stringToWrite)
stringToWrite = 'ROOMS_TOTAL \n'
rooms_types_lines.InsertLineInATG(1, stringToWrite)


print('\n\n------MODULES (Custom GOB3) -------\n\n')
for mod in Modules:
    print(mod)
    stringToWrite = 'MODULE_' + mod + ',\n'
    modules_types_lines.InsertLineInATG(1, stringToWrite)
    
    
stringToWrite = '\n'
modules_types_lines.InsertLineInATG(1, stringToWrite)
stringToWrite = 'MODULE_TOTAL \n'
modules_types_lines.InsertLineInATG(1, stringToWrite)
    

print('\n\n------TYPES (Custom GOB3) -------\n\n')
linecount = -1
zone = 1
for line in AUTOinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    
    
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    if(columns[0] == ''):
        stringToWrite = '\n'
        auto_lines.InsertLineInATG(zone, stringToWrite)
        
        if(zone == 1):
            stringToWrite = 'CHARACTER_TOTAL\n'
        elif(zone == 2):
            stringToWrite = 'ITEM_USE_ANIMATION_TOTAL\n'
        elif(zone == 3):
            stringToWrite = 'INTERACTION_TOTAL\n'
        elif(zone == 4):
            stringToWrite = 'UNCHAIN_TYPE_TOTAL\n'
        elif(zone == 5):
            stringToWrite = 'DIALOG_ANIMATION_TOTAL\n'
        elif(zone == 6):
            stringToWrite = 'ITEM_FAMILY_TYPE_TOTAL\n'
        
        auto_lines.InsertLineInATG(zone, stringToWrite)
        
        zone +=1
        linecount = -1
        continue
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    auto_lines.InsertLineInATG(zone, stringToWrite)
    
stringToWrite = '\n'
auto_lines.InsertLineInATG(7, stringToWrite)
stringToWrite = 'MOMENT_TOTAL\n'
auto_lines.InsertLineInATG(7, stringToWrite)
    
    
    
   
print('\n\n------ACTION CONDITIONS (Custom GOB3) -------\n\n')


zone = 1
linecount = -1
for line in ACTIONCONDSinputFile:
    ItemVar = {}
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    line = line.replace('"','')
    
    line = line.replace(', ', '|')
    columns : list[str] = line.split(',')
    print(columns)

    if(linecount == 0):
        continue 
    
    if(columns[0] == ''):
        if(zone == 1):
            stringToWrite = '\n'
            items_types_lines.InsertLineInATG(3, stringToWrite)
            stringToWrite = 'COND_TOTAL\n'
            items_types_lines.InsertLineInATG(3, stringToWrite)
        
        zone +=1
        linecount = -1
        continue
    
    if(zone == 1):
        ItemVar["name"] = str(columns[1])
        ItemVar["events"] = str(columns[2])
        ItemVar["moment"] = moment_prefix+str(columns[3])
        ItemVar["srcChar"] = character_prefix+str(columns[4])
        ItemVar["srcItem"] = item_prefix+str(columns[5])
        ItemVar["actionOK"] = interaction_prefix+str(columns[6])
        ItemVar["animationOK"] = animation_prefix+str(columns[7])
        ItemVar["dialogOK"] = dialog_prefix+str(columns[8])
        ItemVar["phraseOK"] = phrase_prefix+str(columns[9])
        ItemVar["unchain_event"] = str(columns[10])
        
        # Write in item enum
        stringToWrite = ItemVar["name"]
        stringToWrite += ', \n'
        
        items_types_lines.InsertLineInATG(3, stringToWrite)
        
        # Write in conditions struct
        stringToWrite = 'new( /* '+ ItemVar["name"] + ' */\n'
        items_interaction_lines.InsertLineInATG(2, stringToWrite)
        
        
        options = columns[2].split('|')
        num_options = len(options)
            
        stringToWrite = 'new GameEventCombi['+str(num_options)+']{'
    
        for _option in options:
            _not_ = str('(NOT)' in _option).lower()
            _option = _option.replace('(NOT)','')
            stringToWrite += 'new('+event_prefix + _option + ', ' + _not_ + '),'
            
        stringToWrite += '}, \n'
        items_interaction_lines.InsertLineInATG(2, stringToWrite)
        
        
        stringToWrite = ItemVar["moment"]+','+ItemVar["srcChar"]+','+ItemVar["srcItem"]+','+\
            ItemVar["actionOK"]+',\n'
        items_interaction_lines.InsertLineInATG(2, stringToWrite)
        stringToWrite = ItemVar["animationOK"]+',\n'
        items_interaction_lines.InsertLineInATG(2, stringToWrite)
        stringToWrite = ItemVar["dialogOK"]+","+\
            ItemVar["phraseOK"]+",\n"
        items_interaction_lines.InsertLineInATG(2, stringToWrite)
        
        options = ItemVar["unchain_event"].split('|')
        num_options = len(options)
        stringToWrite = 'new GameEventCombi['+str(num_options)+']{'
    
        for _option in options:
            _not_ = str('(NOT)' in _option).lower()
            _option = _option.replace('(NOT)','')
            stringToWrite += 'new('+event_prefix + _option + ', ' + _not_ + '),'
            
        stringToWrite += '}), \n'
        items_interaction_lines.InsertLineInATG(2, stringToWrite)
        items_interaction_lines.InsertLineInATG(2, '\n')
    elif(zone == 2):
        ItemVar["name"] = str(columns[1])
        ItemVar["repeat"] = str(columns[2])
        ItemVar["type"] = str(columns[3])
        ItemVar["ignoreif"] = str(columns[4])
        ItemVar["neededEvents"] = str(columns[5])
        ItemVar["momentOfDay"] = str(columns[6])
        ItemVar["targetItem"] = str(columns[7])
        ItemVar["targetSprite"] = str(columns[8])
        ItemVar["targetCharacter"] = str(columns[9])
        ItemVar["targetMemento"] = str(columns[10])
        ItemVar["targetEvents"] = str(columns[11])
        ItemVar["targetDecision"] = str(columns[12])
        ItemVar["targetMoment"] = str(columns[13])
        
        # Write in item enum
        stringToWrite = ItemVar["name"]
        stringToWrite += ', \n'
        
        items_types_lines.InsertLineInATG(4, stringToWrite)
        
        # Write in conditions struct
        stringToWrite = 'new( /* '+ ItemVar["name"] + ' */\n'
        items_interaction_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = ItemVar["repeat"].lower() + ',' + unchain_type_prefix + ItemVar["type"]+','
        
        _not_ = str('(NOT)' in ItemVar["ignoreif"]).lower()
        _option = ItemVar["ignoreif"].replace('(NOT)','')
        
        stringToWrite += 'new('+event_prefix + _option + ', ' + _not_ + '), \n'
        items_interaction_lines.InsertLineInATG(1, stringToWrite)
        
        
        options = ItemVar["neededEvents"].split('|')
        num_options = len(options)
            
        stringToWrite = 'new GameEventCombi['+str(num_options)+']{'
    
        for _option in options:
            _not_ = str('(NOT)' in _option).lower()
            _option = _option.replace('(NOT)','')
            stringToWrite += 'new('+event_prefix + _option + ', ' + _not_ + '),'
            
        stringToWrite += '}, \n'
        items_interaction_lines.InsertLineInATG(1, stringToWrite)
        
        
        stringToWrite = moment_prefix + ItemVar["momentOfDay"] + ', ' + item_prefix + ItemVar["targetItem"] +', ' +\
            sprite_prefix + ItemVar["targetSprite"]+','+\
            character_prefix + ItemVar["targetCharacter"] +',' +\
            memento_prefix + ItemVar["targetMemento"] + ", \n"
        items_interaction_lines.InsertLineInATG(1, stringToWrite)
        
        
        options = ItemVar["targetEvents"].split('|')
        num_options = len(options)
            
        stringToWrite = 'new GameEventCombi['+str(num_options)+']{'
    
        for _option in options:
            _not_ = str('(NOT)' in _option).lower()
            _option = _option.replace('(NOT)','')
            stringToWrite += 'new('+event_prefix + _option + ', ' + _not_ + '),'
            
        stringToWrite += '}, \n'
        items_interaction_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = decision_prefix + ItemVar["targetDecision"] + ', '
        stringToWrite += moment_prefix + ItemVar["targetMoment"] + '),\n'
        items_interaction_lines.InsertLineInATG(1, stringToWrite)
        
        items_interaction_lines.InsertLineInATG(1, '\n')
        
        
stringToWrite = '\n'
items_types_lines.InsertLineInATG(4, stringToWrite)
stringToWrite = 'UNCHAIN_TOTAL\n'
items_types_lines.InsertLineInATG(4, stringToWrite)
    




print('\n\n------ITEMS (Custom GOB3) -------\n\n')


items_types_lines.InsertLineInATG(2, 'ITEM_PICK_NONE = -1,\n')

zone = 1
linecount = -1
for line in ITEMSinputFile:
    ItemVar = {}
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    line = line.replace('"','')
    
    line = line.replace(', ','|')
    columns : list[str] = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    ItemVar["name"] = columns[1]
    ItemVar["res_name"] = columns[2]
    ItemVar["family"] = columns[3]
    ItemVar["sprites"] = columns[4]
    ItemVar["defaultSprite"] = columns[5]
    ItemVar["pickable"] = 'true' in columns[6].lower()
    ItemVar["pickablesprite"] = columns[7]
    ItemVar["detailPrefab"] = columns[8]
    ItemVar["conditions"] = columns[9]
    
    # Write in item enum
    stringToWrite = ItemVar["name"]
    if('NONE' in ItemVar["name"]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    
    items_types_lines.InsertLineInATG(1, stringToWrite)
    
    
    if('NONE' not in ItemVar["name"]):
        if(ItemVar["pickable"]):
            pickname = ItemVar["name"].replace('ITEM_', 'ITEM_PICK_')
            stringToWrite = pickname
            stringToWrite += ', \n'
            items_types_lines.InsertLineInATG(2, stringToWrite)
            
            stringToWrite = item_prefix + ItemVar["name"] + ',\t/* '+ pickname +' */\n'
            items_interaction_lines.InsertLineInATG(4, stringToWrite)
            
            stringToWrite = sprite_prefix + ItemVar["pickablesprite"] + ',\t/* '+ pickname +' */\n'
            items_interaction_lines.InsertLineInATG(5, stringToWrite)
        else:
            pickname = 'ITEM_PICK_NONE'

                
        stringToWrite = 'new ( /* '+ItemVar["name"] + ' */\n'
        items_interaction_lines.InsertLineInATG(3, stringToWrite)
        
        stringToWrite = name_prefix+ItemVar["res_name"]+','+\
            family_prefix+ItemVar["family"]+','+'new(new HashSet<GameSprite>('
        options = ItemVar["sprites"].split('|')
        num_options = len(options)
        stringToWrite += str(num_options)+'){'
        
        for _option in options:
            stringToWrite += sprite_prefix+_option+','
        stringToWrite += '}),\n'
        items_interaction_lines.InsertLineInATG(3, stringToWrite)
        
        stringToWrite = sprite_prefix + ItemVar["defaultSprite"] + ','
        stringToWrite += str(ItemVar["pickable"]).lower()+','+\
            sprite_prefix+ItemVar["pickablesprite"]+','+\
            pickable_prefix+pickname+','+\
            detail_prefix + ItemVar["detailPrefab"]+',\n'
        items_interaction_lines.InsertLineInATG(3, stringToWrite)
        
        stringToWrite = 'new(new HashSet<ActionConditions>('
        options = ItemVar["conditions"].split('|')
        num_options = len(options)
        stringToWrite += str(num_options)+'){'
        
        for _option in options:
            stringToWrite += conditiontype_prefix+_option+','
        stringToWrite += '})),\n'
        items_interaction_lines.InsertLineInATG(3, stringToWrite)
        items_interaction_lines.InsertLineInATG(3, '\n')
        
        

    
    
    
    
items_types_lines.InsertLineInATG(1, "\n")
items_types_lines.InsertLineInATG(1, "ITEM_TOTAL\n")
items_types_lines.InsertLineInATG(2, "\n")
items_types_lines.InsertLineInATG(2, "ITEM_PICK_TOTAL\n")


print('\n\n------NAMES (Custom GOB3) -------\n\n')
linecount = -1
zone = 1

for line in NAMESinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    
    
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    names_types_lines.InsertLineInATG(1, stringToWrite)
    
    if('NONE' not in columns[1]):
        stringToWrite = columns[2] + ',' + columns[3] + '\n'
        names_lines.InsertLineInATG(0, stringToWrite)
    


    
stringToWrite = '\n'
names_types_lines.InsertLineInATG(1, stringToWrite)

stringToWrite = 'NAME_TOTAL\n'
names_types_lines.InsertLineInATG(1, stringToWrite)




print('\n\n------SPRITES (Custom GOB3) -------\n\n')
linecount = -1
zone = 1

for line in SPRITESinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    
    
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    sprite_types_lines.InsertLineInATG(1, stringToWrite)
    
    if(not 'NONE' in columns[1]):        
        stringToWrite = 'new("'+columns[2]+'"), /* '+columns[1]+' */ \n'
        sprite_atlas_lines.InsertLineInATG(1, stringToWrite)

    
stringToWrite = '\n'
sprite_types_lines.InsertLineInATG(1, stringToWrite)
stringToWrite = 'SPRITE_TOTAL\n'
sprite_types_lines.InsertLineInATG(1, stringToWrite)


print('\n\n------EVENTS (Custom GOB3) -------\n\n')
linecount = -1
zone = 1

for line in EVENTSinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    
    
    columns = line.split(',')
    print(columns)
    
    if(linecount == 0):
        continue

    if(columns[0] == ''):
        if(zone == 1):
            stringToWrite = '\n'
            event_types_lines.InsertLineInATG(1, stringToWrite)
            stringToWrite = 'EVENT_TOTAL\n'
            event_types_lines.InsertLineInATG(1, stringToWrite)
        elif(zone == 2):
            stringToWrite = '\n'
            event_types_lines.InsertLineInATG(2, stringToWrite)
            stringToWrite = 'MEMENTO_PARENT_TOTAL\n'
            event_types_lines.InsertLineInATG(2, stringToWrite)
        elif(zone == 3):
            stringToWrite = '\n'
            event_types_lines.InsertLineInATG(3, stringToWrite)
            stringToWrite = 'MEMENTO_TOTAL\n'
            event_types_lines.InsertLineInATG(3, stringToWrite)
        elif(zone == 4):
            break
            
        zone += 1
        linecount = -1
        continue
    
    
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    event_types_lines.InsertLineInATG(zone, stringToWrite)
    
    if('NONE' not in columns[1]):
        if(zone == 2):
            stringToWrite = '/* ' + columns[1] + ' */\n'
            items_interaction_lines.InsertLineInATG(6, stringToWrite)
            
            stringToWrite = 'new(\n'
            items_interaction_lines.InsertLineInATG(6, stringToWrite)
            
            stringToWrite = name_prefix + columns[2] + ',' 
            stringToWrite += sprite_prefix + columns[3] + ',\n' 
            items_interaction_lines.InsertLineInATG(6, stringToWrite)
            
            
            stringToWrite = 'new Memento['
            options = columns[4].split('|')
            num_options = len(options)
            stringToWrite += str(num_options)+']{'
            
            for _option in options:
                stringToWrite += memento_prefix + _option+','
            stringToWrite += '}\n'
            items_interaction_lines.InsertLineInATG(6, stringToWrite)
            
            items_interaction_lines.InsertLineInATG(6, '),\n')
            items_interaction_lines.InsertLineInATG(6, '\n')
        elif(zone == 3):
            stringToWrite = '/* ' + columns[1] + ' */\n'
            items_interaction_lines.InsertLineInATG(7, stringToWrite)
            
            stringToWrite = 'new('+memento_parent_prefix+columns[2]+','+\
                phrase_prefix+columns[3]+',\n'
            items_interaction_lines.InsertLineInATG(7, stringToWrite)
            
            stringToWrite = 'new(new HashSet<MementoCombi>('
            options = columns[4].split('|')
            num_options = len(options)
            stringToWrite += str(num_options)+'){'
            
            for _option in options:
                stringToWrite += memento_combi_prefix + _option+','
            stringToWrite += '}),\n'
            items_interaction_lines.InsertLineInATG(7, stringToWrite)
                
                
            stringToWrite = columns[5].lower()+'),\n'
            items_interaction_lines.InsertLineInATG(7, stringToWrite)
        elif(zone == 4):
            stringToWrite = '/* ' + columns[1] + ' */\n'
            items_interaction_lines.InsertLineInATG(8, stringToWrite)
            
            stringToWrite = 'new(\n'
            items_interaction_lines.InsertLineInATG(8, stringToWrite)
            
            stringToWrite = event_prefix + columns[2] + '\n'
            items_interaction_lines.InsertLineInATG(8, stringToWrite)
            items_interaction_lines.InsertLineInATG(8, '),\n')

    
stringToWrite = '\n'
event_types_lines.InsertLineInATG(4, stringToWrite)
stringToWrite = 'MEMENTO_COMBI_TOTAL\n'
event_types_lines.InsertLineInATG(4, stringToWrite)


print('\n\n------DETAILS (Custom GOB3) -------\n\n')
linecount = -1
zone = 1

for line in DETAILSinputFile:
    linecount += 1
    
    line = line.replace('\n','')
    line = line.replace('\r','')
    line = line.replace(', ','|')
    
    
    columns = line.split(',')
    print(columns)

    if(linecount == 0):
        continue
    
    
    stringToWrite = columns[1]
    if('NONE' in columns[1]):
        stringToWrite += ' = -1'
    stringToWrite += ', \n'
    items_types_lines.InsertLineInATG(5, stringToWrite)
    
    if(not 'NONE' in columns[1]):        
        stringToWrite = 'new('+columns[2]+', /* '+columns[1]+' */ \n'
        items_interaction_lines.InsertLineInATG(9, stringToWrite)
        
        stringToWrite = 'new(new HashSet<NameType>('
        
        options = columns[3].split('|')
        num_options = len(options)
        stringToWrite += str(num_options)+'){'
        
        for _option in options:
            stringToWrite += name_prefix+_option+','
        stringToWrite += '}),\n'
        items_interaction_lines.InsertLineInATG(9, stringToWrite)
        
        stringToWrite = 'new(new HashSet<DialogPhrase>('
        
        options = columns[4].split('|')
        num_options = len(options)
        stringToWrite += str(num_options)+'){'
        
        for _option in options:
            stringToWrite += phrase_prefix+_option+','
        stringToWrite += '})),\n\n'
        items_interaction_lines.InsertLineInATG(9, stringToWrite)

    
stringToWrite = '\n'
items_types_lines.InsertLineInATG(5, stringToWrite)
stringToWrite = 'DETAIL_TOTAL\n'
items_types_lines.InsertLineInATG(5, stringToWrite)
    


# Close files
VARMAPinputFile.close()
SERVICESinputFile.close()
ITEMSinputFile.close()
ACTIONCONDSinputFile.close()
AUTOinputFile.close()
DIALOGSinputFile.close()
PHRASESinputFile.close()
ROOMSinputFile.close()
NAMESinputFile.close()
SPRITESinputFile.close()




print('\n\n------ SAVE ATG FILES -------\n\n')

#VARMAP Individual modules
for i in range(0,len(Modulelines)):
    Modulelines[i].SaveFile()

#VARMAP special classes
proto_lines.SaveFile()
initialization_lines.SaveFile()
defaultvalues_lines.SaveFile()
enum_lines.SaveFile()
delegateupdate_lines.SaveFile()
savedata_lines.SaveFile()

#Custom classes
auto_lines.SaveFile()
dialogs_types_lines.SaveFile()
decisions_types_lines.SaveFile()
phrases_lines.SaveFile()
names_lines.SaveFile()
rooms_types_lines.SaveFile()
modules_types_lines.SaveFile()
items_types_lines.SaveFile()
items_interaction_lines.SaveFile()
dialog_atlas_lines.SaveFile()
decision_atlas_lines.SaveFile()
names_types_lines.SaveFile()
sprite_types_lines.SaveFile()
event_types_lines.SaveFile()
sprite_atlas_lines.SaveFile()
room_atlas_lines.SaveFile()

