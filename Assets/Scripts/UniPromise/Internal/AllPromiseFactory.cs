
using System.Collections.Generic;

namespace UniPromise.Internal {
	internal class AllPromiseFactory<T> where T : class {
		List<Promise<T>> promises;
		int resolvedCount;
		int size;
		T[] result;
		Deferred<T[]> deferred;
		
		public Promise<T[]> Create (List<Promise<T>> promises) {
			this.promises = promises;
			resolvedCount = 0;
			
			if(promises.Count == 0)
				return Promises.Resolved(new T[0]);
			
			size = promises.Count;
			deferred = new Deferred<T[]>();
			
			result = new T[size];
			for(int i = 0; i < size; i++){
				ObservePromise(i);
			};
			return deferred;
		}
		
		void ObservePromise(int index){
			var each = promises[index];
			each.Done (t => {
				result [index] = t;
				resolvedCount++;
				if (resolvedCount == size)
					deferred.Resolve (result);
			})
				.Fail (deferred.Reject)
				.Disposed (deferred.Dispose);
		}
	}
}