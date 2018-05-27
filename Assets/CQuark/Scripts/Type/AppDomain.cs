using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CQuark;

namespace CQuark{
	//参考了ILRuntime，把以前Environment和Content整合到了一起。
	//整个项目只需要一个AppDomain,所以改成了全部静态
	//AppDomain的作用是保存了所有的IType。
    public class AppDomain {
		static Dictionary<Class_CQuark, IType> ccq2itype = new Dictionary<Class_CQuark, IType> ();
		static Dictionary<Type, IType> type2itype = new Dictionary<Type, IType> ();
        static Dictionary<string, IType> str2itype = new Dictionary<string, IType>();

        public static void Reset () {
            DebugUtil.Log("Reset Domain");
			ccq2itype.Clear ();
			type2itype.Clear ();
			str2itype.Clear();
            
			DebugUtil.Log("Register Base Type");
			//注册基本类型
			RegisterType(new Type_String());
			RegisterType(new Type_Var());
			RegisterType(new Type_Bool());
			RegisterType(new Type_Lambda());
			RegisterType(new Type_Delegate());
			
			RegisterType<double>("double");
			RegisterType<float>("float");
			RegisterType<long>("long");
			RegisterType<ulong>("ulong");
			RegisterType<int>("int");
			RegisterType<uint>("uint");
			RegisterType<short>("short");
			RegisterType<ushort>("ushort");
			RegisterType<byte>("byte");
			RegisterType<sbyte>("sbyte");
			RegisterType<char>("char");
			
			RegisterType<object>("object");
			RegisterType<IEnumerator>("IEnumerator");
			
			RegisterType(typeof(List<>), "List");	//模板类要独立注册
			RegisterType(typeof(Dictionary<,>), "Dictionary");
			RegisterType(typeof(Stack<>), "Stack");
			RegisterType(typeof(Queue<>), "Queue");
			
			str2itype["null"] = new Type_NULL();
        }

		public static bool ContainsType(string type){
			return str2itype.ContainsKey (type);
		}

			
		//除非是静态类，否则建议都走模板（比如Vector3 a;依然可以取出默认值）
		private static IType MakeIType<T>(string keyword){
			Type type = typeof(T);
			if(type.IsSubclassOf(typeof(Delegate))) 
				return MakeITypeWithDelegate(type, keyword);
			if(NumberUtil.IsNumberType(type))
				return new Type_Number(type, keyword, default(T));
			else
				return new Type_Generic(type, keyword, default(T));
		}
		private static IType MakeIType (Type type, string keyword) {
			if(type.IsSubclassOf(typeof(Delegate))) 
				return MakeITypeWithDelegate(type, keyword);
			return new Type_Generic(type, keyword, null);
		}
		private static IType MakeITypeWithDelegate(Type type, string keyword){
			var method = type.GetMethod("Invoke");
			var pp = method.GetParameters();
			if(method.ReturnType == typeof(void)) {
				if(pp.Length == 0) {
					return new Type_DeleAction(type, keyword);
				}
				else if(pp.Length == 1) {
					var gtype = typeof(Type_DeleAction<>).MakeGenericType(new Type[] { pp[0].ParameterType });
					return gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Generic;
				}
				else if(pp.Length == 2) {
					var gtype = typeof(Type_DeleAction<,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType });
					return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Generic);
				}
				else if(pp.Length == 3) {
					var gtype = typeof(Type_DeleAction<,,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
					return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Generic);
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
				return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as Type_Generic);
			}
		}

		//含命名空间的注册，除非是静态类，否则建议都走模板（比如Vector3 a;依然可以取出默认值）
		public static void RegisterType<T> () {
			string keyword = typeof(T).FullName.Replace('+','.');
			RegisterIType(MakeIType<T>(keyword));
			RegisterIType(MakeIType<T[]>(keyword+"[]"));
		}
		//非静态类走这个函数注册不会报错，但是会取不到默认值
		public static void RegisterType (Type type) {
			string keyword = type.FullName.Replace('+','.');
			RegisterIType(MakeIType(type, keyword));
		}

		private static void RegisterIType (IType type) {
			if (type.typeBridge.type != null)
				type2itype [type.typeBridge.type] = type;
			else if (type.typeBridge.stype != null)
				ccq2itype [type.typeBridge.stype] = type;

			string typename = type.keyword;
			if(string.IsNullOrEmpty(typename)) {//匿名自动注册
			}
			else {
				str2itype[typename] = type;
//				TokenSpliter.RegisterOriType(typename);
			}
		}


		//含命名空间的注册，除非是静态类，否则建议都走模板（比如Vector3 a;依然可以取出默认值）
		[Obsolete]
		public static void RegisterType<T> (string keyword) {
			RegisterType(MakeIType<T>(keyword));
			RegisterType(MakeIType<T[]>(keyword+"[]"));
		}
		[Obsolete]
		//非静态类走这个函数注册不会报错，但是会取不到默认值
        public static void RegisterType (Type type, string keyword) {
            RegisterType(MakeIType(type, keyword));
        }

		public static void RegisterCQType(IType type){
			if (type.typeBridge.stype != null)
				ccq2itype [type.typeBridge.stype] = type;
			string typename = type.keyword;
			if(string.IsNullOrEmpty(typename)) {//匿名自动注册
				
			}
			else {
				str2itype[typename] = type;
				CQ_TokenParser.AddType(typename);
//				TokenSpliter.RegisterCustomType(typename);
			}
		}

        public static void RegisterType (IType type) {
			if (type.typeBridge.type != null)
				type2itype [type.typeBridge.type] = type;
			else if (type.typeBridge.stype != null)
				ccq2itype [type.typeBridge.stype] = type;

            string typename = type.keyword;

            if(string.IsNullOrEmpty(typename)) {//匿名自动注册
            }
            else {
				str2itype[typename] = type;
                CQ_TokenParser.AddType(typename);
//				TokenSpliter.RegisterOriType(typename);
            }
        }

		public static IType GetITypeByCQType(TypeBridge typeBridge){
			if (typeBridge.type != null)
				return GetITypeByType (typeBridge.type);
			else if (typeBridge.stype != null)
				return GetITypeByClassCQ (typeBridge.stype);
			return null;
		}
        public static IType GetITypeByCQValue (CQ_Value val) {
            if(val.m_type != null)
                return GetITypeByType(val.m_type);
            else if(val.m_stype != null)
                return GetITypeByClassCQ(val.m_stype);
            return null;
        }
		public static IType GetITypeByClassCQ (Class_CQuark stype) {
			if(stype == null)
				return str2itype["null"];
			
			IType ret = null;
			if(!ccq2itype.TryGetValue(stype, out ret)) {
				DebugUtil.LogWarning("(Class_CQuark)类型未注册,将自动注册一份匿名:" + stype.ToString());
			}
			return ret;
		}
		public static IType GetITypeByType (Type type) {
			if(type == null)
				return str2itype["null"];
			
			IType ret = null;
			if(!type2itype.TryGetValue(type, out ret)) {
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
			if(str2itype.TryGetValue(keyword, out ret)) 
				return ret;
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
			        Type gentype = GetTypeByKeyword(func).typeBridge;
			        if(gentype.IsGenericTypeDefinition) {
			            Type[] types = new Type[_types.Count];
			            for(int i = 0; i < types.Length; i++) {
			                TypeBridge t = GetTypeByKeyword(_types[i]).typeBridge;
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
			//TODO反射找类
			DebugUtil.LogError("(CQScript)类型未注册:" + keyword);
			return null;
        }
        public static IType GetTypeByKeywordQuiet (string keyword) {
            IType ret = null;
			if(str2itype.TryGetValue(keyword, out ret) == false) {
                return null;
            }
            return ret;
        }

		public static object ConvertTo(object obj, TypeBridge targetType){
			return GetITypeByType(obj.GetType()).ConvertTo(obj, targetType);
		}
    }
}