using System;

namespace UniPromise {
	public class ResolvedPromise<T> : ActualPromise<T> where T : class {
		public ResolvedPromise (T value) : base (State.Resolved) {
			this.value = value;
		}

		public override Promise<T> Clone () {
			return this;
		}

		public override void Dispose () {
		}
	}
}
