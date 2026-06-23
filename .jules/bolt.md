## 2024-05-24 - Avoid COM API Marshalling Overhead in Navisworks Loops
**Learning:** In Navisworks plugins, `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` incur an O(N) marshalling overhead. Looking these up repeatedly inside a simulation loop severely bottlenecks performance.
**Action:** Always cache the resulting `InwOpState10` and `InwOpSelection` objects *outside* any loops and pass them to helper methods (like transform or reset methods) to prevent redundant marshalling.

## 2024-05-24 - Essential Early Exits in Navisworks Clash Testing
**Learning:** `TestsRunTest` is extremely expensive computationally. Allowing a simulation loop to continue running clash tests or checking results after a valid collision has already been found wastes significant CPU cycles.
**Action:** Always implement early exit strategies (using `break`) to exit out of both the result-checking inner loop and the main simulation loop immediately upon detecting the first valid collision.
