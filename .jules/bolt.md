## 2024-04-08 - Navisworks COM Marshalling and Clash Test Early Exit
**Learning:** In Navisworks plugins, calling `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` repeatedly inside simulation loops introduces significant O(N) marshalling overhead. Also, `TestsRunTest` is extremely computationally expensive.
**Action:** Always cache `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` outside of simulation loops. Always implement early exit strategies (`break` or `return`) as soon as a valid collision is found during clash testing to prevent redundant API overhead.
