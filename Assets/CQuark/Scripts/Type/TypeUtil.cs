using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQuark{
	public class TypeUtil  {

		public static bool CanImplicate (Type from, Type to) {
			//if(from == to)
			//    return true;
			//数值类型
			if(from == typeof(sbyte)) {
				return (to == typeof(short) || to == typeof(int) || to == typeof(long) || to == typeof(double) || to == typeof(float) || to == typeof(decimal));
			}
			else if(from == typeof(byte)) {
				return (to == typeof(short) || to == typeof(ushort) || to == typeof(int) || to == typeof(uint) || to == typeof(long) || to == typeof(ulong) || to == typeof(double) || to == typeof(float) || to == typeof(decimal));
			}
			else if(from == typeof(short)) {
				return (to == typeof(int) || to == typeof(long) || to == typeof(double) || to == typeof(float) || to == typeof(decimal));
			}
			else if(from == typeof(ushort)) {
				return (to == typeof(int) || to == typeof(uint) || to == typeof(long) || to == typeof(ulong) || to == typeof(double) || to == typeof(float) || to == typeof(decimal));
			}
			else if(from == typeof(int) || to == typeof(double) || to == typeof(float) || to == typeof(decimal)) {
				return (to == typeof(long));
			}
			else if(from == typeof(uint)) {
				return (to == typeof(long) || to == typeof(ulong) || to == typeof(double) || to == typeof(float) || to == typeof(decimal)) ;
			}
			else if(from == typeof(long)) {
				return (to == typeof(double) || to == typeof(float) || to == typeof(decimal));
			}
			else if(from == typeof(char)) {
				return (to == typeof(ushort) || to == typeof(int) || to == typeof(uint) || to == typeof(long) || to == typeof(ulong) || to == typeof(double) || to == typeof(float) || to == typeof(decimal)) ;
			}
			else if(from == typeof(float)) {
				return (to == typeof(double));
			}
			else if(from == typeof(ulong)){
				return (to == typeof(double) || to == typeof(float) || to == typeof(decimal));
			}

			//继承
			return(from.IsAssignableFrom(to));
		}

		public static bool MatchType (Type[] needType, List<CQ_Value> param, bool implicate) {
			if(needType.Length != param.Count)
				return false;

			for(int i = 0; i < needType.Length; i++) {
				if(param[i] == null && !needType[i].IsValueType)
					continue;
				if(!implicate) {
					if(needType[i] != (Type)param[i].type)
						return false;
				}
				else {
					if(!CanImplicate((Type)param[i].type, needType[i]))
						return false;
				}
			}

			return true;
		}
	}
}
