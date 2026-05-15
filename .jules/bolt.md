## 2024-05-24 - [Avoid O(N) COM Marshalling in Simulation Loops]
**Learning:** In Navisworks API, converting `.NET` objects to `COM` objects (via `ComApiBridge.ToInwOpSelection`) and retrieving internal state (`ComApiBridge.State`) is an expensive marshalling operation. Placing these inside high-frequency simulation loops like `RunSimulation` creates a severe O(N) performance bottleneck.
**Action:** Always calculate and cache `ComApi.InwOpState10` and `ComApi.InwOpSelection` outside the loop, passing them as arguments to helper functions (`MoveItemsUsingCOM`, `ResetItemsUsingCOM`) instead of repeatedly computing them on every loop iteration.

## 2024-05-24 - [Early Exit on Collision Found]
**Learning:** `TestsRunTest` is computationally expensive. Running it in a loop after a collision is already found on a specific step is redundant and wastes cycles.
**Action:** Implement an early exit strategy (`break` out of `ClashResult` loops and the outer simulation loop) as soon as a valid collision (`ClashResultStatus.New` or `ClashResultStatus.Active`) is found, avoiding further unnecessary simulation steps.

## 2024-05-24 - [Cache Property Accesses Outside Loops]
**Learning:** Repeatedly accessing Navisworks document properties like `doc.GetClash().TestsData` inside a loop can be slow due to internal API overhead.
**Action:** Cache these properties in local variables before entering loops or pass them as parameters to simulation methods from the caller that already has the reference.
