using System;
using System.Collections.Generic;
using System.Linq;

namespace UniSpec {
	public class Example : Executable {
		string message;
		Action exampleAction;

		public Example (string message, Action exampleAction) {
			this.message = message;
			this.exampleAction = exampleAction;
		}

		public SpecResult Execute (ExecutionContext execContext) {
			var result = new ExampleSpecResult (message);
			execContext.ExecuteBeforeEachActions (result);
			try {
				exampleAction ();
			}
			catch(Exception e) {
				result.ReportFailure (e);
			}
			execContext.ExecuteAfterEachActions (result);
			return result;
		}
	}
}