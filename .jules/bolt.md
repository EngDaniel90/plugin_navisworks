## 2023-10-25 - Navisworks Loop Bottlenecks
**Learning:** Executing `TestsRunTest` inside tight simulation loops causes extreme performance degradation due to Navisworks API overhead. Furthermore, repeated marshalling of .NET objects to COM (`ComApiBridge.ToInwOpSelection`) inside these loops incurs an O(N) penalty per step.
**Action:** Always implement early `break` logic upon collision detection to skip redundant clash tests, and hoist COM API selection marshalling outside of any simulation loops.
