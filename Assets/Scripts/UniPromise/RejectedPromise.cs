using UnityEngine;
using System.Collections;
using System;


namespace UniPromise {
	public class RejectedPromise<T> : Promise<T> {
		Exception e;
		
		public RejectedPromise (Exception e) {
			this.e = e;
		}
		
		public override Promise<T> Done (Action<T> doneCallback) {
			return this;
		}
		
		public override Promise<T> Fail (Action<Exception> failedCallback) {
			failedCallback(e);
			return this;
		}
		
		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			return this;
		}
	}
}
