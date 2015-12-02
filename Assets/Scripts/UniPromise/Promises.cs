﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniPromise {

	public static class Promises {
		public static Promise<T[]> All<T>(params Promise<T>[] promises) {
			return All(new List<Promise<T>>(promises));
		}
		
		public static Promise<T[]> All<T>(this IEnumerable<Promise<T>> promises) {
			return All(new List<Promise<T>>(promises));
		}
		
		public static Promise<T[]> All<T>(this List<Promise<T>> promises) {
			return new AllPromiseFactory<T>().Create(promises);
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
		
		public static Promise<T> Resolved<T>(T val) {
			return new ResolvedPromise<T>(val);
		}
		
		public static Promise<T> Never<T>() {
			return new NeverPromise<T>();
		}
	}


	public class AllPromiseFactory<T> {
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
			each.Done(t => {
				result[index] = t;
				resolvedCount++;
				if(resolvedCount == size)
					deferred.Resolve(result);
			})
				.Fail(e => deferred.Reject(e));
		}
	}

}