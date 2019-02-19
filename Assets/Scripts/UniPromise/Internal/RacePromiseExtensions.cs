using System.Collections.Generic;
using System.Linq;

namespace UniPromise
{
	public static class RacePromiseExtensions
	{
		public static Promise<T> Race<T>(List<Promise<T>> promises, bool disposeMemberFinally) where T : class
		{
			if (promises.Count == 0)
				return Promises.Disposed<T>();

			var deferred = new Deferred<T>();
			if (disposeMemberFinally)
			{
				deferred.Finally(() =>
					{
						foreach (var each in promises)
							each.Dispose();
					});
			}

			foreach (var each in promises)
			{
				each.Done(deferred.Resolve);
				each.Fail(deferred.Reject);
				each.Disposed(deferred.Dispose);
			}
			return deferred;
		}

		public static Promise<T> Race<T>(this Promise<T> first, bool disposeMemberFinally, params Promise<T>[] others) where T : class
		{
			var list = new List<Promise<T>>(others.Length + 1);
			list.Add(first);
			list.AddRange(others);
			return RacePromiseExtensions.Race(list, disposeMemberFinally);
		}

		public static Promise<T> Race<T>(this Promise<T> first, Promise<T> second, bool disposeMemberFinally) where T : class
		{
			var list = new List<Promise<T>>() {first, second};
			return RacePromiseExtensions.Race(list, disposeMemberFinally);
		}
	}
}
