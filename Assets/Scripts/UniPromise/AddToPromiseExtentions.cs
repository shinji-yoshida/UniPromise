using System.Collections;
using System.Collections.Generic;

namespace UniPromise {
	public static class AddToPromiseExtentions {
		public static T AddTo<T, U>(this T disposable, Promise<U> promise) where T : System.IDisposable where U : class {
			promise.Finally (() => disposable.Dispose ());
			return disposable;
		}
	}
}