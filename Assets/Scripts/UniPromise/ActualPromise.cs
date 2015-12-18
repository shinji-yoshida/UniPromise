using System.Collections.Generic;
using System;

namespace UniPromise {
	public abstract class ActualPromise<T> : Promise<T> {
		protected List<Callback> callbacks;
		protected T value;
		protected Exception exception;
		protected State state;
		
		public ActualPromise() {
			callbacks = new List<Callback>();
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
				UniPromiseManager.Instance.AddCallback(() => doneCallback(value));
			else if(this.IsPending)
				callbacks.Add(new Callback(CallbackType.Done, doneCallback));

			return this;
		}
		
		public override Promise<T> Fail (Action<Exception> failCallback) {
			if(this.IsRejected)
				UniPromiseManager.Instance.AddCallback (() => failCallback(exception));
			else if(this.IsPending)
				callbacks.Add(new Callback(CallbackType.Fail, failCallback));

			return this;
		}

		public override Promise<T> Disposed (Action disposedCallback) {
			if(this.IsDisposed)
				UniPromiseManager.Instance.AddCallback(() => disposedCallback());
			else if(this.IsPending)
				callbacks.Add(new Callback(CallbackType.Disposed, disposedCallback));

			return this;
		}

		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			if(this.IsRejected)
				return Promises.Rejected<U>(exception);
			if(this.IsDisposed)
				return Promises.Disposed<U>();

			var deferred = new Deferred<U>();
			Done(
				t => done(t)
						.Done(u => deferred.Resolve(u))
						.Fail(e => deferred.Reject(e))
						.Disposed(() => deferred.Dispose())
				);
			Fail(e => deferred.Reject(e));
			Disposed(() => deferred.Dispose());
			return deferred;
		}

		public override Promise<T> Clone () {
			return this.Then<T>(_ => this);
		}


		protected enum CallbackType {
			Done, Fail, Disposed
		}

		protected struct Callback {
			public readonly CallbackType type;
			object callback;

			public Callback (CallbackType type, object callback) {
				this.type = type;
				this.callback = callback;
			}

			public void CallDone(T value) {
				var action = (Action<T>)callback;
				UniPromiseManager.Instance.AddCallback(() => action(value));
			}
			
			public void CallFail(Exception e) {
				var action = (Action<Exception>)callback;
				UniPromiseManager.Instance.AddCallback(() => action(e));
			}

			public void CallDisposed() {
				UniPromiseManager.Instance.AddCallback((Action)callback);
			}
		}
	}
}
