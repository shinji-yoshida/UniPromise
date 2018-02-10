using System;

namespace UniPromise
{
	/// <summary>
	/// Based on UniRx.Unit
	/// https://github.com/neuecc/UniRx/blob/5.5.0/Assets/Plugins/UniRx/Scripts/System/Unit.cs
	/// </summary>
	[Serializable]
	public class CUnit : IEquatable<CUnit>
	{
		static readonly CUnit @default = new CUnit();

		public static CUnit Default { get { return @default; } }

		public static bool operator ==(CUnit first, CUnit second)
		{
			return true;
		}

		public static bool operator !=(CUnit first, CUnit second)
		{
			return false;
		}

		public bool Equals(CUnit other)
		{
			return true;
		}
		public override bool Equals(object obj)
		{
			return obj is CUnit;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override string ToString()
		{
			return "()";
		}
	}
}
