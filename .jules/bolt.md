## 2026-02-12 - Navisworks Interop Hoisting
**Learning:** `ComApiBridge.ToInwOpSelection` performs O(N) marshaling. In simulation loops, this call MUST be hoisted outside the loop to avoid severe performance degradation.
**Action:** Always cache `InwOpSelection` and `InwOpState10` before entering animation/simulation loops in Navisworks plugins.
