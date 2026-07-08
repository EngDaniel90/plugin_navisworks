## 2024-05-24 - Navisworks COM API Overhead and Early Exits

**Learning:** Navisworks COM API properties such as `ComApiBridge.State` and `ComApiBridge.ToInwOpSelection(items)` involve cross-boundary marshalling which adds significant O(N) overhead when invoked repeatedly inside simulation or testing loops. Furthermore, running redundant `clashTestsData.TestsRunTest` after a collision is already found generates massive unnecessary computation, blocking the UI thread and starving CPU.

**Action:** Always extract and cache COM references (`State` and `InwOpSelection`) at the beginning of methods before passing them into inner loop calls or utility modifiers (`MoveItemsUsingCOM`). Additionally, ALWAYS implement immediate early exits (`break` or `return`) the moment a valid collision status (`New` or `Active`) is detected to halt further tests and prevent unneeded loop traversals.
