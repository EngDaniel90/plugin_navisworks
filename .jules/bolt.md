## 2024-05-24 - [Avoid O(N) Marshalling Overhead in Loops]
 **Learning:** Calling Navisworks COM API bridging methods like `ComApiBridge.ToInwOpSelection(items)` is an O(N) operation. Executing this repeatedly inside a movement simulation loop creates massive overhead and slows down the loop significantly, especially for large selections.
 **Action:** Always cache COM API objects (`InwOpState10`, `InwOpSelection`) before entering performance-critical simulation or rendering loops, and pass the cached objects to the movement/reset helpers.
