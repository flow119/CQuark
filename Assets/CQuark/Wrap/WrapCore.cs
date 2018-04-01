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
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeNew(param, out returnValue);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3New(param, out returnValue);
			}

			returnValue = null;
	        return false;
	    }

		public static bool StaticValueGet (Type type, string memberName, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeSGet(memberName, out returnValue);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3SGet(memberName, out returnValue);
			}

			returnValue = null;
	        return false;
	    }

	    public static bool StaticValueSet (Type type, string memberName, CQ_Value param) {
			if(type == null){
				return false;
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeSSet(memberName, param);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3SSet(memberName, param);
			}

			return false;
	    }

		public static bool StaticCall (Type type, string functionName, List<CQ_Value> param, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeSCall(functionName, param, out returnValue);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3SCall(functionName, param, out returnValue);
			}

			returnValue = null;
	        return false;
	    }

		public static bool MemberValueGet (Type type, object objSelf, string memberName, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeMGet(objSelf, memberName, out returnValue);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3MGet(objSelf, memberName, out returnValue);
			}

			returnValue = null;
			return false;
	    }

		public static bool MemberValueSet (Type type, object objSelf, string memberName, CQ_Value param) {
			if(type == null){
				return false;
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeMSet(objSelf, memberName, param);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3MSet(objSelf, memberName, param);
			}

			return false;
	    }

		public static bool MemberCall (Type type, object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeMCall(objSelf, functionName, param, out returnValue);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3MCall(objSelf, functionName, param, out returnValue);
			}

			returnValue = null;
	        return false;
	    }

		public static bool IndexGet(Type type, object objSelf, CQ_Value key, out CQ_Value returnValue){
			if(type == null) {
				returnValue = null;
				return false;
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeIGet(objSelf, key, out returnValue);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3IGet(objSelf, key, out returnValue);
			}

			returnValue = null;
			return false;
		}

		public static bool IndexSet(Type type, object objSelf, CQ_Value key, CQ_Value param){
			if(type == null) {
				return false;
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeISet(objSelf, key, param);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3ISet(objSelf, key, param);
			}

			return false;
		}
	}
}
