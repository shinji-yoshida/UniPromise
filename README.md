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

```csharp
using UniPromise;

class RecursionTest {
  public void DoTest() {
    Recursion(1000000);
  }

  Promise<CUnit> Recursion(int count) {
    if(count <= 0)
      return Promises.Resolved(CUnit.Default);

    return Promises.Resolved(CUnit.Default)
      .Then(_ => Recursion(count - 1));
  }
}
```

## Type parameter `T` of `Promise<T>` is reference type (class), in order to avoid AOT exception

To avoid AOT exception, the type parameter is constrained to be reference type.

To return struct type as result of Promise, wrap struct type by `TWrapper<T>`.

# Sample Code

This sample shows how to use `Promise<T>`.

```csharp
using UnityEngine;
using System.Collections;
using UniPromise;

class Sample : MonoBehaviour {
  public Promise<CUnit> PromiseResolved() {
    return Promises.Resolved(CUnit.Default);
  }

  public Promise<CUnit> PromiseResolvedByDeferred() {
    var deferred = new Deferred<CUnit>();
    deferred.Resolve(CUnit.Default);
    return deferred;
  }

  public Promise<CUnit> PromiseRejected() {
    return Promises.Rejected<CUnit>(new System.Exception());
  }

  public Promise<CUnit> PromiseRejectedByDeferred() {
    var deferred = new Deferred<CUnit>();
    deferred.Reject(new System.Exception());
    return deferred;
  }

  public Promise<CUnit> PromiseDisposed() {
    return Promises.Disposed<CUnit>();
  }

  public Promise<CUnit> PromiseDisposedByDeferred() {
    Promise<CUnit> promise = new Deferred<CUnit>();
    promise.Dispose(); // Promise can be disposed because it implements IDisposable
    return promise;
  }

  public void ChainTest() {
    PromiseResolved()
      .Then(_ => {
          return PromiseRejected();
        })
      .Then(_ => {
          // never reach here because previous promise is rejected
          return PromiseResolved();
        })
      .Then(_ => {
          // resolved case: never reach here
          return PromiseResolved();
        },
        e => {
          // rejected case
          return PromiseResolved();
        }
        () => {
          // disposed case: never reach here
        });
  }

  public void TypeTest() {
    // you can use Wrap() extension method to wrap struct by TWrapper
    Promise<TWrapper<int>> intTypePromise = Promises.Resolved(3.Wrap());
    intTypePromise
      .Then(wrappedInt => {
          if(wrappedInt.val < 2)
            return Promises.Disposed<CUnit>();
          else
            return Promises.Resolved(CUnit.Default);
        })
      .Select(_ => "foobar") // you can convert result value by Select()
      .Done(str => Debug.Log(str));
  }


```

## UniRx Integration

You can integrate Promise with [UniRx](https://github.com/neuecc/UniRx) by [UniPromise.UniRxBridge](https://github.com/shinji-yoshida/UniPromise.UniRxBridge).

```csharp
using UnityEngine;
using System.Collections;
using UniRx;
using UniPromise;
using UniPromise.UniRxBridge;

class UniRxIntegrationSample : MonoBehaviour {
  public Promise<CUnit> PromiseTimer(System.TimeSpan span) {
    // convert IObservable to Promise
    return Observable.Timer(span).PromiseOnNextUnit();
  }

  public void PromiseCanBeAddedToMonoBehaviour() {
    PromiseTimer(System.TimeSpan.FromSecond(10)).AddTo(this)
      .Done(_ => Debug.Log("This Debug.Log is not called when this component is destroyed, because the promise is disposed in this case."));
  }
}
```
