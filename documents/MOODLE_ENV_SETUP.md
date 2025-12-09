# **CodeEval – Moodle Development Environment Setup Guide**
_For Moodle 3.8 · Moodle-Docker · WSL2 ·

---

## 1. Install Prerequisites

Install the following tools **on Windows with WSL2**:

- **Docker Desktop** (Use WSL2 backend)
- **WSL2 Ubuntu**
- **Git** (Use Personal Access Token if GitHub 2FA is enabled)

Enable **Ubuntu integration** inside Docker Desktop:

**Settings → Resources → WSL Integration → Enable for Ubuntu**

---

## 2. Clone the Project Repository

```bash
mkdir -p ~/project
cd ~/project
git clone https://github.com/radinarissa/code-evaluation-system.git
```

The plugin source lives here:

```
~/project/code-evaluation-system/moodle-plugin/assignsubmission_codeeval
```

---

## 3. Create Moodle Development Area

```bash
mkdir -p ~/moodle-dev
cd ~/moodle-dev
git clone -b MOODLE_38_STABLE https://github.com/moodle/moodle.git
```

---

## 4. Clone Moodle-Docker

```bash
cd ~/moodle-dev
git clone https://github.com/moodlehq/moodle-docker.git moodle-docker
cd moodle-docker
```

---

## 5. Add Required Environment Variables

Edit your `~/.bashrc`:

```bash
nano ~/.bashrc
```

Add this at the bottom (replace `<username>`):

```bash
export MOODLE_DOCKER_WWWROOT=/home/<username>/moodle-dev/moodle
export MOODLE_DOCKER_DB=pgsql
export MOODLE_DOCKER_PHP_VERSION=7.4
export MOODLE_DOCKER_BRANCH=MOODLE_38_STABLE
```

Apply changes:

```bash
source ~/.bashrc
```

---

## 6. Start Moodle-Docker

```bash
cd ~/moodle-dev/moodle-docker
bin/moodle-docker-compose up -d
```

---

## 7. Install Moodle

```bash
bin/moodle-docker-compose exec webserver php admin/cli/install.php --agree-license --wwwroot=http://localhost:8000 --dataroot=/var/www/moodledata --dbtype=pgsql --dbhost=db --dbname=moodle --dbuser=moodle --dbpass=m@0dl3ing --fullname="CodeEval Dev" --shortname="CodeEval" --adminuser=admin --adminpass='Admin1234@' --non-interactive
```

---

## 8. Deploy the Plugin

The plugin source in the repo:

```
~/project/moodle-plugin/assignsubmission_codeeval
```

Plugin location inside Moodle:

```
~/moodle-dev/moodle/mod/assign/submission/codeeval
```

Install/update plugin:

```bash
mkdir -p ~/moodle-dev/moodle/mod/assign/submission
rm -rf ~/moodle-dev/moodle/mod/assign/submission/codeeval
cp -r ~/project/code-evaluation-system/moodle-plugin/assignsubmission_codeeval ~/moodle-dev/moodle/mod/assign/submission/codeeval
```

Then visit:

**Site administration → Notifications**

Moodle installs/upgrades the plugin.

---

## 9. Add a Sync Script

Create:

```
~/project/code-evaluation-system/scripts/sync-plugin-to-moodle.sh
```

Paste:

```bash
#!/usr/bin/env bash
set -e

REPO_PLUGIN_DIR="$HOME/project/code-evaluation-system/moodle-plugin/assignsubmission_codeeval"
MOODLE_PLUGIN_DIR="$HOME/moodle-dev/moodle/mod/assign/submission/codeeval"

echo "Syncing plugin to Moodle..."
mkdir -p "$MOODLE_PLUGIN_DIR"
rsync -av --delete "$REPO_PLUGIN_DIR/" "$MOODLE_PLUGIN_DIR/"
echo "Done."
```

Make executable:

```bash
chmod +x scripts/sync-plugin-to-moodle.sh
```

Use after every code change:

```bash
./scripts/sync-plugin-to-moodle.sh
```

---

## 10. Fix Moodle Permissions (If Needed)

```bash
bin/moodle-docker-compose exec webserver bash -c \
  "chown www-data:www-data /var/www/html/config.php && chmod 640 /var/www/html/config.php"

bin/moodle-docker-compose exec webserver bash -c \
  "chown -R www-data:www-data /var/www/moodledata"
```

---

## 11. Open Repo in VS Code

```bash
cd ~/project
code .
```

---

## 12. When You Change version.php (Plugin Upgrade)

Moodle must run an upgrade.

Either:

- Visit **Site administration → Notifications**

or run:

```bash
bin/moodle-docker-compose exec webserver php admin/cli/upgrade.php --non-interactive
```

---

# Useful Commands Reference

| Action | Command |
|--------|---------|
| **Start containers** | `bin/moodle-docker-compose up -d` |
| **Stop containers** | `bin/moodle-docker-compose down` |
| **List running services** | `bin/moodle-docker-compose ps` |
| **Check DB environment** | `bin/moodle-docker-compose exec db env` |
| **List plugin files inside container** | `bin/moodle-docker-compose exec webserver ls -l /var/www/html/mod/assign/submission` |
| **Reinstall plugin manually** | Delete folder → copy plugin → visit Notifications |

---
