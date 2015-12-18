using System.Collections.Generic;
using System;

namespace UniPromise.Internal {

	public class CallbackUpdater {
		List<Action> callbacks;

		public CallbackUpdater() {
			callbacks = new List<Action>();
		}

		internal void AddCallback(Action callback) {
			callbacks.Add(callback);
		}

		public void Update() {
			if(callbacks.Count == 0)
				return;
			
			for(int i = 0; i < callbacks.Count; i++) { // callbacks.Count may be changed while looping
				callbacks[i]();
			}
			callbacks.Clear();
		}
	}
}