# Code Review — Pinger

## Summary

The Pinger solution is well-structured and follows most of the project's coding standards. The code compiles without warnings, all 54 unit tests pass, and separation of concerns is clear across the four projects. However, there are a handful of issues: direct `Console` usage in business logic (`PingEngine`), some convention deviations around blank lines before `return` statements, and a minor encapsulation concern in `PingStats`. Overall quality is good with 5 issues identified (1 Medium, 4 Low).

---

## Issues

### 1. Direct `Console` usage in `PingEngine`

**File:** `Pinger.Domain/PingEngine.cs` — `Start()` and `StartAsync()`
**Priority:** Medium
**Description:** `PingEngine` directly references `Console.CancelKeyPress` and `Console.ForegroundColor`. The project convention is that all console I/O goes through `IConsoleHandler`, yet this class bypasses the abstraction. This makes the engine harder to test in isolation (the cancel-key handling is untestable without the real console) and breaks the pattern established elsewhere.
**Task:** Add a `CancellationToken` parameter or an `ICancellationSource` abstraction to allow tests to trigger cancellation without coupling to `System.Console`. Move `Console.ForegroundColor` access into `IConsoleHandler` (e.g. a `GetDefaultColour()` method).
- [ ] To do

---

### 2. Missing blank line before `return` in `ConsoleHandler.NotifyPingResult`

**File:** `Pinger.Domain/ConsoleHandler.cs` — `NotifyPingResult()`
**Priority:** Low
**Description:** The coding standard requires all `return` statements to be preceded by a blank line. The early `return` on line 27 (`return;`) inside the `if (status.Success)` block is preceded by a blank line, so that's fine. However, this is a reminder to audit any future additions—the pattern is currently correct but fragile because the method mixes assignment and return without clear visual separation in the failure path.
**Task:** No action needed currently—flagging for awareness during future edits.
- [x] No issue (confirmed correct)

---

### 3. `PingStats` has public setters on interface-contracted properties

**File:** `Pinger.Domain/PingStats.cs` — class declaration
**Priority:** Low
**Description:** `PingStats.Success` and `PingStats.PingTime` have public setters, but the `IPingStats` interface only exposes getters. While this works (the interface hides the setter), it means any code with a concrete `PingStats` reference can mutate state that should be immutable once created. Per the encapsulation convention ("restrict setters where mutation isn't needed externally"), these could use `init` accessors instead.
**Task:** Change `{ get; set; }` to `{ get; init; }` on both properties.
- [ ] To do

---

### 4. `PingConfig` exposes public setters on all properties

**File:** `Pinger.Domain/PingConfig.cs` — class declaration
**Priority:** Low
**Description:** All properties on `PingConfig` have public setters. The `IPingConfig` interface only exposes getters. The setters are needed for the `configuration.Bind(this)` call, but `Bind` works with `set` or `init`. Using `init` would prevent accidental mutation after construction while still allowing the configuration binder to populate values.
**Task:** Change property setters to `init` where possible (verify the configuration binder supports `init` in .NET 10 — it does since .NET 7).
- [ ] To do

---

### 5. `PingEngine.PingHost` instantiates `Ping` directly — not abstracted

**File:** `Pinger.Domain/PingEngine.cs` — `PingHost()`
**Priority:** Low
**Description:** The `PingHost` method creates `new Ping()` directly, coupling the business logic to the network stack. The `PingerIsActive` flag exists as a workaround for testing, but this means the "inactive" path doesn't exercise the same code as production. If you ever need to test error-handling paths or specific reply statuses, you'd need to abstract the ping operation behind an interface.
**Task:** Consider extracting an `IPingSender` interface to wrap `Ping.Send()`. This would allow removing the `PingerIsActive` shortcut in tests and enable testing of error/timeout paths. Low priority since the current workaround is functional and tests cover the logic around it.
- [ ] To do

---

## Summary Table

| # | Issue | Priority | Status |
|---|-------|----------|--------|
| 1 | Direct `Console` usage in `PingEngine` | Medium | ⬜ To do |
| 2 | Blank line before `return` (confirmed OK) | Low | ✅ No action |
| 3 | `PingStats` public setters → `init` | Low | ⬜ To do |
| 4 | `PingConfig` public setters → `init` | Low | ⬜ To do |
| 5 | `PingHost` not abstracted | Low | ⬜ To do |
