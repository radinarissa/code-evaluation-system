# Setup Guide

This guide covers how to set up the Code Evaluation System on **macOS** and **Windows**.

---

## Prerequisites

| Component | macOS | Windows |
|-----------|-------|---------|
| Package Manager | Homebrew | - |
| PostgreSQL | `brew install postgresql@15` | Download installer |
| .NET 8 SDK | `brew install dotnet@8` | Download installer |
| Web Browser | Any | Any |

---

## 1. Database Setup

### Install PostgreSQL

<details>
<summary><strong>macOS</strong></summary>

```bash
brew install postgresql@15
brew services start postgresql@15
```
</details>

<details>
<summary><strong>Windows</strong></summary>

1. Download PostgreSQL from https://www.postgresql.org/download/windows/
2. Run the installer
3. Remember the password you set for the `postgres` user
4. Keep default port `5432`
5. PostgreSQL service starts automatically after installation
</details>

### Create Database and User

<details>
<summary><strong>macOS</strong></summary>

```bash
# Connect to PostgreSQL
psql -d postgres

# Create user with password
CREATE USER postgres WITH SUPERUSER PASSWORD 'radi';

# Create database
CREATE DATABASE codeevaluator OWNER postgres;

# Exit
\q
```
</details>

<details>
<summary><strong>Windows</strong></summary>

Open **SQL Shell (psql)** from Start Menu, then:

```sql
-- If postgres user already exists (from installer), just set password:
ALTER USER postgres WITH PASSWORD 'radi';

-- Create database
CREATE DATABASE codeevaluator OWNER postgres;

-- Exit
\q
```

Or use **pgAdmin** (installed with PostgreSQL):
1. Right-click "Databases" → Create → Database
2. Name: `codeevaluator`
3. Owner: `postgres`
</details>

### Verify Connection

<details>
<summary><strong>macOS</strong></summary>

```bash
PGPASSWORD=radi psql -h localhost -p 5432 -U postgres -d codeevaluator -c "\dt"
```
</details>

<details>
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
$env:PGPASSWORD='radi'; & 'C:\Program Files\PostgreSQL\15\bin\psql.exe' -h localhost -p 5432 -U postgres -d codeevaluator -c "\dt"
```

Or use **pgAdmin** to connect and verify.
</details>

---

## 2. Backend Setup

### Install .NET 8 SDK

<details>
<summary><strong>macOS</strong></summary>

```bash
brew install dotnet@8
```

Add to PATH (optional):
```bash
echo 'export PATH="/opt/homebrew/opt/dotnet@8/bin:$PATH"' >> ~/.zshrc
source ~/.zshrc
```
</details>

<details>
<summary><strong>Windows</strong></summary>

1. Download .NET 8 SDK from https://dotnet.microsoft.com/download/dotnet/8.0
2. Run the installer
3. Restart terminal/PowerShell after installation

Verify:
```powershell
dotnet --version
```
</details>

### Restore Dependencies

<details>
<summary><strong>macOS</strong></summary>

```bash
cd backend
dotnet restore
```
</details>

<details>
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
cd backend
dotnet restore
```
</details>

### Install EF Core Tools

<details>
<summary><strong>macOS</strong></summary>

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
```
</details>

<details>
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
dotnet tool install --global dotnet-ef --version 8.0.0
```

If you get "command not found" after installing, restart PowerShell.
</details>

### Run Database Migrations

<details>
<summary><strong>macOS</strong></summary>

```bash
cd backend
dotnet ef database update --project CodeEvaluator.Infrastructure --startup-project CodeEvaluator.API
```
</details>

<details>
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
cd backend
dotnet ef database update --project CodeEvaluator.Infrastructure --startup-project CodeEvaluator.API
```
</details>

### Verify Tables Created

Expected tables after migration:
```
 AdditionalFiles
 CourseEnrollments
 Courses
 ReferenceSolutions
 Submissions
 Tasks
 TestCases
 TestResults
 Users
 __EFMigrationsHistory
```

### Run Backend

<details>
<summary><strong>macOS</strong></summary>

```bash
cd backend/CodeEvaluator.API
dotnet run
```
</details>

<details>
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
cd backend\CodeEvaluator.API
dotnet run
```
</details>

Backend will be available at:
- **API**: http://localhost:5218
- **Swagger**: http://localhost:5218/swagger

---

## 3. Frontend Setup

The frontend uses vanilla HTML/JS with Tailwind CSS (via CDN). No build step required.

### Open in Browser

<details>
<summary><strong>macOS</strong></summary>

```bash
open frontend/index.html
```

Or double-click `frontend/index.html` in Finder.
</details>

<details>
<summary><strong>Windows</strong></summary>

```powershell
start frontend\index.html
```

Or double-click `frontend\index.html` in File Explorer.
</details>

### Development Mode

The frontend currently uses mock data. To switch to real API when ready:

1. Open `frontend/js/config.js`
2. Change `USE_MOCK_DATA: true` to `USE_MOCK_DATA: false`

---

## 4. Configuration

### Database Connection

Edit `backend/CodeEvaluator.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=codeevaluator;Username=postgres;Password=radi"
  }
}
```

### API URL (Frontend)

Edit `frontend/js/config.js`:

```javascript
API_BASE_URL: 'http://localhost:5218/api'
```

---

## Quick Start

### macOS

```bash
# 1. Start PostgreSQL
brew services start postgresql@15

# 2. Create database (first time only)
psql -d postgres -c "CREATE USER postgres WITH SUPERUSER PASSWORD 'radi';"
psql -d postgres -c "CREATE DATABASE codeevaluator OWNER postgres;"

# 3. Run migrations (first time only)
cd backend
dotnet restore
dotnet ef database update --project CodeEvaluator.Infrastructure --startup-project CodeEvaluator.API

# 4. Start backend
cd CodeEvaluator.API
dotnet run &

# 5. Open frontend
open ../frontend/index.html
```

### Windows (PowerShell)

```powershell
# 1. PostgreSQL should be running as a service (check Services app)

# 2. Create database (first time only) - use pgAdmin or psql

# 3. Run migrations (first time only)
cd backend
dotnet restore
dotnet ef database update --project CodeEvaluator.Infrastructure --startup-project CodeEvaluator.API

# 4. Start backend
cd CodeEvaluator.API
Start-Process dotnet -ArgumentList "run"

# 5. Open frontend
start ..\frontend\index.html
```

---

## Troubleshooting

### "dotnet: command not found"

<details>
<summary><strong>macOS</strong></summary>

Use the full path or add to PATH:
```bash
/opt/homebrew/opt/dotnet@8/bin/dotnet --version
```
</details>

<details>
<summary><strong>Windows</strong></summary>

Restart PowerShell after installing .NET SDK. If still not found, add to PATH manually:
1. Search "Environment Variables" in Start
2. Edit "Path" under User variables
3. Add `C:\Program Files\dotnet\`
</details>

### "dotnet-ef: command not found"

<details>
<summary><strong>macOS</strong></summary>

Add tools to PATH:
```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```
</details>

<details>
<summary><strong>Windows</strong></summary>

Restart PowerShell. If still not found:
```powershell
$env:Path += ";$env:USERPROFILE\.dotnet\tools"
```
</details>

### "connection refused" on database

<details>
<summary><strong>macOS</strong></summary>

Ensure PostgreSQL is running:
```bash
brew services list | grep postgres
brew services start postgresql@15
```
</details>

<details>
<summary><strong>Windows</strong></summary>

1. Open Services (Win + R, type `services.msc`)
2. Find "postgresql-x64-15" (or similar)
3. Ensure it's "Running"
4. If stopped, right-click → Start
</details>

### "role postgres does not exist" (macOS only)

Create the postgres user:
```bash
psql -d postgres -c "CREATE USER postgres WITH SUPERUSER PASSWORD 'radi';"
```

### "database codeevaluator does not exist"

Create the database:

**macOS:**
```bash
psql -d postgres -c "CREATE DATABASE codeevaluator OWNER postgres;"
```

**Windows (psql):**
```sql
CREATE DATABASE codeevaluator OWNER postgres;
```

### "password authentication failed"

Ensure the password in `appsettings.json` matches what you set for postgres user.

### Frontend shows loading spinner forever

1. Open browser console (F12 → Console tab)
2. Check for JavaScript errors
3. Ensure all JS files in `frontend/js/` are present

### CORS errors when connecting to API

Add CORS configuration to `backend/CodeEvaluator.API/Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// After var app = builder.Build();
app.UseCors();
```

---

## Project Structure

```
code-evaluation-system/
├── backend/
│   ├── CodeEvaluator.API/           # REST API (ASP.NET Core)
│   ├── CodeEvaluator.Application/   # Interfaces & abstractions
│   ├── CodeEvaluator.Domain/        # Entity models
│   └── CodeEvaluator.Infrastructure/# EF Core & migrations
├── frontend/
│   ├── index.html                   # Main HTML
│   └── js/
│       ├── config.js                # Configuration
│       ├── i18n.js                  # Translations (BG/EN)
│       ├── utils.js                 # Helper functions
│       ├── app.js                   # Main application
│       ├── services/
│       │   ├── mockData.js          # Mock data
│       │   └── api.js               # API service
│       └── views/
│           ├── dashboard.js
│           ├── submissions.js
│           ├── tasks.js
│           └── students.js
├── SETUP.md                         # This file
├── PROJECT_ANALYSIS.md              # Architecture documentation
└── README.md                        # Project overview
```

---

## IDE Recommendations

| Platform | Recommended IDE |
|----------|-----------------|
| macOS | VS Code + C# Dev Kit, or JetBrains Rider |
| Windows | Visual Studio 2022, VS Code, or JetBrains Rider |

For VS Code, install these extensions:
- C# Dev Kit
- PostgreSQL (by Chris Kolkman)
- Tailwind CSS IntelliSense
