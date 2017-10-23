using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UniPromise.Tests {
	public class DisposedPromiseTest {
		DisposedPromise<TWrapper<int>> source;
		DoneCallback<TWrapper<int>> doneCallback;
		FailCallback failCallback;
		List<Exception> exceptions;

		[SetUp]
		public void SetUp() {
			source = new DisposedPromise<TWrapper<int>> ();
			doneCallback = new DoneCallback<TWrapper<int>> ();
			failCallback = new FailCallback ();
			exceptions = new List<Exception> ();
		}

		[Test]
		public void ExceptionThrownInDisposedShouldBeReportedToSinkExceptionHandler() {
			Promises.ResetSinkExceptionHandler(e => exceptions.Add(e));
			source.Disposed (() => {
				throw new Exception();
			});
			source.Disposed (() => {
				throw new Exception();
			});
			Assert.That (exceptions.Count, Is.EqualTo (2));
		}
	}
}
