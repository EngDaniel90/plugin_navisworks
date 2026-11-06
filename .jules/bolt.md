## 2024-11-06 - [Navisworks Simulation Loop Optimizations]
**Learning:** In Navisworks API, `ComApiBridge.ToInwOpSelection` has O(N) marshalling overhead which gets severely amplified in tight simulation loops. Additionally, `TestsRunTest` is computationally expensive; failing to exit early upon detecting a collision wastes processing cycles.
**Action:** Always pre-cache COM state and selections outside simulation loops, pass them to helper methods, and use `break` to exit both inner result-checking loops and outer simulation loops immediately when a valid collision is found.
