using System.Collections.Generic;
using System;

namespace UniSpec {
	public class ExecutionContext {
		List<Action> beforeEachActions = new List<Action>();
		List<Action> afterEachActions = new List<Action>();

		public void AddBeforeEachActions (List<Action> actions) {
			beforeEachActions.AddRange (actions);
		}

		public void AddAfterEachActions (List<Action> actions) {
			// TODO 親の context の after each は後でやるべき？
			afterEachActions.AddRange (actions);
		}

		public void RemoveBeforeEachActions (List<Action> actions) {
			foreach (var each in actions)
				beforeEachActions.Remove (each);
		}

		public void RemoveAfterEachActions (List<Action> actions) {
			foreach (var each in actions)
				afterEachActions.Remove (each);
		}

		public void ExecuteBeforeEachActions (ExampleSpecResult specResult) {
			foreach (var each in beforeEachActions) {
				try {
					each ();
				}
				catch(Exception e) {
					specResult.ReportFailure (e);
				}
			}
		}

		public void ExecuteAfterEachActions (ExampleSpecResult specResult) {
			foreach (var each in afterEachActions) {
				try {
					each ();
				}
				catch(Exception e) {
					specResult.ReportFailure (e);
				}
			}
		}
	}
}
