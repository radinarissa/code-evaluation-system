<?php
defined('MOODLE_INTERNAL') || die();


require_once($CFG->dirroot . '/mod/assign/submissionplugin.php');
require_once($CFG->libdir . '/filelib.php');

/**
 * CodeEval submission
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
        //Task id
        $fieldname = 'assignsubmission_codeeval_taskid';  
        $mform->addElement('text', $fieldname, get_string('taskid', 'assignsubmission_codeeval'));
        $mform->setType($fieldname, PARAM_INT);
        $mform->addHelpButton($fieldname, 'taskid', 'assignsubmission_codeeval');

        $mform->setDefault($fieldname, (int)$this->get_config('taskid'));
        //freeze === read-only
        $mform->freeze($fieldname);


        // Limits
        $mform->addElement('text', 'assignsubmission_codeeval_timelimits', 'Time limit (seconds)');
        $mform->setType('assignsubmission_codeeval_timelimits', PARAM_INT);
        $mform->setDefault('assignsubmission_codeeval_timelimits', (int)($this->get_config('timelimits') ?? 3));
        $mform->addRule('assignsubmission_codeeval_timelimits', null, 'required', null, 'client');

        $mform->addElement('text', 'assignsubmission_codeeval_memorylimitkb', 'Memory limit (KB)');
        $mform->setType('assignsubmission_codeeval_memorylimitkb', PARAM_INT);
        $mform->setDefault('assignsubmission_codeeval_memorylimitkb', (int)($this->get_config('memorylimitkb') ?? 262144));
        $mform->addRule('assignsubmission_codeeval_memorylimitkb', null, 'required', null, 'client');

        $mform->addElement('text', 'assignsubmission_codeeval_disksizelimitkb', 'Max file size (KB)');
        $mform->setType('assignsubmission_codeeval_disksizelimitkb', PARAM_INT);
        $mform->setDefault('assignsubmission_codeeval_disksizelimitkb', (int)($this->get_config('disksizelimitkb') ?? 256));
        $mform->addRule('assignsubmission_codeeval_disksizelimitkb', null, 'required', null, 'client');

        // Testcases JSON
        $mform->addElement('textarea', 'assignsubmission_codeeval_testcasesjson', 'Test cases (JSON)', 'wrap="virtual" rows="12" cols="80"');
        $mform->setType('assignsubmission_codeeval_testcasesjson', PARAM_RAW);

        $defaultjson = $this->get_config('testcasesjson');
        if (empty($defaultjson)) {
            $defaultjson = json_encode([
            [
                "name" => "Test 1",
                "input" => "",
                "expectedOutput" => "",
                "isPublic" => true,
                "points" => 1,
                "executionOrder" => 1
            ]], JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE);
        }
        
        $mform->setDefault('assignsubmission_codeeval_testcasesjson', $defaultjson);
    }


    public function validate_settings(MoodleQuickForm $mform, stdClass $data) {
        $errors = [];

        $time = (int)($data->assignsubmission_codeeval_timelimits ?? 0);
        $mem  = (int)($data->assignsubmission_codeeval_memorylimitkb ?? 0);
        $file = (int)($data->assignsubmission_codeeval_disksizelimitkb ?? 0);

        //Match Judge constraints
        if ($time < 1 || $time > 15) {
            $errors['assignsubmission_codeeval_timelimits'] = 'Time limit must be between 1 and 15 seconds.';
        }

        if ($mem < 2048) {
            $errors['assignsubmission_codeeval_memorylimitkb'] = 'Memory limit must be at least 2048 KB.';
        }

        if ($file < 1) {
            $errors['assignsubmission_codeeval_disksizelimitkb'] = 'Max file size must be at least 1 KB.';
        }

        // Validate testcases JSON
        $jsonraw = trim($data->assignsubmission_codeeval_testcasesjson ?? '');
        $testcases = json_decode($jsonraw, true);

        if (!is_array($testcases) || count($testcases) < 1) {
            $errors['assignsubmission_codeeval_testcasesjson'] = 'Test cases JSON must be a non-empty JSON array.';
            return $errors;
        }

        $i = 1;
        foreach ($testcases as $tc) {
            $name = trim((string)($tc['name'] ?? ''));
            if ($name === '') {
                $errors['assignsubmission_codeeval_testcasesjson'] = "Test case #$i is missing a name.";
                break;
            }
            if (!array_key_exists('input', $tc) || !array_key_exists('expectedOutput', $tc)) {
                $errors['assignsubmission_codeeval_testcasesjson'] = "Test case #$i must contain input and expectedOutput.";
                break;
            }
            $i++;
        }

        return $errors;
    }


    /**
     * Save per-assignment settings.
     */
    // public function save_settings(stdClass $data) {
    //     $fieldname = 'assignsubmission_codeeval_taskid';
    //     $taskid = 0;

    //     if (isset($data->$fieldname)) {
    //         $taskid = (int)$data->$fieldname;
    //     }

    //     $this->set_config('taskid', $taskid);
    //     return true;
    // }
    public function save_settings(stdClass $data) {
        global $USER;

        // Read fields
        $timelimits = max(1, min(15, (int)($data->assignsubmission_codeeval_timelimits ?? 3)));
        $memorykb = max(2048, (int)($data->assignsubmission_codeeval_memorylimitkb ?? 262144));
        $diskkb = max(1, (int)($data->assignsubmission_codeeval_disksizelimitkb ?? 256));

        $jsonraw = trim($data->assignsubmission_codeeval_testcasesjson ?? '');
        if ($jsonraw === '') {
            throw new moodle_exception('Missing testcases JSON');
        }

        $testcases = json_decode($jsonraw, true);
        if (!is_array($testcases)) {
            throw new moodle_exception('Invalid JSON in testcases field');
        }

        foreach ($testcases as &$tc) {
            if (isset($tc['input'])) {
                $tc['input'] = str_replace("\\r\\n", "\n", $tc['input']);
                $tc['input'] = str_replace("\\n", "\n", $tc['input']);
            }
            if (isset($tc['expectedOutput'])) {
                $tc['expectedOutput'] = str_replace("\\r\\n", "\n", $tc['expectedOutput']);
                $tc['expectedOutput'] = str_replace("\\n", "\n", $tc['expectedOutput']);
            }
        }
        unset($tc);
        
        // Build backend payload (MoodleTaskUpsertDto)
        $assign = $this->assignment->get_instance();

        $role = is_siteadmin($USER) ? "Admin" : "Teacher";

        $payload = [
        "moodleCourseId" => (int)$this->assignment->get_course()->id,
        "moodleAssignmentId" => (int)$assign->id,
        "moodleAssignmentName" => (string)$assign->name,

        "teacher" => [
            "moodleId" => (int)$USER->id,
            "username" => (string)$USER->username,
            "email" => (string)$USER->email,
            "firstName" => (string)$USER->firstname,
            "lastName" => (string)$USER->lastname,
            "role" => $role,
        ],

        "title" => (string)$assign->name,
        "description" => (string)($assign->intro ?? ''),
        "maxPoints" => (float)($assign->grade ?? 10),

        "timeLimitS" => $timelimits,
        "memoryLimitKb" => $memorykb,
        "diskLimitKb" => $diskkb,
        "stackLimitKb" => null,

        "testCases" => []
    ];

        $order = 1;
        foreach ($testcases as $tc) {
            $payload["testCases"][] = [
                "name" => (string)($tc["name"] ?? ("Test $order")),
                "input" => (string)($tc["input"] ?? ""),
                "expectedOutput" => (string)($tc["expectedOutput"] ?? ""),
                "isPublic" => (bool)($tc["isPublic"] ?? false),
                "points" => (float)($tc["points"] ?? 1),
                "executionOrder" => (int)($tc["executionOrder"] ?? $order),
            ];
            $order++;
        }

        // Call backend
        list($httpcode, $response) = $this->backend_post_json('/api/tasks/moodle/upsert', $payload);
        error_log("CODEEVAL upsert HTTP=$httpcode response=$response");

        if ($httpcode < 200 || $httpcode >= 300) {
            throw new moodle_exception('backendgradingfailed', 'assignsubmission_codeeval', '', null,
                "Task upsert failed HTTP $httpcode: $response");
        }

        $resp = json_decode($response, true);
        if (!is_array($resp) || !isset($resp["taskId"])) {
            if (isset($resp["id"])) {
                $taskid = (int)$resp["id"];
            } else {
                throw new moodle_exception("Backend upsert response missing taskId/id");
            }
        } else {
            $taskid = (int)$resp["taskId"];
        }

        // Save configs
        $this->set_config('taskid', $taskid);
        $this->set_config('timelimits', $timelimits);
        $this->set_config('memorylimitkb', $memorykb);
        $this->set_config('disksizelimitkb', $diskkb);
        
        $normalizedjson = json_encode($testcases, JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE);
        $this->set_config('testcasesjson', $normalizedjson);

        return true;
    }


    /**
     * Add elements to the student submission form.
     */
    public function get_form_elements($submission, $mform, $data) {
        
        $taskid = (int)$this->get_config('taskid');
        if ($taskid > 0) {
            list($httpcode, $body) = $this->backend_get_json('/api/tasks/' . $taskid);

            if ($httpcode >= 200 && $httpcode < 300) {
                $task = json_decode($body, true);
                if (is_array($task) && !empty($task['testCases'])) {

                    // filter public tests
                    $public = array_filter($task['testCases'], function($tc) {
                        return !empty($tc['isPublic']);
                    });

                    if (!empty($public)) {
                        $html = '<div style="border:1px solid #ddd; padding:12px; border-radius:8px; margin-bottom:12px;">';
                        $html .= '<h4>Example tests</h4>';

                        foreach ($public as $tc) {
                            $name = htmlspecialchars($tc['name'] ?? 'Example');
                            $input = htmlspecialchars($tc['input'] ?? '');
                            $expected = htmlspecialchars($tc['expectedOutput'] ?? '');

                            $html .= "<div style='margin-bottom:10px;'>";
                            $html .= "<strong>{$name}</strong><br/>";
                            $html .= "<em>Input:</em><pre>{$input}</pre>";
                            $html .= "<em>Expected output:</em><pre>{$expected}</pre>";
                            $html .= "</div>";
                        }

                        $html .= '</div>';
                        $mform->addElement('html', $html);
                    }
                }
            } else {
                error_log("CODEEVAL could not fetch task examples HTTP=$httpcode body=$body");
            }
        }

        $mform->addElement('textarea', 'codeeval_source', get_string('sourcecode', 'assignsubmission_codeeval'), 'wrap="virtual" rows="20" cols="80"');
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

    $subrec = $DB->get_record('assign_submission', ['id' => (int)$submission->id], 'id, attemptnumber', MUST_EXIST);
    $moodleattemptnumber = (int)$subrec->attemptnumber;

    $payload = [
    'taskId' => $taskid,
    'studentId' => (int)$userid,
    'language' => 51,
    'sourceCode' => $code,
    'moodleUserId' => (int)$userid,
    'moodleCourseId' => (string)$this->assignment->get_course()->id,
    'moodleSubmissionId' => (int)$submission->id,
    'moodleAttemptNumber' => $moodleattemptnumber,
    'user' => [
        'moodleId' => (int)$userid,
        'username' => (string)$USER->username,
        'email' => (string)$USER->email,
        'firstName' => (string)$USER->firstname,
        'lastName' => (string)$USER->lastname,
        'role' => 'Student'
        ],
    ];
    error_log("CODEEVAL moodle attemptnumber=$moodleattemptnumber submissionid={$submission->id} userid=$userid");
    error_log("CODEEVAL payload=" . json_encode($payload));
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

    $islist = array_keys($json) === range(0, count($json) - 1);
    if ($islist) {
        $json = $json[0] ?? [];
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

    $record = $this->get_record($submission->id);

    if ($record) {
        $record->sourcecode   = $code;
        $record->grade        = $gradevalue;
        $record->timemodified = time();

        $DB->update_record('assignsubmission_codeeval', $record);
    } else {
        $record = (object)[
            'assignment' => $this->assignment->get_instance()->id,
            'submission' => $submission->id,
            'userid' => $submission->userid ?? $USER->id,
            'sourcecode' => $code,
            'grade' => $gradevalue,
            'timecreated' => time(),
            'timemodified' => time(),
        ];
        $record->id = $DB->insert_record('assignsubmission_codeeval', $record);
    }

    return true;
    }


    /**
     * Used by Moodle to decide if this plugin has any saved data.
     * If this returns true for all enabled submission plugins,
     * the assignment shows "Nothing was submitted".
     */
    public function is_empty(stdClass $submission) {
        if (empty($submission->id)) {
            return true;
        }

        if ($record = $this->get_record($submission->id)) {
            return trim($record->sourcecode) === '';
        }

        return true;
    }


    public function delete_submission(stdClass $submission) {
        global $DB;

        if (empty($submission->id)) {
            return true;
        }

        // Delete our plugin data for this attempt.
        $DB->delete_records('assignsubmission_codeeval', [
            'submission' => $submission->id,
            'assignment' => $this->assignment->get_instance()->id,
        ]);

        return true;
    }

    //call the remove hook to be sure that it will be deleted.
    public function remove(stdClass $submission) {
        return $this->delete_submission($submission);
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
    // protected function mock_grade_from_code($code): float {
    //     $lower = core_text::strtolower($code);
    //     if (strpos($lower, 'for') !== false) {
    //         return 100.0;
    //     }
    //     if (strpos($lower, 'if') !== false) {
    //         return 80.0;
    //     }
    //     return 60.0;
    // }

    protected function backend_post_json(string $path, array $payload): array {
        $base = get_config('assignsubmission_codeeval', 'backend_url');
        if (empty($base)) {
            throw new moodle_exception('backendurl', 'assignsubmission_codeeval');
        }

        $url = rtrim($base, '/') . $path;

        $curl = new curl();
        $options = [
            'CURLOPT_HTTPHEADER' => ['Content-Type: application/json'],
            'CURLOPT_CONNECTTIMEOUT' => 10,
            'CURLOPT_TIMEOUT' => 120,
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
    
    //helper to fetch a task JSON
    protected function backend_get_json(string $path): array {
        $base = get_config('assignsubmission_codeeval', 'backend_url');
        if (empty($base)) {
            throw new moodle_exception('backendurl', 'assignsubmission_codeeval');
        }

        $url = rtrim($base, '/') . $path;

        $curl = new curl();
        $options = ['CURLOPT_CONNECTTIMEOUT' => 10, 'CURLOPT_TIMEOUT' => 30, ];

        $body = $curl->get($url, [], $options);
        $info = $curl->get_info();
        $code = $info['http_code'] ?? 0;

        return [$code, $body];
    }

    //     protected function moodle_ws_post(array $params): array {
    //     $base = get_config('assignsubmission_codeeval', 'moodle_ws_url');
    //     $token = get_config('assignsubmission_codeeval', 'moodle_ws_token');

    //     if (empty($base) || empty($token)) {
    //         throw new moodle_exception('Missing moodle_ws_url or moodle_ws_token plugin setting');
    //     }

    //     $url = rtrim($base, '/') . '/webservice/rest/server.php';

    //     $params['wstoken'] = $token;
    //     $params['moodlewsrestformat'] = 'json';

    //     $curl = new curl();
    //     $options = [
    //         'CURLOPT_CONNECTTIMEOUT' => 10,
    //         'CURLOPT_TIMEOUT' => 60,
    //     ];

    //     $body = $curl->post($url, $params, $options);
    //     $info = $curl->get_info();
    //     $code = $info['http_code'] ?? 0;

    //     return [$code, $body];
    // }
}
