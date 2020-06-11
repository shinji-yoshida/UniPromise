using System.Collections.Generic;

namespace UniPromise.Internal
{
	/// <summary>
	/// Returns promise which will be rejected if any of given promises is rejected, otherwise disposed.
	/// </summary>
	internal class AnyErrorPromiseFactory<T> where T : class
	{
		int nonErrorCount;
		Deferred<T> deferred;
		int size;
		
		public Promise<T> Create (List<Promise<T>> promises) {
			if (promises.Count == 0)
				return Promises.Disposed<T> ();
			
			nonErrorCount = 0;
			size = promises.Count;
			deferred = new Deferred<T>();

			foreach (var each in promises)
				ObservePromise (each);
			return deferred;
		}

		void ObservePromise(Promise<T> promise){
			promise.Done(_ => {
				nonErrorCount++;
				if (nonErrorCount == size)
					deferred.Dispose ();
			});
			promise.Fail (deferred.Reject);
			promise.Disposed (() => {
				nonErrorCount++;
				if (nonErrorCount == size)
					deferred.Dispose ();
			});
		}
	}
}
