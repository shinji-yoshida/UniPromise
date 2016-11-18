using System;

namespace UniPromise {
	public class ResolvedPromise<T> : Promise<T> {
		T val;
		
		public ResolvedPromise (T val) {
			this.val = val;
		}

		public override State State {
			get {
				return State.Resolved;
			}
		}
		
		public override Promise<T> Done (Action<T> doneCallback) {
			try {
				doneCallback(val);
			}
			catch(Exception e) {
				Promises.ReportSinkException (e);
			}
			return this;
		}
		
		public override Promise<T> Fail (Action<Exception> failedCallback) {
			return this;
		}
		
		public override Promise<T> Disposed (Action disposedCallback) {
			return this;
		}
		
		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			try {
				return done(val);
			}
			catch(Exception e) {
				return Promises.Rejected<U> (e);
			}
		}

		public override Promise<U> Then<U> (Func<T, Promise<U>> done, Func<Exception, Promise<U>> fail) {
			try {
				return done(val);
			}
			catch(Exception e) {
				return Promises.Rejected<U> (e);
			}
		}
		
		public override Promise<T> Clone () {
			return this;
		}

		public override void Dispose () {
		}
	}
}
