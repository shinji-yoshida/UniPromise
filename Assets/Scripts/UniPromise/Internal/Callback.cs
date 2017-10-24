using System.Collections.Generic;
using System;

namespace UniPromise.Internal {
	public enum CallbackType {
		Done, Fail, Disposed
	}

	public struct Callback<T> where T : class {
		public readonly CallbackType type;
		object callback;

		public Callback (CallbackType type, object callback) {
			this.type = type;
			this.callback = callback;
		}

		public void CallDone(T value) {
			try {
				((Action<T>)callback) (value);
			}
			catch(Exception e) {
				Promises.ReportSinkException (e);
			}
		}

		public void CallFail(Exception e) {
			try {
				((Action<Exception>)callback)(e);
			}
			catch(Exception e2) {
				Promises.ReportSinkException(e2);
			}
		}

		public void CallDisposed() {
			try {
				((Action)callback)();
			}
			catch(Exception e) {
				Promises.ReportSinkException (e);
			}
		}
	}
}
