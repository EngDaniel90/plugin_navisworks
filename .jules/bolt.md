## 2024-04-19 - COM API Marshalling and Execution Overhead in Loops
 **Learning:** Converting .NET API collections to COM API selections inside iterative loops introduces severe O(N) marshalling overhead. Additionally, continuing simulation loops after a collision is found leads to redundant and computationally expensive `TestsRunTest` executions.
 **Action:** Cache `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection` once outside simulation loops and pass them to helper methods. Implement early exit strategies (`break`) immediately when a collision is detected.
