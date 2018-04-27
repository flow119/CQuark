using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
	
		static Type[] UnityEngineMathf000197ef = new Type[]{typeof(int)};
		static Type[] UnityEngineMathf05d0225c = new Type[]{typeof(float)};
		static Type[] UnityEngineMathfc3c31980 = new Type[]{typeof(float), typeof(float)};
		static Type[] UnityEngineMathfce2c8527 = new Type[]{typeof(ushort)};
		static Type[] UnityEngineMathfd25106be = new Type[]{typeof(float[])};
		static Type[] UnityEngineMathfb97145a0 = new Type[]{typeof(int), typeof(int)};
		static Type[] UnityEngineMathf05fb6391 = new Type[]{typeof(int[])};
		static Type[] UnityEngineMathfa780f8dc = new Type[]{typeof(float), typeof(float), typeof(float)};
		static Type[] UnityEngineMathf2496ee4f = new Type[]{typeof(int), typeof(int), typeof(int)};

	
		private static bool UnityEngineMathfSGet (string memberName, out CQ_Value returnValue) {
			switch(memberName) {
			case "PI":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.PI);
				return true;
			case "Infinity":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Infinity);
				return true;
			case "NegativeInfinity":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.NegativeInfinity);
				return true;
			case "Deg2Rad":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Deg2Rad);
				return true;
			case "Rad2Deg":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Rad2Deg);
				return true;
			case "Epsilon":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Epsilon);
				return true;
			}
			returnValue = CQ_Value.Null;
	        return false;
	    }

	    private static bool UnityEngineMathfSSet (string memberName, CQ_Value param) {

			return false;
	    }

		private static bool UnityEngineMathfMGet (object objSelf, string memberName, out CQ_Value returnValue) {

			returnValue = CQ_Value.Null;
			return false;
	    }

		private static bool UnityEngineMathfMSet (object objSelf, string memberName, CQ_Value param) {

			return false;
	    }
		
		private static bool UnityEngineMathfNew(CQ_Value[] param, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineMathfSCall (string functionName, CQ_Value[] param, out CQ_Value returnValue, bool mustEqual) {
			int paramCount = param.Length;
			if(paramCount == 1 && functionName == "ClosestPowerOfTwo" && MatchType(param, UnityEngineMathf000197ef, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.ClosestPowerOfTwo((int)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "GammaToLinearSpace" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.GammaToLinearSpace((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "LinearToGammaSpace" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.LinearToGammaSpace((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "CorrelatedColorTemperatureToRGB" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Color), UnityEngine.Mathf.CorrelatedColorTemperatureToRGB((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "IsPowerOfTwo" && MatchType(param, UnityEngineMathf000197ef, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetBool(UnityEngine.Mathf.IsPowerOfTwo((int)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "NextPowerOfTwo" && MatchType(param, UnityEngineMathf000197ef, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.NextPowerOfTwo((int)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "PerlinNoise" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.PerlinNoise((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "FloatToHalf" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(ushort), UnityEngine.Mathf.FloatToHalf((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "HalfToFloat" && MatchType(param, UnityEngineMathfce2c8527, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.HalfToFloat((ushort)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Sin" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Sin((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Cos" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Cos((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Tan" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Tan((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Asin" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Asin((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Acos" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Acos((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Atan" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Atan((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "Atan2" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Atan2((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Sqrt" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Sqrt((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Abs" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Abs((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Abs" && MatchType(param, UnityEngineMathf000197ef, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.Abs((int)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "Min" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Min((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Min" && MatchType(param, UnityEngineMathfd25106be, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Min((float[])param[0].ConvertTo(typeof(float[]))));
				return true;
			}
			if(paramCount == 2 && functionName == "Min" && MatchType(param, UnityEngineMathfb97145a0, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.Min((int)param[0].GetNumber(),(int)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Min" && MatchType(param, UnityEngineMathf05fb6391, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.Min((int[])param[0].ConvertTo(typeof(int[]))));
				return true;
			}
			if(paramCount == 2 && functionName == "Max" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Max((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Max" && MatchType(param, UnityEngineMathfd25106be, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Max((float[])param[0].ConvertTo(typeof(float[]))));
				return true;
			}
			if(paramCount == 2 && functionName == "Max" && MatchType(param, UnityEngineMathfb97145a0, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.Max((int)param[0].GetNumber(),(int)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Max" && MatchType(param, UnityEngineMathf05fb6391, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.Max((int[])param[0].ConvertTo(typeof(int[]))));
				return true;
			}
			if(paramCount == 2 && functionName == "Pow" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Pow((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Exp" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Exp((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "Log" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Log((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Log" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Log((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Log10" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Log10((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Ceil" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Ceil((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Floor" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Floor((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Round" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Round((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "CeilToInt" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.CeilToInt((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "FloorToInt" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.FloorToInt((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "RoundToInt" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.RoundToInt((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Sign" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Sign((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "Clamp" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Clamp((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "Clamp" && MatchType(param, UnityEngineMathf2496ee4f, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), UnityEngine.Mathf.Clamp((int)param[0].GetNumber(),(int)param[1].GetNumber(),(int)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "Clamp01" && MatchType(param, UnityEngineMathf05d0225c, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Clamp01((float)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "Lerp" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Lerp((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "LerpUnclamped" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.LerpUnclamped((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "LerpAngle" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.LerpAngle((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "MoveTowards" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.MoveTowards((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "MoveTowardsAngle" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.MoveTowardsAngle((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "SmoothStep" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.SmoothStep((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "Gamma" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Gamma((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "Approximately" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetBool(UnityEngine.Mathf.Approximately((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "Repeat" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.Repeat((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "PingPong" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.PingPong((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}
			if(paramCount == 3 && functionName == "InverseLerp" && MatchType(param, UnityEngineMathfa780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.InverseLerp((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 2 && functionName == "DeltaAngle" && MatchType(param, UnityEngineMathfc3c31980, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(float), UnityEngine.Mathf.DeltaAngle((float)param[0].GetNumber(),(float)param[1].GetNumber()));
				return true;
			}

			returnValue = CQ_Value.Null;
	        return false;
	    }

		private static bool UnityEngineMathfMCall (object objSelf, string functionName, CQ_Value[] param, out CQ_Value returnValue, bool mustEqual) {

			returnValue = CQ_Value.Null;
	        return false;
	    }

		private static bool UnityEngineMathfIGet(object objSelf, CQ_Value key, out CQ_Value returnValue){


			returnValue = CQ_Value.Null;
			return false;
		}

		private static bool UnityEngineMathfISet(object objSelf, CQ_Value key, CQ_Value param){

			
			return false;
		}
		
		private static bool UnityEngineMathfAdd(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineMathfSub(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineMathfMul(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineMathfDiv(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineMathfMod(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
	}
}
