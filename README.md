UniPromise - Promise For Unity
===
UniPromise is a Promises implementation for Unity C Sharp.
UniPromise follows Promises/A+ specification, but has some differences and extensions to fit Unity and C Sharp.

Fulfilled requirements
---
- [2.2.4.](https://promisesaplus.com/#point-34) onFulfilled or onRejected must not be called until the execution context stack contains only platform code. UniPromise fullfills this requirement by calling all callbacks from Update() of singleton class UniPromiseManager.

Differences and Extensions
---
- When onFulfilled or onRejected callback of Then() throws a exception, it is thrown normally instead of rejecting returned promise. If you want to reject returned promise on exception thrown, use ThenTryCatch() instead.
