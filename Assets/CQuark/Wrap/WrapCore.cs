using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
		public static bool New (Type type, List<CQ_Value> param, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}

			returnValue = null;
	        return false;
	    }

		public static bool StaticValueGet (Type type, string memberName, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}

			returnValue = null;
	        return false;
	    }

	    public static bool StaticValueSet (Type type, string memberName, CQ_Value param) {
			if(type == null){
				return false;
			}

			return false;
	    }

		public static bool StaticCall (Type type, string functionName, List<CQ_Value> param, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}

			returnValue = null;
	        return false;
	    }

		public static bool MemberValueGet (Type type, object objSelf, string memberName, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}

			returnValue = null;
			return false;
	    }

		public static bool MemberValueSet (Type type, object objSelf, string memberName, CQ_Value param) {
			if(type == null){
				return false;
			}

			return false;
	    }

		public static bool MemberCall (Type type, object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}

			returnValue = null;
	        return false;
	    }

		public static bool IndexGet(Type type, object objSelf, CQ_Value key, out CQ_Value returnValue){
			if(type == null) {
				returnValue = null;
				return false;
			}

			returnValue = null;
			return false;
		}

		public static bool IndexSet(Type type, object objSelf, CQ_Value key, CQ_Value param){
			if(type == null) {
				return false;
			}

			return false;
		}
		
//        public static bool OpAddition (CQ_Value left, CQ_Value right, out CQ_Value returnValue) {
//            returnValue = null;
//{9}           
//            return false;
//        }
		
//        public static bool OpSubtraction (CQ_Value left, CQ_Value right, out CQ_Value returnValue) {
//            returnValue = null;
//{10}           
//            return false;
//        }
		
//        public static bool OpMultiply (CQ_Value left, CQ_Value right, out CQ_Value returnValue) {
//            returnValue = null;
//{11}           
//            return false;
//        }
		
//        public static bool OpDivision (CQ_Value left, CQ_Value right, out CQ_Value returnValue) {
//            returnValue = null;
//{12}           
//            return false;
//        }
		
//        public static bool OpModulus (CQ_Value left, CQ_Value right, out CQ_Value returnValue) {
//            returnValue = null;
//{13}           
//            return false;
//        }
	}
}
