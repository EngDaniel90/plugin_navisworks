## 2024-07-06 - [Navisworks COM API Overhead in Simulation Loops]
**Learning:** Instantiating COM objects (`ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) inside a high-frequency simulation loop introduces significant O(N) marshalling overhead and degrades performance in Navisworks plugins.
**Action:** Always cache these COM objects once outside the loop and pass them to helper methods to eliminate repetitive marshalling, and implement early exit strategies (e.g. `break`) as soon as a collision is detected to skip redundant `TestsRunTest` executions.
