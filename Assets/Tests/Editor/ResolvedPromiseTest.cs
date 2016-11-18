using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UniPromise.Tests {
	public class ResolvedPromiseTest {
		ResolvedPromise<int> subject;
		DoneCallback<int> doneCallback;
		FailCallback failCallback;
		List<Exception> exceptions;

		[SetUp]
		public void SetUp() {
			subject = new ResolvedPromise<int> (1);
			doneCallback = new DoneCallback<int> ();
			failCallback = new FailCallback ();
			exceptions = new List<Exception> ();
		}

		[Test]
		public void ShouldRejectWhenDoneInThenThrows() {
			subject.Then<int> (_ => {
				throw new Exception ();
			})
				.Done (doneCallback.Create ()).Fail (failCallback.Create ());
			Assert.That (doneCallback.IsCalled, Is.False);
			Assert.That (failCallback.IsCalled, Is.True);
		}

		[Test]
		public void ShouldRejectWhenDoneInThenThrows_WithFailCase() {
			subject.Then<int> (
				_ => {
					throw new Exception ();
				},
				_ => {
					return Promises.Resolved(1);
				})
				.Done (doneCallback.Create ()).Fail (failCallback.Create ());
			Assert.That (doneCallback.IsCalled, Is.False);
			Assert.That (failCallback.IsCalled, Is.True);
		}

		[Test]
		public void ExceptionThrownInDoneShouldBeReportedToSinkExceptionHandler() {
			Promises.ResetSinkExceptionHandler(e => exceptions.Add(e));
			subject.Done (_ => {
				throw new Exception();
			});
			subject.Done (_ => {
				throw new Exception();
			});
			Assert.That (exceptions.Count, Is.EqualTo (2));
		}
	}
}
