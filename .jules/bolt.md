## 2024-05-15 - Navisworks Simulation Loop Latency Optimization
**Learning:** Hardcoding `Task.Delay(50)` inside a high-frequency Navisworks simulation loop artificially limits throughput to a maximum of 20 iterations per second, creating a massive bottleneck.
**Action:** Replace arbitrary `Task.Delay` with `await Task.Yield()` paired with `System.Windows.Forms.Application.DoEvents()`. This maximizes the loop execution speed while maintaining UI responsiveness and forcing necessary visual geometry updates.
