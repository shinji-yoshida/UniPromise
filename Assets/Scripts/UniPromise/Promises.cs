using System.Collections.Generic;
using UniPromise.Internal;
using System;
using System.Linq;
using UnityEngine;

namespace UniPromise {

	public static class Promises {
		static Action<Exception> sinkExceptionHandler;

		static Promises() {
			ResetSinkExceptionHandler ();
		}

		public static void ResetSinkExceptionHandler () {
			ResetSinkExceptionHandler (e => Debug.LogException (e));
		}

		public static void ResetSinkExceptionHandler (Action<Exception> handler) {
			sinkExceptionHandler = handler;
		}

		internal static void ReportSinkException (Exception e) {
			sinkExceptionHandler (e);
		}

		public static Promise<T[]> AllDone<T>(params Promise<T>[] promises) where T : class {
			return AllDone(new List<Promise<T>>(promises));
		}
		
		public static Promise<T[]> AllDone<T>(this IEnumerable<Promise<T>> promises) where T : class {
			return AllDone(new List<Promise<T>>(promises));
		}
		
		public static Promise<T[]> AllDone<T>(this List<Promise<T>> promises) where T : class {
			return new AllPromiseFactory<T>().Create(promises);
		}

		public static Promise<T> AnyDone<T>(params Promise<T>[] promises) where T : class {
			return AnyDone(new List<Promise<T>>(promises));
		}

		public static Promise<T> AnyDone<T>(this IEnumerable<Promise<T>> promises) where T : class {
			return AnyDone(new List<Promise<T>>(promises));
		}

		public static Promise<T> AnyDone<T>(this List<Promise<T>> promises) where T : class {
			return new AnyDonePromiseFactory<T> ().Create (promises);
		}

		public static Promise<T> AnyFail<T>(params Promise<T>[] promises) where T : class
		{
			return new AnyErrorPromiseFactory<T>().Create(promises.ToList());
		}

		public static Promise<T> Race<T>(this List<Promise<T>> promises, bool disposeMemberFinally) where T : class {
			return RacePromiseExtensions.Race(promises, disposeMemberFinally);
		}
		
		public static Promise<T> Resolved<T>(T val) where T : class {
			return new ResolvedPromise<T>(val);
		}

		public static StructPromise<T> ResolvedStruct<T>(T val) where T : struct {
			return new ResolvedStructPromise<T>(val.Wrap());
		}
		
		public static Promise<T> Rejected<T>(Exception e) where T : class {
			return new RejectedPromise<T>(e);
		}

		public static Promise<T> RejectedWithThrow<T>(Exception e) where T : class {
			try {
				throw e;
			}
			catch(Exception thrown) {
				return new RejectedPromise<T>(thrown);
			}
		}
		
		public static Promise<T> Disposed<T>() where T : class {
			return new DisposedPromise<T>();
		}

		public static Promise<T> PropagateThrough<T>(this Promise<T> src, Deferred<T> dst) where T : class {
			dst.Propagate (src);
			return src;
		}

		public static Promise<T> Using<T, TResource>(
			Func<TResource> resourceFactory, 
			Func<TResource, Promise<T>> promiseFactory
		)
			where TResource : IDisposable
			where T : class
		{
			var resource = resourceFactory ();
			return promiseFactory (resource)
				.Finally (resource.Dispose);
		}
	}
}