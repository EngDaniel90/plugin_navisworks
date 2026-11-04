## 2024-05-24 - [Avoid redundant tests and overhead in Navisworks Clash Simulation]
**Learning:** Repetitive O(N) COM marshalling overhead and expensive redundant `TestsRunTest` executions in Navisworks simulation loops severely impact performance. Evaluating `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection(items)` inside tight loops forces continuous conversion.
**Action:** Cache COM state and selection once before the loop. Furthermore, implement an early exit (`break`) to terminate the simulation as soon as a collision is detected, saving expensive clash test API calls.
