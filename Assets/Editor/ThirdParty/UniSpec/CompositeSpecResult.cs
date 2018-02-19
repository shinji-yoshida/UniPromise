using System.Collections.Generic;
using System.Linq;
using System;


namespace UniSpec {
	public class CompositeSpecResult : SpecResult {
		string specText;
		List<SpecResult> children;

		public CompositeSpecResult (string specText) {
			this.specText = specText;
			children = new List<SpecResult> ();
		}

		public void AddSpecResult(SpecResult result) {
			children.Add (result);
		}

		public List<string> ToSummaryLines () {
			return new List<string>{ specText }
				.Concat (
					children.SelectMany(c => c.ToSummaryLines()).Select(line => "  " + line)
				).ToList();
		}

		public bool HasFailure () {
			return children.Any (c => c.HasFailure ());
		}

		public string ToDetailedString () {
			if(!HasFailure())
				return "";
			var detailedStrings = children.Where (c => c.HasFailure ()).Select (c => c.ToDetailedString ());
			return string.Format ("{0}\n{1}", specText, string.Join("\n", detailedStrings.ToArray()));
		}

		public Exception FirstException () {
			return children.First (c => c.HasFailure ()).FirstException ();
		}

		public int SpecCount {
			get {
				return children.Sum (c => c.SpecCount);
			}
		}

		public int FailureCount {
			get {
				return children.Sum (c => c.FailureCount);
			}
		}
	}
}