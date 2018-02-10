using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UniPromise.Tests {
	public class RejectedPromiseTest {
		RejectedPromise<TWrapper<int>> subject;
		DoneCallback<TWrapper<int>> doneCallback;
		FailCallback failCallback;
		List<Exception> exceptions;

		[SetUp]
		public void SetUp() {
			subject = new RejectedPromise<TWrapper<int>> (new Exception());
			doneCallback = new DoneCallback<TWrapper<int>> ();
			failCallback = new FailCallback ();
			exceptions = new List<Exception> ();
		}

		[Test]
		public void ShouldRejectWhenFailInThenThrows() {
			subject.Then<TWrapper<int>> (
				_ => {
					return Promises.Resolved(1.Wrap());
				},
				_ => {
					throw new Exception("expected");
				})
				.Done (doneCallback.Create ()).Fail (failCallback.Create ());
			Assert.That (doneCallback.IsCalled, Is.False);
			Assert.That (failCallback.IsCalled, Is.True);
			Assert.That (failCallback.Exception.Message, Is.EqualTo("expected"));
		}

		[Test]
		public void ExceptionThrownInFailShouldBeReportedToSinkExceptionHandler() {
			Promises.ResetSinkExceptionHandler(e => exceptions.Add(e));
			subject.Fail (_ => {
				throw new Exception();
			});
			subject.Fail (_ => {
				throw new Exception();
			});
			Assert.That (exceptions.Count, Is.EqualTo (2));
		}
	}
}
