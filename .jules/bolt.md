## 2024-05-26 - [Cache COM objects in Navisworks simulation loops]
**Learning:** Calling `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection(items)` repeatedly within a tight Navisworks simulation loop creates significant O(N) marshalling overhead, slowing down visual transformations and rendering.
**Action:** Always cache these Navisworks COM objects outside of performance-critical simulation loops and pass the cached `ComApi.InwOpState10` and `ComApi.InwOpSelection` references directly to helper methods.
