## 2024-05-23 - Navisworks API Loop Optimization
**Learning:** Repeated access to Navisworks API properties like `doc.Models` and `doc.GetClash()` inside tight simulation loops incurs significant overhead, likely due to COM interop or internal lookups.
**Action:** Always cache these API objects into local variables outside of `while` or `for` loops before iterating.
