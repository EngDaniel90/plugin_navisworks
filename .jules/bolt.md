## 2024-05-18 - Avoid redundant COM marshalling and test evaluations in Simulation Loops
**Learning:** In Navisworks, making repeated calls to `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection()` inside a tight simulation loop adds significant O(N) marshalling overhead. Also, `TestsRunTest` is computationally expensive and shouldn't be repeatedly called if a collision has already been detected.
**Action:** Cache COM states and selection bridges outside loops. Always implement an early exit (e.g. `break`) across all nested simulation logic layers as soon as a valid clash is resolved.
