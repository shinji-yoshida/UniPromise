using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;

namespace UniPromise.Tests {
	public class DoneCallback<T> {
		bool called;
		T result;

		public Action<T> Create() {
			return (t) => {
				called = true;
				result = t;
			};
		}

		public bool IsCalled {
			get { return called; }
		}

		public T Result {
			get { return result; }
		}
	}

	[TestFixture]
	public class DeferredTest {
		TestUniPromiseManager manager;

		[SetUp]
		public void SetUp() {
			manager = new TestUniPromiseManager();
			manager.OverrideManager();
		}

		void UpdateManager() {
			manager.Update();
		}

		[Test]
		public void ShouldCallDoneRegisteredBeforeResolved() {
			var deferred = new Deferred<int>();
			var callback = new DoneCallback<int>();
			deferred.Done(callback.Create());
			deferred.Resolve(3);
			UpdateManager();
			Assert.That(deferred.IsResolved, Is.True);
			Assert.That(callback.IsCalled, Is.True);
			Assert.That(callback.Result, Is.EqualTo(3));
		}
		
		[Test]
		public void ShouldCallDoneRegisteredAfterResolved() {
			var deferred = new Deferred<int>();
			var callback = new DoneCallback<int>();
			deferred.Resolve(3);
			deferred.Done(callback.Create());
			UpdateManager();
			Assert.That(deferred.IsResolved, Is.True);
			Assert.That(callback.IsCalled, Is.True);
			Assert.That(callback.Result, Is.EqualTo(3));
		}
		
		[Test]
		public void ShouldNotCallDoneRegisteredBeforeManagerUpdated() {
			var deferred = new Deferred<int>();
			var callback = new DoneCallback<int>();
			deferred.Done(callback.Create());
			deferred.Resolve(3);
			Assert.That(deferred.IsResolved, Is.True);
			Assert.That(callback.IsCalled, Is.False);
		}
	}
}