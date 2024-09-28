# -*- coding: utf-8 -*-
"""
Spyder Editor

This is not a temporary script file.
"""
atg_path = "./../Assets/Scripts/VARMAP/"
proto_path = atg_path+"VARMAP.cs"
initialization_path = atg_path+"VARMAP_datasystem.cs"
defaultvalues_path = atg_path+"VARMAP_defaultvalues.cs"
enum_path = atg_path+"VARMAP_enum.cs"
delegateupdate_path = atg_path+"VARMAP_UpdateDelegates.cs"
savedata_path = atg_path+"VARMAP_savedata.cs"

MODULES_START_COLUMN = 8

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

added_savedata_lines = 0



enumName = "VARMAP_Variable_ID"
enumPrefix = "VARMAP_ID_"

VARMAPinputFile = open("VARMAP.csv", "r")
SERVICESinputFile = open("SERVICES.csv", "r")


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

ServiceVars = []
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
    ServiceVar["writers"] = 0

    if(ServiceVar["delegate"] == ''):
        ServiceVar["delegate"] = ServiceVar["name"]+'_DELEGATE'

    #PROTO FILE
    stringToWrite = "protected static "+ServiceVar["delegate"]
    stringToWrite += " _"+ServiceVar["name"]+";\n"
    proto_lines.InsertLineInATG(2, stringToWrite)

    #UPDATE_DELEGATE FILE
    stringToWrite = "_"+ServiceVar["name"]+" = " + ServiceVar["route"]+";\n"    
    delegateupdate_lines.InsertLineInATG(2, stringToWrite)

    for i in range(5,len(columns)):
        indextouse = i-5
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
            stringToWrite = "public static "+ServiceVar["delegate"]
            stringToWrite += " "+ServiceVar["name"]+";\n"
            Modulelines[indextouse].InsertLineInATG(3, stringToWrite)
            
            stringToWrite = ServiceVar["name"] + " = _"+ServiceVar["name"]+";\n"
            Modulelines[indextouse].InsertLineInATG(1, stringToWrite)
    if(ServiceVar["writers"] == 0):
        print("Service "+str(linecount)+" has no writer")
        exit()
        


for i in range(0,len(Modulelines)):
    Modulelines[i].SaveFile()

proto_lines.SaveFile()
initialization_lines.SaveFile()
defaultvalues_lines.SaveFile()
enum_lines.SaveFile()
delegateupdate_lines.SaveFile()
savedata_lines.SaveFile()

