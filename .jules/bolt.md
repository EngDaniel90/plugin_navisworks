## 2024-05-24 - [Optimize Navisworks COM interop in simulation loops]
 **Learning:** Repetitive calls to `ComApiBridge.ToInwOpSelection` inside a simulation loop cause unnecessary O(N) marshalling overhead per frame. The resulting `InwOpState10` and `InwOpSelection` objects can safely be cached outside the loop and reused for sequential translations.
 **Action:** Always hoist Navisworks COM API bridging calls (like `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) out of tight simulation loops to improve visual fluidity and reduce overall processing time.
