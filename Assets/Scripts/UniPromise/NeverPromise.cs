﻿using System;

namespace UniPromise {
	public class NeverPromise<T> : Promise<T> {
		public NeverPromise () {
		}
		
		public override Promise<T> Done (Action<T> doneCallback) {
			return this;
		}
		
		public override Promise<T> Fail (Action<Exception> failedCallback) {
			return this;
		}
		
		public override Promise<U> Then<U> (Func<T, Promise<U>> done) {
			return new NeverPromise<U>();
		}
	}
}
