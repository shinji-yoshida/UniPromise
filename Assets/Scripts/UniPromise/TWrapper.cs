
namespace UniPromise {
	/// <summary>
	/// Wrap struct by this class in order to avoid AOT exception.
	/// </summary>
	public class TWrapper<T> {
		public readonly T val;

		public TWrapper (T val) {
			this.val = val;
		}

		public static TWrapper<T> Create<T>(T val) {
			return new TWrapper<T> (val);
		}
	}

	public static class TWrapperExtensions {
		public static TWrapper<T> Wrap<T>(this T t) where T : struct {
			return new TWrapper<T> (t);
		}
	}
}
