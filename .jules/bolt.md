## 2024-05-18 - Early exits in Navisworks simulation loops
**Learning:** Navisworks collision detection loops (`TestsRunTest`) are computationally expensive. Additionally, repeated COM API marshalling (like `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) within tight loops causes massive overhead.
**Action:** Implement early exit strategies (`break` or `return`) as soon as a valid collision is found to prevent redundant API overhead. Cache COM objects and Navisworks API properties outside of simulation loops and pass them directly to helper methods.
