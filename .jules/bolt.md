## 2024-05-24 - Navisworks API COM Marshalling Overhead
**Learning:** Resolving COM selections (`ComApiBridge.ToInwOpSelection`) and accessing the COM state (`ComApiBridge.State`) inside tight rendering or simulation loops creates an O(N) marshalling overhead that significantly degrades performance. Navisworks COM API interactions are expensive and thread-affine, compounding the cost.
**Action:** Always cache `InwOpState10` and `InwOpSelection` outside of simulation loops and pass the cached instances to helper methods that execute transformations.

## 2024-05-24 - Navisworks Clash Testing Early Exit
**Learning:** Calling `TestsRunTest` in the Navisworks Clash API is computationally expensive. Running it repeatedly in a loop without early-out logic after a collision is found wastes processing time.
**Action:** Implement `break` statements immediately upon detecting a valid collision (e.g., `ClashResultStatus.New` or `ClashResultStatus.Active`) to stop redundant validations and loop iterations.
