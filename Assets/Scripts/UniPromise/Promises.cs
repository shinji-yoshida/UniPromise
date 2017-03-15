using System.Collections.Generic;
using UniPromise.Internal;
using System;
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

		public static Promise<T[]> AllDone<T>(params Promise<T>[] promises) {
			return AllDone(new List<Promise<T>>(promises));
		}
		
		public static Promise<T[]> AllDone<T>(this IEnumerable<Promise<T>> promises) {
			return AllDone(new List<Promise<T>>(promises));
		}
		
		public static Promise<T[]> AllDone<T>(this List<Promise<T>> promises) {
			return new AllPromiseFactory<T>().Create(promises);
		}

		public static Promise<T> AnyDone<T>(params Promise<T>[] promises) {
			return AnyDone(new List<Promise<T>>(promises));
		}

		public static Promise<T> AnyDone<T>(this IEnumerable<Promise<T>> promises) {
			return AnyDone(new List<Promise<T>>(promises));
		}

		public static Promise<T> AnyDone<T>(this List<Promise<T>> promises) {
			return new AnyPromiseFactory<T> ().Create (promises);
		}
		
		public static Promise<T> Resolved<T>(T val) {
			return new ResolvedPromise<T>(val);
		}
		
		public static Promise<T> Rejected<T>(Exception e) {
			return new RejectedPromise<T>(e);
		}

		public static Promise<T> RejectedWithThrow<T>(Exception e) {
			try {
				throw e;
			}
			catch(Exception thrown) {
				return new RejectedPromise<T>(thrown);
			}
		}
		
		public static Promise<T> Disposed<T>() {
			return new DisposedPromise<T>();
		}

		public static Promise<T> PropagateThrough<T>(this Promise<T> src, Deferred<T> dst) {
			dst.Propagate (src);
			return src;
		}
	}
}