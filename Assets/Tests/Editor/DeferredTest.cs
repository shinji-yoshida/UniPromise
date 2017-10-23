using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;

namespace UniPromise.Tests {
	[TestFixture]
	public class DeferredTest {

		[Test]
		public void ShouldCallDoneRegisteredBeforeResolved() {
			var deferred = new Deferred<TWrapper<int>>();
			var callback = new DoneCallback<TWrapper<int>>();
			deferred.Done(callback.Create());
			deferred.Resolve(3.Wrap());
			Assert.That(deferred.IsResolved, Is.True);
			Assert.That(callback.IsCalled, Is.True);
			Assert.That(callback.Result.val, Is.EqualTo(3));
		}
		
		[Test]
		public void ShouldCallDoneRegisteredAfterResolved() {
			var deferred = new Deferred<TWrapper<int>>();
			var callback = new DoneCallback<TWrapper<int>>();
			deferred.Resolve(3.Wrap());
			deferred.Done(callback.Create());
			Assert.That(deferred.IsResolved, Is.True);
			Assert.That(callback.IsCalled, Is.True);
			Assert.That(callback.Result.val, Is.EqualTo(3));
		}
	}
}