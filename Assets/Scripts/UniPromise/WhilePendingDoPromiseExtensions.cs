using System;


namespace UniPromise {
	public static class WhilePendingDoPromiseExtensions {
		public static Promise<T> WhilePendingDo<T>(this Promise<T> promise, Func<IDisposable> action) where T : class {
			if(promise.IsNotPending)
				return promise;
			var disposable = action ();
			promise.Finally(disposable.Dispose);
			return promise;
		}
	}
}
