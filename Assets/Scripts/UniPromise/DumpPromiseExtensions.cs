using System.Collections.Generic;
using UnityEngine;

namespace UniPromise {
	public static class DumpPromiseExtensions {
		public static Promise<T> Dump<T>(this Promise<T> promise, string name) {
			promise
				.Done (i => Debug.Log (string.Format ("{0}-->{1}", name, i)))
				.Fail (ex => Debug.Log (string.Format ("{0} failed-->{1}", name, ex)))
				.Disposed (() => Debug.Log (string.Format ("{0} disposed", name)));
			return promise;
		}
	}
}
