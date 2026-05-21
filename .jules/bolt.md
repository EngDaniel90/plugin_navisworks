
## 2024-05-24 - [Optimizing COM Interop in Navisworks API]
**Learning:** Calling `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection()` inside a tight simulation loop causes severe O(N) COM marshalling overhead, slowing down the animation/clash detection process significantly.
**Action:** Always cache these COM references at the start of the simulation loop and pass them explicitly to transformation helper methods.
