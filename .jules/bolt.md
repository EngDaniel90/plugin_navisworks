## 2024-05-24 - Navisworks TestsRunTest Performance Overhead
**Learning:** In Navisworks collision detection loops, executing `TestsRunTest` is extremely computationally expensive. If a simulation loop does not exit immediately after a collision is found, it will perform redundant tests for the remaining loop iterations, significantly degrading performance.
**Action:** Always implement early exit strategies (e.g., `break` or `return`) in simulation or iteration loops as soon as a valid collision is found to prevent redundant API overhead.
