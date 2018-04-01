using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
		private static bool UnityEngineVector3New(List<CQ_Value> param, out CQ_Value returnValue){

			returnValue = null;
			return false;
		}

		public static bool UnityEngineVector3SGet (string memberName, out CQ_Value returnValue) {
			switch(memberName) {
			case "kEpsilon":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Vector3.kEpsilon;
				return true;
			case "zero":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.zero;
				return true;
			case "one":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.one;
				return true;
			case "forward":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.forward;
				return true;
			case "back":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.back;
				return true;
			case "up":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.up;
				return true;
			case "down":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.down;
				return true;
			case "left":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.left;
				return true;
			case "right":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.right;
				return true;
			case "fwd":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.fwd;
				return true;
			}
			returnValue = null;
	        return false;
	    }

	    public static bool UnityEngineVector3SSet (string memberName, CQ_Value param) {

			return false;
	    }

		public static bool UnityEngineVector3SCall (string functionName, List<CQ_Value> param, out CQ_Value returnValue) {

			returnValue = null;
	        return false;
	    }

		public static bool UnityEngineVector3MGet (object objSelf, string memberName, out CQ_Value returnValue) {
			UnityEngine.Vector3 obj = (UnityEngine.Vector3)objSelf;
			switch(memberName) {
			case "x":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = obj.x;
				return true;
			case "y":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = obj.y;
				return true;
			case "z":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = obj.z;
				return true;
			case "normalized":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.normalized;
				return true;
			case "magnitude":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = obj.magnitude;
				return true;
			case "sqrMagnitude":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = obj.sqrMagnitude;
				return true;
			}
			returnValue = null;
			return false;
	    }

		public static bool UnityEngineVector3MSet (object objSelf, string memberName, CQ_Value param) {

			return false;
	    }

		public static bool UnityEngineVector3MCall (object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue) {

			returnValue = null;
	        return false;
	    }

		public static bool UnityEngineVector3IGet(object objSelf, CQ_Value key, out CQ_Value returnValue){

			returnValue = null;
			return false;
		}

		public static bool UnityEngineVector3ISet(object objSelf, CQ_Value key, CQ_Value param){

			return false;
		}
	}
}
