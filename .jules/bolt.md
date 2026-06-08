## 2025-02-20 - [Performance Optimization in Navisworks Plugin Simulation Loop]
**Learning:** In Navisworks collision detection loops (`TestsRunTest`), fetching properties (like COM API `State` and `ToInwOpSelection`) and continuing iterative loops after a clash is found can create severe O(N) overhead.
**Action:** Cache the COM state (`ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) outside of simulation loops and always employ an early exit (`break`) immediately when a `ClashResult` indicates a new or active collision.
