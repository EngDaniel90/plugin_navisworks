## 2024-05-22 - [Optimization vs Functionality]
**Learning:** Optimizing a loop by skipping operations (like `TestsRunTest`) based on a boolean flag ("already failed") can break implicit functionality (like reporting *all* failures or populating a UI artifact).
**Action:** When optimizing "Pass/Fail" loops, verify if the *intermediate* results (e.g., specific errors found after the first one) are consumed by the user or system. If so, only optimize "pure" overhead like UI updates (`DoEvents`), not the core logic.
