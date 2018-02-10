using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPromise {
	/// 
	/// To avoid AOT exception, convert struct to class
	/// 
	public class CBool {
		public readonly bool val;

		public CBool (bool val) {
			this.val = val;
		}

		public static CBool True = new CBool (true);
		public static CBool False = new CBool (false);

		public override string ToString ()
		{
			return string.Format ("[CBool: {0}]", val);
		}
	}

	public static class CBoolExtensions {
		public static CBool AsCBool(this bool b) {
			return b ? CBool.True : CBool.False;
		}
	}
}