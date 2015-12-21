using System;

namespace UniPromise {
	public class DisposedPromise<T> : Promise<T> {
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
			disposedCallback();
			return this;
		}
		
		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			return new DisposedPromise<U>();
		}

		public override Promise<T> Clone () {
			return this;
		}

		public override void Dispose () {
		}
	}
}

