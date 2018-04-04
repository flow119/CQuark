using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
		private static bool UnityEngineTimeNew(List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual){
			if(param.Count == 0){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Time);
				returnValue.value = new UnityEngine.Time();
				return true;
			}

			returnValue = null;
			return false;
		}

		public static bool UnityEngineTimeSGet (string memberName, out CQ_Value returnValue) {
			switch(memberName) {
			case "time":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.time;
				return true;
			case "timeSinceLevelLoad":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.timeSinceLevelLoad;
				return true;
			case "deltaTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.deltaTime;
				return true;
			case "fixedTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.fixedTime;
				return true;
			case "unscaledTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.unscaledTime;
				return true;
			case "fixedUnscaledTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.fixedUnscaledTime;
				return true;
			case "unscaledDeltaTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.unscaledDeltaTime;
				return true;
			case "fixedUnscaledDeltaTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.fixedUnscaledDeltaTime;
				return true;
			case "fixedDeltaTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.fixedDeltaTime;
				return true;
			case "maximumDeltaTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.maximumDeltaTime;
				return true;
			case "smoothDeltaTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.smoothDeltaTime;
				return true;
			case "maximumParticleDeltaTime":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.maximumParticleDeltaTime;
				return true;
			case "timeScale":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.timeScale;
				return true;
			case "frameCount":
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Time.frameCount;
				return true;
			case "renderedFrameCount":
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Time.renderedFrameCount;
				return true;
			case "realtimeSinceStartup":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Time.realtimeSinceStartup;
				return true;
			case "captureFramerate":
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Time.captureFramerate;
				return true;
			case "inFixedTimeStep":
				returnValue = new CQ_Value();
				returnValue.type = typeof(bool);
				returnValue.value = UnityEngine.Time.inFixedTimeStep;
				return true;
			}
			returnValue = null;
	        return false;
	    }

	    public static bool UnityEngineTimeSSet (string memberName, CQ_Value param) {
			switch(memberName) {
			case "fixedDeltaTime":
				if(param.EqualOrImplicateType(typeof(float))){
					UnityEngine.Time.fixedDeltaTime = (float)param.ConvertTo(typeof(float));
					return true;
				}
				break;
			case "maximumDeltaTime":
				if(param.EqualOrImplicateType(typeof(float))){
					UnityEngine.Time.maximumDeltaTime = (float)param.ConvertTo(typeof(float));
					return true;
				}
				break;
			case "maximumParticleDeltaTime":
				if(param.EqualOrImplicateType(typeof(float))){
					UnityEngine.Time.maximumParticleDeltaTime = (float)param.ConvertTo(typeof(float));
					return true;
				}
				break;
			case "timeScale":
				if(param.EqualOrImplicateType(typeof(float))){
					UnityEngine.Time.timeScale = (float)param.ConvertTo(typeof(float));
					return true;
				}
				break;
			case "captureFramerate":
				if(param.EqualOrImplicateType(typeof(int))){
					UnityEngine.Time.captureFramerate = (int)param.ConvertTo(typeof(int));
					return true;
				}
				break;
			}
			return false;
	    }

		public static bool UnityEngineTimeSCall (string functionName, List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual) {

			returnValue = null;
	        return false;
	    }

		public static bool UnityEngineTimeMGet (object objSelf, string memberName, out CQ_Value returnValue) {

			returnValue = null;
			return false;
	    }

		public static bool UnityEngineTimeMSet (object objSelf, string memberName, CQ_Value param) {

			return false;
	    }

		public static bool UnityEngineTimeMCall (object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual) {
			UnityEngine.Time obj = (UnityEngine.Time)objSelf;
			if(param.Count == 1 && functionName == "Equals" && MatchType(param, new Type[] {typeof(object)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(bool);
				returnValue.value = obj.Equals((object)param[0].ConvertTo(typeof(object)));
				return true;
			}
			if(param.Count == 0 && functionName == "GetHashCode"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = obj.GetHashCode();
				return true;
			}
			if(param.Count == 0 && functionName == "GetType"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(System.Type);
				returnValue.value = obj.GetType();
				return true;
			}
			if(param.Count == 0 && functionName == "ToString"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(string);
				returnValue.value = obj.ToString();
				return true;
			}
			
			returnValue = null;
	        return false;
	    }

		public static bool UnityEngineTimeIGet(object objSelf, CQ_Value key, out CQ_Value returnValue){

			returnValue = null;
			return false;
		}

		public static bool UnityEngineTimeISet(object objSelf, CQ_Value key, CQ_Value param){

			return false;
		}
	}
}
