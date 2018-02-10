using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UniPromise.Tests {
	public class ResolvedPromiseTest {
		ResolvedPromise<TWrapper<int>> subject;
		DoneCallback<TWrapper<int>> doneCallback;
		FailCallback failCallback;
		List<Exception> exceptions;

		[SetUp]
		public void SetUp() {
			subject = new ResolvedPromise<TWrapper<int>> (1.Wrap());
			doneCallback = new DoneCallback<TWrapper<int>> ();
			failCallback = new FailCallback ();
			exceptions = new List<Exception> ();
		}

		[Test]
		public void SuccessCase() {
			int actual = 0;
			subject.Done(val => actual = val.val);
			Assert.That (actual, Is.EqualTo (1));
		}

		[Test]
		public void ShouldRejectWhenDoneInThenThrows() {
			subject.Then<TWrapper<int>> (_ => {
				throw new Exception ();
			})
				.Done (doneCallback.Create ()).Fail (failCallback.Create ());
			Assert.That (doneCallback.IsCalled, Is.False);
			Assert.That (failCallback.IsCalled, Is.True);
		}

		[Test]
		public void ShouldRejectWhenDoneInThenThrows_WithFailCase() {
			subject.Then<TWrapper<int>> (
				_ => {
					throw new Exception ();
				},
				_ => {
					return Promises.Resolved(1.Wrap());
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
