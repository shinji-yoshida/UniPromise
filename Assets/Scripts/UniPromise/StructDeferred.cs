
namespace UniPromise {
	public class StructDeferred<T> : Deferred<TWrapper<T>>, StructPromise<T> where T : struct {
	}
}