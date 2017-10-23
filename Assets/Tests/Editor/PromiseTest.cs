using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UniPromise.Tests {
	public class PromiseTest {
		DoneCallback<TWrapper<int>> doneCallback;
		FailCallback failCallback;
		List<Exception> exceptions;

		[SetUp]
		public void SetUp() {
			doneCallback = new DoneCallback<TWrapper<int>> ();
			failCallback = new FailCallback ();
			exceptions = new List<Exception> ();
		}

		[Test]
		public void ShouldRejectWhenDoneInThenThrows_DeferredCase() {
			var source = new Deferred<TWrapper<int>> ();
			source.Then<TWrapper<int>> (_ => {
				throw new Exception ();
			})
				.Done (doneCallback.Create ()).Fail (failCallback.Create ());
			source.Resolve (1.Wrap());
			Assert.That (doneCallback.IsCalled, Is.False);
			Assert.That (failCallback.IsCalled, Is.True);
		}

		[Test]
		public void ShouldRejectWhenDoneInThenThrows_ImmediateCase() {
			var source = new Deferred<TWrapper<int>> ();
			source.Resolve (1.Wrap());
			source.Then<TWrapper<int>> (_ => {
				throw new Exception ();
			})
				.Done (doneCallback.Create ()).Fail (failCallback.Create ());
			Assert.That (doneCallback.IsCalled, Is.False);
			Assert.That (failCallback.IsCalled, Is.True);
		}

		[Test]
		public void ShouldRejectWhenDoneInThenThrows_DeferredWithFailCase() {
			var source = new Deferred<TWrapper<int>> ();
			source.Then<TWrapper<int>> (
				_ => {
					throw new Exception ();
				},
				_ => {
					return Promises.Resolved(1.Wrap());
				})
				.Done (doneCallback.Create ()).Fail (failCallback.Create ());
			source.Resolve (1.Wrap());
			Assert.That (doneCallback.IsCalled, Is.False);
			Assert.That (failCallback.IsCalled, Is.True);
		}

		[Test]
		public void ShouldRejectWhenFailInThenThrows() {
			var source = new Deferred<TWrapper<int>> ();
			source.Then<TWrapper<int>> (
				_ => {
					return Promises.Resolved(1.Wrap());
				},
				_ => {
					throw new Exception("expected");
				})
				.Done (doneCallback.Create ()).Fail (failCallback.Create ());
			source.Reject (new Exception("NOT expected"));
			Assert.That (doneCallback.IsCalled, Is.False);
			Assert.That (failCallback.IsCalled, Is.True);
			Assert.That (failCallback.Exception.Message, Is.EqualTo("expected"));
		}

		[Test]
		public void ExceptionThrownInDoneShouldBeReportedToSinkExceptionHandler() {
			Promises.ResetSinkExceptionHandler(e => exceptions.Add(e));
			var source = new Deferred<TWrapper<int>> ();
			source.Done (_ => {
				throw new Exception();
			});
			source.Done (_ => {
				throw new Exception();
			});
			source.Resolve (1.Wrap());
			source.Done (_ => {
				throw new Exception();
			});
			source.Done (_ => {
				throw new Exception();
			});
			Assert.That (exceptions.Count, Is.EqualTo (4));
		}

		[Test]
		public void ExceptionThrownInFailShouldBeReportedToSinkExceptionHandler() {
			Promises.ResetSinkExceptionHandler(e => exceptions.Add(e));
			var source = new Deferred<TWrapper<int>> ();
			source.Fail (_ => {
				throw new Exception();
			});
			source.Fail (_ => {
				throw new Exception();
			});
			source.Reject (new Exception());
			source.Fail (_ => {
				throw new Exception();
			});
			source.Fail (_ => {
				throw new Exception();
			});
			Assert.That (exceptions.Count, Is.EqualTo (4));
		}

		[Test]
		public void ExceptionThrownInDisposedShouldBeReportedToSinkExceptionHandler() {
			Promises.ResetSinkExceptionHandler(e => exceptions.Add(e));
			var source = new Deferred<TWrapper<int>> ();
			source.Disposed (() => {
				throw new Exception();
			});
			source.Disposed (() => {
				throw new Exception();
			});
			source.Dispose ();
			source.Disposed (() => {
				throw new Exception();
			});
			source.Disposed (() => {
				throw new Exception();
			});
			Assert.That (exceptions.Count, Is.EqualTo (4));
		}

		[Test]
		public void TestExceptionThrownInThenInMidOfChain() {
			var source = new Deferred<TWrapper<int>> ();
			source.Then (_ => Promises.Resolved (1.Wrap()))
				.Then <TWrapper<int>>(_ => {
					throw new Exception ();
				})
				.Then (_ => Promises.Resolved (1.Wrap()))
				.Fail (failCallback.Create ());
			source.Resolve (1.Wrap());
			Assert.That (failCallback.IsCalled);
		}

		[Test]
		public void TestSelect() {
			var actual = 0;
			Promises.Resolved (1.Wrap()).Select (_ => 2.Wrap()).Done (_ => actual++);
			Promises.Rejected<TWrapper<int>> (new Exception ()).Select (_ => 2.Wrap()).Fail (_ => actual++);
			Promises.Disposed<TWrapper<int>> ().Select (_ => 2.Wrap()).Disposed (() => actual++);
			Promises.Resolved (1.Wrap()).Select<TWrapper<int>> (_ => {
				throw new Exception ();
			}).Fail (_ => actual++);
			Assert.That (actual, Is.EqualTo (4));
		}

		[Test]
		public void TestWhere() {
			var actual = 0;
			Promises.Resolved (1.Wrap()).Where (n => n.val == 1).Done (_ => actual++);
			Promises.Resolved (1.Wrap()).Where (n => n.val == 2)
				.Done (_ => Assert.Fail()).Fail(_ => Assert.Fail())
				.Disposed(() => actual++);
			Promises.Rejected<TWrapper<int>> (new Exception ()).Where (_ => true).Fail (_ => actual++);
			Promises.Disposed<TWrapper<int>> ().Where (_ => true).Disposed (() => actual++);
			Promises.Resolved (1.Wrap()).Where (_ => {
				throw new Exception ();
			}).Fail (_ => actual++);
			Assert.That (actual, Is.EqualTo (5));
		}
	}
}
