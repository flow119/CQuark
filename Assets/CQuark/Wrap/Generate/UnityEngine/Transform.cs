using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
	
		static Type[] UnityEngineTransform081ed015 = new Type[]{typeof(UnityEngine.Transform)};
		static Type[] UnityEngineTransform3d8cf87f = new Type[]{typeof(UnityEngine.Transform), typeof(bool)};
		static Type[] UnityEngineTransformcbb99d0e = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion)};
		static Type[] UnityEngineTransforma475c239 = new Type[]{typeof(UnityEngine.Vector3)};
		static Type[] UnityEngineTransform2621e916 = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Space)};
		static Type[] UnityEngineTransforma780f8dc = new Type[]{typeof(float), typeof(float), typeof(float)};
		static Type[] UnityEngineTransform290c6cd3 = new Type[]{typeof(float), typeof(float), typeof(float), typeof(UnityEngine.Space)};
		static Type[] UnityEngineTransform79fcdb7c = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Transform)};
		static Type[] UnityEngineTransform385538b9 = new Type[]{typeof(float), typeof(float), typeof(float), typeof(UnityEngine.Transform)};
		static Type[] UnityEngineTransform4fe7dbc3 = new Type[]{typeof(UnityEngine.Vector3), typeof(float)};
		static Type[] UnityEngineTransformc2a8d4cc = new Type[]{typeof(UnityEngine.Vector3), typeof(float), typeof(UnityEngine.Space)};
		static Type[] UnityEngineTransform2e78c2fc = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3), typeof(float)};
		static Type[] UnityEngineTransform3a0b8804 = new Type[]{typeof(UnityEngine.Transform), typeof(UnityEngine.Vector3)};
		static Type[] UnityEngineTransformb15d7b60 = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3)};
		static Type[] UnityEngineTransform000197ef = new Type[]{typeof(int)};
		static Type[] UnityEngineTransformcad56011 = new Type[]{typeof(string)};
		static Type[] UnityEngineTransform05287799 = new Type[]{typeof(System.Type)};
		static Type[] UnityEngineTransformc6af3e03 = new Type[]{typeof(System.Type), typeof(bool)};
		static Type[] UnityEngineTransform002e3aea = new Type[]{typeof(bool)};
		static Type[] UnityEngineTransform6cb40646 = new Type[]{typeof(string), typeof(object), typeof(UnityEngine.SendMessageOptions)};
		static Type[] UnityEngineTransform3397f290 = new Type[]{typeof(string), typeof(object)};
		static Type[] UnityEngineTransform21c19c07 = new Type[]{typeof(string), typeof(UnityEngine.SendMessageOptions)};
		static Type[] UnityEngineTransformc300a33f = new Type[]{typeof(object)};

	
		private static bool UnityEngineTransformSGet (string memberName, out CQ_Value returnValue) {

			returnValue = CQ_Value.Null;
	        return false;
	    }

	    private static bool UnityEngineTransformSSet (string memberName, CQ_Value param) {

			return false;
	    }

		private static bool UnityEngineTransformMGet (object objSelf, string memberName, out CQ_Value returnValue) {
			UnityEngine.Transform obj = (UnityEngine.Transform)objSelf;
			switch(memberName) {
			case "position":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.position);
				return true;
			case "localPosition":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.localPosition);
				return true;
			case "eulerAngles":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.eulerAngles);
				return true;
			case "localEulerAngles":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.localEulerAngles);
				return true;
			case "right":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.right);
				return true;
			case "up":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.up);
				return true;
			case "forward":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.forward);
				return true;
			case "rotation":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Quaternion), obj.rotation);
				return true;
			case "localRotation":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Quaternion), obj.localRotation);
				return true;
			case "localScale":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.localScale);
				return true;
			case "parent":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Transform), obj.parent);
				return true;
			case "worldToLocalMatrix":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Matrix4x4), obj.worldToLocalMatrix);
				return true;
			case "localToWorldMatrix":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Matrix4x4), obj.localToWorldMatrix);
				return true;
			case "root":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Transform), obj.root);
				return true;
			case "childCount":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), obj.childCount);
				return true;
			case "lossyScale":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.lossyScale);
				return true;
			case "hasChanged":
				returnValue = new CQ_Value();
				returnValue.SetBool(obj.hasChanged);
				return true;
			case "hierarchyCapacity":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), obj.hierarchyCapacity);
				return true;
			case "hierarchyCount":
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), obj.hierarchyCount);
				return true;
			case "transform":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Transform), obj.transform);
				return true;
			case "gameObject":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.GameObject), obj.gameObject);
				return true;
			case "tag":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), obj.tag);
				return true;
			case "name":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), obj.name);
				return true;
			case "hideFlags":
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.HideFlags), obj.hideFlags);
				return true;
			}
			returnValue = CQ_Value.Null;
			return false;
	    }

		private static bool UnityEngineTransformMSet (object objSelf, string memberName, CQ_Value param) {
			UnityEngine.Transform obj = (UnityEngine.Transform)objSelf;
			switch(memberName) {
			case "position":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Vector3))){
					obj.position = (UnityEngine.Vector3)param.ConvertTo(typeof(UnityEngine.Vector3));
					return true;
				}
				break;
			case "localPosition":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Vector3))){
					obj.localPosition = (UnityEngine.Vector3)param.ConvertTo(typeof(UnityEngine.Vector3));
					return true;
				}
				break;
			case "eulerAngles":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Vector3))){
					obj.eulerAngles = (UnityEngine.Vector3)param.ConvertTo(typeof(UnityEngine.Vector3));
					return true;
				}
				break;
			case "localEulerAngles":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Vector3))){
					obj.localEulerAngles = (UnityEngine.Vector3)param.ConvertTo(typeof(UnityEngine.Vector3));
					return true;
				}
				break;
			case "right":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Vector3))){
					obj.right = (UnityEngine.Vector3)param.ConvertTo(typeof(UnityEngine.Vector3));
					return true;
				}
				break;
			case "up":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Vector3))){
					obj.up = (UnityEngine.Vector3)param.ConvertTo(typeof(UnityEngine.Vector3));
					return true;
				}
				break;
			case "forward":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Vector3))){
					obj.forward = (UnityEngine.Vector3)param.ConvertTo(typeof(UnityEngine.Vector3));
					return true;
				}
				break;
			case "rotation":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Quaternion))){
					obj.rotation = (UnityEngine.Quaternion)param.ConvertTo(typeof(UnityEngine.Quaternion));
					return true;
				}
				break;
			case "localRotation":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Quaternion))){
					obj.localRotation = (UnityEngine.Quaternion)param.ConvertTo(typeof(UnityEngine.Quaternion));
					return true;
				}
				break;
			case "localScale":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Vector3))){
					obj.localScale = (UnityEngine.Vector3)param.ConvertTo(typeof(UnityEngine.Vector3));
					return true;
				}
				break;
			case "parent":
				if(param.EqualOrImplicateType(typeof(UnityEngine.Transform))){
					obj.parent = (UnityEngine.Transform)param.ConvertTo(typeof(UnityEngine.Transform));
					return true;
				}
				break;
			case "hasChanged":
				if(param.EqualOrImplicateType(typeof(bool))){
					obj.hasChanged = (bool)param.ConvertTo(typeof(bool));
					return true;
				}
				break;
			case "hierarchyCapacity":
				if(param.EqualOrImplicateType(typeof(int))){
					obj.hierarchyCapacity = (int)param.GetNumber();
					return true;
				}
				break;
			case "tag":
				if(param.EqualOrImplicateType(typeof(string))){
					obj.tag = (string)param.ConvertTo(typeof(string));
					return true;
				}
				break;
			case "name":
				if(param.EqualOrImplicateType(typeof(string))){
					obj.name = (string)param.ConvertTo(typeof(string));
					return true;
				}
				break;
			case "hideFlags":
				if(param.EqualOrImplicateType(typeof(UnityEngine.HideFlags))){
					obj.hideFlags = (UnityEngine.HideFlags)param.ConvertTo(typeof(UnityEngine.HideFlags));
					return true;
				}
				break;
			}
			return false;
	    }
		
		private static bool UnityEngineTransformNew(CQ_Value[] param, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTransformSCall (string functionName, CQ_Value[] param, out CQ_Value returnValue, bool mustEqual) {

			returnValue = CQ_Value.Null;
	        return false;
	    }

		private static bool UnityEngineTransformMCall (object objSelf, string functionName, CQ_Value[] param, out CQ_Value returnValue, bool mustEqual) {
			UnityEngine.Transform obj = (UnityEngine.Transform)objSelf;
			int paramCount = param.Length;
			if(paramCount == 1 && functionName == "SetParent" && MatchType(param, UnityEngineTransform081ed015, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SetParent((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(paramCount == 2 && functionName == "SetParent" && MatchType(param, UnityEngineTransform3d8cf87f, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SetParent((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)),(bool)param[1].ConvertTo(typeof(bool)));
				return true;
			}
			if(paramCount == 2 && functionName == "SetPositionAndRotation" && MatchType(param, UnityEngineTransformcbb99d0e, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SetPositionAndRotation((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Quaternion)param[1].ConvertTo(typeof(UnityEngine.Quaternion)));
				return true;
			}
			if(paramCount == 1 && functionName == "Translate" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Translate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(paramCount == 2 && functionName == "Translate" && MatchType(param, UnityEngineTransform2621e916, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Translate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Space)param[1].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(paramCount == 3 && functionName == "Translate" && MatchType(param, UnityEngineTransforma780f8dc, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Translate((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber());
				return true;
			}
			if(paramCount == 4 && functionName == "Translate" && MatchType(param, UnityEngineTransform290c6cd3, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Translate((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber(),(UnityEngine.Space)param[3].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(paramCount == 2 && functionName == "Translate" && MatchType(param, UnityEngineTransform79fcdb7c, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Translate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Transform)param[1].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(paramCount == 4 && functionName == "Translate" && MatchType(param, UnityEngineTransform385538b9, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Translate((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber(),(UnityEngine.Transform)param[3].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(paramCount == 1 && functionName == "Rotate" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Rotate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(paramCount == 2 && functionName == "Rotate" && MatchType(param, UnityEngineTransform2621e916, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Rotate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Space)param[1].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(paramCount == 3 && functionName == "Rotate" && MatchType(param, UnityEngineTransforma780f8dc, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Rotate((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber());
				return true;
			}
			if(paramCount == 4 && functionName == "Rotate" && MatchType(param, UnityEngineTransform290c6cd3, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Rotate((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber(),(UnityEngine.Space)param[3].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(paramCount == 2 && functionName == "Rotate" && MatchType(param, UnityEngineTransform4fe7dbc3, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Rotate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[1].GetNumber());
				return true;
			}
			if(paramCount == 3 && functionName == "Rotate" && MatchType(param, UnityEngineTransformc2a8d4cc, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.Rotate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[1].GetNumber(),(UnityEngine.Space)param[2].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(paramCount == 3 && functionName == "RotateAround" && MatchType(param, UnityEngineTransform2e78c2fc, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.RotateAround((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].GetNumber());
				return true;
			}
			if(paramCount == 1 && functionName == "LookAt" && MatchType(param, UnityEngineTransform081ed015, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.LookAt((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(paramCount == 2 && functionName == "LookAt" && MatchType(param, UnityEngineTransform3a0b8804, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.LookAt((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(paramCount == 2 && functionName == "LookAt" && MatchType(param, UnityEngineTransformb15d7b60, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.LookAt((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(paramCount == 1 && functionName == "LookAt" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.LookAt((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(paramCount == 1 && functionName == "TransformDirection" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.TransformDirection((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 3 && functionName == "TransformDirection" && MatchType(param, UnityEngineTransforma780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.TransformDirection((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "InverseTransformDirection" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.InverseTransformDirection((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 3 && functionName == "InverseTransformDirection" && MatchType(param, UnityEngineTransforma780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.InverseTransformDirection((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "TransformVector" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.TransformVector((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 3 && functionName == "TransformVector" && MatchType(param, UnityEngineTransforma780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.TransformVector((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "InverseTransformVector" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.InverseTransformVector((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 3 && functionName == "InverseTransformVector" && MatchType(param, UnityEngineTransforma780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.InverseTransformVector((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "TransformPoint" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.TransformPoint((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 3 && functionName == "TransformPoint" && MatchType(param, UnityEngineTransforma780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.TransformPoint((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "InverseTransformPoint" && MatchType(param, UnityEngineTransforma475c239, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.InverseTransformPoint((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3))));
				return true;
			}
			if(paramCount == 3 && functionName == "InverseTransformPoint" && MatchType(param, UnityEngineTransforma780f8dc, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Vector3), obj.InverseTransformPoint((float)param[0].GetNumber(),(float)param[1].GetNumber(),(float)param[2].GetNumber()));
				return true;
			}
			if(paramCount == 0 && functionName == "DetachChildren"){
				returnValue = CQ_Value.Null;
				obj.DetachChildren();
				return true;
			}
			if(paramCount == 0 && functionName == "SetAsFirstSibling"){
				returnValue = CQ_Value.Null;
				obj.SetAsFirstSibling();
				return true;
			}
			if(paramCount == 0 && functionName == "SetAsLastSibling"){
				returnValue = CQ_Value.Null;
				obj.SetAsLastSibling();
				return true;
			}
			if(paramCount == 1 && functionName == "SetSiblingIndex" && MatchType(param, UnityEngineTransform000197ef, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SetSiblingIndex((int)param[0].GetNumber());
				return true;
			}
			if(paramCount == 0 && functionName == "GetSiblingIndex"){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), obj.GetSiblingIndex());
				return true;
			}
			if(paramCount == 1 && functionName == "Find" && MatchType(param, UnityEngineTransformcad56011, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Transform), obj.Find((string)param[0].ConvertTo(typeof(string))));
				return true;
			}
			if(paramCount == 1 && functionName == "IsChildOf" && MatchType(param, UnityEngineTransform081ed015, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetBool(obj.IsChildOf((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform))));
				return true;
			}
			if(paramCount == 1 && functionName == "GetChild" && MatchType(param, UnityEngineTransform000197ef, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Transform), obj.GetChild((int)param[0].GetNumber()));
				return true;
			}
			if(paramCount == 1 && functionName == "GetComponent" && MatchType(param, UnityEngineTransform05287799, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component), obj.GetComponent((System.Type)param[0].ConvertTo(typeof(System.Type))));
				return true;
			}
			if(paramCount == 1 && functionName == "GetComponent" && MatchType(param, UnityEngineTransformcad56011, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component), obj.GetComponent((string)param[0].ConvertTo(typeof(string))));
				return true;
			}
			if(paramCount == 2 && functionName == "GetComponentInChildren" && MatchType(param, UnityEngineTransformc6af3e03, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component), obj.GetComponentInChildren((System.Type)param[0].ConvertTo(typeof(System.Type)),(bool)param[1].ConvertTo(typeof(bool))));
				return true;
			}
			if(paramCount == 1 && functionName == "GetComponentInChildren" && MatchType(param, UnityEngineTransform05287799, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component), obj.GetComponentInChildren((System.Type)param[0].ConvertTo(typeof(System.Type))));
				return true;
			}
			if(paramCount == 1 && functionName == "GetComponentsInChildren" && MatchType(param, UnityEngineTransform05287799, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component[]), obj.GetComponentsInChildren((System.Type)param[0].ConvertTo(typeof(System.Type))));
				return true;
			}
			if(paramCount == 2 && functionName == "GetComponentsInChildren" && MatchType(param, UnityEngineTransformc6af3e03, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component[]), obj.GetComponentsInChildren((System.Type)param[0].ConvertTo(typeof(System.Type)),(bool)param[1].ConvertTo(typeof(bool))));
				return true;
			}
			if(paramCount == 1 && functionName == "GetComponentInParent" && MatchType(param, UnityEngineTransform05287799, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component), obj.GetComponentInParent((System.Type)param[0].ConvertTo(typeof(System.Type))));
				return true;
			}
			if(paramCount == 1 && functionName == "GetComponentsInParent" && MatchType(param, UnityEngineTransform05287799, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component[]), obj.GetComponentsInParent((System.Type)param[0].ConvertTo(typeof(System.Type))));
				return true;
			}
			if(paramCount == 2 && functionName == "GetComponentsInParent" && MatchType(param, UnityEngineTransformc6af3e03, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component[]), obj.GetComponentsInParent((System.Type)param[0].ConvertTo(typeof(System.Type)),(bool)param[1].ConvertTo(typeof(bool))));
				return true;
			}
			if(paramCount == 1 && functionName == "GetComponents" && MatchType(param, UnityEngineTransform05287799, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(UnityEngine.Component[]), obj.GetComponents((System.Type)param[0].ConvertTo(typeof(System.Type))));
				return true;
			}
			if(paramCount == 1 && functionName == "CompareTag" && MatchType(param, UnityEngineTransformcad56011, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetBool(obj.CompareTag((string)param[0].ConvertTo(typeof(string))));
				return true;
			}
			if(paramCount == 3 && functionName == "SendMessageUpwards" && MatchType(param, UnityEngineTransform6cb40646, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SendMessageUpwards((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)),(UnityEngine.SendMessageOptions)param[2].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(paramCount == 2 && functionName == "SendMessageUpwards" && MatchType(param, UnityEngineTransform3397f290, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SendMessageUpwards((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)));
				return true;
			}
			if(paramCount == 1 && functionName == "SendMessageUpwards" && MatchType(param, UnityEngineTransformcad56011, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SendMessageUpwards((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(paramCount == 2 && functionName == "SendMessageUpwards" && MatchType(param, UnityEngineTransform21c19c07, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SendMessageUpwards((string)param[0].ConvertTo(typeof(string)),(UnityEngine.SendMessageOptions)param[1].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(paramCount == 3 && functionName == "SendMessage" && MatchType(param, UnityEngineTransform6cb40646, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SendMessage((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)),(UnityEngine.SendMessageOptions)param[2].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(paramCount == 2 && functionName == "SendMessage" && MatchType(param, UnityEngineTransform3397f290, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SendMessage((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)));
				return true;
			}
			if(paramCount == 1 && functionName == "SendMessage" && MatchType(param, UnityEngineTransformcad56011, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SendMessage((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(paramCount == 2 && functionName == "SendMessage" && MatchType(param, UnityEngineTransform21c19c07, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.SendMessage((string)param[0].ConvertTo(typeof(string)),(UnityEngine.SendMessageOptions)param[1].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(paramCount == 3 && functionName == "BroadcastMessage" && MatchType(param, UnityEngineTransform6cb40646, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.BroadcastMessage((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)),(UnityEngine.SendMessageOptions)param[2].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(paramCount == 2 && functionName == "BroadcastMessage" && MatchType(param, UnityEngineTransform3397f290, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.BroadcastMessage((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)));
				return true;
			}
			if(paramCount == 1 && functionName == "BroadcastMessage" && MatchType(param, UnityEngineTransformcad56011, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.BroadcastMessage((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(paramCount == 2 && functionName == "BroadcastMessage" && MatchType(param, UnityEngineTransform21c19c07, mustEqual)){
				returnValue = CQ_Value.Null;
				obj.BroadcastMessage((string)param[0].ConvertTo(typeof(string)),(UnityEngine.SendMessageOptions)param[1].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(paramCount == 0 && functionName == "ToString"){
				returnValue = new CQ_Value();
				returnValue.SetObject(typeof(string), obj.ToString());
				return true;
			}
			if(paramCount == 0 && functionName == "GetInstanceID"){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), obj.GetInstanceID());
				return true;
			}
			if(paramCount == 0 && functionName == "GetHashCode"){
				returnValue = new CQ_Value();
				returnValue.SetNumber(typeof(int), obj.GetHashCode());
				return true;
			}
			if(paramCount == 1 && functionName == "Equals" && MatchType(param, UnityEngineTransformc300a33f, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.SetBool(obj.Equals((object)param[0].ConvertTo(typeof(object))));
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

		private static bool UnityEngineTransformIGet(object objSelf, CQ_Value key, out CQ_Value returnValue){


			returnValue = CQ_Value.Null;
			return false;
		}

		private static bool UnityEngineTransformISet(object objSelf, CQ_Value key, CQ_Value param){

			
			return false;
		}
		
		private static bool UnityEngineTransformAdd(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTransformSub(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTransformMul(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTransformDiv(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
		
		private static bool UnityEngineTransformMod(CQ_Value left, CQ_Value right, out CQ_Value returnValue, bool mustEqual){

			returnValue = CQ_Value.Null;
			return false;
		}
	}
}
