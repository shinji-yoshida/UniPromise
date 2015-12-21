UniPromise - Promise For Unity
===
UniPromise is a Promises implementation for Unity C Sharp.
UniPromise follows Promises/A+ specification, but has some differences and extensions to fit Unity and C Sharp.

fulfilled requirements
---
- 2.2.4. onFulfilled or onRejected must not be called until the execution context stack contains only platform code. UniPromise fullfills this requirement by calling all callbacks from Update() of singleton class UniPromiseManager.
