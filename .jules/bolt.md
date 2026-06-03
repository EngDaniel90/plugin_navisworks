## 2025-02-18 - Navisworks COM API Overhead in Simulation Loops
**Learning:** In Navisworks plugins, interacting with the COM API (`Autodesk.Navisworks.Interop.ComApi`) inside simulation loops (e.g., iteratively updating model item positions) introduces significant O(N) marshalling overhead when repeatedly converting .NET objects to COM objects (`ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`).
**Action:** Always cache the COM state (`InwOpState10`) and COM selection objects (`InwOpSelection`) *outside* of high-frequency loops and pass them as parameters to helper methods (e.g., `MoveItemsUsingCOM`) to avoid redundant cross-boundary conversions.

## 2025-02-18 - Early Exits in Navisworks Clash Tests
**Learning:** Executing `TestsRunTest` inside collision detection loops is extremely computationally expensive. Processing the entire sequence of steps when a collision has already occurred wastes significant API resources.
**Action:** Implement aggressive early exit strategies (`break` or `return`) in both the inner validation loop (clash results iteration) and the outer simulation loop as soon as a valid collision (`ClashResultStatus.New` or `ClashResultStatus.Active`) is confirmed.
