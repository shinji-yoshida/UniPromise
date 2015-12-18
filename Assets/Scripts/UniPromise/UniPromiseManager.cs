using UnityEngine;
using System.Collections.Generic;
using System;
using UniPromise.Internal;

namespace UniPromise {
	public class UniPromiseManager : MonoBehaviour, IUniPromiseManager {
		internal static IUniPromiseManager soleInstance;
		CallbackUpdater callbackUpdater;

		internal static IUniPromiseManager Instance {
			get {
				if(soleInstance != null)
					return soleInstance;
				
				var go = new GameObject("UniPromiseManager", typeof(UniPromiseManager));
				soleInstance = go.GetComponent<UniPromiseManager>();
				return soleInstance;
			}
		}

		internal static void Reset(IUniPromiseManager newInstance) {
			soleInstance = newInstance;
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
			callbackUpdater = new CallbackUpdater();
		}

		protected void Update() {
			callbackUpdater.Update();
		}

//		internal void AddCallback(Action callback) {
//			callbackUpdater.AddCallback(callback);
//		}

		void IUniPromiseManager.AddCallback (Action callback) {
			callbackUpdater.AddCallback(callback);
		}
	}
}