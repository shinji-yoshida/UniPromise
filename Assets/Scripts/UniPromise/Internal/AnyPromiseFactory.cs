using System.Collections.Generic;

namespace UniPromise.Internal {
	internal class AnyPromiseFactory<T> where T : class {
		int nonResolvedCount;
		int size;
		Deferred<T> deferred;

		public Promise<T> Create (List<Promise<T>> promises) {
			if (promises.Count == 0)
				return Promises.Disposed<T> ();
			
			nonResolvedCount = 0;
			size = promises.Count;
			deferred = new Deferred<T>();

			foreach (var each in promises)
				ObservePromise (each);
			return deferred;
		}

		void ObservePromise(Promise<T> promise){
			promise.Done (deferred.Resolve);
			promise.Fail (t => {
				nonResolvedCount++;
				if (nonResolvedCount == size)
					deferred.Dispose ();
			});
			promise.Disposed (() => {
				nonResolvedCount++;
				if (nonResolvedCount == size)
					deferred.Dispose ();
			});
		}
	}
}
