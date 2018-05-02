using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class WrapTextTools  {

    public static void WriteAllText (string folder, string name, string content) {
        if(!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        File.WriteAllText(folder + "/" + name, content, System.Text.Encoding.UTF8);
    }

    public static string Type2String (Type type) {
        string retType = type.ToString();
        retType = retType.Replace('+', '.');//A+Enum实际上是A.Enum
        retType = retType.Replace("System.Boolean", "bool");
        retType = retType.Replace("System.Byte", "byte");
        retType = retType.Replace("System.Char", "char");
        retType = retType.Replace("System.Double", "double");
        retType = retType.Replace("System.Int16", "short");
        retType = retType.Replace("System.Int32", "int");
        retType = retType.Replace("System.Int64", "long");
        retType = retType.Replace("System.Object", "object");
        retType = retType.Replace("System.Single", "float");
        retType = retType.Replace("System.String", "string");
        retType = retType.Replace("System.UInt16", "ushort");
        retType = retType.Replace("System.UInt32", "uint");
        retType = retType.Replace("System.UInt64", "ulong");
        retType = retType.Replace("System.Void", "void");
        return retType;
    }

    static string GetHashCodeByTypes (string prefix, string[] types) {
        string hashkey = "";
        foreach(string s in types) {
            hashkey += s;
        }
        //返回类似 UnityEngineTransform75CC8236 这样的值
        return prefix.Replace(".","") + hashkey.GetHashCode().ToString("x8");
    }
    public static string[] Propertys2PartStr (string classFullName, List<Property> propertys) {
        string wrapSVGet = "";
        string wrapSVSet = "";
        string wrapMVGet = "";
        string wrapMVSet = "";

        for(int i = 0; i < propertys.Count; i++) {
            if(propertys[i].m_obsolete && WrapMakerGUI.m_ignoreObsolete)
                continue;
            if(propertys[i].m_name == "value__")//枚举长度
                continue;
            if(!Finish(propertys[i].m_type))
                continue;

            if(propertys[i].m_isStatic) {
                if(propertys[i].m_canGet) {
                    wrapSVGet += "\t\t\tcase \"" + propertys[i].m_name + "\":\n";
                    wrapSVGet += "\t\t\t\treturnValue = new CQ_Value();\n";
                    if(IsNumberType(propertys[i].m_type))
                        wrapSVGet += "\t\t\t\treturnValue.SetNumber(typeof(" + propertys[i].m_type + "), ";
                    else if(IsBoolType(propertys[i].m_type))
                        wrapSVGet += "\t\t\t\treturnValue.SetBool(";
                    else
                        wrapSVGet += "\t\t\t\treturnValue.SetObject(typeof(" + propertys[i].m_type + "), ";
                    wrapSVGet += classFullName + "." + propertys[i].m_name + ");\n";
                    wrapSVGet += "\t\t\t\treturn true;\n";
                }
                if(propertys[i].m_canSet) {
                    wrapSVSet += "\t\t\tcase \"" + propertys[i].m_name + "\":\n";
                    wrapSVSet += "\t\t\t\tif(param.EqualOrImplicateType(typeof(" + propertys[i].m_type + "))){\n";
                    wrapSVSet += "\t\t\t\t\t" + classFullName + "." + propertys[i].m_name + " = " + ConvertStr("param", propertys[i].m_type) + ";\n";
                    wrapSVSet += "\t\t\t\t\treturn true;\n";
                    wrapSVSet += "\t\t\t\t}\n";
                    wrapSVSet += "\t\t\t\tbreak;\n";
                }
            }
            else {
                if(propertys[i].m_canGet) {
                    wrapMVGet += "\t\t\tcase \"" + propertys[i].m_name + "\":\n";
                    wrapMVGet += "\t\t\t\treturnValue = new CQ_Value();\n";
                    if(IsNumberType(propertys[i].m_type))
                        wrapMVGet += "\t\t\t\treturnValue.SetNumber(typeof(" + propertys[i].m_type + "), ";
                    else if(IsBoolType(propertys[i].m_type))
                        wrapMVGet += "\t\t\t\treturnValue.SetBool(";
                    else
                        wrapMVGet += "\t\t\t\treturnValue.SetObject(typeof(" + propertys[i].m_type + "), ";
                    wrapMVGet += "obj." + propertys[i].m_name + ");\n";
                    wrapMVGet += "\t\t\t\treturn true;\n";
                }
                if(propertys[i].m_canSet) {
                    wrapMVSet += "\t\t\tcase \"" + propertys[i].m_name + "\":\n";
                    wrapMVSet += "\t\t\t\tif(param.EqualOrImplicateType(typeof(" + propertys[i].m_type + "))){\n";
                    wrapMVSet += "\t\t\t\t\tobj." + propertys[i].m_name + " = " + ConvertStr("param", propertys[i].m_type) + ";\n";
                    wrapMVSet += "\t\t\t\t\treturn true;\n";
                    wrapMVSet += "\t\t\t\t}\n";
                    wrapMVSet += "\t\t\t\tbreak;\n";
                }
            }
        }

        if(!string.IsNullOrEmpty(wrapSVGet)) {
            wrapSVGet = "\t\t\tswitch(memberName) {\n"
                + wrapSVGet + "\t\t\t}";
        }
        if(!string.IsNullOrEmpty(wrapSVSet)) {
            wrapSVSet = "\t\t\tswitch(memberName) {\n"
                + wrapSVSet + "\t\t\t}";
        }
        if(!string.IsNullOrEmpty(wrapMVGet)) {
            wrapMVGet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
                + "\t\t\tswitch(memberName) {\n"
                + wrapMVGet + "\t\t\t}";
        }
        if(!string.IsNullOrEmpty(wrapMVSet)) {
            wrapMVSet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
                + "\t\t\tswitch(memberName) {\n"
                + wrapMVSet + "\t\t\t}";
        }
        return new string[] { wrapSVGet, wrapSVSet, wrapMVGet, wrapMVSet };
    }
    public static string Constructor2PartStr (string classFullName, List<Method> constructor) {
        string wrapNew = "";
        for(int i = 0; i < constructor.Count; i++) {
            if(!Finish(constructor[i].m_returnType) || !Finish(constructor[i].m_inType))
                continue;

            if(constructor[i].m_obsolete && WrapMakerGUI.m_ignoreObsolete)
                continue;

            if(constructor[i].m_inType.Length == 0)
                wrapNew += "\t\t\tif(param.Length == 0){\n";
            else {
                string typehash = GetHashCodeByTypes(classFullName, constructor[i].m_inType);
                wrapNew += "\t\t\tif(param.Length == " + constructor[i].m_inType.Length + " && MatchType(param, " + typehash + ", mustEqual)){\n";
            }
            wrapNew += "\t\t\t\treturnValue = new CQ_Value();\n";
            wrapNew += "\t\t\t\treturnValue.SetObject(typeof(" + classFullName + "), new " + classFullName + "(";
            for(int j = 0; j < constructor[i].m_inType.Length; j++) {
                wrapNew += ConvertStr("param[" + j + "]", constructor[i].m_inType[j]);
                if(j != constructor[i].m_inType.Length - 1)
                    wrapNew += ",";
            }
            wrapNew += "));\n";
            wrapNew += "\t\t\t\treturn true;\n";
            wrapNew += "\t\t\t}\n";
        }
        return wrapNew;
    }
    public static string SCall2PartStr (string classFullName, List<Method> staticMethods) {
        string wrapSCall = "";
        for(int i = 0; i < staticMethods.Count; i++) {
            if(staticMethods[i].m_obsolete && WrapMakerGUI.m_ignoreObsolete)
                continue;
            if(!Finish(staticMethods[i].m_returnType) || !Finish(staticMethods[i].m_inType))
                continue;

            if(staticMethods[i].m_inType.Length == 0)
                wrapSCall += "\t\t\tif(paramCount == 0 && functionName == \"" + staticMethods[i].m_methodName + "\"){\n";
            else {
                string typehash = GetHashCodeByTypes(classFullName, staticMethods[i].m_inType);
                wrapSCall += "\t\t\tif(paramCount == " + staticMethods[i].m_inType.Length + " && functionName == \"" + staticMethods[i].m_methodName + "\" && MatchType(param, " + typehash + ", mustEqual)){\n";
            }
            string v = "";
            v += classFullName + "." + staticMethods[i].m_methodName + "(";
            for(int j = 0; j < staticMethods[i].m_inType.Length; j++) {
                v += ConvertStr("param[" + j + "]", staticMethods[i].m_inType[j]);
                if(j != staticMethods[i].m_inType.Length - 1)
                    v += ",";
            }
            v += ")";

            if(staticMethods[i].m_returnType == "void") {
                wrapSCall += "\t\t\t\treturnValue = CQ_Value.Null;\n";
                wrapSCall += v + ";\n";
            }
            else {
                wrapSCall += "\t\t\t\treturnValue = new CQ_Value();\n";
                if(IsNumberType(staticMethods[i].m_returnType))
                    wrapSCall += "\t\t\t\treturnValue.SetNumber(typeof(" + staticMethods[i].m_returnType + "), " + v + ");\n";
                else if(IsBoolType(staticMethods[i].m_returnType))
                    wrapSCall += "\t\t\t\treturnValue.SetBool(" + v + ");\n";
                else
                    wrapSCall += "\t\t\t\treturnValue.SetObject(typeof(" + staticMethods[i].m_returnType + "), " + v + ");\n";
            }

            wrapSCall += "\t\t\t\treturn true;\n";
            wrapSCall += "\t\t\t}\n";
        }
        if(!string.IsNullOrEmpty(wrapSCall))
            wrapSCall = "\t\t\tint paramCount = param.Length;\n" + wrapSCall;
        return wrapSCall;
    }
    public static string MCall2PartStr (string classFullName, List<Method> instanceMethods) {
        string wrapMCall = "";
        for(int i = 0; i < instanceMethods.Count; i++) {
            if(instanceMethods[i].m_obsolete && WrapMakerGUI.m_ignoreObsolete)
                continue;
            if(!Finish(instanceMethods[i].m_returnType) || !Finish(instanceMethods[i].m_inType))
                continue;

            if(instanceMethods[i].m_inType.Length == 0)
                wrapMCall += "\t\t\tif(paramCount == 0 && functionName == \"" + instanceMethods[i].m_methodName + "\"){\n";
            else {
                string typehash = GetHashCodeByTypes(classFullName, instanceMethods[i].m_inType);
                wrapMCall += "\t\t\tif(paramCount == " + instanceMethods[i].m_inType.Length + " && functionName == \"" + instanceMethods[i].m_methodName + "\" && MatchType(param, " + typehash + ", mustEqual)){\n";
            }

            string v = "";
            v += "obj." + instanceMethods[i].m_methodName + "(";
            for(int j = 0; j < instanceMethods[i].m_inType.Length; j++) {
                v += ConvertStr("param[" + j + "]", instanceMethods[i].m_inType[j]);
                if(j != instanceMethods[i].m_inType.Length - 1)
                    v += ",";
            }
            v += ")";

            if(instanceMethods[i].m_returnType == "void") {
                wrapMCall += "\t\t\t\treturnValue = CQ_Value.Null;\n";
                wrapMCall += "\t\t\t\t" + v + ";\n";
            }
            else {
                wrapMCall += "\t\t\t\treturnValue = new CQ_Value();\n";
                if(IsNumberType(instanceMethods[i].m_returnType))
                    wrapMCall += "\t\t\t\treturnValue.SetNumber(typeof(" + instanceMethods[i].m_returnType + "), " + v + ");\n";
                else if(IsBoolType(instanceMethods[i].m_returnType))
                    wrapMCall += "\t\t\t\treturnValue.SetBool(" + v + ");\n";
                else
                    wrapMCall += "\t\t\t\treturnValue.SetObject(typeof(" + instanceMethods[i].m_returnType + "), " + v + ");\n";
            }

            wrapMCall += "\t\t\t\treturn true;\n";
            wrapMCall += "\t\t\t}\n";
        }
        if(!string.IsNullOrEmpty(wrapMCall)) {
            wrapMCall = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
                + "\t\t\tint paramCount = param.Length;\n"
                + wrapMCall;
        }
        return wrapMCall;
    }
    public static void CallTypes2TypeStr (string prefix, List<Method> methods, List<string> typeDic) {
        for(int i = 0; i < methods.Count; i++) {
            if(methods[i].m_inType.Length == 0)
                continue;

            if(!Finish(methods[i].m_inType))
                continue;

            string hashCode = GetHashCodeByTypes(prefix, methods[i].m_inType);
            string text = "static Type[] " + hashCode + " = new Type[]{";
            for(int j = 0; j < methods[i].m_inType.Length; j++) {
                text += "typeof(" + methods[i].m_inType[j] + ")";
                if(j != methods[i].m_inType.Length - 1)
                    text += ", ";
            }
            text += "};";
            if(!typeDic.Contains(text)) {
                typeDic.Add(text);
            }
        }
    }

    public static string[] Index2PartStr (string classFullName, List<Method> indexMethods) {
        string wrapIGet = "";
        string wrapISet = "";
        for(int i = 0; i < indexMethods.Count; i++) {
            if(indexMethods[i].m_obsolete && WrapMakerGUI.m_ignoreObsolete)
                continue;
            if(!Finish(indexMethods[i].m_returnType) || !Finish(indexMethods[i].m_inType))
                continue;
            //TODO 可能有多位数组this[x,y]
            if(indexMethods[i].m_methodType == "IndexGet") {
                wrapIGet += "\t\t\tif(key.EqualOrImplicateType(typeof(" + indexMethods[i].m_inType[0] + "))){\n";
                wrapIGet += "\t\t\t\treturnValue = new CQ_Value();\n";
                if(IsNumberType(indexMethods[i].m_returnType))
                    wrapIGet += "\t\t\t\treturnValue.SetNumber(typeof(" + indexMethods[i].m_returnType + "), ";
                else if(IsBoolType(indexMethods[i].m_returnType))
                    wrapIGet += "\t\t\t\treturnValue.SetBool(";
                else
                    wrapIGet += "\t\t\t\treturnValue.SetObject(typeof(" + indexMethods[i].m_returnType + "), ";
                wrapIGet += "obj[" + ConvertStr("key", indexMethods[i].m_inType[0]) + "]);\n";
                wrapIGet += "\t\t\t\treturn true;\n";
                wrapIGet += "\t\t\t}";
            }
            else if(indexMethods[i].m_methodType == "IndexSet") {
                wrapISet += "\t\t\tif(param.EqualOrImplicateType(typeof(" + indexMethods[i].m_inType[0] + "))){\n";
                wrapISet += "\t\t\t\tobj[" + ConvertStr("key", indexMethods[i].m_inType[0]) + "] = ";
                wrapISet += ConvertStr("param", indexMethods[i].m_inType[indexMethods[i].m_inType.Length - 1]) + ";\n";
                wrapISet += "\t\t\t\treturn true;\n";
                wrapISet += "\t\t\t}";
            }
        }
        if(!string.IsNullOrEmpty(wrapIGet)) {
            wrapIGet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
                + wrapIGet;
        }
        if(!string.IsNullOrEmpty(wrapISet)) {
            wrapISet = "\t\t\t" + classFullName + " obj = (" + classFullName + ")objSelf;\n"
                + wrapISet;
        }
        return new string[] { wrapIGet, wrapISet };
    }
    public static string[] Op2PartStr (List<Method> opMethods) {
        string wrapAdd = "";
        string wrapSub = "";
        string wrapMul = "";
        string wrapDiv = "";
        string wrapMod = "";

        string wrapGreater = "";
        string wrapLess = "";
        string wrapEGreater = "";
        string wrapELess = "";
        string wrapEqual = "";
        string wrapInequal = "";

        string wrapImplicit = "";
        string wrapExplicit = "";
        string wrapNegation = "";

        for(int i = 0; i < opMethods.Count; i++) {
            if(opMethods[i].m_obsolete && WrapMakerGUI.m_ignoreObsolete)
                continue;

            if(opMethods[i].m_methodName == "op_Addition") {
                wrapAdd += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.SetObject(typeof(" + opMethods[i].m_returnType + "), " + ConvertStr("left", opMethods[i].m_inType[0]) + " + " + ConvertStr("right", opMethods[i].m_inType[1]) + ");\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
            if(opMethods[i].m_methodName == "op_Subtraction") {
                wrapSub += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.SetObject(typeof(" + opMethods[i].m_returnType + "), " + ConvertStr("left", opMethods[i].m_inType[0]) + " - " + ConvertStr("right", opMethods[i].m_inType[1]) + ");\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
            if(opMethods[i].m_methodName == "op_Multiply") {
                wrapMul += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.SetObject(typeof(" + opMethods[i].m_returnType + "), " + ConvertStr("left", opMethods[i].m_inType[0]) + " * " + ConvertStr("right", opMethods[i].m_inType[1]) + ");\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
            if(opMethods[i].m_methodName == "op_Division") {
                wrapDiv += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.SetObject(typeof(" + opMethods[i].m_returnType + "), " + ConvertStr("left", opMethods[i].m_inType[0]) + " / " + ConvertStr("right", opMethods[i].m_inType[1]) + ");\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
            if(opMethods[i].m_methodName == "op_Modulus") {
                wrapMod += "\t\t\tif((mustEqual && left.EqualType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualType(typeof(" + opMethods[i].m_inType[1] + ")))\n"
                + "\t\t\t\t|| (!mustEqual && left.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[0] + ")) && right.EqualOrImplicateType(typeof(" + opMethods[i].m_inType[1] + ")))){\n"
                + "\t\t\t\treturnValue = new CQ_Value();\n"
                + "\t\t\t\treturnValue.SetObject(typeof(" + opMethods[i].m_returnType + "), " + ConvertStr("left", opMethods[i].m_inType[0]) + " % " + ConvertStr("right", opMethods[i].m_inType[1]) + ");\n"
                + "\t\t\t\treturn true;\n"
                + "\t\t\t}\n";
            }
        }

        return new string[]{
            wrapAdd,
            wrapSub,
            wrapMul,
            wrapDiv,
            wrapMod,

            wrapGreater, 
            wrapLess,
            wrapEGreater,
            wrapELess,
            wrapEqual, 
            wrapInequal, 

            wrapImplicit,
            wrapExplicit,
            wrapNegation,
        };
    }

    public static bool IsNumberType (string type) {
        return (type == "double")
            || (type == "float")
            || (type == "long")
            || (type == "ulong")
            || (type == "int")
            || (type == "uint")
            || (type == "short")
            || (type == "ushort")
            || (type == "sbyte")
            || (type == "byte")
            || (type == "char");
    }

    public static bool IsBoolType (string type) {
        return type == "bool";
    }
    public static string ConvertStr (string cqval, string type) {
        if(IsNumberType(type))
            return "(" + type + ")" + cqval + ".GetNumber()";
        else
            return "(" + type + ")" + cqval + ".ConvertTo(typeof(" + type + "))";
    }

    protected static bool Finish (string type) {
        //ref
        //out
        if(string.IsNullOrEmpty(type))
            return false;
        if(type.EndsWith("&"))
            return false;
    
        //List`
        //IEnumerable`
        //System.Action`1
        if(type.Contains("`"))
            return false;
        if(type == "T" || type == "[T]" || type == "T[]")	//T , 比如GetComponent<T>
            return false;
        if(type == "System.Collections.IEnumerator")			//
            return false;
        return true;
    }
    protected static bool Finish (string[] types) {
        for(int j = 0; j < types.Length; j++) {
            if(!Finish(types[j]))
                return false;
        }
        return true;
    }

}
