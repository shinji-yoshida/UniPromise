
namespace UniPromise {
	public interface StructPromise<T> : Promise<TWrapper<T>> where T : struct {
	}
}