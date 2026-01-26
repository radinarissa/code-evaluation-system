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

    $settings->add(new admin_setting_configtext(
        'assignsubmission_codeeval/moodle_ws_url',
        'Moodle Webservice URL',
        'Base URL of Moodle for REST calls, e.g. http://localhost:8000',
        'http://localhost:8000',
        PARAM_URL
    ));

    // Token for the grading service (teacher/admin token that includes mod_assign_save_grade)
    $settings->add(new admin_setting_configtext(
        'assignsubmission_codeeval/moodle_ws_token',
        'Moodle Webservice Token (grading)',
        'Token for service that can call mod_assign_save_grade.',
        '',
        PARAM_ALPHANUMEXT
    ));
}
