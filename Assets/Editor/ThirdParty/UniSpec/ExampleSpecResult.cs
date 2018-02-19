using System;
using System.Collections.Generic;
using System.Linq;

namespace UniSpec {
	public class ExampleSpecResult : SpecResult {
		string specText;
		bool failed;
		List<Exception> exceptions;

		public ExampleSpecResult (string specText) {
			this.specText = specText;
			exceptions = new List<Exception> ();
		}

		public void ReportFailure(Exception e) {
			failed = true;
			exceptions.Add (e);
		}

		public List<string> ToSummaryLines() {
			var color = failed ? "red" : "green";
			return new List<string>{ string.Format ("<color={0}>{1}</color>", color, specText) };
		}

		public bool HasFailure () {
			return failed;
		}

		public string ToDetailedString () {
			return string.Format ("<color=red>{0}</color>\n{1}\n", specText, string.Join("\n", exceptions.Select(e => e.ToString()).ToArray()));
		}

		public Exception FirstException () {
			return exceptions.First ();
		}

		public int SpecCount {
			get {
				return 1;
			}
		}

		public int FailureCount {
			get {
				return failed ? 1 : 0;
			}
		}
	}
}