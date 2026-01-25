# Code Evaluation System - Project Analysis

## Overview

This is a **Code Evaluation System** - a Moodle-integrated platform for automated code submission evaluation using Judge0. The system manages programming assignments, evaluates student code submissions against test cases, and synchronizes results back to Moodle.

**Current Status:** Backend architecture is complete, controllers/services need implementation, frontend not started.

---

## Directory Structure

```
code-evaluation-system/
├── backend/                    # .NET 8.0 API with Clean Architecture
│   ├── CodeEvaluator.sln      # Visual Studio solution file
│   ├── CodeEvaluator.Domain/  # Domain layer (entities)
│   ├── CodeEvaluator.Application/  # Application layer (interfaces)
│   ├── CodeEvaluator.Infrastructure/  # Infrastructure (data access)
│   ├── CodeEvaluator.API/     # Presentation layer (REST API)
│   └── Database/              # SQL schema definitions
├── frontend/                   # Frontend (not implemented - .gitkeep only)
├── moodle-plugin/             # Moodle plugin (symlink to external)
├── docker/                     # Docker configuration (placeholder)
├── documents/                  # Project documentation (Bulgarian PDFs)
└── .github/                    # GitHub Actions (placeholder)
```

---

## Technology Stack

| Layer | Technology |
|-------|------------|
| Backend Framework | .NET 8.0 / ASP.NET Core |
| ORM | Entity Framework Core 9.0 |
| Database | PostgreSQL |
| Code Execution | Judge0 v1.13.1 |
| LMS Integration | Moodle |
| API Documentation | Swagger/OpenAPI |
| Frontend | Not implemented yet |

---

## Backend Architecture

The backend follows **Clean Architecture** principles with four distinct layers:

### 1. Domain Layer (`CodeEvaluator.Domain`)

**Purpose:** Contains core business entities with no external dependencies.

**Location:** `/backend/CodeEvaluator.Domain/Entities/`

| Entity | Purpose |
|--------|---------|
| `User.cs` | System users (students, teachers, admins) linked to Moodle via `MoodleId` |
| `Course.cs` | Academic courses linked to Moodle via `MoodleCourseId` |
| `CourseEnrollment.cs` | Many-to-many relationship between users and courses with roles |
| `Task.cs` | Programming assignments with constraints (time/memory/disk limits) |
| `TestCase.cs` | Individual test cases for tasks with input/expected output |
| `Submission.cs` | Student code submissions with status tracking |
| `TestResult.cs` | Results of running a submission against a test case |
| `ReferenceSolution.cs` | Teacher-provided reference solutions for validation |
| `AdditionalFile.cs` | Supporting files for tasks (headers, data files, etc.) |

### 2. Application Layer (`CodeEvaluator.Application`)

**Purpose:** Defines interfaces and abstractions for the infrastructure layer.

**Location:** `/backend/CodeEvaluator.Application/Interfaces/Repositories/`

**Repository Interfaces:**

| Interface | Key Methods |
|-----------|-------------|
| `IRepository<T>` | Generic CRUD: `GetByIdAsync`, `GetAllAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync` |
| `IUserRepository` | `GetByMoodleIdAsync`, `GetByEmailAsync`, `GetUsersByRoleAsync` |
| `ICourseRepository` | `GetByMoodleCourseIdAsync`, `GetCourseWithTasksAsync` |
| `ITaskRepository` | `GetTasksByCourseIdAsync`, `GetActiveTasksAsync`, `GetTaskByMoodleAssignmentIdAsync` |
| `ISubmissionRepository` | `GetPendingSubmissionsAsync`, `GetSubmissionWithTestResultsAsync` |
| `ITestCaseRepository` | `GetTestCasesByTaskIdAsync`, `GetPublicTestCasesByTaskIdAsync` |
| `ITestResultRepository` | `GetResultsBySubmissionIdAsync` |
| `IReferenceSolutionRepository` | `GetValidSolutionByTaskIdAsync` |
| `IAdditionalFileRepository` | `GetFilesByTaskIdAsync` |

### 3. Infrastructure Layer (`CodeEvaluator.Infrastructure`)

**Purpose:** Implements data access using Entity Framework Core.

**Key Files:**

| File | Purpose |
|------|---------|
| `Data/ApplicationDbContext.cs` | EF Core DbContext with entity configurations, constraints, indexes |
| `Migrations/` | Database migration files for PostgreSQL |

**Database Features:**
- Automatic timestamp triggers (`update_*_updated_at`)
- Analytics views (`TaskStatistics`, `StudentProgress`)
- Comprehensive indexing for Moodle IDs and common queries

### 4. API Layer (`CodeEvaluator.API`)

**Purpose:** HTTP endpoints, middleware configuration, dependency injection.

**Key Files:**

| File | Purpose |
|------|---------|
| `Program.cs` | Application entry point, service registration, middleware pipeline |
| `appsettings.json` | Configuration (database connection, logging) |
| `launchSettings.json` | Development URLs (HTTP: 5218, HTTPS: 7145) |

**Current Status:** Skeleton setup only - no controllers implemented yet.

---

## Database Schema

### Entity Relationship Diagram

```
┌─────────────────┐     ┌─────────────────────┐     ┌─────────────────┐
│     Users       │     │  CourseEnrollments  │     │    Courses      │
├─────────────────┤     ├─────────────────────┤     ├─────────────────┤
│ Id (PK)         │◄────│ UserId (FK)         │     │ Id (PK)         │
│ MoodleId        │     │ CourseId (FK)       │────►│ MoodleCourseId  │
│ Username        │     │ Role                │     │ Name            │
│ Email           │     │ EnrolledAt          │     │ AcademicYear    │
│ Role            │     └─────────────────────┘     │ Semester        │
└─────────────────┘                                 └────────┬────────┘
        │                                                    │
        │                                                    │
        ▼                                                    ▼
┌─────────────────┐                                 ┌─────────────────┐
│   Submissions   │                                 │     Tasks       │
├─────────────────┤                                 ├─────────────────┤
│ Id (PK)         │                                 │ Id (PK)         │
│ TaskId (FK)     │────────────────────────────────►│ CourseId (FK)   │
│ UserId (FK)     │                                 │ Title           │
│ Code            │                                 │ MaxPoints       │
│ Status          │                                 │ TimeLimitMs     │
│ FinalGrade      │                                 │ MemoryLimitMb   │
│ MoodleSubmissionId                                │ MoodleAssignmentId
└────────┬────────┘                                 └────────┬────────┘
         │                                                   │
         │              ┌─────────────────┐                  │
         │              │   TestCases     │                  │
         │              ├─────────────────┤                  │
         │              │ Id (PK)         │◄─────────────────┘
         │              │ TaskId (FK)     │
         │              │ Input           │
         │              │ ExpectedOutput  │
         │              │ IsPublic        │
         │              │ Points          │
         │              └────────┬────────┘
         │                       │
         │    ┌──────────────────┴──────────────────┐
         │    │                                     │
         ▼    ▼                                     │
┌─────────────────┐                                 │
│   TestResults   │                                 │
├─────────────────┤                                 │
│ Id (PK)         │                                 │
│ SubmissionId (FK)                                 │
│ TestCaseId (FK) │◄────────────────────────────────┘
│ Status          │
│ ExecutionTime   │
│ MemoryUsage     │
│ Judge0Token     │
└─────────────────┘

Additional Tables:
┌─────────────────────┐     ┌─────────────────────┐
│ ReferenceSolutions  │     │   AdditionalFiles   │
├─────────────────────┤     ├─────────────────────┤
│ Id (PK)             │     │ Id (PK)             │
│ TaskId (FK)         │     │ TaskId (FK)         │
│ SourceCode          │     │ Filename            │
│ UploadedBy (FK)     │     │ FileContent (BYTEA) │
│ IsValid             │     │ FileSize            │
└─────────────────────┘     └─────────────────────┘
```

### Key Enumerations

**User/Enrollment Roles:**
- `Student` - Can submit code, view own results
- `Teacher` - Can create tasks, test cases, view all submissions
- `Admin` - Full system access

**Submission Status:**
- `Pending` - Awaiting evaluation
- `Processing` - Currently being evaluated
- `Completed` - Evaluation finished
- `Error` - Evaluation failed

**Test Result Status:**
- `Pass` - Output matches expected
- `Fail` - Output doesn't match
- `Timeout` - Exceeded time limit
- `RuntimeError` - Code crashed
- `MemoryLimit` - Exceeded memory
- `DiskLimit` - Exceeded disk space
- `CompilationError` - Code didn't compile

---

## Configuration

### Database Connection
**File:** `/backend/CodeEvaluator.API/appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=codeevaluator;Username=postgres;Password=radi"
  }
}
```

### Development URLs
**File:** `/backend/CodeEvaluator.API/Properties/launchSettings.json`
- HTTP: `http://localhost:5218`
- HTTPS: `https://localhost:7145`
- Swagger UI: `http://localhost:5218/swagger`

---

## External Integrations

### Judge0 (Code Execution Engine)

**Purpose:** Compiles and executes student code in sandboxed environments.

**Integration Points:**
- `TestResult.Judge0Token` - Stores Judge0 submission token for result retrieval
- Task constraints (`TimeLimitS`, `MemoryLimitMb`, `DiskLimitMb`) passed to Judge0

**Setup:** Detailed in `README.md` - requires Docker, Ubuntu VM, specific network configuration.

### Moodle (Learning Management System)

**Purpose:** User authentication, course management, grade synchronization.

**Integration Points:**

| Entity Field | Purpose |
|--------------|---------|
| `User.MoodleId` | Links system user to Moodle user |
| `Course.MoodleCourseId` | Links course to Moodle course |
| `Task.MoodleAssignmentId` | Links task to Moodle assignment |
| `Submission.MoodleSubmissionId` | Links submission to Moodle submission |
| `Submission.MoodleSyncStatus` | Tracks grade sync status |

**Plugin Location:** `/moodle-plugin/assignsubmission_codeeval` (symlink to Moodle installation)

---

## What Needs Implementation

### Backend (Priority Order)

1. **Repository Implementations** - Concrete classes implementing repository interfaces
2. **Service Layer** - Business logic for:
   - Submission processing
   - Judge0 integration
   - Moodle synchronization
   - Grade calculation
3. **API Controllers** - REST endpoints for:
   - User management
   - Course management
   - Task CRUD
   - Submission handling
   - Test case management
   - Results retrieval
4. **Authentication/Authorization** - JWT or Moodle-based auth
5. **Background Workers** - For processing pending submissions

### Frontend (Not Started)

**Directory:** `/frontend/` (currently empty)

**Likely Requirements:**
- Student dashboard (view tasks, submit code, view results)
- Teacher dashboard (create tasks, manage test cases, view all submissions)
- Admin panel (user management, system configuration)
- Code editor with syntax highlighting
- Real-time submission status updates
- Grade visualization

---

## Running the Project

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL database
- Judge0 instance (see README.md for setup)

### Database Setup
```bash
cd backend/CodeEvaluator.Infrastructure
dotnet ef database update
```

### Run API
```bash
cd backend/CodeEvaluator.API
dotnet run
```

### Access
- API: http://localhost:5218
- Swagger: http://localhost:5218/swagger

---

## File Reference

### Domain Entities
| File | Lines | Description |
|------|-------|-------------|
| `User.cs` | ~25 | User entity with Moodle integration |
| `Course.cs` | ~20 | Academic course entity |
| `CourseEnrollment.cs` | ~15 | User-Course relationship |
| `Task.cs` | ~35 | Programming task with constraints |
| `TestCase.cs` | ~20 | Test case with I/O |
| `Submission.cs` | ~30 | Code submission entity |
| `TestResult.cs` | ~25 | Test execution result |
| `ReferenceSolution.cs` | ~15 | Reference solution entity |
| `AdditionalFile.cs` | ~15 | Task attachment entity |

### Repository Interfaces
| File | Methods | Description |
|------|---------|-------------|
| `IRepository.cs` | 9 | Generic CRUD operations |
| `IUserRepository.cs` | 4 | User-specific queries |
| `ICourseRepository.cs` | 3 | Course-specific queries |
| `ITaskRepository.cs` | 4 | Task-specific queries |
| `ISubmissionRepository.cs` | 5 | Submission queries |
| `ITestCaseRepository.cs` | 2 | Test case queries |
| `ITestResultRepository.cs` | 2 | Result queries |
| `IReferenceSolutionRepository.cs` | 2 | Solution queries |
| `IAdditionalFileRepository.cs` | 1 | File queries |

---

## Notes for Frontend Development

The frontend will need to integrate with:

1. **REST API** - All data operations through API controllers (to be implemented)
2. **Real-time Updates** - Consider WebSocket/SignalR for submission status
3. **Authentication** - Likely Moodle SSO or JWT tokens
4. **Code Editor** - Monaco Editor or CodeMirror recommended
5. **Responsive Design** - Students may use mobile devices

**Key API Endpoints to Expect:**
- `GET /api/tasks` - List available tasks
- `GET /api/tasks/{id}` - Task details with public test cases
- `POST /api/submissions` - Submit code
- `GET /api/submissions/{id}` - Submission status and results
- `GET /api/courses/{id}/submissions` - Teacher view of all submissions
