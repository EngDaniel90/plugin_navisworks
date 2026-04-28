## 2024-04-28 - [Minimize Navisworks API Overhead]
**Learning:** Navisworks API interactions, specifically `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection`, incur significant O(N) marshalling overhead when executed inside loops (e.g., simulation descents). Furthermore, `TestsRunTest` is extremely computationally expensive.
**Action:** Always cache these COM objects once outside of any loop to avoid repetitive overhead. Additionally, implement early exit strategies (`break` or `return`) as soon as a valid collision is detected to prevent redundant and expensive clash engine executions.
