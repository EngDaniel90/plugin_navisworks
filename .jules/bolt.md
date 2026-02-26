## 2025-05-15 - Hoist COM Marshalling out of Simulation Loop
**Learning:** Navisworks COM interop (specifically `ComApiBridge.ToInwOpSelection`) is an O(N) operation that creates COM wrappers for .NET ModelItems. Calling this inside a high-frequency simulation loop (every 50ms) causes significant overhead and CPU usage.
**Action:** Always hoist `ComApiBridge` calls (especially selection conversion) out of loops. Cache the `InwOpSelection` and `InwOpState10` objects and pass them to helper methods.
