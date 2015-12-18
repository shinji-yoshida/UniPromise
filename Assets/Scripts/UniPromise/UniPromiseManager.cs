using UnityEngine;
using System.Collections.Generic;
using System;

namespace UniPromise {
	public class UniPromiseManager : MonoBehaviour {
		protected static UniPromiseManager soleInstance;
		List<Action> callbacks;

		public static UniPromiseManager Instance {
			get {
				if(soleInstance != null)
					return soleInstance;
				
				var go = new GameObject("UniPromiseManager", typeof(UniPromiseManager));
				soleInstance = go.GetComponent<UniPromiseManager>();
				return soleInstance;
			}
		}

		protected void Awake() {
			if(soleInstance == null || soleInstance == this) {
				soleInstance = this;
				DontDestroyOnLoad(gameObject);
				
				Init();
			}
			else {
				DestroyImmediate(this);
			}
		}
		
		void Init () {
			callbacks = new List<Action>();
		}

		protected void Update() {
			if(callbacks.Count == 0)
				return;

			for(int i = 0; i < callbacks.Count ; i++) { // callbacks.Count may be changed while looping
				callbacks[i]();
			}
			callbacks.Clear();
		}

		internal void AddCallback(Action callback) {
			callbacks.Add(callback);
		}
	}
}