using System;
using System.Collections.Generic;

namespace UniPromise.Internal
{
	internal class WaitAllPromiseFactory
	{
		int resolvedCount;
		private int disposedCount;
		int size;
		Deferred<CUnit> deferred;
		private List<Exception> exceptions;
		
		public Promise<CUnit> Create<T> (List<Promise<T>> promises) where T : class {
			resolvedCount = 0;
			exceptions = new List<Exception>();
			
			if(promises.Count == 0)
				return Promises.Resolved(CUnit.Default);
			
			size = promises.Count;
			deferred = new Deferred<CUnit>();

			foreach (var each in promises) {
				ObservePromise(each);
			}
			
			deferred.Disposed(() =>
			{
				foreach(var each in promises)
					each.Dispose();
			});
			return deferred;
		}
		
		void ObservePromise<T>(Promise<T> promise) where T : class
		{
			promise
				.Done(t => resolvedCount++)
				.Fail(exceptions.Add)
				.Disposed(() => disposedCount++)
				.Finally(() =>
				{
					if (resolvedCount + exceptions.Count + disposedCount != size) {
						return;
					}

					if (exceptions.Count > 0) {
						deferred.Reject(new BundledException(exceptions));
					}
					else if (disposedCount > 0) {
						deferred.Dispose();
					}
					else {
						deferred.Resolve(CUnit.Default);
					}
				});
		}
	}
}
