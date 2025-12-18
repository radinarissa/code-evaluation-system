# Create a user in local Moodle instance
# Usage: .\create-moodle-user.ps1 -Username <username> [-Password <password>]
# Password defaults to Admin1234@ (meets Moodle password policy)

param(
    [Parameter(Mandatory=$true)]
    [string]$Username,

    [Parameter(Mandatory=$false)]
    [string]$Password = "Admin1234@"
)

$ContainerName = "moodle-docker-webserver-1"
$Email = "$Username@test.local"

Write-Host "Creating Moodle user: $Username" -ForegroundColor Cyan
Write-Host "Password: $Password"
Write-Host "Email: $Email"
Write-Host ""

$PhpCode = @"
define('CLI_SCRIPT', true);
require('/var/www/html/config.php');
require_once(\`$CFG->dirroot.'/user/lib.php');

\`$user = new stdClass();
\`$user->username = '$Username';
\`$user->password = '$Password';
\`$user->firstname = '$($Username.Substring(0,1).ToUpper() + $Username.Substring(1))';
\`$user->lastname = 'User';
\`$user->email = '$Email';
\`$user->confirmed = 1;
\`$user->mnethostid = \`$CFG->mnet_localhost_id;

try {
    \`$id = user_create_user(\`$user);
    echo \"Success! Created user '$Username' with ID \`$id\n\";
} catch (Exception \`$e) {
    echo \"Error: \" . \`$e->getMessage() . \"\n\";
    exit(1);
}
"@

docker exec $ContainerName php -r $PhpCode

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "You can now login at http://localhost:5500 with:" -ForegroundColor Green
    Write-Host "  Username: $Username" -ForegroundColor Green
    Write-Host "  Password: $Password" -ForegroundColor Green
}
