## 2024-05-24 - [Navisworks COM Interop Loop Overhead]
**Learning:** `ComApiBridge.ToInwOpSelection` performs O(N) marshalling between .NET and COM. Calling it inside a high-frequency loop (e.g., simulation) creates significant CPU overhead, especially with large selections.
**Action:** Always hoist `ComApiBridge` calls (like `State` and `ToInwOpSelection`) outside of iterative loops. Pass the resulting `InwOpState10` and `InwOpSelection` objects to helper methods instead of re-fetching them.
