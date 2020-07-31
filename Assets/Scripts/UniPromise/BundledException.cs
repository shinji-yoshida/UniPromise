using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace UniPromise
{
	public class BundledException : Exception
	{
		private List<Exception> exceptions;

		public BundledException(List<Exception> exceptions)
		{
			this.exceptions = exceptions;
		}
		
		public BundledException(string message, List<Exception> exceptions) : base(message)
		{
			this.exceptions = exceptions;
		}

		public IEnumerable<Exception> Exceptions
		{
			get { return exceptions; }
		}
	}
}
