using System;
using System.Collections.Generic;
using UnityEngine;
using UniSpec;
using NUnit.Framework;
using UniPromise;

public class StackOverflowTest : SpecSuite {
	[Test]
	public void Test() {
		Describe ("Promise Recursion Test", () => {
			List<string> events = null;
			Deferred<CUnit> trigger = null;

			BeforeEach(() => {
				events = new List<string>();
				trigger = new Deferred<CUnit>();
			});

			It("Recursion should not increase callstack", () => {
				Recursion(3, e => events.Add(e));
				Assert.That(events, Is.Not.EqualTo(new List<string>{"Enter_3", "Enter_2", "Enter_1", "Enter_0", "Exit_0", "Exit_1", "Exit_2", "Exit_3"}));
				Assert.That(events, Is.EqualTo(new List<string>{"Enter_3", "Enter_2", "Exit_2", "Enter_1", "Exit_1", "Enter_0", "Exit_0", "Exit_3"}));
			});

			It("Can call multiple times", () => {
				Recursion(1, e => events.Add(e));
				Recursion(1, e => events.Add(e));
				Recursion(1, e => events.Add(e));
				Assert.That(events, Is.EqualTo(new List<string>{
					"Enter_1", "Enter_0", "Exit_0", "Exit_1",
					"Enter_1", "Enter_0", "Exit_0", "Exit_1",
					"Enter_1", "Enter_0", "Exit_0", "Exit_1"
				}));
			});

			It("Should not trigger stack overflow", () => {
				Assert.That(() => SimpleRecursion(100000), Throws.Nothing);
			});
		});
	}

	Promise<CUnit> Recursion(int recursionCount, Action<string> eventReporter) {
		eventReporter ("Enter_" + recursionCount);
		try {
			if (recursionCount <= 0)
				return Promises.Resolved (CUnit.Default);
			
			return Promises.Resolved(CUnit.Default)
				.Then (_ => Recursion (recursionCount - 1, eventReporter));
		}
		finally {
			eventReporter ("Exit_" + recursionCount);
		}
	}

	Promise<CUnit> SimpleRecursion(int recursionCount) {
		if (recursionCount <= 0)
			return Promises.Resolved (CUnit.Default);

		return Promises.Resolved(CUnit.Default)
			.Then (_ => SimpleRecursion (recursionCount - 1));
	}
}
