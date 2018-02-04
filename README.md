# UniPromise

Promise library for Unity C#.

This library has some characteristics:
* `Promise<T>` implements `IDisposable`
* Must have one type parameter `T` for result type
* Recursion safe
* Type parameter `T` of `Promise<T>` is reference type (class), in order to avoid AOT exception

## `Promise<T>` implements `IDisposable`
Unlike js Promise, `Promise<T>` of this library has four state:
* Pending
* Resolved
* Rejected
* Disposed

By disposing Promise which is pending, you can block Done or Rejected callback to be called.

## Must have one type parameter `T` for result type

For simplicity, `Promise` must have one type parameter `T` for result type.

When you don't need for result type, use `CUnit` class. When you need instance of `CUnit` to resolving `Deferred<CUnit>` (implementation of `Promise<T>` which can be resolved or rejected), use `CUnit.Default`.

## Recursion safe

This code does not trigger stack overflow exception:

    using UniPromise;

    class RecursionTest {
      public void DoTest() {
        Recursion(1000000);
      }

      Promise<CUnit> Recursion(int count) {
        if(count <= 0)
          Promises.Resolved(CUnit.Default);

        Promises.Resolved(CUnit.Default)
          .Then(_ => Recursion(count - 1));
      }
    }

## Type parameter `T` of `Promise<T>` is reference type (class), in order to avoid AOT exception
