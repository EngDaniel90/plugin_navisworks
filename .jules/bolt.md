## 2024-05-24 - Optimizing Navisworks COM API marshalling and clash simulation
**Learning:** Caching Navisworks COM objects (`ComApiBridge.State` and `ToInwOpSelection`) before entering simulation loops significantly reduces O(N) marshalling overhead. Also, implementing early exit (`break`) upon clash detection prevents extremely expensive and redundant `TestsRunTest` executions.
**Action:** When working with Navisworks COM API in iterative loops, always pre-cache state/selections outside the loop and implement early exit paths to avoid unnecessary COM/API overhead.
