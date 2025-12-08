DROP TABLE IF EXISTS TestResults CASCADE;
DROP TABLE IF EXISTS Submissions CASCADE;
DROP TABLE IF EXISTS AdditionalFiles CASCADE;
DROP TABLE IF EXISTS ReferenceSolutions CASCADE;
DROP TABLE IF EXISTS TestCases CASCADE;
DROP TABLE IF EXISTS Tasks CASCADE;
DROP TABLE IF EXISTS CourseEnrollments CASCADE;
DROP TABLE IF EXISTS Courses CASCADE;
DROP TABLE IF EXISTS Users CASCADE;

CREATE TABLE Courses (
    Id SERIAL PRIMARY KEY,
    MoodleCourseId INT NOT NULL UNIQUE,
    Name VARCHAR(255) NOT NULL,
    AcademicYear VARCHAR(255) NOT NULL,
    Semester VARCHAR(255) NOT NULL CHECK (Semester IN ('First', 'Second', 'Third', 'Fourth', 'Fifth', 'Sixth', 'Seventh', 'Eighth')),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    MoodleId INT NOT NULL UNIQUE,
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Role VARCHAR(50) NOT NULL CHECK (Role IN ('Student', 'Teacher', 'Admin')),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE CourseEnrollments (
    Id SERIAL PRIMARY KEY,
    CourseId INT NOT NULL REFERENCES Courses(Id) ON DELETE CASCADE,
    UserId INT NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    Role VARCHAR(50) NOT NULL CHECK (Role IN ('Student', 'Teacher')),
    EnrolledAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(CourseId, UserId)
);

CREATE TABLE Tasks (
    Id SERIAL PRIMARY KEY,
    CourseId INT NOT NULL REFERENCES Courses(Id) ON DELETE CASCADE,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    MaxPoints DECIMAL(5,2) NOT NULL DEFAULT 10.00,
    TimeLimitMs INT NOT NULL DEFAULT 5000,
    MemoryLimitMb INT NOT NULL DEFAULT 256,
    DiskLimitMb INT NOT NULL DEFAULT 256,
    CreatedBy INT NOT NULL REFERENCES Users(Id),
    CreationDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    DueDate TIMESTAMP,
    MoodleAssignmentId INT UNIQUE,
    MoodleAssignmentName VARCHAR(255),
    IsActive BOOLEAN DEFAULT TRUE,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE TestCases (
    Id SERIAL PRIMARY KEY,
    TaskId INT NOT NULL REFERENCES Tasks(Id) ON DELETE CASCADE,
    Name VARCHAR(255) NOT NULL,
    Input TEXT NOT NULL,
    ExpectedOutput TEXT NOT NULL,
    IsPublic BOOLEAN DEFAULT FALSE,
    Points DECIMAL(5,2) NOT NULL DEFAULT 1.00,
    ExecutionOrder INT NOT NULL DEFAULT 1,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ReferenceSolutions (
    Id SERIAL PRIMARY KEY,
    TaskId INT NOT NULL REFERENCES Tasks(Id) ON DELETE CASCADE,
    SourceCode TEXT NOT NULL,
    UploadedBy INT NOT NULL REFERENCES Users(Id),
    UploadedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsValid BOOLEAN DEFAULT FALSE,
    ValidationMessage TEXT
);

CREATE TABLE AdditionalFiles (
    Id SERIAL PRIMARY KEY,
    TaskId INT NOT NULL REFERENCES Tasks(Id) ON DELETE CASCADE,
    Filename VARCHAR(255) NOT NULL,
    FileContent BYTEA NOT NULL,
    FileSize INT NOT NULL,
    UploadedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Submissions (
    Id SERIAL PRIMARY KEY,
    TaskId INT NOT NULL REFERENCES Tasks(Id) ON DELETE CASCADE,
    UserId INT NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    SubmissionTime TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Code TEXT NOT NULL,
    Status VARCHAR(50) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Processing', 'Completed', 'Error')),
    FinalGrade DECIMAL(5,2),
    Feedback TEXT,
    CompilationOutput TEXT,
    EvaluationStartedAt TIMESTAMP,
    EvaluationCompletedAt TIMESTAMP,
    MoodleSubmissionId INT,
    MoodleSyncStatus VARCHAR(255),
    MoodleSyncOutput TEXT,
    MoodleSyncCreatedAt TIMESTAMP
);

CREATE TABLE TestResults (
    Id SERIAL PRIMARY KEY,
    TestCaseId INT NOT NULL REFERENCES TestCases(Id) ON DELETE CASCADE,
    SubmissionId INT NOT NULL REFERENCES Submissions(Id) ON DELETE CASCADE,
    Status VARCHAR(255) NOT NULL CHECK (Status IN ('Pass', 'Fail', 'Timeout', 'Runtime Error', 'Memory Limit', 'Disk Limit', 'Compilation Error')),
    ExecutionTime FLOAT NOT NULL,
    MemoryUsage FLOAT,
    DiskUsedMb DECIMAL(10,2),
    Output TEXT,
    ErrorMessage TEXT,
    Judge0Token VARCHAR(100),
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_users_moodle_id ON Users(MoodleId);
CREATE INDEX idx_users_role ON Users(Role);
CREATE INDEX idx_courses_moodle_id ON Courses(MoodleCourseId);
CREATE INDEX idx_enrollments_course ON CourseEnrollments(CourseId);
CREATE INDEX idx_enrollments_user ON CourseEnrollments(UserId);
CREATE INDEX idx_tasks_course ON Tasks(CourseId);
CREATE INDEX idx_tasks_created_by ON Tasks(CreatedBy);
CREATE INDEX idx_tasks_moodle_id ON Tasks(MoodleAssignmentId);
CREATE INDEX idx_tasks_active ON Tasks(IsActive);
CREATE INDEX idx_testcases_task ON TestCases(TaskId);
CREATE INDEX idx_testcases_order ON TestCases(TaskId, ExecutionOrder);
CREATE INDEX idx_refsol_task ON ReferenceSolutions(TaskId);
CREATE INDEX idx_addfiles_task ON AdditionalFiles(TaskId);
CREATE INDEX idx_submissions_task ON Submissions(TaskId);
CREATE INDEX idx_submissions_user ON Submissions(UserId);
CREATE INDEX idx_submissions_status ON Submissions(Status);
CREATE INDEX idx_submissions_date ON Submissions(SubmissionTime DESC);
CREATE INDEX idx_submissions_task_user ON Submissions(TaskId, UserId, SubmissionTime DESC);
CREATE INDEX idx_testresults_submission ON TestResults(SubmissionId);
CREATE INDEX idx_testresults_testcase ON TestResults(TestCaseId);
CREATE INDEX idx_testresults_status ON TestResults(Status);

CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.UpdatedAt = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_users_updated_at BEFORE UPDATE ON Users
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_courses_updated_at BEFORE UPDATE ON Courses
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_tasks_updated_at BEFORE UPDATE ON Tasks
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_testcases_updated_at BEFORE UPDATE ON TestCases
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE OR REPLACE VIEW TaskStatistics AS
SELECT 
    t.Id as TaskId,
    t.Title,
    t.CourseId,
    COUNT(DISTINCT s.UserId) as UniqueStudents,
    COUNT(s.Id) as TotalSubmissions,
    COALESCE(AVG(s.FinalGrade), 0) as AverageGrade,
    MAX(s.SubmissionTime) as LastSubmissionDate
FROM Tasks t
LEFT JOIN Submissions s ON t.Id = s.TaskId AND s.Status = 'Completed'
GROUP BY t.Id, t.Title, t.CourseId;

CREATE OR REPLACE VIEW StudentProgress AS
SELECT 
    s.Id as SubmissionId,
    s.TaskId,
    t.Title as TaskTitle,
    s.UserId,
    u.Username,
    s.SubmissionTime,
    s.FinalGrade,
    t.MaxPoints,
    s.Status,
    COUNT(tr.Id) FILTER (WHERE tr.Status = 'Pass') as PassedTests,
    COUNT(tr.Id) as TotalTests
FROM Submissions s
JOIN Tasks t ON s.TaskId = t.Id
JOIN Users u ON s.UserId = u.Id
LEFT JOIN TestResults tr ON s.Id = tr.SubmissionId
GROUP BY s.Id, s.TaskId, t.Title, s.UserId, u.Username, 
         s.SubmissionTime, s.FinalGrade, t.MaxPoints, s.Status;