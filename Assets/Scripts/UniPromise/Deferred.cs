using System;

namespace UniPromise {
	public class Deferred<T> : ActualPromise<T> {
		public void Resolve(T val) {
			if(this.IsNotPending)
				return;
			state = State.Resolved;
			this.value = val;
			foreach(var each in doneCallbacks)
				each(val);
			ClearCallbacks();
		}
		
		public void Reject(Exception e) {
			if(this.IsNotPending)
				return;
			state = State.Rejected;
			this.exception = e;
			foreach(var each in failCallbacks)
				each(e);
			ClearCallbacks();
		}

		public override void Dispose () {
			if(this.IsNotPending)
				return;
			state = State.Disposed;
			ClearCallbacks();
		}

		void ClearCallbacks() {
			doneCallbacks.Clear();
			failCallbacks.Clear();
		}
	}
}