## 2024-11-11 - Optimize Navisworks Simulation Loops
**Learning:** Repetitive O(N) COM marshalling (like calling `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) inside collision simulation loops adds significant overhead in Navisworks API interactions. Additionally, calling `TestsRunTest` is extremely computationally expensive.
**Action:** Always pre-calculate and cache COM objects outside loops, and implement early exit strategies (`break`) in Navisworks collision detection as soon as a collision is valid, to prevent redundant test executions.
