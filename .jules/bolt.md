## 2024-05-22 - Visual Simulation vs. Optimization
**Learning:** Legacy comments indicating "visual simulation" requirements must be respected, even if the strict code output (e.g. a list of errors) doesn't seem to need it. Breaking the visual flow constitutes a regression.
**Action:** Optimize inner loops or logic *within* the simulation steps, rather than skipping the steps entirely, unless explicitly authorized to change the feature's behavior.
