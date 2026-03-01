## 2024-05-24 - [Optimize Navisworks Simulation Loops]
**Learning:** In long-running Navisworks simulation loops, `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection(items)` introduce significant O(N) marshalling overhead when called repeatedly. Hardcoded delays like `Task.Delay(50)` also unnecessarily block UI thread processing.
**Action:** Cache COM objects (`InwOpState10` and `InwOpSelection`) once outside the simulation loop. Replace hardcoded blocking delays with `await Task.Yield()` to maximize throughput while maintaining UI responsiveness.
