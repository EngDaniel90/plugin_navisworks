## 2024-03-17 - Navisworks COM Marshalling Bottleneck
**Learning:** Calling `ComApiBridge.ToInwOpSelection` inside a simulation loop creates massive O(N) marshalling overhead per step because it crosses the COM boundary repeatedly for the same static selection.
**Action:** Always cache COM selections (`InwOpSelection`) and state (`InwOpState10`) outside of tight loops to prevent repetitive interop overhead.
