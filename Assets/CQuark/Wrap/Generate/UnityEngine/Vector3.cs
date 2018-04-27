using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
	
		static Type[] UnityEngineVector3a780f8dc = new Type[]{typeof(float), typeof(float), typeof(float)};
		static Type[] UnityEngineVector3c3c31980 = new Type[]{typeof(float), typeof(float)};
		static Type[] UnityEngineVector32e78c2fc = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3), typeof(float)};
		static Type[] UnityEngineVector3dc85dce0 = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3), typeof(float), typeof(float)};
		static Type[] UnityEngineVector3b15d7b60 = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3)};
		static Type[] UnityEngineVector3a475c239 = new Type[]{typeof(UnityEngine.Vector3)};
		static Type[] UnityEngineVector34fe7dbc3 = new Type[]{typeof(UnityEngine.Vector3), typeof(float)};
		static Type[] UnityEngineVector3c300a33f = new Type[]{typeof(object)};
		static Type[] UnityEngineVector3cad56011 = new Type[]{typeof(string)};

	
		private static bool UnityEngineVector3SGet (string memberName, out CQ_Value returnValue) {
			switch(memberName) {
			case "kEpsilon":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Vector3.kEpsilon);
				return true;
			case "zero":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.zero);
				return true;
			case "one":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.one);
				return true;
			case "forward":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.forward);
				return true;
			case "back":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.back);
				return true;
			case "up":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.up);
				return true;
			case "down":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.down);
				return true;
			case "left":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.left);
				return true;
			case "right":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.right);
				return true;
			}
			returnValue = CQ_Value.Null;
	        return false;
	    }

	    private static bool UnityEngineVector3SSet (string memberName, CQ_Value param) {

			return false;
	    }

		private static bool UnityEngineVector3MGet (object objSelf, string memberName, out CQ_Value returnValue) {
			UnityEngine.Vector3 obj = (UnityEngine.Vector3)objSelf;
			switch(memberName) {
			case "x":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), obj.x);
				return true;
			case "y":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), obj.y);
				return true;
			case "z":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), obj.z);
				return true;
			case "normalized":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.normalized);
				return true;
			case "magnitude":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), obj.magnitude);
				return true;
			case "sqrMagnitude":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), obj.sqrMagnitude);
				return true;
			}
			returnValue = CQ_Value.Null;
			return false;
	    }

		private static bool UnityEngineVector3MSet (object objSelf, string memberName, CQ_Value param) {
			UnityEngine.Vector3 obj = (UnityEngine.Vector3)objSelf;
			switch(memberName) {
			case "x":
				if(param.EqualOrImplicateType(typeof(float))){
					obj.x = (float)param.GetNumber();
					return true;
				}
				break;
			case "y":
				if(param.EqualOrImplicateType(typeof(float))){
					obj.y = (float)param.GetNumber();
					return true;
				}
				break;
			case "z":
				if(param.EqualOrImplicateType(typeof(float))){
					obj.z = (float)param.GetNumber();
					return true;
				}
				break;
			}
			return false;
	    }
		
		private static bool UnityEngineVector3New(CQ_Value[] param, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineVector3SCall (string functionName, CQ_Value[] param, out CQ_Value returnValue, bool mustEqual) {
			int paramCount = param.Length;
			if(paramCount == 3 && functionName == "Slerp" && MatchType(param, UnityEngineVector32e78c2fc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Slerp((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "SlerpUnclamped" && MatchType(param, UnityEngineVector32e78c2fc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.SlerpUnclamped((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 4 && functionName == "RotateTowards" && MatchType(param, UnityEngineVector3dc85dce0, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.RotateTowards((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].GetNumber(),(float)param[3].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "Lerp" && MatchType(param, UnityEngineVector32e78c2fc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Lerp((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "LerpUnclamped" && MatchType(param, UnityEngineVector32e78c2fc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.LerpUnclamped((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "MoveTowards" && MatchType(param, UnityEngineVector32e78c2fc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.MoveTowards((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "Scale" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Scale((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "Cross" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Cross((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "Reflect" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Reflect((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 1 && functionName == "Normalize" && MatchType(param, UnityEngineVector3a475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Normalize((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "Dot" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Vector3.Dot((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "Project" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Project((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "ProjectOnPlane" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.ProjectOnPlane((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "Angle" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Vector3.Angle((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "Distance" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Vector3.Distance((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "ClampMagnitude" && MatchType(param, UnityEngineVector34fe7dbc3, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.ClampMagnitude((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Magnitude" && MatchType(param, UnityEngineVector3a475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Vector3.Magnitude((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 1 && functionName == "SqrMagnitude" && MatchType(param, UnityEngineVector3a475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Vector3.SqrMagnitude((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "Min" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Min((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 2 && functionName == "Max" && MatchType(param, UnityEngineVector3b15d7b60, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), UnityEngine.Vector3.Max((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}

			returnValue = CQ_Value.Null;
	        return false;
	    }

		private static bool UnityEngineVector3MCall (object objSelf, string functionName, CQ_Value[] param, out CQ_Value returnValue, bool mustEqual) {
			UnityEngine.Vector3 obj = (UnityEngine.Vector3)objSelf;
			int paramCount = param.Length;
			if(paramCount == 3 && functionName == "Set" && MatchType(param, UnityEngineVector3a780f8dc, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Set((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber());
				return true;
			}
			if(paramCount == 1 && functionName == "Scale" && MatchType(param, UnityEngineVector3a475c239, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Scale((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(paramCount == 0 && functionName == "GetHashCode"){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), obj.GetHashCode());
				return true;
			}
			if(paramCount == 1 && functionName == "Equals" && MatchType(param, UnityEngineVector3c300a33f, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetBool(obj.Equals((object)param[0].ConvertTo(typeof(object))));
				return true;
			}
			if(paramCount == 0 && functionName == "Normalize"){
				returnValue = CQ_Value.Null;
				obj.Normalize();
				return true;
			}
			if(paramCount == 0 && functionName == "ToString"){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), obj.ToString());
				return true;
			}
			if(paramCount == 1 && functionName == "ToString" && MatchType(param, UnityEngineVector3cad56011, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), obj.ToString((string)param[0].ConvertTo(typeof(string))));
				return true;
			}
			if(paramCount == 0 && functionName == "GetType"){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(System.Type), obj.GetType());
				return true;
			}

			returnValue = CQ_Value.Null;
	        return false;
	    }

		private static bool UnityEngineVector3IGet(object objSelf, CQ_Value key, out CQ_Value returnValue){
			UnityEngine.Vector3 obj = (UnityEngine.Vector3)objSelf;
			if(key.EqualOrImplicateType(typeof(int))){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), obj[(int)key.GetNumber()]);
				return true;
			}

			returnValue = CQ_Value.Null;
			return false;
		}

		private static bool UnityEngineVector3ISet(object objSelf, CQ_Value key, CQ_Value param){
			UnityEngine.Vector3 obj = (UnityEngine.Vector3)objSelf;
			if(param.EqualOrImplicateType(typeof(int))){
				obj[(int)key.GetNumber()] = (float)param.GetNumber();
				return true;
			}
			
			return false;
		}
		
		private static bool UnityEngineVector3Add(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){
			if((mustEqual && left.EqualType(typeof(UnityEngine.Vector3)) && right.EqualType(typeof(UnityEngine.Vector3)))
				|| (!mustEqual && left.EqualOrImplicateType(typeof(UnityEngine.Vector3)) && right.EqualOrImplicateType(typeof(UnityEngine.Vector3)))){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), (UnityEngine.Vector3)left.ConvertTo(typeof(UnityEngine.Vector3)) + (UnityEngine.Vector3)right.ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineVector3Sub(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){
			if((mustEqual && left.EqualType(typeof(UnityEngine.Vector3)) && right.EqualType(typeof(UnityEngine.Vector3)))
				|| (!mustEqual && left.EqualOrImplicateType(typeof(UnityEngine.Vector3)) && right.EqualOrImplicateType(typeof(UnityEngine.Vector3)))){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), (UnityEngine.Vector3)left.ConvertTo(typeof(UnityEngine.Vector3)) - (UnityEngine.Vector3)right.ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineVector3Mul(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){
			if((mustEqual && left.EqualType(typeof(UnityEngine.Vector3)) && right.EqualType(typeof(float)))
				|| (!mustEqual && left.EqualOrImplicateType(typeof(UnityEngine.Vector3)) && right.EqualOrImplicateType(typeof(float)))){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), (UnityEngine.Vector3)left.ConvertTo(typeof(UnityEngine.Vector3)) * (float)right.GetNumber());
				return true;
			}
			if((mustEqual && left.EqualType(typeof(float)) && right.EqualType(typeof(UnityEngine.Vector3)))
				|| (!mustEqual && left.EqualOrImplicateType(typeof(float)) && right.EqualOrImplicateType(typeof(UnityEngine.Vector3)))){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), (float)left.GetNumber() * (UnityEngine.Vector3)right.ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineVector3Div(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){
			if((mustEqual && left.EqualType(typeof(UnityEngine.Vector3)) && right.EqualType(typeof(float)))
				|| (!mustEqual && left.EqualOrImplicateType(typeof(UnityEngine.Vector3)) && right.EqualOrImplicateType(typeof(float)))){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), (UnityEngine.Vector3)left.ConvertTo(typeof(UnityEngine.Vector3)) / (float)right.GetNumber());
				return true;
			}

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineVector3Mod(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
	}
}
