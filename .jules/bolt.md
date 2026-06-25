## 2026-06-25 - Caching COM Objects and Early Exit Strategy in Collision Detection Loops
**Learning:** Navisworks COM API marshalling (e.g., `ComApiBridge.ToInwOpSelection`) and `TestsRunTest` executions are extremely expensive when placed inside simulation loops.
**Action:** Cache COM objects outside simulation loops and pass them as parameters to visual override helpers. Always implement an early exit strategy (e.g. `break` out of loops) as soon as a collision is detected to avoid redundant overhead.
