using System;
using UniPromise.Internal;
using System.Linq;

namespace UniPromise {
	public class Deferred<T> : ActualPromise<T> where T : class {
		public void Resolve(T val) {
			if(this.IsNotPending)
				return;
			state = State.Resolved;
			this.value = val;

			var dispatchables = callbacks
				.Where(cb => cb.type == CallbackType.Done)
				.Select(cb => new DoneDispatchable<T> (cb, val) as Dispatchable);
			ThreadStaticDispatcher.Instance.Dispatch (dispatchables);

			ClearCallbacks();
		}
		
		public void Reject(Exception e) {
			if(this.IsNotPending)
				return;
			state = State.Rejected;
			this.exception = e;

			var dispatchables = callbacks
				.Where(cb => cb.type == CallbackType.Fail)
				.Select(cb => new FailDispatchable<T> (cb, e) as Dispatchable);
			ThreadStaticDispatcher.Instance.Dispatch (dispatchables);

			ClearCallbacks();
		}

		public void Propagate(Promise<T> source) {
			source.Done (Resolve).Fail (Reject).Disposed (Dispose);
		}

		public override void Dispose () {
			if(this.IsNotPending)
				return;
			state = State.Disposed;

			var dispatchables = callbacks
				.Where(cb => cb.type == CallbackType.Disposed)
				.Select(cb => new DisposedDispatchable<T> (cb) as Dispatchable);
			ThreadStaticDispatcher.Instance.Dispatch (dispatchables);

			ClearCallbacks();
		}

		void ClearCallbacks() {
			callbacks.Clear();
			callbacks = null;
		}
	}
}