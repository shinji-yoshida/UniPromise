using System.Collections.Generic;
using UnityEngine;

namespace UniPromise {
	public static class DumpPromiseExtensions {
		public static Promise<T> Dump<T>(this Promise<T> promise, string name) where T : class {
			promise
				.Done (i => Debug.Log (string.Format ("{0}-->{1}", name, i)))
				.Fail (ex => Debug.Log (string.Format ("{0} failed-->{1}", name, ex)))
				.Disposed (() => Debug.Log (string.Format ("{0} disposed", name)));
			return promise;
		}

		public static StructPromise<T> Dump<T>(this StructPromise<T> promise, string name) where T : struct {
			promise
				.Done (i => Debug.Log (string.Format ("{0}-->{1}", name, i.val)))
				.Fail (ex => Debug.Log (string.Format ("{0} failed-->{1}", name, ex)))
				.Disposed (() => Debug.Log (string.Format ("{0} disposed", name)));
			return promise;
		}
	}
}
