## 2025-05-23 - COM Marshalling Overhead in Simulation Loops
**Learning:** Invoking `ComApiBridge.ToInwOpSelection` creates significant O(N) marshalling overhead.
**Action:** Hoist this call outside of iterative loops and cache the result.
