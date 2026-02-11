## 2024-05-23 - Navisworks COM Selection Performance
**Learning:** `ComApiBridge.ToInwOpSelection(items)` is an O(N) operation that creates significant overhead when called inside a simulation loop. Hoisting it outside the loop avoids repeated marshalling of the same object set.
**Action:** Always hoist COM selection conversions outside of iterative loops (animations, simulations).
