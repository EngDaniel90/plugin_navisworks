# Bolt's Journal

## 2024-05-23 - Performance Anti-Pattern: Loop COM Marshalling
**Learning:** In Navisworks API, converting .NET collections to COM selections (`ComApiBridge.ToInwOpSelection`) is an O(N) operation. Doing this inside a simulation loop (even implicitly via helper methods) causes massive performance degradation as N grows.
**Action:** Always hoist `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` calls outside of iterative loops. Pass the resulting COM objects (`InwOpState10`, `InwOpSelection`) to helper methods.
