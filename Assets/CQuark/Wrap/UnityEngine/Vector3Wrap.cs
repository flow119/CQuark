using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQuark{
	public partial class UnityWrap {

		private static bool Vector3New(List<CQ_Value> param, out CQ_Value cqVal){
			cqVal = new CQ_Value();
			cqVal.type = typeof(Vector3);
			cqVal.value = new Vector3(param[0].GetFloat(), param[1].GetFloat(), param[2].GetFloat());
			return true;
		}

		private static bool Vector3SGet(string memberName, out CQ_Value cqVal){
			switch(memberName) {
			case "up":
				cqVal = new CQ_Value();
				cqVal.type = typeof(Vector3);
				cqVal.value = Vector3.up;
				return true;
			case "down":
				cqVal = new CQ_Value();
				cqVal.type = typeof(Vector3);
				cqVal.value = Vector3.down;
				return true;
			case "forward":
				cqVal = new CQ_Value();
				cqVal.type = typeof(Vector3);
				cqVal.value = Vector3.forward;
				return true;
			case "back":
				cqVal = new CQ_Value();
				cqVal.type = typeof(Vector3);
				cqVal.value = Vector3.back;
				return true;
			case "left":
				cqVal = new CQ_Value();
				cqVal.type = typeof(Vector3);
				cqVal.value = Vector3.left;
				return true;
			case "right":
				cqVal = new CQ_Value();
				cqVal.type = typeof(Vector3);
				cqVal.value = Vector3.right;
				return true;
			case "one":
				cqVal = new CQ_Value();
				cqVal.type = typeof(Vector3);
				cqVal.value = Vector3.one;
				return true;
			case "zero":
				cqVal = new CQ_Value();
				cqVal.type = typeof(Vector3);
				cqVal.value = Vector3.zero;
				return true;
			}
			cqVal = null;
			return false;
		}

		private static bool Vector3SSet(string memberName, CQ_Value param){
			return false;
		}

		private static bool Vector3MGet(object obj_this, string memberName, out CQ_Value cqVal){
			Vector3 obj = (Vector3)obj_this;
			switch(memberName){
			case "x":
				cqVal = new CQ_Value();
				cqVal.type = typeof(float);
				cqVal.value = obj.x;
				return true;
			case "y":
				cqVal = new CQ_Value();
				cqVal.type = typeof(float);
				cqVal.value = obj.y;
				return true;
			case "z":
				cqVal = new CQ_Value();
				cqVal.type = typeof(float);
				cqVal.value = obj.z;
				return true;
			}
			cqVal = null;
			return false;
		}

		private static bool Vector3MSet(object obj_this, string memberName, CQ_Value param){
			Vector3 obj = (Vector3)obj_this;
			switch(memberName){
			case "x":
				obj.x = param.GetFloat();
				return true;
			case "y":
				obj.y = param.GetFloat();
				return true;
			case "z":
				obj.z = param.GetFloat();
				return true;
			}
			return false;
		}
	}
}
