using System.Collections.Generic;
using UniPromise.Internal;
using System;

namespace UniPromise {

	public static class Promises {
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
		
		public static Promise<T> Disposed<T>() {
			return new DisposedPromise<T>();
		}
	}
}