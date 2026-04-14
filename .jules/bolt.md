
## 2024-04-14 - Navisworks COM API Overhead inside Simulation Loops
**Learning:** In Navisworks plugins, COM API interop (specifically `ComApiBridge.ToInwOpSelection` and accessing `ComApiBridge.State`) introduces significant marshalling overhead. Repeatedly invoking these inside a simulation loop causes measurable lag. Furthermore, calling `TestsRunTest` is extremely slow.
**Action:** Always cache COM state and converted selections outside tight loops. Implement an early exit (e.g., `break` or `return`) as soon as the desired condition (e.g., collision detected) is met to avoid redundant expensive API calls.
