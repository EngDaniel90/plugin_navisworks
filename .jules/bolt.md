## 2024-05-22 - Repeated Marshalling in Simulation Loop
**Learning:** In Navisworks API, converting `.NET` objects (like `ModelItemCollection`) to `COM` objects (like `InwOpSelection`) via `ComApiBridge` is an expensive operation due to marshalling. Doing this inside a high-frequency simulation loop kills performance.
**Action:** Always perform `ComApiBridge` conversions *once* before entering loops and reuse the COM object.

## 2024-05-22 - Missing Navisworks Dependencies
**Learning:** The project depends on local Navisworks installation DLLs (`Autodesk.Navisworks.Api`, etc.) which are not available in the CI/CD or this sandbox environment.
**Action:** Verification must rely on static code analysis and "best effort" builds (checking for syntax errors while ignoring reference errors). Cannot run unit tests or full builds.
