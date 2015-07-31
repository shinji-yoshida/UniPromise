using System;

namespace UniPromise {

	public static class DisposablePromiseExtension {
		public static IDisposable AsDisposable<T>(this Promise<T> promise, Func<T, IDisposable> converter) {
			var disposablePromise = promise.Select(converter);
			return disposablePromise.AsDisposable();
		}

		public static IDisposable AsDisposable(this Promise<IDisposable> promise) {
			return new DisposableWrappter(() => promise.Done(disp => disp.Dispose()));
		}
	}

}