> **Note:** Some of the bugs listed below have proposed fixes and test strategies 
that are not yet implemented. These staged fixes are documented for tracking and 
prioritization but may not reflect the current state of the codebase. See 
individual entries for details.


# Bug Reports

---

### BUG-2025-341: Notes Field Missing  
**Reported by:** QA Intern  
**Date:** 2025-06-02  

**Steps to Reproduce:**  
- Use POST `/api/assignment` to create an assignment with `"Notes": "Final project guidelines"`  
- Use GET `/api/assignment` to retrieve the assignment list  
- The Notes field is missing or empty in the response  

**Root Cause:**  
The `Assignment` constructor previously failed to assign the `notes` parameter to the `Notes` property.

**Fix:**  
This bug was resolved in a prior week. No code or test changes were made in Week 9.

**Test Added:**  
A regression test (`Constructor_WithNotes_ShouldAssignNotes`) was added in the earlier week alongside the fix to ensure the Notes field is correctly assigned.

---

### BUG-2025-349: IsOverdue Logic Incorrect  
**Reported by:** API Developer  
**Date:** 2025-06-02  

**Steps to Reproduce:**  
- Create an incomplete assignment with no due date — it incorrectly shows as overdue  
- Create a completed assignment with a past due date — it still shows as overdue  

**Root Cause:**  
The `IsOverdue()` method did not check for a null due date or completion status.

**Fix:**  
The logic was updated to return `true` only when:  
- A due date exists  
- The assignment is not completed  
- The due date is in the past

**Test Added:**  
The test `IsOverdue_ShouldReturnFalse_WhenAssignmentIsCompleted` confirms that completed assignments are not flagged as overdue. Additional coverage ensures the method handles null due dates correctly.

---

### BUG-2025-350: ConsoleUI Allows Empty Input
**Reported by:** QA Intern  
**Date:** 2025-06-11  

**Steps to Reproduce:**  
- Launch Console UI
- Select "Add Assignment"
- Leave title or description blank and press Enter
- Assignment fails or exception is caught

**Root Cause:**  
The ConsoleUI does not validate title or description before calling the constructor

**Fix:**  
Validation logic was added to ensure non-empty input before creating the assignment.

**Test Added:**  
Not applicable (manual test only)

---

### BUG-2025-351: Duplicate Assignments Allowed
**Reported by:** Manual Tester
**Date:** 2025-06-11  

**Steps to Reproduce:**  
- Add an assignment with the title "Quiz 1"
- Add another assignment with title "Quiz 1"
- Both are added without conflict

**Root Cause:**  
AssignmentService does not check for an existing assignment with the same title.
**Fix:**  
A check was added in 'AssignmentService.AddAssignment()' to prevent duplicates by comparing titles (case-insensitive).

**Test Added:**  
AddAssignment_ShouldRejectDuplicateTitle in AssignmentServiceTests.cs verifies that duplicates are rejected.

---

### BUG-2025-352: Missing Logging When Marking Nonexistent Assignment Complete
**Reported by:** QA Reviewer
**Date:** 2025-06-11  

**Steps to Reproduce:**  
- Choose "Mark Complete" in ConsoleUI
- Enter a title that doesn't exist
- Message is shown, but no logging occurs

**Root Cause:**  
Missing logging inside the failure branch of MarkAssignmentComplete()

**Fix:**  
Added logging inside AssignmentService.MarkAssignmentComplete() when assignment is not found.
**Test Added:**  
MarkAssignmentComplete_ShoudlLog_WhenAssignmentNotFound verifes the logger records a message for missing titles.

---

### BUG-2025-353: AssignmentPriority Enum Not Validated in ConsoleUI  
**Reported by:** API Tester  
**Date:** 2025-06-11  

**Steps to Reproduce:**  
- Launch Console UI  
- Choose "Add Assignment"  
- Enter valid title and description  
- Enter an invalid priority (e.g., "Extreme")  

**Root Cause:**  
Previously, `ConsoleUI` passed raw string input directly to the `Assignment` constructor without validating it. This caused the app to throw an exception on invalid enum values.

**Fix:**  
Input validation was added using `Enum.TryParse()` to ensure only valid values (Low, Medium, High) are accepted before calling the `Assignment` constructor.

**Test Added:**  
Manual testing was performed through the Console UI. Invalid input now triggers a validation message and prevents assignment creation.

---

### BUG-2025-354: DI Registration for IAssignmentService Missing  
**Reported by:** Developer (Brittany)  
**Date:** 2025-06-11  

**Steps to Reproduce:**  
- Register `AssignmentService` as `services.AddSingleton<AssignmentService, AssignmentService>();`
- Inject `IAssignmentService` into `ConsoleUI`
- Run the console app  

**Root Cause:**  
`IAssignmentService` was not registered in the DI container. Only the concrete `AssignmentService` was, which caused a runtime `InvalidOperationException`.

**Fix:**  
Change registration to `services.AddSingleton<IAssignmentService, AssignmentService>();`  

**Test Added:**  
No automated test; manually verified by launching `ConsoleUI`

---

### BUG-2025-355: Mock Verification Failed in Overdue Logging Test  
**Reported by:** Developer (Brittany)  
**Date:** 2025-06-11  

**Steps to Reproduce:**  
- Run the unit test `CheckIfOverdue_ShouldLogResult_WhenAssignmentExists`  
- The test fails with `Moq.MockException`  
- Message shows:  
  `Expected invocation on the mock once, but was 0 times`

**Root Cause:**  
The test expected a specific log format, but the logging in `CheckIfOverdue()` was not implemented at the time.

**Fix:**  
Implemented `CheckIfOverdue(string title)` in `AssignmentService`.  
The method logs a message using `LogAssignmentAction()` when an assignment is found, and logs a fallback message if not.

**Test Added:**  
The unit test `CheckIfOverdue_ShouldLogResult_WhenAssignmentExists` was completed and passes.  
It confirms that the method logs the correct message format and returns the correct overdue status.

---

### BUG-2025-356: Assignment Constructor Does Not Reject Invalid Priority or Blank Description 
**Reported by:** Developer (Brittany)  
**Date:** 2025-06-11  

**Steps to Reproduce:**  
- Manually cast an invalid integer to `AssignmentPriority` (e.g., `(AssignmentPriority)999`)  
- Pass it to the `Assignment` constructor  
- No exception is thrown

**Root Cause:**  
The constructor of `Assignment` did not validate that the passed enum value was within the defined range of `AssignmentPriority`. This allowed invalid enum values to be assigned silently.

**Fix:**  
Added a guard clause to the `Assignment` constructor to:
- Validate that the priority enum is defined
- Reject whitespace-only or null descriptions

**Test Added:**  
`AssignmentConstructor_ShouldThrowException_OnInvalidPriority` ensures invalid enum values now raise a meaningful error.
`Constructor_WhitespaceOnlyDescription_ShouldThrowException` ensures that whitespace-only descriptions are rejected.

---

### BUG-2025-357: Missing Paging & `totalCount` in GET /assignment  
**Reported by:** QA Automation Bot  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Seed 40 assignments via POST  
- Call GET `/api/assignment`  
- Response body is a raw JSON array with no paging metadata  

**Root Cause:**  
`AssignmentController` returns `List<Assignment>` directly; no paging envelope exists  

**Fix:**  
Return an envelope object, e.g.:  
```csharp
public record PagedResult<T>(IEnumerable<T> Items, int TotalCount);
```
Add `page` / `pageSize` query-string handling.  
Planned unit test: `GetAssignments_ShouldReturnPagedResult_WithTotalCount`

**Test Added:**  
Planned — see Fix section for proposed unit test to validate pagination and metadata.

---

### BUG-2025-358: AssignmentService Handles Validation & Storage (High Coupling)  
**Reported by:** Code Review  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Open `AssignmentService.cs`  
- Observe that it performs duplicate title checks, logging, instantiation, and storage  

**Root Cause:**  
Violates Single Responsibility Principle by combining multiple concerns in one class  

**Fix:**  
Refactor responsibilities:  
- `AssignmentService` to coordinate  
- `AssignmentValidator` for validation  
- `AssignmentRepository` for persistence  
Planned unit test: `ValidationService_ShouldRejectDuplicateTitles`

**Test Added:**  
Planned — see Fix section for refactor path and future test target.

---

### BUG-2025-359: No Central Factory for Creating Assignments  
**Reported by:** Architecture Lead  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Assignment objects are created in UI, Console, and Service layers  
- Validation logic is inconsistently applied  

**Root Cause:**  
No single factory enforces required structure or validation  

**Fix:**  
Implement `IAssignmentFactory` to encapsulate creation and enforce rules consistently.  
Constructor logic and exceptions should be managed through factory methods.  
Planned test: `AssignmentFactory_ShouldEnforceFieldValidation`

**Test Added:**  
Planned — see Fix section for factory pattern and expected validation test.

---

### BUG-2025-360: Console UI Lacks Confirmation for Deletes  
**Reported by:** Manual Tester  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Run Console UI  
- Select “Delete Assignment”  
- Enter title and press Enter  
- Item is deleted immediately, even if entered in error  

**Root Cause:**  
No confirmation prompt before performing deletion  

**Fix:**  
Add Y/N confirmation prompt in `ConsoleUI.DeleteAssignment()` method.  
Planned test: manual testing required; console input mocks optional for regression.

**Test Added:**  
Planned — manual verification suggested. Optionally mock Console input for unit coverage.

---

### BUG-2025-361: Console UI Omits Notes Field in Display  
**Reported by:** QA Intern  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Add an assignment with notes  
- List all or view by title  
- Notes field is not shown  

**Root Cause:**  
`ConsoleUI.ListAllAssignments()` and related methods omit Notes from display string  

**Fix:**  
Update all relevant console print logic to include `assignment.Notes`, using conditional formatting to skip empty notes.  
Planned visual test: `ConsoleDisplay_ShouldIncludeNotes_WhenPresent`

**Test Added:**  
Planned — see Fix section. Visual confirmation recommended; unit tests can validate string content.

---

### BUG-2025-362: No Feedback When Marking Already Completed Assignment  
**Reported by:** QA Tester  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Mark an assignment complete  
- Try marking it complete again  
- Console reports success even though no state change occurred  

**Root Cause:**  
`Assignment.MarkComplete()` is not idempotent-aware; no feedback for redundant operations  

**Fix:**  
Add a guard clause or state check before confirming success.  
Planned test: `MarkComplete_ShouldReturnFalse_IfAlreadyComplete`

**Test Added:**  
Planned — see Fix section. Update `Assignment.MarkComplete()` behavior and confirm feedback logic.

---

### BUG-2025-363: AssignmentConstructor Allows Null Description  
**Reported by:** Code Reviewer  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Pass `null` to `description` in constructor  
- No exception is thrown  
- Assignment is created with null description  

**Root Cause:**  
Missing null check for `description` in constructor  

**Fix:**  
Add guard clause in constructor to reject `null` and blank descriptions  
Planned test: `Constructor_ShouldThrow_WhenDescriptionIsNull`

**Test Added:**  
Planned — see Fix section for null check and constructor exception handling.

---

### BUG-2025-364: UI Accepts Empty Priority Input as Default Enum Value  
**Reported by:** UX Feedback  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Run Console UI  
- Leave priority field blank during Add  
- Assignment is still created (with default enum value)  

**Root Cause:**  
`Enum.TryParse()` succeeds with null or empty string by defaulting to zero  

**Fix:**  
Add explicit input validation for blank/invalid priority before calling `Enum.TryParse()`  
Planned test: `ConsoleUI_ShouldRejectEmptyPriorityInput`

**Test Added:**  
TryParsePriority_ShouldRejectBlankOrInvalidInput — verifies ConsoleUI rejects blank and invalid priority inputs and accepts valid values (case-insensitive).

---

### BUG-2025-365: Swagger Docs Lack Enum Descriptions for AssignmentPriority  
**Reported by:** API Documentation Reviewer  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Open `/swagger`  
- View POST `/api/assignment` request model  
- `Priority` shows as `integer`, no values listed  

**Root Cause:**  
`AssignmentPriority` enum not annotated or registered with Swagger configuration  

**Fix:**  
Add `[JsonConverter]` and `SwaggerSchemaFilter` to include enum names and values in docs.  
Planned test: Manual Swagger review and optional automated OpenAPI schema comparison.

**Test Added:**  
Planned — manual validation suggested. Optional integration test via Swagger schema parsing.

---

### BUG-2025-366: Unit Tests Rely on Hardcoded Dates  
**Reported by:** CI Maintainer  
**Date:** 2025-06-13  

**Steps to Reproduce:**  
- Run tests in different time zones or months  
- Tests intermittently fail due to date logic  

**Root Cause:**  
Many unit tests use `DateTime.Now.AddDays()` without fixed baseline context  

**Fix:**  
Replace real-time references with deterministic inputs (e.g., inject `IDateProvider`)  
Planned test: Refactor tests to use mockable date context and validate consistency

**Test Added:**  
Planned — see Fix section. Migrate to testable time abstraction for consistent assertions.
