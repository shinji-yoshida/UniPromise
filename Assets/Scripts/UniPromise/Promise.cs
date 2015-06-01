using UnityEngine;
using System.Collections;
using System;

namespace UniPromise {
	public abstract class Promise<T> {
		public static Promise<T> Resolved(T val) {
			return new ResolvedPromise<T>(val);
		}
		
		public abstract Promise<T> Done(Action<T> doneCallback);
		
		public abstract Promise<T> Fail(Action<Exception> failedCallback);
		
		public abstract Promise<U> Then<U>(Func<T, Promise<U>> done);
		
		public Promise<U> Select<U>(Func<T, U> selector) {
			var result = new Deferred<U>();
			Done(t => result.Resolve(selector(t))).Fail(e => result.Reject(e));
			return result;
		}
	}
}
