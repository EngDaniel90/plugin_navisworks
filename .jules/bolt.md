## 2024-11-20 - [Optimize COM API Marshalling in Loops]
**Learning:** In Navisworks plugins, calling `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` inside tight simulation loops causes significant O(N) marshalling overhead due to repeated COM boundary crossings.
**Action:** Always cache the `InwOpState10` and `InwOpSelection` objects outside of loops and pass them directly to helper methods (like `MoveItemsUsingCOM` and `ResetItemsUsingCOM`) to eliminate repetitive COM marshalling.

## 2024-11-20 - [Avoid Redundant Clash Tests]
**Learning:** `TestsRunTest` is computationally expensive in Navisworks. If a collision is found during a simulation step, continuing the clash loop or further height steps is redundant and wastes processing time.
**Action:** Implement early exit strategies (`break` or `return`) from both inner clash evaluation loops and outer simulation loops as soon as a valid collision (`ClashResultStatus.New` or `ClashResultStatus.Active`) is detected.
