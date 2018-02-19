using System.Collections.Generic;
using System;


namespace UniSpec {
	public interface SpecResult {
		List<string> ToSummaryLines();
		bool HasFailure();
		string ToDetailedString();
		Exception FirstException ();

		int SpecCount {
			get;
		}

		int FailureCount {
			get;
		}
	}
}