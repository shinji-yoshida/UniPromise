using System.Collections.Generic;
using System;
using UniPromise.Internal;

namespace UniPromise {
	public abstract class ActualPromise<T> : AbstractPromise<T> where T : class {
		protected List<Callback<T>> callbacks;
		protected T value;
		protected Exception exception;
		protected State state;

		protected ActualPromise (State state) {
			this.state = state;
		}
		
		
		protected ActualPromise() {
			callbacks = new List<Callback<T>>();
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

			if (this.IsResolved) {
				Internal.ThreadStaticDispatcher.Instance.DispatchDone (doneCallback, value);
			} else if (this.IsPending) {
				callbacks.Add (new Callback<T> (CallbackType.Done, doneCallback));
			}

			return this;
		}
		
		public override Promise<T> Fail (Action<Exception> failCallback) {
			if (this.IsRejected) {
				ThreadStaticDispatcher.Instance.DispatchFail<T> (failCallback, exception);
			} else if (this.IsPending) {
				callbacks.Add (new Callback<T> (CallbackType.Fail, failCallback));
			}

			return this;
		}

		public override Promise<T> Disposed (Action disposedCallback) {
			if (this.IsDisposed) {
				ThreadStaticDispatcher.Instance.DispatchDisposed<T> (disposedCallback);
			} else if (this.IsPending) {
				callbacks.Add (new Callback<T> (CallbackType.Disposed, disposedCallback));
			}

			return this;
		}

		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			if(this.IsRejected)
				return Promises.Rejected<U>(exception);
			if(this.IsDisposed)
				return Promises.Disposed<U>();

			var deferred = new Deferred<U>();
			Done(t => {
				try {
					done(t)
						.Done(u => deferred.Resolve(u))
						.Fail(e => deferred.Reject(e))
						.Disposed(() => deferred.Dispose());
				}
				catch(Exception e) {
					deferred.Reject(e);
				}
			});
			Fail(e => deferred.Reject(e));
			Disposed(() => deferred.Dispose());
			return deferred;
		}

		public override Promise<U> Then<U> (Func<T, Promise<U>> done, Func<Exception, Promise<U>> fail) {
			if(this.IsDisposed)
				return Promises.Disposed<U>();

			var deferred = new Deferred<U>();
			Done(t => {
				try {
					done(t)
						.Done(u => deferred.Resolve(u))
						.Fail(e => deferred.Reject(e))
						.Disposed(() => deferred.Dispose());
				}
				catch(Exception e) {
					deferred.Reject(e);
				}
			});
			Fail (e => {
				try {
					fail (e)
						.Done (u => deferred.Resolve (u))
						.Fail (e2 => deferred.Reject (e2))
						.Disposed (() => deferred.Dispose ());
				}
				catch(Exception exceptionFromFail) {
					deferred.Reject(exceptionFromFail);
				}
			});
			Disposed(() => deferred.Dispose());
			return deferred;
		}

		public override Promise<U> Then<U> (
			Func<T, Promise<U>> done, Func<Exception, Promise<U>> fail, Func<Promise<U>> disposed)
		{
			var deferred = new Deferred<U>();
			Done(t => {
				try {
					done(t)
						.Done(u => deferred.Resolve(u))
						.Fail(e => deferred.Reject(e))
						.Disposed(() => deferred.Dispose());
				}
				catch(Exception e) {
					deferred.Reject(e);
				}
			});
			Fail (e => {
				try {
					fail (e)
						.Done (u => deferred.Resolve (u))
						.Fail (e2 => deferred.Reject (e2))
						.Disposed (() => deferred.Dispose ());
				}
				catch(Exception exceptionFromFail) {
					deferred.Reject(exceptionFromFail);
				}
			});
			Disposed(() => {
				try {
					disposed()
						.Done(u => deferred.Resolve(u))
						.Fail(e => deferred.Reject(e))
						.Disposed(() => deferred.Dispose());
				}
				catch(Exception e) {
					deferred.Reject(e);
				}
			});
			return deferred;
		}

		public override Promise<T> Clone () {
			return this.Then<T>(_ => this);
		}
	}
}
