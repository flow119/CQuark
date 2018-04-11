using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CQuark;

namespace CQuark{
	//参考了ILRuntime，把以前Environment和Content整合到了一起。
	//整个项目只需要一个AppDomain,所以改成了全部静态
    public class AppDomain {
        //CQ_Content contentGloabl = null;
//        static Dictionary<CQ_Type, IType> cqtype2itype = new Dictionary<CQ_Type, IType>();
		static Dictionary<Class_CQuark, IType> ccq2itype = new Dictionary<Class_CQuark, IType> ();
		static Dictionary<Type, IType> type2itype = new Dictionary<Type, IType> ();
        static Dictionary<string, IType> str2itype = new Dictionary<string, IType>();

        public static void Reset () {
            DebugUtil.Log("Reset Domain");
//			cqtype2itype.Clear();
			ccq2itype.Clear ();
			type2itype.Clear ();
			str2itype.Clear();
            RegisterDefaultType();
        }
        public static void RegisterDefaultType () {
            //最好能默认
            RegisterType(new Type_Int());
            RegisterType(new Type_UInt());
            RegisterType(new Type_Float());
            RegisterType(new Type_Double());
            RegisterType(new Type_Byte());
            RegisterType(new Type_Char());
            RegisterType(new Type_UShort());
            RegisterType(new Type_Sbyte());
            RegisterType(new Type_Short());
            RegisterType(new Type_Long());
            RegisterType(new Type_ULong());

            RegisterType(new Type_String());
            RegisterType(new Type_Var());
            RegisterType(new Type_Bool());
            RegisterType(new Type_Lambda());
            RegisterType(new Type_Delegate());
            RegisterType(typeof(IEnumerator), "IEnumerator");

            RegisterType(typeof(object), "object");

            RegisterType(typeof(List<>), "List");	//模板类要独立注册
            RegisterType(typeof(Dictionary<,>), "Dictionary");
            RegisterType(typeof(Stack<>), "Stack");
            RegisterType(typeof(Queue<>), "Queue");

			str2itype["null"] = new Type_NULL();


			RegisterType(typeof(WaitForSeconds),"WaitForSeconds");
			RegisterType(typeof(WaitForEndOfFrame), "WaitForEndOfFrame");
			RegisterType(typeof(WaitForFixedUpdate), "WaitForFixedUpdate");
//			RegisterType(typeof(WaitForSecondsRealtime),"WaitForSecondsRealtime");

            //对于AOT环境，比如IOS，get set不能用RegHelper直接提供，就用AOTExt里面提供的对应类替换
            RegisterType(typeof(int[]), "int[]");	//数组要独立注册
            RegisterType(typeof(string[]), "string[]");
            RegisterType(typeof(float[]), "float[]");
            RegisterType(typeof(bool[]), "bool[]");
            RegisterType(typeof(byte[]), "byte[]");

            RegisterType(typeof(System.DateTime), "DateTime");
            RegisterType(typeof(System.DayOfWeek), "DayOfWeek");
            RegisterType(typeof(System.IO.Directory), "Directory");
            RegisterType(typeof(System.IO.File), "File");
        }

		private static IType MakeIType (Type type, string keyword) {
            if(!type.IsSubclassOf(typeof(Delegate))) {
                return new Type_Numeric(type, keyword, false);
            }
            var method = type.GetMethod("Invoke");
            var pp = method.GetParameters();
            if(method.ReturnType == typeof(void)) {
                if(pp.Length == 0) {
                    return new Type_DeleAction(type, keyword);
                }
                else if(pp.Length == 1) {
                    var gtype = typeof(Type_DeleAction<>).MakeGenericType(new Type[] { pp[0].ParameterType });
                    return gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Numeric;
                }
                else if(pp.Length == 2) {
                    var gtype = typeof(Type_DeleAction<,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType });
                    return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Numeric);
                }
                else if(pp.Length == 3) {
                    var gtype = typeof(Type_DeleAction<,,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
                    return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Numeric);
                }
                else {
                    throw new Exception("还没有支持这么多参数的委托");
                }
            }
            else {
                Type gtype = null;
                if(pp.Length == 0) {
                    gtype = typeof(Type_DeleNonVoidAction<>).MakeGenericType(new Type[] { method.ReturnType });
                }
                else if(pp.Length == 1) {
                    gtype = typeof(Type_DeleNonVoidAction<,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType });
                }
                else if(pp.Length == 2) {
                    gtype = typeof(Type_DeleNonVoidAction<,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType });
                }
                else if(pp.Length == 3) {
                    gtype = typeof(Type_DeleNonVoidAction<,,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
                }
                else {
                    throw new Exception("还没有支持这么多参数的委托");
                }
                return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Numeric);
            }
        }
        public static void RegisterType (Type type, string keyword) {
            RegisterType(MakeIType(type, keyword));
        }
        public static void RegisterType (IType type) {
			if (type.cqType.type != null)
				type2itype [type.cqType.type] = type;
			else if (type.cqType.stype != null)
				ccq2itype [type.cqType.stype] = type;
//			cqtype2itype[type.cqType] = type;

            string typename = type.keyword;
            //if (useNamespace)
            //{

            //    if (string.IsNullOrEmpty(type._namespace) == false)
            //    {
            //        typename = type._namespace + "." + type.keyword;
            //    }
            //}
            if(string.IsNullOrEmpty(typename)) {//匿名自动注册
            }
            else {
				str2itype[typename] = type;
                CQ_TokenParser.AddType(typename);
            }
        }
//        public static IType GetType (CQ_Type type) {
//            if(type == null)
//				return str2itype["null"];
//			
//			IType ret = null;
//			if(cqtype2itype.TryGetValue(type, out ret) == false) {
//                DebugUtil.LogWarning("(CQcript)类型未注册,将自动注册一份匿名:" + type.ToString());
//                ret = MakeIType(type, "");
//                RegisterType(ret);
//            }
//            return ret;
//        }

		public static IType GetITypeByCQType(CQ_Type type){
			if (type.type != null)
				return GetITypeByType (type.type);
			else if (type.stype != null)
				return GetITypeByClassCQ (type.stype);
			return null;
		}
		public static IType GetITypeByClassCQ (Class_CQuark type) {
			if(type == null)
				return str2itype["null"];
			
			IType ret = null;
			if(ccq2itype.TryGetValue(type, out ret) == false) {
				DebugUtil.LogWarning("(Class_CQuark)类型未注册,将自动注册一份匿名:" + type.ToString());
//				ret = MakeIType(type, "");
//				RegisterType(ret);
			}
			return ret;
		}
		public static IType GetITypeByType (Type type) {
			if(type == null)
				return str2itype["null"];
			
			IType ret = null;
			if(type2itype.TryGetValue(type, out ret) == false) {
				DebugUtil.LogWarning("(Type)类型未注册,将自动注册一份匿名:" + type.ToString());
				ret = MakeIType(type, "");
				RegisterType(ret);
			}
			return ret;
		}
		
		public static IType GetTypeByKeyword (string keyword) {
			IType ret = null;
            if(string.IsNullOrEmpty(keyword)) {
                return null;
            }
			if(str2itype.TryGetValue(keyword, out ret) == false) {
                if(keyword[keyword.Length - 1] == '>') {
                    int iis = keyword.IndexOf('<');
                    string func = keyword.Substring(0, iis);
                    List<string> _types = new List<string>();
                    int istart = iis + 1;
                    int inow = istart;
                    int dep = 0;
                    while(inow < keyword.Length) {
                        if(keyword[inow] == '<') {
                            dep++;
                        }
                        if(keyword[inow] == '>') {
                            dep--;
                            if(dep < 0) {
                                _types.Add(keyword.Substring(istart, inow - istart));
                                break;
                            }
                        }

                        if(keyword[inow] == ',' && dep == 0) {
                            _types.Add(keyword.Substring(istart, inow - istart));
                            istart = inow + 1;
                            inow = istart;
                            continue; ;
                        }

                        inow++;
                    }

                    //var funk = keyword.Split(new char[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
					if(str2itype.ContainsKey(func)) {
                        Type gentype = GetTypeByKeyword(func).cqType;
                        if(gentype.IsGenericTypeDefinition) {
                            Type[] types = new Type[_types.Count];
                            for(int i = 0; i < types.Length; i++) {
                                CQ_Type t = GetTypeByKeyword(_types[i]).cqType;
                                Type rt = t;
                                if(rt == null && t != null) {
                                    rt = typeof(object);
                                }
                                types[i] = rt;
                            }
                            Type IType = gentype.MakeGenericType(types);
                            RegisterType(MakeIType(IType, keyword));
                            return GetTypeByKeyword(keyword);
                        }
                    }
                }
                DebugUtil.LogError("(CQcript)类型未注册:" + keyword);
            }

            return ret;
        }
        public static IType GetTypeByKeywordQuiet (string keyword) {
            IType ret = null;
			if(str2itype.TryGetValue(keyword, out ret) == false) {
                return null;
            }
            return ret;
        }

		public static object ConvertTo(object obj, Type targetType){
//			return GetType(obj.GetType()).ConvertTo(obj, targetType);
			return GetITypeByType(obj.GetType()).ConvertTo(obj, targetType);
		}

        private static void Project_Compile (Dictionary<string, IList<Token>> project, bool embDebugToken) {
            foreach(KeyValuePair<string, IList<Token>> f in project) {
                //先把所有代码里的类注册一遍
                IList<IType> types = CQ_Expression_Compiler.FilePreCompile(f.Key, f.Value);
                foreach(var type in types) {
                    RegisterType(type);
                }
            }
            foreach(KeyValuePair<string, IList<Token>> f in project) {
                //预处理符号
                for(int i = 0; i < f.Value.Count; i++) {
                    if(f.Value[i].type == TokenType.IDENTIFIER && CQ_TokenParser.ContainsType(f.Value[i].text)) {//有可能预处理导致新的类型
                        if(i > 0
                            &&
                            (f.Value[i - 1].type == TokenType.TYPE || f.Value[i - 1].text == ".")) {
                            continue;
                        }
                        Token rp = f.Value[i];
                        rp.type = TokenType.TYPE;
                        f.Value[i] = rp;
                    }
                }
                File_CompileToken(f.Key, f.Value, embDebugToken);
            }
        }
        private static void File_CompileToken (string filename, IList<Token> listToken, bool embDebugToken) {
            DebugUtil.Log("File_CompilerToken:" + filename);
            IList<IType> types = CQ_Expression_Compiler.FileCompile(filename, listToken, embDebugToken);
            foreach(var type in types) {
                if(GetTypeByKeywordQuiet(type.keyword) == null)
                    RegisterType(type);
            }
        }
        /// <summary>
        /// 这里的filename只是为了编译时报错可以看到出错文件
        /// </summary>

        public static ICQ_Expression BuildBlock (string code) {
			var token = CQ_TokenParser.Parse(code);
            return CQ_Expression_Compiler.Compile(token);
        }
        public static void BuildFile (string filename, string code) {
			var token = CQ_TokenParser.Parse(code);
            File_CompileToken(filename, token, false);
        }
        public static void BuildProject (string path, string pattern) {
            string[] files = System.IO.Directory.GetFiles(path, pattern, System.IO.SearchOption.AllDirectories);
            Dictionary<string, IList<CQuark.Token>> project = new Dictionary<string, IList<CQuark.Token>>();
            foreach(string fileName in files) {
				if(project.ContainsKey(fileName))
                    continue;
				string text = System.IO.File.ReadAllText(fileName);
                var tokens = CQ_TokenParser.Parse(text);
				project.Add(fileName, tokens);
            }
            Project_Compile(project, true);
        }
    }
}