## 2026-04-06 - Optimizing Navisworks COM API Calls & Simulation Loops
**Learning:** Repeatedly calling ComApiBridge.ToInwOpSelection(items) and ComApiBridge.State within a simulation loop introduces severe O(N) marshalling overhead. Navisworks Clash Engine is computationally expensive.
**Action:** Always cache COM objects outside loops. Always implement early exits (break) in simulation loops as soon as a valid clash is detected to avoid redundant testing.
