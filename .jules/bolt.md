## 2024-05-23 - Navisworks COM Interop Optimization
**Learning:** Repeatedly marshalling .NET `ModelItemCollection` to COM `InwOpSelection` inside a simulation loop causes significant overhead in Navisworks plugins. The conversion is O(N) where N is the number of items.
**Action:** Convert the selection once before the loop using `ComApiBridge.ToInwOpSelection(items)` and pass the COM selection object to the loop methods.
