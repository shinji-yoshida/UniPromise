using System;


namespace UniPromise.Tests {
	public class FailCallback {
		bool called;
		Exception exception;

		public Action<Exception> Create() {
			return e => {
				called = true;
				exception = e;
			};
		}

		public bool IsCalled {
			get { return called; }
		}

		public Exception Exception {
			get { return exception; }
		}
	}
}