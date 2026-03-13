
## 2024-05-19 - [Optimize Navisworks Simulation Loops]
**Learning:** In Navisworks COM API interactions, repetitive marshalling like `ComApiBridge.ToInwOpSelection` and resolving `ComApiBridge.State` inside loops creates massive O(N) overhead.
**Action:** Always extract COM selection and state resolution outside of frequent `while`/`for` simulation loops, passing the resolved COM objects to helper methods.
