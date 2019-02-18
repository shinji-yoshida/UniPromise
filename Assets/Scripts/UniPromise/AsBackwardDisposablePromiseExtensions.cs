
namespace UniPromise
{
	public static class AsBackwardDisposablePromiseExtensions
	{
		public static Promise<T> AsBackwardDisposable<T>(this Promise<T> promise) where T : class
		{
			return new BackwardDisposablePromise<T>(promise);
		}
	}
}
