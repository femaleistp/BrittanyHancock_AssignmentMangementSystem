# Assignment Management System

## Overview
This app allows students to track and manage assignments via a modular architecture using test-driven development.

## Features
- Add, update, delete assignments
- Web API with Swagger UI
- Console UI (for internal testing)
- Unit tests with xUnit and Moq
- Bug tracking and regression test support

## Setup
1. Clone the repo
2. Open `AssignmentMangementSystem.sln` in Visual Studio 2022
3. Set `AssignmentManagement.Api` as the startup project
4. Run the project and visit `/swagger` to test endpoints

## Running Tests
Open **Test Explorer** in Visual Studio  
Run all tests across these projects:
- `AssignmentManagement.Tests`
- `AssignmentManagement.ApiTests`

## Contributing
Fork the repo and submit a pull request. All contributions should include related unit tests and be consistent with existing architecture.

## Maintenance Plan

### Known Limitations or Technical Debt
- `AssignmentService.cs` still centralizes multiple responsibilities (logic, logging, and formatting).
- Current implementation uses in-memory storage; no persistence layer is implemented.
- Console UI exists only for testing and does not support full application features.

### Future Goals
- Split `AssignmentService` into smaller services if functionality expands (e.g., persistence, validation).
- Add a real database layer (e.g., using EF Core or Dapper).
- Expand test coverage for edge cases and error handling.
- Add user authentication if multi-user support is introduced.

### Bug Reports and Feature Requests
To report a bug or request a feature, please open an issue on the [GitHub repository](https://github.com/your-repo-link-here).

## Architecture Overview
AssignmentManagementSystem
├── AssignmentManagement.Api
│   └── Controllers
│       └── AssignmentController.cs
│   └── Program.cs
│   └── appsettings.json
│   └── AssignmentManagement.Api.http
├── AssignmentManagement.ApiTests
│   └── AssignmentApiTests.cs
├── AssignmentManagement.Console
│   └── Program.cs
├── AssignmentManagement.Core
│   ├── Assignment.cs
│   ├── AssignmentFormatter.cs
│   ├── AssignmentService.cs
│   ├── ConsoleAppLogger.cs
│   ├── IAssignmentFormatter.cs
│   ├── IAssignmentService.cs
│   └── IAppLogger.cs
├── AssignmentManagement.Tests
│   └── AssignmentServiceTests.cs
├── AssignmentManagement.UI
│   └── ConsoleUI.cs
├── README.md
├── MaintenancePlan.md
├── BugReports.md
└── TechnicalDebtNotes.txt

## Author
Brittany Hancock – IT340 Software Engineering II – Spring 2025