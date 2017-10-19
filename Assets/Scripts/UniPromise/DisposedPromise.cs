using System;

namespace UniPromise {
	public class DisposedPromise<T> : AbstractPromise<T> where T : class {
		public DisposedPromise () {
		}

		public override State State {
			get {
				return State.Disposed;
			}
		}
		
		public override Promise<T> Done (Action<T> doneCallback) {
			return this;
		}
		
		public override Promise<T> Fail (Action<Exception> failedCallback) {
			return this;
		}

		public override Promise<T> Disposed (Action disposedCallback) {
			try {
				disposedCallback();
			}
			catch(Exception e) {
				Promises.ReportSinkException (e);
			}
			return this;
		}
		
		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			return new DisposedPromise<U>();
		}

		public override Promise<U> Then<U> (Func<T, Promise<U>> done, Func<Exception, Promise<U>> fail) {
			return new DisposedPromise<U>();
		}

		public override Promise<U> Then<U> (
			Func<T, Promise<U>> done, Func<Exception, Promise<U>> fail, Func<Promise<U>> disposed)
		{
			try {
				return disposed();
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

