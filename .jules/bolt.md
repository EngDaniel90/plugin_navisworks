
## 2024-05-24 - Avoid Repetitive O(N) Marshalling and redundant Navisworks Checks
**Learning:** Initializing COM selection variables (`ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) inside inner loops creates massive O(N) overhead during simulation because it repetitively recalculates the same collections. Also, `TestsRunTest` is a computationally expensive operation in Navisworks API, calling it after a collision is already found in the same height stack is a waste of resources.
**Action:** Always cache static `ComApiBridge` state and selection properties outside of recurrent simulation/clash loops, and pass them as parameters to helper methods (like Move or Reset). Also, enforce an early exit (`break`) from iteration loops immediately upon collision confirmation.
