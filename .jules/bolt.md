## 2024-06-10 - Navisworks COM Marshalling and Clash Loop Optimization
**Learning:** Navisworks collision detection loops suffer massive performance degradation when executing `TestsRunTest` redundantly after a collision is found, and recreating `ComApi.InwOpState10` and `ComApi.InwOpSelection` repeatedly in movement loops causes O(N) marshalling overhead.
**Action:** Always implement early exits (`break`) once a collision is verified, and cache `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` outside of simulation loops to pass into helper functions.
