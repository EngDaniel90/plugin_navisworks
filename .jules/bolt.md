## 2024-05-24 - Performance Optimizations
**Learning:** Found several optimization opportunities in `LiftingForm.cs`:
1. Navisworks Clash Simulation: Iterating the `testsRunTest` is computationally expensive. There is an early exit condition `// Opcional: break; se quiser parar na primeira batida` that is missing. Adding `break` when collision is detected avoids unnecessary iteration.
2. In `RunSimulation`, we call `MoveItemsUsingCOM` multiple times and inside `finally` we call `ResetItemsUsingCOM`. Each time they are called, `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` are evaluated. We should cache them outside the loop to avoid COM API overhead.
**Action:** Implement these optimizations. Cache `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection(items)` outside the simulation loop. Break the clash loop once `collisionDetected` is true.
