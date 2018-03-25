using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class UnityWrap {
		public static bool New (Type type, List<CQ_Value> param, out CQ_Value cqVal) {
			if(type == null){
				cqVal = null;
				return false;
			}
			cqVal = null;
			if(type == typeof(Vector3)){
				return Vector3New(param, out cqVal);
	        }
			//TODO
	        return false;
	    }

		public static bool StaticValueGet (Type type, string membername, out CQ_Value cqVal) {
			if(type == null){
				cqVal = null;
				return false;
			}
			cqVal = null;
			if(type == typeof(Vector3)){
				return Vector3SGet(membername, out cqVal);
			}
			//TODO
	        return false;
	    }

	    public static bool StaticValueSet (Type type, string membername, CQ_Value param) {
			if(type == null){
				return false;
			}
			if(type == typeof(Vector3)){
				return Vector3SSet(membername, param);
			}
			//TODO
			return false;
	    }

		public static bool StaticCall (Type type, string functionname, List<CQ_Value> param, out CQ_Value cqVal) {
			if(type == null){
				cqVal = null;
				return false;
			}
			if(type == typeof(Mathf)){
				return MathfSCall(functionname, param, out cqVal);
			}
			cqVal = null;
	        return false;
	    }

		public static bool MemberValueGet (Type type, object object_this, string membername, out CQ_Value cqVal) {
			if(type == null){
				cqVal = null;
				return false;
			}
			if(type == typeof(Vector3)){
				return Vector3MGet(object_this, membername, out cqVal);
			}
			cqVal = null;
			return false;
	    }

		public static bool MemberValueSet (Type type, object object_this, string membername, CQ_Value param) {
			if(type == null){
				return false;
			}
			if(type == typeof(Vector3)){
				return Vector3MSet(object_this, membername, param);
			}
			return false;
	    }

		public static bool MemberCall (Type type, object object_this, string functionname, List<CQ_Value> param, out CQ_Value cqVal) {
			if(type == null){
				cqVal = null;
				return false;
			}
			cqVal = null;
			if(type == typeof(Transform)){
				return TransformCall(object_this, functionname, param, out cqVal);
			}

	        return false;
	    }




		private static bool TransformCall(object object_this, string functionname, List<CQ_Value> param, out CQ_Value cqVal) {
			Transform t = (Transform)object_this;
			switch(functionname) {
			case "Rotate":
				cqVal = null;
				t.Rotate((Vector3)param[0].value, (float)param[1].value);
				return true;
			}
			cqVal = null;
			return false;
		}


		private static bool MathfSCall(string functionname, List<CQ_Value> param, out CQ_Value cqVal) {
			switch(functionname) {
			case "Abs":
				cqVal = new CQ_Value();
				if((Type)param[0].type == typeof(int)) {
					cqVal.type = typeof(int);
					cqVal.value = Mathf.Abs((int)(param[0].value));
					return true;
				}
				else if((Type)param[0].type == typeof(float)) {
					cqVal.type = typeof(float);
					cqVal.value = Mathf.Abs((float)(param[0].value));
					return true;
				}
				break;
			case "Sin":
				cqVal = new CQ_Value();
				cqVal.type = typeof(float);
				cqVal.value = Mathf.Sin(NumericTypeUtils.GetFloat(param[0].type, param[0].value));
				return true;
			}
			cqVal = null;
			return false;
		}
	}
}
