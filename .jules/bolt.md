## 2024-05-23 - Navisworks API Simulation Loop Bottlenecks
**Learning:** In Navisworks plugins, `ComApiBridge.ToInwOpSelection()` performs expensive O(N) marshalling and `TestsRunTest()` is highly computationally intensive. Running these inside a simulation loop without caching and early exits causes significant performance degradation.
**Action:** Always cache COM bridged objects (`InwOpState10`, `InwOpSelection`) outside simulation loops. Implement aggressive early exits (`break`) from both inner and outer loops immediately upon detecting a valid collision to avoid redundant clash tests.
