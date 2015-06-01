using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniPromise {
	public class ActualPromise<T> : Promise<T> {
		protected LinkedList<Action<T>> doneCallbacks;
		protected LinkedList<Action<Exception>> failCallbacks;
		protected bool resolved = false;
		protected bool failed = false;
		protected T value;
		protected Exception exception;
		
		public ActualPromise() {
			doneCallbacks = new LinkedList<Action<T>>();
			failCallbacks = new LinkedList<Action<Exception>>();
		}
		
		public override Promise<T> Done (Action<T> doneCallback) {
			if(resolved){
				doneCallback(value);
				return this;
			}
			else if(failed){
				return this;
			}
			else{
				doneCallbacks.AddLast(doneCallback);
				return this;
			}
		}
		
		public override Promise<T> Fail (Action<Exception> failCallback) {
			if(failed){
				failCallback(exception);
				return this;
			}
			else if(resolved){
				return this;
			}
			else{
				failCallbacks.AddLast(failCallback);
				return this;
			}
		}
		
		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			var deferred = new Deferred<U>();
			Done(
				t => done(t)
				.Done(u => deferred.Resolve(u))
				.Fail(e => deferred.Reject(e))
				);
			Fail(e => deferred.Reject(e));
			return deferred;
		}
	}
}
