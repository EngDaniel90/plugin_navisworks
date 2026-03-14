## 2024-05-24 - Navisworks UI Thread Affinity and COM Marshalling Overhead
**Learning:**
1. In Navisworks API simulation loops, using `Task.Delay(50)` artificially bottlenecks throughput because it introduces a hard delay. However, removing it entirely freezes the UI and prevents Navisworks from rendering geometry updates required for accurate visual simulation. The solution for synchronous rendering throughput without blocking is using `await Task.Yield();` paired directly with `Application.DoEvents();` to force the Windows Message pump.
2. `ComApiBridge.ToInwOpSelection(items)` performs an O(N) mapping from .NET selection to COM wrappers. Calling this inside a tight simulation loop (e.g. running every few millimeters of movement) introduces severe performance overhead.

**Action:**
1. Always prefer `Task.Yield()` + `Application.DoEvents()` over hardcoded `Task.Delay()` when forcing Navisworks view updates in high-frequency loops.
2. Always cache COM API wrappers (`InwOpState10`, `InwOpSelection`) outside of iteration loops and pass them as method arguments.
