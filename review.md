# PingUtil Code Review - Task List

## Critical

- [x] 1. `RollingStatistics.Shortest` is never initialised — defaults to `0` so never gets set from a positive ping time. Fixed by initialising to `long.MaxValue`.
- [x] 2. `Ping` object not used with `using` and exception handling too narrow — only `PingException` was caught. Fixed with `using` declaration and widened catch to `Exception`.
- [x] 3. `FluentAssertions` referenced in console app project — moved to test project only.

## High

- [ ] 4. Configuration is hardcoded with no external source — no way to change host/timeout/snooze without recompiling.
- [x] 5. Display output is cryptic — single-letter abbreviations replaced with readable labels.
- [ ] 6. `SetDisplayColour` logic is incomplete — colour from previous iteration can persist; failed ping can be overwritten to white.
- [ ] 7. `AudioCue` state management leak — cluster count tracked via method parameter/return instead of internal state.
- [ ] 8. Unused `using static System.Console` in `PingEngine.cs`.

## Medium

- [ ] 9. `Thread.Sleep` blocks the thread — prevents graceful cancellation.
- [ ] 10. No graceful shutdown mechanism — no way to stop other than killing the process.
- [ ] 11. Statistics logic lives inside `PingEngine` — should be in `RollingStatistics` class.
- [ ] 12. `IPingTools` is a grab-bag of unrelated utilities.
- [ ] 13. Interfaces expose mutable setters unnecessarily.
- [ ] 14. `Stopwatch` passed around as a dependency — pass `TimeSpan` instead.

## Low

- [ ] 15. Inconsistent naming: `PingToolKit` property vs `IPingTools`/`PingTools` class.
- [ ] 16. `PingConfig.Data` has a whimsical default value.
- [ ] 17. GitHub Actions workflow uses `actions/checkout@v1` — should be v4.
- [ ] 18. `$Env:GITHUB_WORKSPACE` syntax in workflow — should use `${{ github.workspace }}`.
- [ ] 19. No `.editorconfig` or code-style enforcement.
