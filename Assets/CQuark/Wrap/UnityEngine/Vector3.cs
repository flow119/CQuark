using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
		private static bool UnityEngineVector3New(List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual){
			if(param.Count == 3 && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = new UnityEngine.Vector3((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = new UnityEngine.Vector3((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}

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

		public static bool UnityEngineVector3SCall (string functionName, List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual) {
			if(param.Count == 3 && functionName == "Slerp" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Slerp((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "SlerpUnclamped" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.SlerpUnclamped((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 4 && functionName == "RotateTowards" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.RotateTowards((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].ConvertTo(typeof(float)),(float)param[3].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "Exclude" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Exclude((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 3 && functionName == "Lerp" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Lerp((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "LerpUnclamped" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.LerpUnclamped((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "MoveTowards" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.MoveTowards((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "Scale" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Scale((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Cross" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Cross((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Reflect" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Reflect((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 1 && functionName == "Normalize" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Normalize((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Dot" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Vector3.Dot((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Project" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Project((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "ProjectOnPlane" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.ProjectOnPlane((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Angle" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Vector3.Angle((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Distance" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Vector3.Distance((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "ClampMagnitude" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.ClampMagnitude((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Magnitude" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Vector3.Magnitude((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 1 && functionName == "SqrMagnitude" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Vector3.SqrMagnitude((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Min" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Min((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Max" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = UnityEngine.Vector3.Max((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "AngleBetween" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Vector3.AngleBetween((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}

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
			UnityEngine.Vector3 obj = (UnityEngine.Vector3)objSelf;
			switch(memberName) {
			case "x":
				if(param.EqualOrImplicateType(typeof(float))){
					obj.x = (float)param.ConvertTo(typeof(float));
					return true;
				}
				break;
			case "y":
				if(param.EqualOrImplicateType(typeof(float))){
					obj.y = (float)param.ConvertTo(typeof(float));
					return true;
				}
				break;
			case "z":
				if(param.EqualOrImplicateType(typeof(float))){
					obj.z = (float)param.ConvertTo(typeof(float));
					return true;
				}
				break;
			}
			return false;
	    }

		public static bool UnityEngineVector3MCall (object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual) {
			UnityEngine.Vector3 obj = (UnityEngine.Vector3)objSelf;
			if(param.Count == 3 && functionName == "Set" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = null;
				obj.Set((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Scale" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = null;
				obj.Scale((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 0 && functionName == "GetHashCode"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = obj.GetHashCode();
				return true;
			}
			if(param.Count == 1 && functionName == "Equals" && MatchType(param, new Type[] {typeof(object)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(bool);
				returnValue.value = obj.Equals((object)param[0].ConvertTo(typeof(object)));
				return true;
			}
			if(param.Count == 0 && functionName == "Normalize"){
				returnValue = null;
				obj.Normalize();
				return true;
			}
			if(param.Count == 0 && functionName == "ToString"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(string);
				returnValue.value = obj.ToString();
				return true;
			}
			if(param.Count == 1 && functionName == "ToString" && MatchType(param, new Type[] {typeof(string)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(string);
				returnValue.value = obj.ToString((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(param.Count == 0 && functionName == "GetType"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(System.Type);
				returnValue.value = obj.GetType();
				return true;
			}
			
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
