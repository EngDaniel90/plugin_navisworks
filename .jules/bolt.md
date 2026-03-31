## 2024-05-24 - Avoid O(N) COM Marshalling Overhead in Simulation Loops
**Learning:** In Navisworks plugins, invoking `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` inside tight simulation loops causes significant O(N) marshalling overhead on every iteration.
**Action:** Cache the resulting `InwOpState10` and `InwOpSelection` objects once outside the loop and pass them as references to helper methods to eliminate repetitive marshalling.

## 2024-05-24 - Early Exit for Expensive Navisworks Operations
**Learning:** Executing `TestsRunTest` in collision detection loops is extremely computationally expensive. Without early exit strategies, redundant API overhead occurs even after a collision is found.
**Action:** Always implement early exit strategies (e.g., `break`) as soon as a valid collision is detected in loops calling `TestsRunTest`.
