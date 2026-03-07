## 2024-05-18 - [COM Marshalling Overhead in Simulation Loops]
**Learning:** [Repeatedly calling ComApiBridge.ToInwOpSelection inside a simulation loop creates a significant O(N) marshalling overhead because it converts the entire .NET selection to COM on every step.]
**Action:** [Cache the COM state and selection outside the loop and pass them as arguments to transformation methods like MoveItemsUsingCOM.]
