# Bolt's Journal

## 2024-05-22 - Hoisting COM Interop Selection
**Learning:** `ComApiBridge.ToInwOpSelection(items)` is an O(N) operation that creates a new COM selection object from a .NET `ModelItemCollection`. Calling this repeatedly inside a simulation loop (e.g., inside `MoveItemsUsingCOM`) causes significant performance overhead due to marshalling and object creation.
**Action:** Always hoist `ComApiBridge.ToInwOpSelection` and `ComApiBridge.State` outside of iterative loops. Cache these objects and pass them as arguments to helper methods that perform the COM operations.
