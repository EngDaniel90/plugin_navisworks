## 2024-05-22 - [Navisworks COM Interop Performance]
**Learning:** Invoking `ComApiBridge.ToInwOpSelection` creates significant O(N) marshalling overhead inside loops.
**Action:** Always hoist this call (and `ComApiBridge.State` access) outside of iterative simulation loops and pass the resulting COM objects as arguments to helper methods.
