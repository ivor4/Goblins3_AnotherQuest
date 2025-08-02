# -*- coding: utf-8 -*-
"""
Spyder Editor

This is not a temporary script file.
"""

#Global part
atg_path = "./../Gob3AQ/Assets/Resources/Scripts/VARMAP/"
proto_path = atg_path+"VARMAP.cs"
initialization_path = atg_path+"VARMAP_datasystem.cs"
defaultvalues_path = atg_path+"VARMAP_defaultvalues.cs"
enum_path = atg_path+"VARMAP_enum.cs"
delegateupdate_path = atg_path+"VARMAP_UpdateDelegates.cs"
savedata_path = atg_path+"VARMAP_savedata.cs"

#This is Custom part
items_types_path = atg_path + "VARMAP_types_items.cs"
items_interaction_path = atg_path + "../Static/ItemsInteractionsClass.cs"

MODULES_START_COLUMN = 8
SERVICE_MODULES_START_COLUMN = 6

#ATG Class Definition
class ATGFile:
    def __init__(self, pathString, maxATGZones):
        self.path = pathString
        self.maxATGZones = maxATGZones
        self.ATGWritingLine = []
        self.ATGIndent = []

        readfile = open(self.path)
        self.filelines = readfile.readlines()
        readfile.close()
        
        self.DetectAndClearATGZones()

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
        self.filelines.insert(self.ATGWritingLine[atg_index-1], self.ATGIndent[atg_index-1] + line_str)
        for i in range(atg_index-1, self.maxATGZones):
            self.ATGWritingLine[i] += 1

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
items_types_lines = ATGFile(items_types_path, 2)
items_interaction_lines = ATGFile(items_interaction_path, 4)

added_savedata_lines = 0



enumName = "VARMAP_Variable_ID"
enumPrefix = "VARMAP_ID_"

VARMAPinputFile = open("VARMAP.csv", "r")
SERVICESinputFile = open("SERVICES.csv", "r")
ITEMSinputFile = open("ITEMS.csv", "r")


VARMAPPermissionFile = []

#ENUM PRE-VALUES
enum_lines.InsertLineInATG(1, "VARMAP_ID_NONE,\n")

#INITIALIZATION PRE-VALUES
initialization_lines.InsertLineInATG(1, "DATA[(int)VARMAP_Variable_ID.VARMAP_ID_NONE] = null;\n")


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
        stringToWrite += "("+enumstring+", "+arrayString+safetyString+VARMAPVar["type"]+".StaticParseFromBytes, "+VARMAPVar["type"]+".StaticParseToBytes, "+"null"
    else:
        stringToWrite += "("+enumstring+", "+arrayString+safetyString+"VARMAP_parsers."+VARMAPVar["type"]+"_ParseFromBytes, "+"VARMAP_parsers."+VARMAPVar["type"]+"_ParseToBytes, "+"null"
    
        

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
        
        stringToWrite = "protected static SetVARMAPValueDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _SET_"+VARMAPVar["name"]+";\n"

        proto_lines.InsertLineInATG(1, stringToWrite)
    
    else:
        stringToWrite = "protected static GetVARMAPArrayElemValueDelegate<"
        stringToWrite += VARMAPVar["type"]+"> _GET_ELEM_"+VARMAPVar["name"]+";\n"
        
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
        
        stringToWrite = "_SET_"+VARMAPVar["name"]+" = " + variableinarray + ".SetValue;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
    else:
        stringToWrite = "_GET_ELEM_"+VARMAPVar["name"]+" = " + variableinarray + ".GetListElem;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_SET_ELEM_"+VARMAPVar["name"]+" = " + variableinarray + ".SetListElem;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_GET_SIZE_"+VARMAPVar["name"]+" = " + variableinarray + ".GetListSize;\n"
        
        delegateupdate_lines.InsertLineInATG(1, stringToWrite)
        
        stringToWrite = "_GET_ARRAY_"+VARMAPVar["name"]+" = " + variableinarray + ".GetListCopy;\n"
        
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
        

if(False):
    print('\n\n------ITEMS (Custom GOB3) -------\n\n')
    
    CHARACTERS = ['MAIN','PARROT','SNAKE']
    
    totalcharacters = len(CHARACTERS)
    
    pickable_prefix = 'GamePickableItem.'
    interaction_prefix = 'ItemInteractionType.'
    animation_prefix = 'CharacterAnimation.'
    event_prefix = 'GameEvent.'
    
    items_types_lines.InsertLineInATG(1, "ITEM_NONE = -1,\n\n")
    items_types_lines.InsertLineInATG(2, "ITEM_PICK_NONE = -1,\n\n")
    
    linecount = -1
    Items = []
    for line in ITEMSinputFile:
        ItemVar = {}
        linecount += 1
        
        line = line.replace('\n','')
        line = line.replace('\r','')
        
        columns = line.split(',')
        print(columns)
    
        if(linecount == 0):
            continue
        
        ItemVar["name"] = columns[1]
        ItemVar["pickable"] = int(columns[2])
        ItemVar["disposable"] = int(columns[3])
        
        if(ItemVar["name"] == 'ITEM_LAST'):
            continue
        
        ItemVar["c2i"] = []
        for ch in range(0,totalcharacters):
            c2iVar = {}
            c2iVar["use"] = columns[5+(ch*3)]
            c2iVar["anim"] = columns[5+(ch*3)+1]
            c2iVar["event"] = columns[5+(ch*3)+2]
            ItemVar["c2i"].append(c2iVar)
        
    
        ItemVar["i2i_matrix"] = columns[(6 + totalcharacters*3):]
        
        items_types_lines.InsertLineInATG(1, ItemVar["name"]+",\n")
        
        if(ItemVar["pickable"] == 1):
            pickablename = ItemVar["name"].replace('ITEM_','ITEM_PICK_')
            items_types_lines.InsertLineInATG(2, pickablename +",\n")
            items_interaction_lines.InsertLineInATG(1, pickable_prefix + pickablename + ',\t\t/* ' + ItemVar["name"] + ' */ \n')
            
            if(ItemVar["disposable"] == 1):
                boolstr = 'true'
            else:
                boolstr = 'false'
            
            items_interaction_lines.InsertLineInATG(2, boolstr + ',\t\t/* ' + pickablename + ' */ \n')
            
        else:
            items_interaction_lines.InsertLineInATG(1, pickable_prefix + 'ITEM_PICK_NONE' + ',\t\t/* ' + ItemVar["name"] + ' */ \n')
        
        Items.append(ItemVar)
        
    # Now total items are known, matrix can be explored
    totalitems = len(Items)
    
    #Character to Item (c2i matrix)
    for ch in range(0,totalcharacters):    
        items_interaction_lines.InsertLineInATG(3, '/* CHARACTER_' + CHARACTERS[ch] + ' */\n')
        items_interaction_lines.InsertLineInATG(3, '{\n')
        
        #Iterate through items
        for it in range(0, totalitems):
            #Player with item interaction (c2i)
            c2iVar = Items[it]["c2i"][ch]
                    
            items_interaction_lines.InsertLineInATG(3, '\tnew(' + interaction_prefix + c2iVar["use"] + ', ' + animation_prefix + c2iVar["anim"] + ',\n')
            items_interaction_lines.InsertLineInATG(3, '\t' + event_prefix + c2iVar["event"] + '),\t/* ' + Items[it]["name"] + ' */ \n')
            
        items_interaction_lines.InsertLineInATG(3, '},\n')
    
    #Item to Item (i2i matrix)
    for it_src in range(0, totalitems):
        ItemVar = Items[it_src]
        i2iVar = ItemVar["i2i_matrix"]
        
        items_interaction_lines.InsertLineInATG(4, '/* ' + ItemVar["name"] + ' */\n')
        items_interaction_lines.InsertLineInATG(4, '{\n')
        
        for it_dst in range(0, totalitems):      
            items_interaction_lines.InsertLineInATG(4, '\tnew(' + interaction_prefix + i2iVar[it_dst*3] + ', ' + 
                    animation_prefix + i2iVar[it_dst*3 + 1] + ',\n')
            items_interaction_lines.InsertLineInATG(4, '\t' + event_prefix + i2iVar[it_dst*3 + 2] + '),\t/* ' + Items[it_dst]["name"] + ' */\n')
        
        items_interaction_lines.InsertLineInATG(4, '},\n')
        
        
        
    items_types_lines.InsertLineInATG(1, "\n")
    items_types_lines.InsertLineInATG(1, "ITEM_TOTAL\n")
    items_types_lines.InsertLineInATG(2, "\n")
    items_types_lines.InsertLineInATG(2, "ITEM_PICK_TOTAL\n")
    

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
items_types_lines.SaveFile()
items_interaction_lines.SaveFile()

