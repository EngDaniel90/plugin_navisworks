## 2024-05-24 - Optimization of Navisworks COM API Marshalling and Throughput in Simulation Loops

**Learning:**
In `AutoLiftingClashAnalysis`, repeatedly converting .NET `ModelItemCollection` objects to COM `InwOpSelection` objects (`ComApiBridge.ToInwOpSelection`) and retrieving the internal COM state (`ComApiBridge.State`) inside a tight simulation loop causes significant O(N) marshalling overhead. Additionally, using hardcoded delays like `Task.Delay(50)` blocks the UI thread unnecessarily, and the simulation loop was continuing to process subsequent descent steps even after a collision was found.

**Action:**
- Extract and cache `ComApi.InwOpState10` and `ComApi.InwOpSelection` objects before the simulation loop begins, passing them as parameters to helper methods to eliminate repeated marshalling overhead.
- Replace `await Task.Delay(50)` with `Application.DoEvents(); await Task.Yield();` to maintain UI responsiveness and Navisworks rendering updates while maximizing throughput without hardcoded wait times.
- Implement early loop termination (`break;`) as soon as a valid collision is detected to skip unnecessary operations.