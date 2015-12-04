using System;

namespace UniPromise {
	public class Deferred<T> : ActualPromise<T> {
		public void Resolve(T val) {
			if(this.IsNotPending)
				return;
			state = State.Resolved;
			this.value = val;
			foreach(var each in callbacks) {
				if(each.type == CallbackType.Done)
					each.Done(val);
			}
			ClearCallbacks();
		}
		
		public void Reject(Exception e) {
			if(this.IsNotPending)
				return;
			state = State.Rejected;
			this.exception = e;
			foreach(var each in callbacks) {
				if(each.type == CallbackType.Fail)
					each.Fail(e);
			}
			ClearCallbacks();
		}

		public override void Dispose () {
			if(this.IsNotPending)
				return;
			state = State.Disposed;
			foreach(var each in callbacks) {
				if(each.type == CallbackType.Disposed)
					each.Disposed();
			}
			ClearCallbacks();
		}

		void ClearCallbacks() {
			callbacks.Clear();
			callbacks = null;
		}
	}
}