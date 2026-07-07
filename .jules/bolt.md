## 2024-05-24 - Optimizing Navisworks Collision Simulation
**Learning:** In Navisworks collision detection loops, executing `TestsRunTest` is extremely computationally expensive. Additionally, repeatedly fetching `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` within the loop incurs redundant O(N) marshalling overhead.
**Action:** Always cache COM objects outside simulation loops and implement early exit strategies (`break` or `return`) as soon as a valid collision is found to prevent redundant API overhead.
