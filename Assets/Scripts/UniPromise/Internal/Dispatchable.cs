using System;


namespace UniPromise.Internal {
	public interface Dispatchable {
		void Dispatch();
	}

	public class DoneDispatchable<T> : Dispatchable where T : class {
		Callback<T> callback;
		T val;

		public DoneDispatchable (Callback<T> callback, T val) {
			this.callback = callback;
			this.val = val;
		}

		public void Dispatch () {
			callback.CallDone (val);
		}
	}

	public class FailDispatchable<T> : Dispatchable where T : class {
		Callback<T> callback;
		Exception e;

		public FailDispatchable (Callback<T> callback, Exception e)
		{
			this.callback = callback;
			this.e = e;
		}

		public void Dispatch () {
			callback.CallFail (e);
		}
	}

	public class DisposedDispatchable<T> : Dispatchable where T : class {
		Callback<T> callback;

		public DisposedDispatchable (Callback<T> callback)
		{
			this.callback = callback;
		}

		public void Dispatch () {
			callback.CallDisposed ();
		}
	}
}
