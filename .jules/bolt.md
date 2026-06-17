## 2026-06-17 - [COM Marshalling Overhead in Simulation Loops]
**Learning:** `ComApiBridge.ToInwOpSelection` causes O(N) marshalling overhead when placed inside a simulation loop in Navisworks API. Also, Navisworks `TestsRunTest` is computationally expensive so early exits are crucial.
**Action:** Cache `ComApi.InwOpState10` and `ComApi.InwOpSelection` outside of simulation loops and pass them to COM manipulation methods, and add explicit break conditions as soon as a collision is detected.
