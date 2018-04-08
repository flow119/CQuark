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
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfNew(param, out returnValue, true) || UnityEngineMathfNew(param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeNew(param, out returnValue, true) || UnityEngineTimeNew(param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformNew(param, out returnValue, true) || UnityEngineTransformNew(param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3New(param, out returnValue, true) || UnityEngineVector3New(param, out returnValue, false);
			}

			returnValue = null;
	        return false;
	    }

		public static bool StaticValueGet (Type type, string memberName, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfSGet(memberName, out returnValue);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeSGet(memberName, out returnValue);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformSGet(memberName, out returnValue);
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
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfSSet(memberName, param);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeSSet(memberName, param);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformSSet(memberName, param);
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
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfSCall(functionName, param, out returnValue, true) || UnityEngineMathfSCall(functionName, param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeSCall(functionName, param, out returnValue, true) || UnityEngineTimeSCall(functionName, param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformSCall(functionName, param, out returnValue, true) || UnityEngineTransformSCall(functionName, param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3SCall(functionName, param, out returnValue, true) || UnityEngineVector3SCall(functionName, param, out returnValue, false);
			}

			returnValue = null;
	        return false;
	    }

		public static bool MemberValueGet (Type type, object objSelf, string memberName, out CQ_Value returnValue) {
			if(type == null){
				returnValue = null;
				return false;
			}
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfMGet(objSelf, memberName, out returnValue);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeMGet(objSelf, memberName, out returnValue);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformMGet(objSelf, memberName, out returnValue);
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
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfMSet(objSelf, memberName, param);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeMSet(objSelf, memberName, param);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformMSet(objSelf, memberName, param);
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
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfMCall(objSelf, functionName, param, out returnValue, true) || UnityEngineMathfMCall(objSelf, functionName, param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeMCall(objSelf, functionName, param, out returnValue, true) || UnityEngineTimeMCall(objSelf, functionName, param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformMCall(objSelf, functionName, param, out returnValue, true) || UnityEngineTransformMCall(objSelf, functionName, param, out returnValue, false);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3MCall(objSelf, functionName, param, out returnValue, true) || UnityEngineVector3MCall(objSelf, functionName, param, out returnValue, false);
			}

			returnValue = null;
	        return false;
	    }

		public static bool IndexGet(Type type, object objSelf, CQ_Value key, out CQ_Value returnValue){
			if(type == null) {
				returnValue = null;
				return false;
			}
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfIGet(objSelf, key, out returnValue);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeIGet(objSelf, key, out returnValue);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformIGet(objSelf, key, out returnValue);
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
			if(type == typeof(UnityEngine.Mathf)){
				return UnityEngineMathfISet(objSelf, key, param);
			}
			if(type == typeof(UnityEngine.Time)){
				return UnityEngineTimeISet(objSelf, key, param);
			}
			if(type == typeof(UnityEngine.Transform)){
				return UnityEngineTransformISet(objSelf, key, param);
			}
			if(type == typeof(UnityEngine.Vector3)){
				return UnityEngineVector3ISet(objSelf, key, param);
			}

			return false;
		}
		
	}
}
