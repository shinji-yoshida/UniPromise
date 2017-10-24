using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UniPromise.Internal {
	public class ThreadStaticDispatcher {
		[ThreadStatic]
		static ThreadStaticDispatcher threadStaticInstance;

		Queue<Dispatchable> dispatchableQueue;
		bool dispatching;

		public static ThreadStaticDispatcher Instance {
			get {
				if (threadStaticInstance != null)
					return threadStaticInstance;
				threadStaticInstance = new ThreadStaticDispatcher ();
				return threadStaticInstance;
			}
		}

		ThreadStaticDispatcher() {
			dispatchableQueue = new Queue<Dispatchable> ();
		}

		public void Dispatch(Dispatchable dispatchable) {
			dispatchableQueue.Enqueue (dispatchable);
			StartDispatchLoop ();
		}

		public void Dispatch(IEnumerable<Dispatchable> dispatchables) {
			foreach(var each in dispatchables)
				dispatchableQueue.Enqueue (each);
			StartDispatchLoop ();
		}

		void StartDispatchLoop () {
			if (dispatching) {
				return;
			}

			dispatching = true;
			while (dispatchableQueue.Count > 0) {
				dispatchableQueue.Dequeue ().Dispatch ();
			}
			dispatching = false;
		}

		public void DispatchDone<T>(Action<T> cb, T val) where T : class {
			Dispatch (new DoneDispatchable<T>(new Callback<T> (CallbackType.Done, cb), val));
		}

		public void DispatchFail<T>(Action<Exception> cb, Exception val) where T : class {
			Dispatch (new FailDispatchable<T>(new Callback<T> (CallbackType.Fail, cb), val));
		}

		public void DispatchDisposed<T>(Action cb) where T : class {
			Dispatch (new DisposedDispatchable<T>(new Callback<T> (CallbackType.Disposed, cb)));
		}
	}
}
