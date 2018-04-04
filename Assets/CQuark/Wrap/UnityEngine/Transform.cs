using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQuark;
using System;

//这个类由工具自动生成，不需要手动修改

namespace CQuark{
	public partial class Wrap {
		private static bool UnityEngineTransformNew(List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual){

			returnValue = null;
			return false;
		}

		public static bool UnityEngineTransformSGet (string memberName, out CQ_Value returnValue) {

			returnValue = null;
	        return false;
	    }

	    public static bool UnityEngineTransformSSet (string memberName, CQ_Value param) {

			return false;
	    }

		public static bool UnityEngineTransformSCall (string functionName, List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual) {

			returnValue = null;
	        return false;
	    }

		public static bool UnityEngineTransformMGet (object objSelf, string memberName, out CQ_Value returnValue) {
			UnityEngine.Transform obj = (UnityEngine.Transform)objSelf;
			switch(memberName) {
			case "position":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.position;
				return true;
			case "localPosition":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.localPosition;
				return true;
			case "eulerAngles":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.eulerAngles;
				return true;
			case "localEulerAngles":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.localEulerAngles;
				return true;
			case "right":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.right;
				return true;
			case "up":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.up;
				return true;
			case "forward":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.forward;
				return true;
			case "rotation":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Quaternion);
				returnValue.value = obj.rotation;
				return true;
			case "localRotation":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Quaternion);
				returnValue.value = obj.localRotation;
				return true;
			case "localScale":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.localScale;
				return true;
			case "parent":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Transform);
				returnValue.value = obj.parent;
				return true;
			case "worldToLocalMatrix":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Matrix4x4);
				returnValue.value = obj.worldToLocalMatrix;
				return true;
			case "localToWorldMatrix":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Matrix4x4);
				returnValue.value = obj.localToWorldMatrix;
				return true;
			case "root":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Transform);
				returnValue.value = obj.root;
				return true;
			case "childCount":
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = obj.childCount;
				return true;
			case "lossyScale":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.lossyScale;
				return true;
			case "hasChanged":
				returnValue = new CQ_Value();
				returnValue.type = typeof(bool);
				returnValue.value = obj.hasChanged;
				return true;
			case "hierarchyCapacity":
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = obj.hierarchyCapacity;
				return true;
			case "hierarchyCount":
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = obj.hierarchyCount;
				return true;
			case "transform":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Transform);
				returnValue.value = obj.transform;
				return true;
			case "gameObject":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.GameObject);
				returnValue.value = obj.gameObject;
				return true;
			case "tag":
				returnValue = new CQ_Value();
				returnValue.type = typeof(string);
				returnValue.value = obj.tag;
				return true;
			case "rigidbody":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<Rigidbody>();
				return true;
			case "rigidbody2D":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<Rigidbody2D>();
				return true;
			case "camera":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<Camera>();
				return true;
			case "light":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<Light>();
				return true;
			case "animation":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<Animation>();
				return true;
			case "constantForce":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<ConstantForce>();
				return true;
			case "renderer":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<Renderer>();
				return true;
			case "audio":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<AudioSource>();
				return true;
			case "guiText":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<GUIText>();
				return true;
			case "networkView":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<NetworkView>();
				return true;
			case "guiElement":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<GUIElement>();
				return true;
			case "guiTexture":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<GUITexture>();
				return true;
			case "collider":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<Collider>();
				return true;
			case "collider2D":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<Collider2D>();
				return true;
			case "hingeJoint":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<HingeJoint>();
				return true;
			case "particleEmitter":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<ParticleEmitter>();
				return true;
			case "particleSystem":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent<ParticleSystem>();
				return true;
			case "name":
				returnValue = new CQ_Value();
				returnValue.type = typeof(string);
				returnValue.value = obj.name;
				return true;
			case "hideFlags":
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.HideFlags);
				returnValue.value = obj.hideFlags;
				return true;
			}
			returnValue = null;
			return false;
	    }

		public static bool UnityEngineTransformMSet (object objSelf, string memberName, CQ_Value param) {
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
					obj.hierarchyCapacity = (int)param.ConvertTo(typeof(int));
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

		public static bool UnityEngineTransformMCall (object objSelf, string functionName, List<CQ_Value> param, out CQ_Value returnValue, bool mustEqual) {
			UnityEngine.Transform obj = (UnityEngine.Transform)objSelf;
			if(param.Count == 1 && functionName == "SetParent" && MatchType(param, new Type[] {typeof(UnityEngine.Transform)}, mustEqual)){
				returnValue = null;
				obj.SetParent((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(param.Count == 2 && functionName == "SetParent" && MatchType(param, new Type[] {typeof(UnityEngine.Transform),typeof(bool)}, mustEqual)){
				returnValue = null;
				obj.SetParent((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)),(bool)param[1].ConvertTo(typeof(bool)));
				return true;
			}
			if(param.Count == 2 && functionName == "SetPositionAndRotation" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Quaternion)}, mustEqual)){
				returnValue = null;
				obj.SetPositionAndRotation((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Quaternion)param[1].ConvertTo(typeof(UnityEngine.Quaternion)));
				return true;
			}
			if(param.Count == 1 && functionName == "Translate" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = null;
				obj.Translate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Translate" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Space)}, mustEqual)){
				returnValue = null;
				obj.Translate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Space)param[1].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(param.Count == 3 && functionName == "Translate" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = null;
				obj.Translate((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 4 && functionName == "Translate" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float),typeof(UnityEngine.Space)}, mustEqual)){
				returnValue = null;
				obj.Translate((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)),(UnityEngine.Space)param[3].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(param.Count == 2 && functionName == "Translate" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Transform)}, mustEqual)){
				returnValue = null;
				obj.Translate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Transform)param[1].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(param.Count == 4 && functionName == "Translate" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float),typeof(UnityEngine.Transform)}, mustEqual)){
				returnValue = null;
				obj.Translate((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)),(UnityEngine.Transform)param[3].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(param.Count == 1 && functionName == "Rotate" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = null;
				obj.Rotate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "Rotate" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Space)}, mustEqual)){
				returnValue = null;
				obj.Rotate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Space)param[1].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(param.Count == 3 && functionName == "Rotate" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = null;
				obj.Rotate((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 4 && functionName == "Rotate" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float),typeof(UnityEngine.Space)}, mustEqual)){
				returnValue = null;
				obj.Rotate((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)),(UnityEngine.Space)param[3].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(param.Count == 2 && functionName == "Rotate" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = null;
				obj.Rotate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 3 && functionName == "Rotate" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(float),typeof(UnityEngine.Space)}, mustEqual)){
				returnValue = null;
				obj.Rotate((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[1].ConvertTo(typeof(float)),(UnityEngine.Space)param[2].ConvertTo(typeof(UnityEngine.Space)));
				return true;
			}
			if(param.Count == 3 && functionName == "RotateAround" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = null;
				obj.RotateAround((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "LookAt" && MatchType(param, new Type[] {typeof(UnityEngine.Transform)}, mustEqual)){
				returnValue = null;
				obj.LookAt((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(param.Count == 2 && functionName == "LookAt" && MatchType(param, new Type[] {typeof(UnityEngine.Transform),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = null;
				obj.LookAt((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 2 && functionName == "LookAt" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = null;
				obj.LookAt((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(UnityEngine.Vector3)param[1].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 1 && functionName == "LookAt" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = null;
				obj.LookAt((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 1 && functionName == "TransformDirection" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.TransformDirection((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 3 && functionName == "TransformDirection" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.TransformDirection((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "InverseTransformDirection" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.InverseTransformDirection((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 3 && functionName == "InverseTransformDirection" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.InverseTransformDirection((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "TransformVector" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.TransformVector((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 3 && functionName == "TransformVector" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.TransformVector((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "InverseTransformVector" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.InverseTransformVector((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 3 && functionName == "InverseTransformVector" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.InverseTransformVector((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "TransformPoint" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.TransformPoint((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 3 && functionName == "TransformPoint" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.TransformPoint((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "InverseTransformPoint" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.InverseTransformPoint((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)));
				return true;
			}
			if(param.Count == 3 && functionName == "InverseTransformPoint" && MatchType(param, new Type[] {typeof(float),typeof(float),typeof(float)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Vector3);
				returnValue.value = obj.InverseTransformPoint((float)param[0].ConvertTo(typeof(float)),(float)param[1].ConvertTo(typeof(float)),(float)param[2].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 0 && functionName == "DetachChildren"){
				returnValue = null;
				obj.DetachChildren();
				return true;
			}
			if(param.Count == 0 && functionName == "SetAsFirstSibling"){
				returnValue = null;
				obj.SetAsFirstSibling();
				return true;
			}
			if(param.Count == 0 && functionName == "SetAsLastSibling"){
				returnValue = null;
				obj.SetAsLastSibling();
				return true;
			}
			if(param.Count == 1 && functionName == "SetSiblingIndex" && MatchType(param, new Type[] {typeof(int)}, mustEqual)){
				returnValue = null;
				obj.SetSiblingIndex((int)param[0].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 0 && functionName == "GetSiblingIndex"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = obj.GetSiblingIndex();
				return true;
			}
			if(param.Count == 1 && functionName == "Find" && MatchType(param, new Type[] {typeof(string)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Transform);
				returnValue.value = obj.Find((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(param.Count == 1 && functionName == "IsChildOf" && MatchType(param, new Type[] {typeof(UnityEngine.Transform)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(bool);
				returnValue.value = obj.IsChildOf((UnityEngine.Transform)param[0].ConvertTo(typeof(UnityEngine.Transform)));
				return true;
			}
			if(param.Count == 1 && functionName == "FindChild" && MatchType(param, new Type[] {typeof(string)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Transform);
				returnValue.value = obj.Find((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(param.Count == 2 && functionName == "RotateAround" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = null;
				obj.RotateAround((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 2 && functionName == "RotateAroundLocal" && MatchType(param, new Type[] {typeof(UnityEngine.Vector3),typeof(float)}, mustEqual)){
				returnValue = null;
				obj.RotateAroundLocal((UnityEngine.Vector3)param[0].ConvertTo(typeof(UnityEngine.Vector3)),(float)param[1].ConvertTo(typeof(float)));
				return true;
			}
			if(param.Count == 1 && functionName == "GetChild" && MatchType(param, new Type[] {typeof(int)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Transform);
				returnValue.value = obj.GetChild((int)param[0].ConvertTo(typeof(int)));
				return true;
			}
			if(param.Count == 0 && functionName == "GetChildCount"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = obj.GetChildCount();
				return true;
			}
			if(param.Count == 1 && functionName == "GetComponent" && MatchType(param, new Type[] {typeof(System.Type)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent((System.Type)param[0].ConvertTo(typeof(System.Type)));
				return true;
			}
			if(param.Count == 1 && functionName == "GetComponent" && MatchType(param, new Type[] {typeof(string)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponent((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(param.Count == 2 && functionName == "GetComponentInChildren" && MatchType(param, new Type[] {typeof(System.Type),typeof(bool)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponentInChildren((System.Type)param[0].ConvertTo(typeof(System.Type)),(bool)param[1].ConvertTo(typeof(bool)));
				return true;
			}
			if(param.Count == 1 && functionName == "GetComponentInChildren" && MatchType(param, new Type[] {typeof(System.Type)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponentInChildren((System.Type)param[0].ConvertTo(typeof(System.Type)));
				return true;
			}
			if(param.Count == 1 && functionName == "GetComponentsInChildren" && MatchType(param, new Type[] {typeof(System.Type)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component[]);
				returnValue.value = obj.GetComponentsInChildren((System.Type)param[0].ConvertTo(typeof(System.Type)));
				return true;
			}
			if(param.Count == 2 && functionName == "GetComponentsInChildren" && MatchType(param, new Type[] {typeof(System.Type),typeof(bool)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component[]);
				returnValue.value = obj.GetComponentsInChildren((System.Type)param[0].ConvertTo(typeof(System.Type)),(bool)param[1].ConvertTo(typeof(bool)));
				return true;
			}
			if(param.Count == 1 && functionName == "GetComponentInParent" && MatchType(param, new Type[] {typeof(System.Type)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component);
				returnValue.value = obj.GetComponentInParent((System.Type)param[0].ConvertTo(typeof(System.Type)));
				return true;
			}
			if(param.Count == 1 && functionName == "GetComponentsInParent" && MatchType(param, new Type[] {typeof(System.Type)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component[]);
				returnValue.value = obj.GetComponentsInParent((System.Type)param[0].ConvertTo(typeof(System.Type)));
				return true;
			}
			if(param.Count == 2 && functionName == "GetComponentsInParent" && MatchType(param, new Type[] {typeof(System.Type),typeof(bool)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component[]);
				returnValue.value = obj.GetComponentsInParent((System.Type)param[0].ConvertTo(typeof(System.Type)),(bool)param[1].ConvertTo(typeof(bool)));
				return true;
			}
			if(param.Count == 1 && functionName == "GetComponents" && MatchType(param, new Type[] {typeof(System.Type)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(UnityEngine.Component[]);
				returnValue.value = obj.GetComponents((System.Type)param[0].ConvertTo(typeof(System.Type)));
				return true;
			}
			if(param.Count == 1 && functionName == "CompareTag" && MatchType(param, new Type[] {typeof(string)}, mustEqual)){
				returnValue = new CQ_Value();
				returnValue.type = typeof(bool);
				returnValue.value = obj.CompareTag((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(param.Count == 3 && functionName == "SendMessageUpwards" && MatchType(param, new Type[] {typeof(string),typeof(object),typeof(UnityEngine.SendMessageOptions)}, mustEqual)){
				returnValue = null;
				obj.SendMessageUpwards((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)),(UnityEngine.SendMessageOptions)param[2].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(param.Count == 2 && functionName == "SendMessageUpwards" && MatchType(param, new Type[] {typeof(string),typeof(object)}, mustEqual)){
				returnValue = null;
				obj.SendMessageUpwards((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)));
				return true;
			}
			if(param.Count == 1 && functionName == "SendMessageUpwards" && MatchType(param, new Type[] {typeof(string)}, mustEqual)){
				returnValue = null;
				obj.SendMessageUpwards((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(param.Count == 2 && functionName == "SendMessageUpwards" && MatchType(param, new Type[] {typeof(string),typeof(UnityEngine.SendMessageOptions)}, mustEqual)){
				returnValue = null;
				obj.SendMessageUpwards((string)param[0].ConvertTo(typeof(string)),(UnityEngine.SendMessageOptions)param[1].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(param.Count == 3 && functionName == "SendMessage" && MatchType(param, new Type[] {typeof(string),typeof(object),typeof(UnityEngine.SendMessageOptions)}, mustEqual)){
				returnValue = null;
				obj.SendMessage((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)),(UnityEngine.SendMessageOptions)param[2].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(param.Count == 2 && functionName == "SendMessage" && MatchType(param, new Type[] {typeof(string),typeof(object)}, mustEqual)){
				returnValue = null;
				obj.SendMessage((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)));
				return true;
			}
			if(param.Count == 1 && functionName == "SendMessage" && MatchType(param, new Type[] {typeof(string)}, mustEqual)){
				returnValue = null;
				obj.SendMessage((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(param.Count == 2 && functionName == "SendMessage" && MatchType(param, new Type[] {typeof(string),typeof(UnityEngine.SendMessageOptions)}, mustEqual)){
				returnValue = null;
				obj.SendMessage((string)param[0].ConvertTo(typeof(string)),(UnityEngine.SendMessageOptions)param[1].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(param.Count == 3 && functionName == "BroadcastMessage" && MatchType(param, new Type[] {typeof(string),typeof(object),typeof(UnityEngine.SendMessageOptions)}, mustEqual)){
				returnValue = null;
				obj.BroadcastMessage((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)),(UnityEngine.SendMessageOptions)param[2].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(param.Count == 2 && functionName == "BroadcastMessage" && MatchType(param, new Type[] {typeof(string),typeof(object)}, mustEqual)){
				returnValue = null;
				obj.BroadcastMessage((string)param[0].ConvertTo(typeof(string)),(object)param[1].ConvertTo(typeof(object)));
				return true;
			}
			if(param.Count == 1 && functionName == "BroadcastMessage" && MatchType(param, new Type[] {typeof(string)}, mustEqual)){
				returnValue = null;
				obj.BroadcastMessage((string)param[0].ConvertTo(typeof(string)));
				return true;
			}
			if(param.Count == 2 && functionName == "BroadcastMessage" && MatchType(param, new Type[] {typeof(string),typeof(UnityEngine.SendMessageOptions)}, mustEqual)){
				returnValue = null;
				obj.BroadcastMessage((string)param[0].ConvertTo(typeof(string)),(UnityEngine.SendMessageOptions)param[1].ConvertTo(typeof(UnityEngine.SendMessageOptions)));
				return true;
			}
			if(param.Count == 0 && functionName == "ToString"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(string);
				returnValue.value = obj.ToString();
				return true;
			}
			if(param.Count == 0 && functionName == "GetInstanceID"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(int);
				returnValue.value = obj.GetInstanceID();
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
			if(param.Count == 0 && functionName == "GetType"){
				returnValue = new CQ_Value();
				returnValue.type = typeof(System.Type);
				returnValue.value = obj.GetType();
				return true;
			}
			
			returnValue = null;
	        return false;
	    }

		public static bool UnityEngineTransformIGet(object objSelf, CQ_Value key, out CQ_Value returnValue){

			returnValue = null;
			return false;
		}

		public static bool UnityEngineTransformISet(object objSelf, CQ_Value key, CQ_Value param){

			return false;
		}
	}
}
