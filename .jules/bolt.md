## 2024-05-24 - [Performance Improvement in Navisworks Simulation Loop]
**Learning:** Resolving `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` on every visual update inside a simulation loop creates massive O(N) marshalling overhead. Also, `TestsRunTest` is extremely computationally expensive.
**Action:** Always cache Navisworks COM API state and selection arrays outside of movement or simulation loops. Additionally, implement early exit strategies (`break`) for `TestsRunTest` validations to avoid redundant collision checks once a clash is established.
