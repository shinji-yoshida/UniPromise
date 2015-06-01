using UnityEngine;
using System.Collections;
using System;
using UniRx;

namespace UniPromise {
	public class ResolvedPromise<T> : Promise<T> {
		T val;
		
		public ResolvedPromise (T val) {
			this.val = val;
		}
		
		public override Promise<T> Done (Action<T> doneCallback) {
			doneCallback(val);
			return this;
		}
		
		public override Promise<T> Fail (Action<Exception> failedCallback) {
			return this;
		}
		
		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			return done(val);
		}
	}
}
