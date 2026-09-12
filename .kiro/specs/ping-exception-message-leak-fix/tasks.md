# Implementation Plan

- [x] 1. Write bug condition exploration test
  - **Property 1: Bug Condition** - Friendly Exception Message
  - **CRITICAL**: This test MUST FAIL on unfixed code - failure confirms the leak exists
  - **DO NOT attempt to fix the test or the code when it fails**
  - **NOTE**: This test encodes the expected behavior - it will validate the fix when it passes after implementation
  - **GOAL**: Surface counterexamples that demonstrate the exception message leak exists
  - **Scoped PBT Approach**: This bug is deterministic. Scope the property to concrete failing inputs that reliably cause `pinger.Send(...)` to throw (e.g. an invalid/unresolvable host such as `"this.host.does.not.exist.invalid"`), with the pinger active
  - Configure `PingEngine` via Moq with `IPingTools`, `IPingDisplay`, `IConsoleHandler`, `IPingConfig`, and `IRollingStatistics`; set `IPingConfig.PingerIsActive` to `true` and `RemoteServer` to a value that makes `Send` throw (from Bug Condition `isBugCondition` in design)
  - Capture the string passed to `IConsoleHandler.WriteToConsole` using a Moq `Callback` or `Verify` with a captured argument
  - Test assertions should match the Expected Behavior in design: the captured message must NOT contain the .NET exception type name (e.g. `"PingException"`) and must NOT contain the raw internal exception message (e.g. `"An exception occurred during a Ping request."`)
  - Run test on UNFIXED code
  - **EXPECTED OUTCOME**: Test FAILS (this is correct - the captured message currently contains `PingException` and the internal text, proving the leak exists)
  - Document counterexamples found (e.g. `[Ping error: PingException - An exception occurred during a Ping request.]`) to confirm root cause
  - Mark task complete when test is written, run, and failure is documented
  - _Requirements: 1.1, 1.2, 2.1, 2.2_

- [x] 2. Write preservation property tests (BEFORE implementing fix)
  - **Property 2: Preservation** - Non-Throwing And Inactive Behavior
  - **IMPORTANT**: Follow observation-first methodology - run the UNFIXED code first, record actual outputs, then assert those outputs
  - Observe on UNFIXED code: with `PingerIsActive == false`, the ping flow records/returns `PingStats { Success = true }` (inactive-pinger short-circuit)
  - Observe on UNFIXED code: a successful ping records/returns `Success = true` with the reply `PingTime`
  - Observe on UNFIXED code: a non-success reply that does not throw records/returns `Success = false` with the reply `PingTime`
  - Observe on UNFIXED code: an exception yields a failed `PingStats` (`Success == false`) via `IRollingStatistics.RecordPing`
  - Write property-based tests (varied non-throwing outcomes and the inactive path) that assert the recorded/returned `PingStats` matches the observed behavior from the Preservation Requirements in design
  - Use Moq to drive `IRollingStatistics.RecordPing` capture and to configure `IPingConfig` for each path
  - Run tests on UNFIXED code
  - **EXPECTED OUTCOME**: Tests PASS (this confirms the baseline behavior to preserve)
  - Mark task complete when tests are written, run, and passing on unfixed code
  - _Requirements: 3.1, 3.2, 3.3_

- [x] 3. Fix the exception message leak in PingEngine.PingHost

  - [x] 3.1 Implement the fix
    - In `Pinger.Domain/PingEngine.cs`, in the `catch` block of `PingHost`, replace the interpolated string `$"[Ping error: {ex.GetType().Name} - {ex.Message}]"` with a static friendly `const string` message that references neither `ex.GetType().Name` nor `ex.Message` (e.g. `"[Ping failed: the host could not be reached or a network error occurred.]"`)
    - Since the message no longer uses `ex`, change the clause to `catch (Exception)` (drop the unused binding)
    - Keep `ConsoleHandler.WriteToConsole(friendlyMessage);` and preserve the return contract with `return new PingStats();`
    - Ensure the `return` statement is preceded by a blank line (per coding standards)
    - Do not alter the `PingerIsActive` short-circuit, the `try` block, or the success/non-success return
    - _Bug_Condition: isBugCondition(input) where PingerIsActive == true AND pinger.Send(input) throws AND message CONTAINS ex.GetType().Name OR ex.Message_
    - _Expected_Behavior: expectedBehavior(result) - message contains neither the exception type name nor the raw internal message, and PingHost returns a failed PingStats (Success == false)_
    - _Preservation: Preservation Requirements from design - successful, non-success-no-throw, inactive short-circuit, and failed-result-on-exception behaviors unchanged_
    - _Requirements: 2.1, 2.2, 3.3_

  - [x] 3.2 Verify bug condition exploration test now passes
    - **Property 1: Expected Behavior** - Friendly Exception Message
    - **IMPORTANT**: Re-run the SAME test from task 1 - do NOT write a new test
    - The test from task 1 encodes the expected behavior; when it passes, it confirms the friendly message is emitted and no internal details leak
    - Run the bug condition exploration test from step 1
    - **EXPECTED OUTCOME**: Test PASSES (confirms the leak is fixed)
    - _Requirements: 2.1, 2.2_

  - [x] 3.3 Verify preservation tests still pass
    - **Property 2: Preservation** - Non-Throwing And Inactive Behavior
    - **IMPORTANT**: Re-run the SAME tests from task 2 - do NOT write new tests
    - Run the preservation property tests from step 2
    - **EXPECTED OUTCOME**: Tests PASS (confirms no regressions in success, non-success-no-throw, inactive, and failed-result-on-exception behavior)
    - Confirm all tests still pass after the fix
    - _Requirements: 3.1, 3.2, 3.3_

- [x] 4. Checkpoint - Ensure all tests pass
  - Build the solution and run the full `Pinger.Test` suite
  - Ensure all tests pass, ask the user if questions arise
