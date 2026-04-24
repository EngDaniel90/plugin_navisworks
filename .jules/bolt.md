## 2024-05-24 - Navisworks COM API Caching
**Learning:** Instantiating COM state (`ComApiBridge.State`) and marshalling .NET selections to COM (`ComApiBridge.ToInwOpSelection`) inside high-frequency simulation loops incurs severe O(N) performance overhead and latency in Navisworks Manage due to boundary crossing.
**Action:** Always cache COM state and converted COM selections into local variables outside simulation/rendering loops and pass them by reference to visual update methods.

## 2024-05-24 - TestsRunTest Early Exit Strategy
**Learning:** `TestsRunTest` in the Navisworks Clash engine is extremely computationally expensive. Running it redundantly after a valid collision has already been detected wastes significant CPU cycles.
**Action:** Implement an early exit (`break` or `return`) as soon as a collision status of `New` or `Active` is confirmed during iterative clash test loops to prevent unnecessary subsequent test runs.
