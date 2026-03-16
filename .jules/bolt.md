## 2024-05-24 - [Avoid Repetitive Navisworks COM API Marshalling in Loops]
 **Learning:** Using `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` inside a simulation or iterative visual loop introduces heavy, repetitive O(N) marshalling overhead across the .NET/COM boundary.
 **Action:** Always cache these core COM interop objects (`InwOpState10` and `InwOpSelection`) before entering high-frequency movement or collision analysis loops, and pass them as method arguments.
