using NUnit.Framework;
using System;

namespace UniPromise.Tests {
	public class DoneCallback<T> {
		bool called;
		T result;

		public Action<T> Create() {
			return (t) => {
				called = true;
				result = t;
			};
		}

		public bool IsCalled {
			get { return called; }
		}

		public T Result {
			get { return result; }
		}
	}
}