using UnityEngine;
using System.Collections;
using CQuark;

namespace CQuark{
	public class CQuarkBehaviourBase : MonoBehaviour {
		
		protected CQ_Content content;
//		protected MethodCache cache = new MethodCache();

        protected virtual void Initialize() { }

		protected virtual void CallScript(string method){
			CallScript (method, false);
		}

		protected virtual void CallScript(string method, bool useCorountine){
		}


		void Awake(){
			Initialize ();
			CallScript ("Awake");
		}

		void OnEnable(){
			CallScript ("OnEnable");
		}

		void OnDisable(){
			CallScript ("OnDisable");
		}

		void Start () {
			CallScript ("Start");
		}

		void OnDestroy(){
			CallScript ("OnDestroy");
		}
	}
}
