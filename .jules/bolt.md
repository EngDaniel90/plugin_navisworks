## 2024-05-24 - [Overhead of COM Selection Marshalling and early TestsRunTest exits]
 **Learning:** In Navisworks plugins, `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` incur O(N) marshalling overhead. Additionally, `TestsRunTest` is computationally expensive within simulation loops.
 **Action:** Always cache COM state and selection outside of simulation loops and pass them to helper methods. Implement an early exit strategy (e.g. `break`) to stop clash test evaluations and loop iterations as soon as a collision is verified.
