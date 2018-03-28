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
//			if(type == typeof(Vector3)){
//				return Vector3New(param, out returnValue);
//	        }
			//TODO
	        return false;
	    }

		public static bool StaticValueGet (Type type, string memberName, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			returnValue = null;
//			if(type == typeof(Vector3)){
//				return Vector3SGet(memberName, out returnValue);
//			}
			//TODO
	        return false;
	    }

	    public static bool StaticValueSet (Type type, string memberName, CQ_Value param) {
			if(type == null){
				return false;
			}
//			if(type == typeof(Vector3)){
//				return Vector3SSet(memberName, param);
//			}
			//TODO
			return false;
	    }

		public static bool StaticCall (Type type, string functionName, List<CQ_Value> param, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			if(type == typeof(Mathf)){
				return MathfSCall(functionName, param, out returnValue);
			}
			returnValue = null;
	        return false;
	    }

		public static bool MemberValueGet (Type type, object objSelf, string memberName, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
//			if(type == typeof(Vector3)){
//				return Vector3MGet(objSelf, memberName, out returnValue);
//			}
			returnValue = null;
			return false;
	    }

		public static bool MemberValueSet (Type type, object objSelf, string memberName, CQ_Value param) {
			if(type == null){
				return false;
			}
//			if(type == typeof(Vector3)){
//				return Vector3MSet(objSelf, memberName, param);
//			}
			return false;
	    }

		public static bool MemberCall (Type type, object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			returnValue = null;
			if(type == typeof(Transform)){
				return TransformCall(objSelf, functionName, param, out returnValue);
			}

	        return false;
	    }

		public static bool IndexGet(Type type, object objSelf, CQ_Value key, out CQ_Value returnValue){
			if(type == null) {
				returnValue = null;
				return false;
			}
			if(type == typeof (Vector3)){
				return Vector3IGet(objSelf, key, out returnValue);
			}
			returnValue = null;
			return false;
		}

		public static bool IndexSet(Type type, object objSelf, CQ_Value key, CQ_Value value){
			if(type == null) {
				return false;
			}
			if(type == typeof (Vector3)){
				return Vector3ISet(objSelf, key, value);
			}
			return false;
		}

		private static bool Vector3IGet(object objSelf, CQ_Value key, out CQ_Value returnValue){
			Vector3 target = (Vector3)objSelf;
			if(key.type.type == typeof(int)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = target[(int)key.value]; 
				return true;
			}
			returnValue = null;
			return false;
		}

		private static bool Vector3ISet(object objSelf, CQ_Value key, CQ_Value value){
			Vector3 target = (Vector3)objSelf;
			if(key.type.type == typeof(int)){
				target[(int)key.value] = value.GetFloat();
				return true;
			}
			return false;
		}

		private static bool TransformCall(object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue) {
			Transform t = (Transform)objSelf;
			switch(functionName) {
			case "Rotate":
				returnValue = null;
				t.Rotate((Vector3)param[0].value, (float)param[1].value);
				return true;
			}
			returnValue = null;
			return false;
		}


		private static bool MathfSCall(string functionName, List<CQ_Value> param, out CQ_Value returnValue) {
			switch(functionName) {
			case "Abs":
				returnValue = new CQ_Value();
				if((Type)param[0].type == typeof(int)) {
					returnValue.type = typeof(int);
					returnValue.value = Mathf.Abs((int)(param[0].value));
					return true;
				}
				else if((Type)param[0].type == typeof(float)) {
					returnValue.type = typeof(float);
					returnValue.value = Mathf.Abs((float)(param[0].value));
					return true;
				}
				break;
			case "Sin":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = Mathf.Sin(NumericTypeUtils.GetFloat(param[0].type, param[0].value));
				return true;
			}
			returnValue = null;
			return false;
		}
	}
}
