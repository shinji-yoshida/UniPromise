using UnityEngine;
using System.Collections;
using System;

namespace UniPromise {
	public class AsyncTask<R> {
		Func<Promise<R>> func;
		Deferred<R> result;
		bool started;

		public AsyncTask (Func<Promise<R>> func) {
			this.func = func;
			result = new Deferred<R>();
		}

		public Promise<R> Execute() {
			if(started)
				return result;

			started = true;
			func()
				.Done(r => this.result.Resolve(r))
					.Fail(e => this.result.Reject(e));
			return result;
		}

		public Promise<R> Result {
			get {
				return result;
			}
		}
	}
}