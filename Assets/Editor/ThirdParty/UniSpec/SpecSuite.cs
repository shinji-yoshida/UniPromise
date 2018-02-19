using System.Collections.Generic;
using NUnit.Framework;
using System;
using UnityEngine;
using System.Linq;

namespace UniSpec {
	public class SpecSuite {
		Stack<Context> contexts;

		[SetUp]
		public void SetUp() {
			contexts = new Stack<Context> ();
		}

		protected void Describe(string message, Action descriptionBuilder) {
			Debug.Assert (contexts.Count == 0, "'Describe' should be at root");

			var execContext = new ExecutionContext ();

			var description = new Context(message);

			contexts.Push (description);
			descriptionBuilder ();
			contexts.Pop ();

			var specResult = description.Execute (execContext);

			var summaryHeader = string.Format ("-------------------------------\n<color={2}>{0} specs, {1} failures</color>\n-------------------------------",
				specResult.SpecCount, specResult.FailureCount, specResult.HasFailure() ? "red" : "green");

			var summaryBody = string.Join("\n", specResult.ToSummaryLines ().ToArray());

			var detailHeader = "stack traces:\n-------------------------------";

			var detailBody = specResult.ToDetailedString ();

			if (specResult.HasFailure ()) {
				Debug.Log (string.Format("{0}\n\n{1}\n\n{2}\n\n{3}\n\n{0}\n\n\n", summaryHeader, summaryBody, detailHeader, detailBody));
			} else {
				Debug.Log (string.Format("{0}\n\n{1}\n\n{0}\n\n\n", summaryHeader, summaryBody));
			}

			if (specResult.HasFailure ())
				throw specResult.FirstException ();
		}

		protected void Context(string message, Action contextBuilder) {
			Debug.Assert (contexts.Count != 0, "'Context' should not be at root");

			var context = new Context(message);
			contexts.Peek ().AddExecutable (context);

			contexts.Push (context);
			contextBuilder ();
			contexts.Pop ();
		}

		protected void It(string message, Action exampleBuilder) {
			var example = new Example (message, exampleBuilder);
			contexts.Peek ().AddExecutable (example);
		}

		/// Ignore example
		protected void XIt(string message, Action exampleBuilder) {
		}

		protected void BeforeEach(Action beforeEachBuilder) {
			contexts.Peek ().AddBeforeEach (beforeEachBuilder);
		}

		protected void AfterEach(Action afterEachBuilder) {
			contexts.Peek ().AddAfterEach (afterEachBuilder);
		}
	}
}
