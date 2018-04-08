using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
		private static bool UnityEngineMathfNew(List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual){

			returnValue = null;
			return false;
		}

		public static bool UnityEngineMathfSGet (string memberName, out CQ_Value returnValue) {
			switch(memberName) {
			case "PI":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.PI;
				return true;
			case "Infinity":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Infinity;
				return true;
			case "NegativeInfinity":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.NegativeInfinity;
				return true;
			case "Deg2Rad":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Deg2Rad;
				return true;
			case "Rad2Deg":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Rad2Deg;
				return true;
			case "Epsilon":
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Epsilon;
				return true;
			}
			returnValue = null;
	        return false;
	    }

	    public static bool UnityEngineMathfSSet (string memberName, CQ_Value param) {

			return false;
	    }

		public static bool UnityEngineMathfSCall (string functionName, List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual) {
			if(param.Count == 1 && functionName == "ClosestPowerOfTwo" && MatchType(param, new Type[] {typeof(int)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.ClosestPowerOfTwo((int)param[0].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 1 && functionName == "GammaToLinearSpace" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.GammaToLinearSpace((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "LinearToGammaSpace" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.LinearToGammaSpace((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "CorrelatedColorTemperatureToRGB" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Color);
				returnValue.value = UnityEngine.Mathf.CorrelatedColorTemperatureToRGB((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "IsPowerOfTwo" && MatchType(param, new Type[] {typeof(int)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(bool);
				returnValue.value = UnityEngine.Mathf.IsPowerOfTwo((int)param[0].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 1 && functionName == "NextPowerOfTwo" && MatchType(param, new Type[] {typeof(int)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.NextPowerOfTwo((int)param[0].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 2 && functionName == "PerlinNoise" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.PerlinNoise((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "FloatToHalf" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(ushort);
				returnValue.value = UnityEngine.Mathf.FloatToHalf((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "HalfToFloat" && MatchType(param, new Type[] {typeof(ushort)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.HalfToFloat((ushort)param[0].ConvertTo(typeof(ushort)));
				return true;
			}
			if(param.Count == 1 && functionName == "Sin" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Sin((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Cos" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Cos((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Tan" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Tan((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Asin" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Asin((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Acos" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Acos((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Atan" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Atan((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "Atan2" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Atan2((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Sqrt" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Sqrt((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Abs" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Abs((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Abs" && MatchType(param, new Type[] {typeof(int)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.Abs((int)param[0].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 2 && functionName == "Min" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Min((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Min" && MatchType(param, new Type[] {typeof(float[])}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Min((float[])param[0].ConvertTo(typeof(float[])));
				return true;
			}
			if(param.Count == 2 && functionName == "Min" && MatchType(param, new Type[] {typeof(int),typeof(int)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.Min((int)param[0].ConvertTo(typeof(int)),(int)param[1].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 1 && functionName == "Min" && MatchType(param, new Type[] {typeof(int[])}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.Min((int[])param[0].ConvertTo(typeof(int[])));
				return true;
			}
			if(param.Count == 2 && functionName == "Max" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Max((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Max" && MatchType(param, new Type[] {typeof(float[])}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Max((float[])param[0].ConvertTo(typeof(float[])));
				return true;
			}
			if(param.Count == 2 && functionName == "Max" && MatchType(param, new Type[] {typeof(int),typeof(int)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.Max((int)param[0].ConvertTo(typeof(int)),(int)param[1].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 1 && functionName == "Max" && MatchType(param, new Type[] {typeof(int[])}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.Max((int[])param[0].ConvertTo(typeof(int[])));
				return true;
			}
			if(param.Count == 2 && functionName == "Pow" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Pow((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Exp" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Exp((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "Log" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Log((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Log" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Log((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Log10" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Log10((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Ceil" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Ceil((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Floor" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Floor((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Round" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Round((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "CeilToInt" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.CeilToInt((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "FloorToInt" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.FloorToInt((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "RoundToInt" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.RoundToInt((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "Sign" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Sign((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "Clamp" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Clamp((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "Clamp" && MatchType(param, new Type[] {typeof(int),typeof(int),typeof(int)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = UnityEngine.Mathf.Clamp((int)param[0].ConvertTo(typeof(int)),(int)param[1].ConvertTo(typeof(int)),(int)param[2].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 1 && functionName == "Clamp01" && MatchType(param, new Type[] {typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Clamp01((float)param[0].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "Lerp" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Lerp((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "LerpUnclamped" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.LerpUnclamped((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "LerpAngle" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.LerpAngle((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "MoveTowards" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.MoveTowards((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "MoveTowardsAngle" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.MoveTowardsAngle((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "SmoothStep" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.SmoothStep((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "Gamma" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Gamma((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "Approximately" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(bool);
				returnValue.value = UnityEngine.Mathf.Approximately((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "Repeat" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.Repeat((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "PingPong" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.PingPong((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "InverseLerp" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.InverseLerp((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "DeltaAngle" && MatchType(param, new Type[] {typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(float);
				returnValue.value = UnityEngine.Mathf.DeltaAngle((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}

			returnValue = null;
	        return false;
	    }

		public static bool UnityEngineMathfMGet (object objSelf, string memberName, out CQ_Value returnValue) {

			returnValue = null;
			return false;
	    }

		public static bool UnityEngineMathfMSet (object objSelf, string memberName, CQ_Value param) {

			return false;
	    }

		public static bool UnityEngineMathfMCall (object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual) {

			returnValue = null;
	        return false;
	    }

		public static bool UnityEngineMathfIGet(object objSelf, CQ_Value key, out CQ_Value returnValue){

			returnValue = null;
			return false;
		}

		public static bool UnityEngineMathfISet(object objSelf, CQ_Value key, CQ_Value param){

			return false;
		}
	}
}
