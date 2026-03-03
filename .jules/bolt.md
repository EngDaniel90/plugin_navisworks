## 2024-05-15 - [Navisworks COM API Loop Optimization]
**Learning:** In Navisworks API, repeated calls to ComApiBridge.ToInwOpSelection(items) and ComApiBridge.State inside a tight simulation loop cause severe O(N) marshalling overhead.
**Action:** Cache the state and converted selection COM objects before the loop and pass them to helper methods like MoveItemsUsingCOM and ResetItemsUsingCOM.
