using System;

namespace UniPromise {
	public class DisposedPromise<T> : ActualPromise<T> where T : class {
		public DisposedPromise () : base(State.Disposed) {
		}

		public override Promise<T> Clone () {
			return this;
		}

		public override void Dispose () {
		}
	}
}

