<?php
defined('MOODLE_INTERNAL') || die();


require_once($CFG->dirroot . '/mod/assign/submissionplugin.php');
require_once($CFG->libdir . '/filelib.php');

/**
 * CodeEval submission plugin (mock implementation).
 *
 * @package   assignsubmission_codeeval
 */
class assign_submission_codeeval extends assign_submission_plugin {

    /**
     * Name shown in UI.
     */
    public function get_name() {
        return get_string('pluginname', 'assignsubmission_codeeval');
    }

    /**
 * Add per-assignment settings (shown when editing an assignment).
 */
    public function get_settings(MoodleQuickForm $mform) {
        // Field name MUST be prefixed like this:
        $fieldname = 'assignsubmission_codeeval_taskid';

        $mform->addElement(
            'text',
            $fieldname,
            get_string('taskid', 'assignsubmission_codeeval')
        );
        $mform->setType($fieldname, PARAM_INT);
        $mform->addHelpButton($fieldname, 'taskid', 'assignsubmission_codeeval');

        // Load current value when editing assignment settings.
        $mform->setDefault($fieldname, (int)$this->get_config('taskid'));
    }

    /**
     * Save per-assignment settings.
     */
    public function save_settings(stdClass $data) {
        $fieldname = 'assignsubmission_codeeval_taskid';
        $taskid = 0;

        if (isset($data->$fieldname)) {
            $taskid = (int)$data->$fieldname;
        }

        $this->set_config('taskid', $taskid);
        return true;
        }


    /**
     * Add elements to the student submission form.
     */
    public function get_form_elements($submission, $mform, $data) {

        $mform->addElement(
            'textarea',
            'codeeval_source',
            get_string('sourcecode', 'assignsubmission_codeeval'),
            'wrap="virtual" rows="20" cols="80"'
        );
        $mform->setType('codeeval_source', PARAM_RAW);

        // Preload existing source, if any.
        if (!empty($submission->id)) {
            if ($record = $this->get_record($submission->id)) {
                $mform->setDefault('codeeval_source', $record->sourcecode);
            }
        }

        return true;
    }

    /**
     * Save a student's submission.
     */
    public function save($submission, $data) {
    global $DB, $USER;

    $code = trim($data->codeeval_source ?? '');

    $userid = $submission->userid ?? $USER->id;

    $taskid = (int)$this->get_config('taskid');
    if ($taskid <= 0) {
        throw new moodle_exception('taskidmissing', 'assignsubmission_codeeval');
    }

    $payload = [
        'taskId' => $taskid,
        'studentId' => (string)$userid,
        'language' => 71,
        'sourceCode' => $code,
        'moodleUserId' => (int)$userid,
        'moodleCourseId' => (string)$this->assignment->get_course()->id,
        'moodleSubmissionId' => (int)$submission->id,
    ];

    list($httpcode, $response) = $this->backend_post_json('/api/submissions', $payload);
    error_log("CODEEVAL backend POST /api/submissions HTTP=$httpcode response=$response");

    if ($httpcode < 200 || $httpcode >= 300) {
        throw new moodle_exception('backendgradingfailed', 'assignsubmission_codeeval', '', null,
            "Backend returned HTTP $httpcode: $response");
    }


    $gradevalue = null;
    $feedback = '';

    $json = json_decode($response, true);
    if (!is_array($json)) {
        throw new moodle_exception('backendgradingfailed', 'assignsubmission_codeeval', '', null,
            "Backend did not return JSON: $response");
    }

    if (isset($json['finalGrade'])) {
        $gradevalue = (float)$json['finalGrade'];
    } elseif (isset($json['grade'])) {
        $gradevalue = (float)$json['grade'];
    } elseif (isset($json['score'])) {
        $gradevalue = (float)$json['score'];
    }

    if (isset($json['feedback'])) {
        $feedback = (string)$json['feedback'];
    } elseif (isset($json['message'])) {
        $feedback = (string)$json['message'];
    }

    if ($gradevalue === null) {
        throw new moodle_exception('backendgradingfailed', 'assignsubmission_codeeval', '', null,
            "Backend JSON missing grade field. Response: $response");
    }

    $record = $this->get_record($submission->id);

    if ($record) {
        $record->sourcecode   = $code;
        $record->grade        = $gradevalue;
        $record->timemodified = time();

        $DB->update_record('assignsubmission_codeeval', $record);
    } else {
        $record = (object)[
            'assignment'   => $this->assignment->get_instance()->id,
            'submission'   => $submission->id,
            'userid'       => $submission->userid ?? $USER->id,
            'sourcecode'   => $code,
            'grade'        => $gradevalue,
            'timecreated'  => time(),
            'timemodified' => time(),
        ];
        $record->id = $DB->insert_record('assignsubmission_codeeval', $record);
    }

    // 3) Update the core assign grade (+ gradebook) correctly.
    $userid = $submission->userid ?? $USER->id;

    // This returns a row from mdl_assign_grades, creating it if needed.
    $grade = $this->assignment->get_user_grade($userid, true);

    $grade->grade        = $gradevalue;   // Column 'grade' in mdl_assign_grades.
    $grade->timemodified = time();
    $grade->grader       = $USER->id;

    // Now $grade has an id, so this will not throw the codingerror.
    $this->assignment->update_grade($grade, false);

    return true;
    }


    /**
     * Used by Moodle to decide if this plugin has any saved data.
     * If this returns true for all enabled submission plugins,
     * the assignment shows "Nothing was submitted".
     */
    public function is_empty(stdClass $submission) {
        // If we don't even have a submission id, treat it as empty.
        if (empty($submission->id)) {
            return true;
        }

        if ($record = $this->get_record($submission->id)) {
            // Empty if there is no source code or just whitespace.
            return trim($record->sourcecode) === '';
        }

        // No record in our table => empty.
        return true;
    }


    
    // ---------- Helpers ---------- //

     /**
     * Used on the *form data* before save to decide whether this plugin
     * counts as having anything submitted.
     */
    public function submission_is_empty(stdClass $data) {
        return trim($data->codeeval_source ?? '') === '';
    }

    /**
     * Get our record for a specific submission id, or false if none.
     */
    protected function get_record($submissionid) {
        global $DB;
        return $DB->get_record('assignsubmission_codeeval', [
            'submission' => $submissionid,
            'assignment' => $this->assignment->get_instance()->id,
        ]);
    }

    /**
     * Very simple mock grading rule:
     * - contains 'for' -> 100
     * - else if contains 'if' -> 80
     * - else -> 60
     */
    protected function mock_grade_from_code($code): float {
        $lower = core_text::strtolower($code);
        if (strpos($lower, 'for') !== false) {
            return 100.0;
        }
        if (strpos($lower, 'if') !== false) {
            return 80.0;
        }
        return 60.0;
    }

    protected function backend_post_json(string $path, array $payload): array {
    $base = get_config('assignsubmission_codeeval', 'backend_url');
    if (empty($base)) {
        throw new moodle_exception('backendurl', 'assignsubmission_codeeval');
    }

    $url = rtrim($base, '/') . $path;

    $curl = new curl();
    $options = [
        'CURLOPT_HTTPHEADER' => ['Content-Type: application/json'],
        'CURLOPT_TIMEOUT' => 20,
    ];

    $body = $curl->post($url, json_encode($payload), $options);
    $info = $curl->get_info();
    $code = $info['http_code'] ?? 0;
    $errno = $curl->get_errno();
    $errmsg = $curl->error;
    if ($errno) {
        error_log("CODEEVAL curl errno=$errno err=$errmsg url=$url");
    }
    return [$code, $body];
    }
}
