using UnityEngine;
using System.Collections;
using System;

namespace UniPromise {
	public class Deferred<T> : ActualPromise<T> {
		public void Resolve(T val) {
			if(resolved || failed)
				return;
			resolved = true;
			this.value = val;
			foreach(var each in doneCallbacks)
				each(val);
			doneCallbacks.Clear();
		}
		
		public void Reject(Exception e) {
			if(resolved || failed)
				return;
			failed = true;
			this.exception = e;
			foreach(var each in failCallbacks)
				each(e);
			failCallbacks.Clear();
		}
	}
}