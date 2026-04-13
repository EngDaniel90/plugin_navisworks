## 2024-05-24 - [Cache COM Marshalling in Simulation Loops]
**Learning:** In Navisworks plugins, converting large `.NET` `ModelItemCollection` objects to COM objects (`ComApiBridge.ToInwOpSelection`) is an O(N) operation and extremely slow. Calling this repeatedly inside an animation/simulation loop blocks the main thread and ruins performance.
**Action:** Always cache COM bridge states (`InwOpState10` and `InwOpSelection`) outside of simulation loops and pass the cached objects to rendering/transformation helper methods.
