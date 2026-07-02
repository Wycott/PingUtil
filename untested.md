# Untested Modules — Pinger

## Summary

15 source files were checked across `Pinger/`, `Pinger.Domain/`, and `Pinger.Interfaces/`. All source files have corresponding test coverage — every class has a dedicated test file, and every interface is referenced (via mocks or direct usage) in at least one test. There are no untested modules.

---

## Untested Files

| # | File | Project | Class/Interface | Reason |
|---|------|---------|-----------------|--------|
| — | None | — | — | All source files are covered by tests |

---

## Tested Files

| # | File | Project | Class/Interface | Test File |
|---|------|---------|-----------------|-----------|
| 1 | `Pinger/Program.cs` | Pinger | Program | `ProgramTest.cs` |
| 2 | `Pinger.Domain/ConsoleHandler.cs` | Pinger.Domain | ConsoleHandler | `ConsoleHandlerTest.cs` |
| 3 | `Pinger.Domain/PingConfig.cs` | Pinger.Domain | PingConfig | `PingConfigTest.cs` |
| 4 | `Pinger.Domain/PingDisplay.cs` | Pinger.Domain | PingDisplay | `PingDisplayTest.cs` |
| 5 | `Pinger.Domain/PingEngine.cs` | Pinger.Domain | PingEngine | `PingEngineTest.cs` |
| 6 | `Pinger.Domain/PingStats.cs` | Pinger.Domain | PingStats | `PingStatsTest.cs` |
| 7 | `Pinger.Domain/PingTools.cs` | Pinger.Domain | PingTools | `PingToolsTest.cs` |
| 8 | `Pinger.Domain/RollingStatistics.cs` | Pinger.Domain | RollingStatistics | `RollingStatisticsTest.cs` |
| 9 | `Pinger.Interfaces/IConsoleHandler.cs` | Pinger.Interfaces | IConsoleHandler | `ConsoleHandlerTest.cs`, `PingDisplayTest.cs`, `PingEngineTest.cs` |
| 10 | `Pinger.Interfaces/IPingConfig.cs` | Pinger.Interfaces | IPingConfig | `ConsoleHandlerTest.cs`, `PingDisplayTest.cs`, `PingEngineTest.cs` |
| 11 | `Pinger.Interfaces/IPingDisplay.cs` | Pinger.Interfaces | IPingDisplay | `PingEngineTest.cs` |
| 12 | `Pinger.Interfaces/IPingEngine.cs` | Pinger.Interfaces | IPingEngine | `ProgramTest.cs`, `PingEngineTest.cs` |
| 13 | `Pinger.Interfaces/IPingStats.cs` | Pinger.Interfaces | IPingStats | `ConsoleHandlerTest.cs`, `PingDisplayTest.cs`, `PingEngineTest.cs` |
| 14 | `Pinger.Interfaces/IPingTools.cs` | Pinger.Interfaces | IPingTools | `PingToolsTest.cs`, `PingEngineTest.cs` |
| 15 | `Pinger.Interfaces/IRollingStatistics.cs` | Pinger.Interfaces | IRollingStatistics | `PingDisplayTest.cs`, `PingEngineTest.cs` |
