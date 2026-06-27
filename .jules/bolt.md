## 2026-06-27 - Expensive API operations inside Navisworks clash simulation loops
**Learning:** Calling Navisworks APIs like `TestsRunTest` or converting .NET objects to COM objects inside tight simulation loops causes significant performance overhead and unnecessary calculations, especially when a collision has already been detected.
**Action:** Always cache COM marshalling results (`ComApiBridge.ToInwOpSelection`) outside of loops and implement early exit strategies (`break`) to terminate expensive simulations as soon as the target condition is met.
