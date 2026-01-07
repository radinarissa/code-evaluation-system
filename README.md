
# Code-evaluation-system

## Prerequisites

- Package Manager
- PostgreSQL
- .NET 8 SDK
- Node.js
- Web Browser



## Table of Contents
  - [Judge0](#Judge0setup)
  - [Postman](#Postman)
  - [DataBase Setup](#DatabaseSetup)
  - [Backend Setup](#BackendSetup)
  - [Frontend Setup](#FrontendSetup)
  - [Quick Start](#Quick)
  - [Configuration](#Configuration)
  - [Troubleshooting](#Troubleshooting)
  - [Project Structure](#Project)
  - [IDE Recommendations](#IDE)


## Judge0setup
<details>
<summary><strong>Windows</strong></summary>
<pre>
в windows търсачката -> turn windows features on or off  
чекваме hyper-v  
(ако липсва като опция:  
pushd "%~dp0"  
dir /b %SystemRoot%\servicing\Packages*Hyper-V*.mum >hyper-v.txt  
for /f %%i in ('findstr /i . hyper-v.txt 2^>nul') do dism /online /add-package:"%SystemRoot%\servicing\Packages%%i"  
del hyper-v.txt  
Dism /online /enable-feature /featurename:Microsoft-Hyper-V-All /LimitAccess /ALL  
pause  
(в notepad и после на .bat file)  

Даваме рестарт на системата.  

Отваряме Hyper-V Manager и в ляво виждаме машината си.  
Right click - new Virtual Machine  
Specify Generation - first generation  
Assign memory - 4096 (би трябвало да стига)  
Configure networking - default switch  
Connect virtual hard disk - 40gb  
Installation options - Install an operating system from bootable cd/dvd-rom чек и Image file(iso) чек.
</pre>
Теглим https://ubuntu.com/download/desktop и го избираме.
<pre>
Finish.  

Double click за да пуснем vm-a и enter -> try or install ubuntu.  
В ubuntu install wizard-a само next, няма нужда да променяме нищо и си правим админ акаунта.  
Изчакваме инсталацията и рестарт когато е готово(може да излязат грешки в конзолата, но не е проблем за момента).  

Пускаме отново vm-a и би трябвало вече да сме с инсталирано ubuntu 24.04  


Отваряме си терминал и започваме:  

sudo nano /etc/default/grub  
в кавичките на GRUB_CMDLINE_LINUX слагаме systemd.unified_cgroup_hierarchy=0  
ctrl + x > y > enter за да запазим промените  
sudo update-grub  
sudo reboot  

След рестарта отново отваряме терминал и продължаваме:  

sudo apt remove docker docker-engine docker.io containerd runc  
sudo apt update  
sudo apt install ca-certificates curl gnupg  
sudo install -m 0755 -d /etc/apt/keyrings  
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg  
sudo chmod a+r /etc/apt/keyrings/docker.gpg  
echo \ "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] \ 
  https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo $VERSION_CODENAME) stable" \ 
  | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null  
sudo apt update  
sudo apt install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin  
sudo systemctl enable --now docker  
sudo usermod -aG docker $USER  
newgrp docker  
Ако ни поиска рестарт/ъпдейт се съгласяваме и продължаваме със стъпките след това.  
Можем да тестваме дали за момента нещата са наред с docker run  hello-world и docker compose version.  

Продължаваме:  
cd /  
ip a  
Търсим inet при eth0 и си записваме ip-то някъде, което ще ни трябва в бъдеще. (това което е последвано от /20)   

Продължаваме:  
sudo mkdir judge0  
cd judge0  
sudo wget https://github.com/judge0/judge0/releases/download/v1.13.1/judge0-v1.13.1.zip  
sudo unzip judge0-v1.13.1.zip  
cd judge0-v1.13.1  
sudo nano judge0.conf  
Търсим REDIS_PASSWORD и POSTGRES_PASSWORD и им даваме някакви стойности, няма значение какви.  
ctrl+x > y > enter  
docker compose up -d db redis (изчакваме да приключи)  
docker compose up -d (изчакваме да приключи)  
Ако забие/трябва да рестартираме тук:  
cd /judge0  
cd judge0-v1.13.1  
И отновно рънваме docker compose up -d db redis или docker compose up -d, според това до къде сме стигнали.</pre>

Когато приключи би трябвало да сме готови.  
Във vm-а judge0 e на http://localhost:2358  
В windows машинати е на [ip-то което си запазихме някъде преди малко]:2358  
Простичък тест е да напраим crul http://localhost:2358/languages или [ip-то]:2358/languages в терминал.  
</details>

## Postman
Информация за най-основните [API calls в judge0](https://bobber-1e394336-9141436.postman.co/workspace/9ffb0b89-7422-4417-ab67-3dcfa60c6089)

## DatabaseSetup

### Install PostgreSQL

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
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
$env:PGPASSWORD='radi'; & 'C:\Program Files\PostgreSQL\15\bin\psql.exe' -h localhost -p 5432 -U postgres -d codeevaluator -c "\dt"
```

Or use **pgAdmin** to connect and verify.
</details>

---

## BackendSetup

### Install .NET 8 SDK



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
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
cd backend
dotnet restore
```
</details>

### Install EF Core Tools

<details>
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
dotnet tool install --global dotnet-ef --version 8.0.0
```

If you get "command not found" after installing, restart PowerShell.
</details>

### Run Database Migrations

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

## FrontendSetup

The frontend uses vanilla HTML/JS with Tailwind CSS (via CDN). No build step required.

### Install Node.js (Recommended)

<details>
<summary><strong>Windows</strong></summary>

1. Download Node.js from https://nodejs.org/ (LTS version recommended)
2. Run the installer
3. Restart terminal/PowerShell after installation

Verify:
```powershell
node --version
```
</details>

### Run Frontend with Local Server (Recommended)

Running on localhost is recommended over opening as a file, especially when connecting to the API.

<details>
<summary><strong>Windows (PowerShell)</strong></summary>

```powershell
cd frontend
npx serve
```

Or with a specific port:
```powershell
npx serve -p 3000
```

Frontend will be available at http://localhost:3000
</details>

### Alternative: Open as File

If you don't have Node.js, you can open the HTML file directly (some features may be limited):

<details>
<summary><strong>Windows</strong></summary>

```powershell
start frontend\index.html
```

Or double-click `frontend\index.html` in File Explorer.
</details>

### Switching between mock data and real API

The frontend can use either mock data or the API, to switch:
1. Open `frontend/js/config.js`
2. Change `USE_MOCK_DATA: true` or `USE_MOCK_DATA: false`
3. Ensure backend is running at http://localhost:5218

---

## Configuration

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

# 5. Start frontend (in new terminal)
cd ..\frontend
npx serve -p 3000
```

---

## Troubleshooting

### "dotnet: command not found"

<details>
<summary><strong>Windows</strong></summary>

Restart PowerShell after installing .NET SDK. If still not found, add to PATH manually:
1. Search "Environment Variables" in Start
2. Edit "Path" under User variables
3. Add `C:\Program Files\dotnet\`
</details>

### "dotnet-ef: command not found"

<details>
<summary><strong>Windows</strong></summary>

Restart PowerShell. If still not found:
```powershell
$env:Path += ";$env:USERPROFILE\.dotnet\tools"
```
</details>

### "connection refused" on database

<details>
<summary><strong>Windows</strong></summary>

1. Open Services (Win + R, type `services.msc`)
2. Find "postgresql-x64-15" (or similar)
3. Ensure it's "Running"
4. If stopped, right-click → Start
</details>

### "database codeevaluator does not exist"

Create the database:

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
| Windows | Visual Studio 2022, VS Code, or JetBrains Rider |

For VS Code, install these extensions:
- C# Dev Kit
- PostgreSQL (by Chris Kolkman)
- Tailwind CSS IntelliSense
