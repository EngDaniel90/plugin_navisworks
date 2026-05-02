## 2024-05-02 - Optimize Navisworks Simulation Loops
**Learning:** Repetitive COM marshalling inside a simulation loop (e.g., retrieving `ComApiBridge.State` and casting `InwOpSelection`) introduces O(N) overhead. Additionally, executing `TestsRunTest` is extremely computationally expensive in Navisworks collision detection.
**Action:** Always cache COM objects outside loops to eliminate repetitive marshalling overhead and implement early exit strategies (e.g., `break`) as soon as a valid collision is found to prevent redundant API overhead.
