using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPromise
{

	public class BackwardDisposablePromise<T> : Promise<T> where T : class
	{
		Promise<T> upstream;

		public BackwardDisposablePromise(Promise<T> upstream)
		{
			this.upstream = upstream;
		}

		public Promise<T> Done(System.Action<T> doneCallback)
		{
			return upstream.Done(doneCallback);
		}

		public Promise<T> Fail(System.Action<System.Exception> failedCallback)
		{
			return upstream.Fail(failedCallback);
		}

		public Promise<T> Disposed(System.Action disposedCallback)
		{
			return upstream.Disposed(disposedCallback);
		}

		public Promise<T> Finally(System.Action callback, bool includeDisposed = true)
		{
			return upstream.Finally(callback, includeDisposed);
		}

		public Promise<T> ThrowOnFail()
		{
			return upstream.ThrowOnFail();
		}

		public Promise<U> Then<U>(System.Func<T, Promise<U>> done) where U : class
		{
			return upstream.Then(done).Disposed(upstream.Dispose);
		}

		public Promise<U> Then<U>(System.Func<T, Promise<U>> done, System.Func<System.Exception, Promise<U>> fail) where U : class
		{
			return upstream.Then(done, fail).Disposed(upstream.Dispose);
		}

		public Promise<U> Then<U>(System.Func<T, Promise<U>> done, System.Func<System.Exception, Promise<U>> fail, System.Func<Promise<U>> disposed) where U : class
		{
			return upstream.Then(done, fail, disposed).Disposed(upstream.Dispose);
		}

		public Promise<U> Select<U>(System.Func<T, U> selector) where U : class
		{
			return upstream.Select(selector).Disposed(upstream.Dispose);
		}

		public Promise<T> Where(System.Predicate<T> condition)
		{
			return upstream.Where(condition).Disposed(upstream.Dispose);
		}

		public Promise<T> Clone()
		{
			return new BackwardDisposablePromise<T>(upstream.Clone());
		}

		public State State
		{
			get
			{
				return upstream.State;
			}
		}

		public bool IsPending
		{
			get
			{
				return upstream.IsPending;
			}
		}

		public bool IsNotPending
		{
			get
			{
				return upstream.IsNotPending;
			}
		}

		public bool IsResolved
		{
			get
			{
				return upstream.IsResolved;
			}
		}

		public bool IsRejected
		{
			get
			{
				return upstream.IsRejected;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return upstream.IsDisposed;
			}
		}

		public void Dispose()
		{
			upstream.Dispose();
		}
	}
}
