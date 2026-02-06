## 2025-05-15 - [Navisworks COM Interop Performance]
**Learning:** `ComApiBridge.ToInwOpSelection` is expensive as it marshals .NET `ModelItemCollection` to COM `InwOpSelection`. Calling it repeatedly inside a simulation loop severely degrades performance.
**Action:** Always convert selections to COM objects once outside of loops and pass the COM object to methods requiring it.
