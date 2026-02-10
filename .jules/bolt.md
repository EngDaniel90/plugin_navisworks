## 2024-05-22 - Optimizing Navisworks COM Marshalling
**Learning:** `ComApiBridge.ToInwOpSelection` performs expensive O(N) marshalling. Calling it inside a loop (like a simulation) kills performance.
**Action:** Always hoist `ToInwOpSelection` calls outside loops. Cache the `InwOpSelection` object and pass it to helper methods.

## 2024-05-22 - Optimizing Clash Detection Loops
**Learning:** When checking for *existence* of a collision in a step, use `break` after the first hit.
**Action:** Add `break` statements in clash result loops if you only need a boolean result for that step.
