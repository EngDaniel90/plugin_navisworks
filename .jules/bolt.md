## 2026-06-29 - Expensive Navisworks Clash Tests
**Learning:** In Navisworks collision detection loops, executing `TestsRunTest` is extremely computationally expensive and can block the main UI thread.
**Action:** Always implement early exit strategies (e.g., `break` or `return`) as soon as a valid collision is found to prevent redundant API overhead and unnecessary simulation steps.
