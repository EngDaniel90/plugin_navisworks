## 2024-06-25 - Navisworks COM API Overhead inside Simulation Loops
**Learning:** Calling `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection(items)` repeatedly inside rendering or simulation loops (like `RunSimulation`) introduces significant unnecessary O(N) marshalling overhead between .NET and COM boundaries.
**Action:** Always cache these heavy COM boundary conversions once outside the main simulation loops and pass them to visual transformation methods.

## 2024-06-25 - Navisworks Clash Testing Redundancy
**Learning:** Executing `clashTestsData.TestsRunTest(test)` inside a tight descent loop is extremely computationally expensive. Processing every subsequent result node and continuing the loop after a collision has already been verified is redundant.
**Action:** Implement early exit strategies (`break` or `return`) from results loops and parent simulation loops immediately upon detecting the first valid `ClashResult` (`New` or `Active`) to halt unnecessary geometry rendering and collision checks.
