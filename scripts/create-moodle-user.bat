@echo off
REM Create a user in local Moodle instance
REM Usage: create-moodle-user.bat <username> [password]
REM Password defaults to Admin1234@ (meets Moodle password policy)

setlocal enabledelayedexpansion

set CONTAINER_NAME=moodle-docker-webserver-1
set DEFAULT_PASSWORD=Admin1234@

if "%~1"=="" (
    echo Usage: %~nx0 ^<username^> [password]
    echo Example: %~nx0 jsmith
    echo Example: %~nx0 jsmith MyPassword1@
    echo.
    echo Password must contain: uppercase, lowercase, number, special character
    echo Default password: %DEFAULT_PASSWORD%
    exit /b 1
)

set USERNAME=%~1
if "%~2"=="" (
    set PASSWORD=%DEFAULT_PASSWORD%
) else (
    set PASSWORD=%~2
)
set EMAIL=%USERNAME%@test.local

echo Creating Moodle user: %USERNAME%
echo Password: %PASSWORD%
echo Email: %EMAIL%
echo.

docker exec %CONTAINER_NAME% php -r "define('CLI_SCRIPT', true); require('/var/www/html/config.php'); require_once($CFG->dirroot.'/user/lib.php'); $user = new stdClass(); $user->username = '%USERNAME%'; $user->password = '%PASSWORD%'; $user->firstname = '%USERNAME%'; $user->lastname = 'User'; $user->email = '%EMAIL%'; $user->confirmed = 1; $user->mnethostid = $CFG->mnet_localhost_id; try { $id = user_create_user($user); echo 'Success! Created user %USERNAME% with ID ' . $id . chr(10); } catch (Exception $e) { echo 'Error: ' . $e->getMessage() . chr(10); exit(1); }"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo You can now login at http://localhost:5500 with:
    echo   Username: %USERNAME%
    echo   Password: %PASSWORD%
)
