using System;

namespace UniPromise {
	public interface Promise<T> : IDisposable where T : class {
		State State { get; }

		bool IsPending { get; }
		bool IsNotPending { get; }
		bool IsResolved { get; }
		bool IsRejected { get; }
		bool IsDisposed { get; }

		Promise<T> Done(Action<T> doneCallback);
		
		Promise<T> Fail(Action<Exception> failedCallback);

		Promise<T> Disposed(Action disposedCallback);

		Promise<T> Finally (Action callback, bool includeDisposed = true);

		Promise<T> ThrowOnFail ();
		
		Promise<U> Then<U>(Func<T, Promise<U>> done) where U : class;

		Promise<U> Then<U>(Func<T, Promise<U>> done, Func<Exception, Promise<U>> fail) where U : class;

		Promise<U> Then<U>(
			Func<T, Promise<U>> done, Func<Exception, Promise<U>> fail, Func<Promise<U>> disposed) where U : class;

		Promise<U> Select<U> (Func<T, U> selector) where U : class;

		Promise<T> Where (Predicate<T> condition);

		Promise<T> Clone();
	}
}
