## 2024-05-24 - [Optimize Navisworks COM API Caching and Early Exit]
**Learning:** In Navisworks collision detection loops, recreating COM API objects (`ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) inside every simulation step adds severe O(N) marshalling overhead. Also, `TestsRunTest` is computationally expensive, so it should be short-circuited if a clash is detected.
**Action:** Always cache `InwOpState10` and `InwOpSelection` objects outside the simulation loop. Implement early exits (`break`) to stop redundant processing as soon as a collision is verified.
