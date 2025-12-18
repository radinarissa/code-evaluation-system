<?php
defined('MOODLE_INTERNAL') || die();

if ($hassiteconfig) {
    $settings->add(new admin_setting_configtext(
        'assignsubmission_codeeval/backend_url',
        get_string('backendurl', 'assignsubmission_codeeval'),
        '',
        'http://host.docker.internal:5218',
        PARAM_URL
    ));
}
