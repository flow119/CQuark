using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
	
		static Type[] UnityEngineTimec300a33f = new Type[]{typeof(object)};

	
		private static bool UnityEngineTimeSGet (string memberName, out CQ_Value returnValue) {
			switch(memberName) {
			case "time":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.time);
				return true;
			case "timeSinceLevelLoad":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.timeSinceLevelLoad);
				return true;
			case "deltaTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.deltaTime);
				return true;
			case "fixedTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.fixedTime);
				return true;
			case "unscaledTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.unscaledTime);
				return true;
			case "fixedUnscaledTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.fixedUnscaledTime);
				return true;
			case "unscaledDeltaTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.unscaledDeltaTime);
				return true;
			case "fixedUnscaledDeltaTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.fixedUnscaledDeltaTime);
				return true;
			case "fixedDeltaTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.fixedDeltaTime);
				return true;
			case "maximumDeltaTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.maximumDeltaTime);
				return true;
			case "smoothDeltaTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.smoothDeltaTime);
				return true;
			case "maximumParticleDeltaTime":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.maximumParticleDeltaTime);
				return true;
			case "timeScale":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.timeScale);
				return true;
			case "frameCount":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Time.frameCount);
				return true;
			case "renderedFrameCount":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Time.renderedFrameCount);
				return true;
			case "realtimeSinceStartup":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Time.realtimeSinceStartup);
				return true;
			case "captureFramerate":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Time.captureFramerate);
				return true;
			case "inFixedTimeStep":
				returnValue = new CQ_Value();
				returnValue.SetBool(UnityEngine.Time.inFixedTimeStep);
				return true;
			}
			returnValue = CQ_Value.Null;
	        return false;
	    }

	    private static bool UnityEngineTimeSSet (string memberName, CQ_Value param) {
			switch(memberName) {
			case "fixedDeltaTime":
				if(param.EqualOrImplicateType(typeof(float))){
					UnityEngine.Time.fixedDeltaTime = (float)param.GetNumber();
					return true;
				}
				break;
			case "maximumDeltaTime":
				if(param.EqualOrImplicateType(typeof(float))){
					UnityEngine.Time.maximumDeltaTime = (float)param.GetNumber();
					return true;
				}
				break;
			case "maximumParticleDeltaTime":
				if(param.EqualOrImplicateType(typeof(float))){
					UnityEngine.Time.maximumParticleDeltaTime = (float)param.GetNumber();
					return true;
				}
				break;
			case "timeScale":
				if(param.EqualOrImplicateType(typeof(float))){
					UnityEngine.Time.timeScale = (float)param.GetNumber();
					return true;
				}
				break;
			case "captureFramerate":
				if(param.EqualOrImplicateType(typeof(int))){
					UnityEngine.Time.captureFramerate = (int)param.GetNumber();
					return true;
				}
				break;
			}
			return false;
	    }

		private static bool UnityEngineTimeMGet (object objSelf, string memberName, out CQ_Value returnValue) {

			returnValue = CQ_Value.Null;
			return false;
	    }

		private static bool UnityEngineTimeMSet (object objSelf, string memberName, CQ_Value param) {

			return false;
	    }
		
		private static bool UnityEngineTimeNew(CQ_Value[] param, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTimeSCall (string functionName, CQ_Value[] param, out CQ_Value returnValue, bool mustEqual) {

			returnValue = CQ_Value.Null;
	        return false;
	    }

		private static bool UnityEngineTimeMCall (object objSelf, string functionName, CQ_Value[] param, out CQ_Value returnValue, bool mustEqual) {
			UnityEngine.Time obj = (UnityEngine.Time)objSelf;
			int paramCount = param.Length;
			if(paramCount == 1 && functionName == "Equals" && MatchType(param, UnityEngineTimec300a33f, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetBool(obj.Equals((object)param[0].ConvertTo(typeof(object))));
				return true;
			}
			if(paramCount == 0 && functionName == "GetHashCode"){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), obj.GetHashCode());
				return true;
			}
			if(paramCount == 0 && functionName == "GetType"){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(System.Type), obj.GetType());
				return true;
			}
			if(paramCount == 0 && functionName == "ToString"){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), obj.ToString());
				return true;
			}

			returnValue = CQ_Value.Null;
	        return false;
	    }

		private static bool UnityEngineTimeIGet(object objSelf, CQ_Value key, out CQ_Value returnValue){


			returnValue = CQ_Value.Null;
			return false;
		}

		private static bool UnityEngineTimeISet(object objSelf, CQ_Value key, CQ_Value param){

			
			return false;
		}
		
		private static bool UnityEngineTimeAdd(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTimeSub(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTimeMul(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTimeDiv(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTimeMod(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
	}
}
