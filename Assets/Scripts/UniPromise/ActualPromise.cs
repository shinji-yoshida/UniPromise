using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniPromise {
	public abstract class ActualPromise<T> : Promise<T> {
		protected LinkedList<Action<T>> doneCallbacks;
		protected LinkedList<Action<Exception>> failCallbacks;
		protected T value;
		protected Exception exception;
		protected State state;
		
		public ActualPromise() {
			doneCallbacks = new LinkedList<Action<T>>();
			failCallbacks = new LinkedList<Action<Exception>>();
			state = State.Pending;
		}

		public override State State {
			get {
				return state;
			}
		}
		
		public override Promise<T> Done (Action<T> doneCallback) {
			if(doneCallback == null)
				throw new Exception("doneCallback is null");

			if(this.IsResolved)
				doneCallback(value);
			else if(this.IsPending)
				doneCallbacks.AddLast(doneCallback);

			return this;
		}
		
		public override Promise<T> Fail (Action<Exception> failCallback) {
			if(this.IsRejected)
				failCallback(exception);
			else if(this.IsPending)
				failCallbacks.AddLast(failCallback);

			return this;
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
