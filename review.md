# PingUtil Code Review - Task List

## Critical

- [x] 1. `RollingStatistics.Shortest` is never initialised — defaults to `0` so never gets set from a positive ping time. Fixed by initialising to `long.MaxValue`.
- [x] 2. `Ping` object not used with `using` and exception handling too narrow — only `PingException` was caught. Fixed with `using` declaration and widened catch to `Exception`.
- [x] 3. `FluentAssertions` referenced in console app project — moved to test project only.

## High

- [x] 4. Configuration is hardcoded with no external source — now loads from `appsettings.json` with fallback to defaults.
- [x] 5. Display output is cryptic — single-letter abbreviations replaced with readable labels.
- [x] 6. `SetDisplayColour` logic is incomplete — restructured as if/else if/else so every ping gets the correct colour (red=fail, white=slow, green=good).
- [x] 7. `AudioCue` state management leak — replaced with `NotifyPingResult` that tracks cluster count internally in `ConsoleHandler`.
- [x] 8. Unused `using static System.Console` in `PingEngine.cs` — removed.

## Medium

- [x] 9. `Thread.Sleep` blocks the thread — replaced with `async/await` and `Task.Delay` with `CancellationToken`.
- [x] 10. No graceful shutdown mechanism — added `Console.CancelKeyPress` handler with `CancellationTokenSource` for clean Ctrl+C exit.
- [x] 11. Statistics logic lives inside `PingEngine` — moved into `RollingStatistics.RecordPing()` method.
- [x] 12. `IPingTools` is a grab-bag of unrelated utilities — removed `Stopwatch` dependency, simplified to pure utility methods.
- [x] 13. Interfaces expose mutable setters unnecessarily — narrowed `IPingConfig` and `IPingStats` to get-only, `IRollingStatistics` properties now have private setters.
- [x] 14. `Stopwatch` passed around as a dependency — now passes `TimeSpan` via `FormatElapsedTime(TimeSpan)`.

## Low

- [x] 15. Inconsistent naming: `PingToolKit` property vs `IPingTools`/`PingTools` class — renamed property to `PingTools`.
- [x] 16. `PingConfig.Data` has a whimsical default value — replaced with standard alphanumeric padding string.
- [x] 17. GitHub Actions workflow uses `actions/checkout@v1` — updated to v4.
- [x] 18. `$Env:GITHUB_WORKSPACE` syntax in workflow — removed env variable, referencing `Pinger.sln` directly.
- [x] 19. No `.editorconfig` or code-style enforcement — added `.editorconfig` with C# conventions.
