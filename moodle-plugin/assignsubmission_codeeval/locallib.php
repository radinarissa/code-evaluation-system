<?php
defined('MOODLE_INTERNAL') || die();

require_once($CFG->dirroot . '/mod/assign/submissionplugin.php');

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
     * Add elements to the student submission form.
     */
    public function get_form_elements($submission, MoodleQuickForm $mform, stdClass $data) {

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
    public function save(stdClass $submission, stdClass $data) {
    global $DB, $USER;

    $code = trim($data->codeeval_source ?? '');

    // 1) Decide mock grade.
    $gradevalue = $this->mock_grade_from_code($code);

    // 2) Insert/update our own table.
    $record = $this->get_record($submission->id);

    if ($record) {
        $record->sourcecode   = $code;
        $record->grade        = $gradevalue;
        $record->timemodified = time();

        // $record->id is guaranteed to exist because it comes from get_record().
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
    protected function get_record(int $submissionid) {
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
    protected function mock_grade_from_code(string $code): float {
        $lower = core_text::strtolower($code);
        if (strpos($lower, 'for') !== false) {
            return 100.0;
        }
        if (strpos($lower, 'if') !== false) {
            return 80.0;
        }
        return 60.0;
    }
}
