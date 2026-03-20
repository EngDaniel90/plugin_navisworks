## 2024-05-24 - [Optimize COM Object Marshalling]
**Learning:** In Navisworks plugins, fetching COM API state (`ComApiBridge.State`) and converting `.NET` selection sets to COM selections (`ComApiBridge.ToInwOpSelection()`) incurs a significant O(N) performance overhead when done repeatedly within simulation loops.
**Action:** Always cache these COM bridge objects once outside of any iterative or repetitive processing loops and pass the cached instances to helper methods, preventing unnecessary recalculation and improving overall throughput.
