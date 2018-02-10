using System;


namespace UniPromise {
	public class RejectedPromise<T> : ActualPromise<T> where T : class {
		public RejectedPromise (Exception e) : base(State.Rejected) {
			this.exception = e;
		}

		public override Promise<T> Clone () {
			return this;
		}

		public override void Dispose () {
		}
	}
}
