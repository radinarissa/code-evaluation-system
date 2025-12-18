#!/bin/bash
# Create a user in local Moodle instance
# Usage: ./create-moodle-user.sh <username> [password]
# Password defaults to Admin1234@ (meets Moodle password policy)

CONTAINER_NAME="moodle-docker-webserver-1"
DEFAULT_PASSWORD="Admin1234@"

if [ -z "$1" ]; then
    echo "Usage: $0 <username> [password]"
    echo "Example: $0 jsmith"
    echo "Example: $0 jsmith MyPassword1@"
    echo ""
    echo "Password must contain: uppercase, lowercase, number, special character"
    echo "Default password: $DEFAULT_PASSWORD"
    exit 1
fi

USERNAME="$1"
PASSWORD="${2:-$DEFAULT_PASSWORD}"
EMAIL="${USERNAME}@test.local"

echo "Creating Moodle user: $USERNAME"
echo "Password: $PASSWORD"
echo "Email: $EMAIL"
echo ""

docker exec "$CONTAINER_NAME" php -r "
define('CLI_SCRIPT', true);
require('/var/www/html/config.php');
require_once(\$CFG->dirroot.'/user/lib.php');

\$user = new stdClass();
\$user->username = '$USERNAME';
\$user->password = '$PASSWORD';
\$user->firstname = '${USERNAME^}';
\$user->lastname = 'User';
\$user->email = '$EMAIL';
\$user->confirmed = 1;
\$user->mnethostid = \$CFG->mnet_localhost_id;

try {
    \$id = user_create_user(\$user);
    echo \"Success! Created user '$USERNAME' with ID \$id\n\";
    echo \"You can now login at http://localhost:5500 with:\n\";
    echo \"  Username: $USERNAME\n\";
    echo \"  Password: $PASSWORD\n\";
} catch (Exception \$e) {
    echo \"Error: \" . \$e->getMessage() . \"\n\";
    exit(1);
}
"
