## 2024-05-23 - Navisworks COM Interop Overhead
**Learning:** Frequent marshalling between Navisworks .NET API and COM API (e.g., `ComApiBridge.ToInwOpSelection`) inside iterative loops creates significant performance overhead.
**Action:** Always hoist COM selection and state retrieval outside of simulation loops. Cache `InwOpState10` and `InwOpSelection` objects and reuse them.
