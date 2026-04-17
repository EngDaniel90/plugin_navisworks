## 2025-02-24 - [COM Marshalling and Early Exits]
**Learning:** Navisworks API properties like `ComApiBridge.State` and `.ToInwOpSelection(items)` have significant marshalling overhead and `TestsRunTest` is extremely computationally expensive. Calling them inside a tight simulation loop degrades performance.
**Action:** Cache COM objects outside the loop and implement early exit strategies (`break`) upon detecting collisions to avoid redundant operations.
