## 2024-05-24 - [Avoid Repeated Marshalling in Simulation Loops]
**Learning:** Invoking COM object wrappers (like `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection(items)`) inside tight simulation loops causes O(N) performance overhead per frame due to repeated .NET-to-COM marshalling.
**Action:** Always cache these COM references outside the simulation loop and pass them to frame-updating methods (like `MoveItemsUsingCOM` and `ResetItemsUsingCOM`) to maximize Navisworks API throughput.
