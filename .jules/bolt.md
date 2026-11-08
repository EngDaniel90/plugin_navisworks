## 2024-11-08 - [Navisworks COM API Marshalling Bottleneck]
**Learning:** Resolving COM objects (`ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`) inside high-frequency simulation loops in Navisworks causes severe O(N) performance overhead due to repeated interop marshalling.
**Action:** Always cache these COM references (`InwOpState10` and `InwOpSelection`) outside the iterative loop and pass them as arguments to movement/reset helper methods to minimize interop boundaries crossing.
