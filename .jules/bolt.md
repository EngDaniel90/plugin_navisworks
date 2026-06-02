## 2025-02-18 - Caching Navisworks API objects and Early Exits
**Learning:** In Navisworks collision detection loops, API calls like `TestsRunTest` are computationally expensive, and repeated marshalling of COM objects inside a loop incurs significant overhead.
**Action:** Always cache Navisworks API properties (`ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) outside simulation loops, and implement early exit strategies (`break`) as soon as a valid clash is found to prevent redundant API overhead.
