
function mapSubmissionDtoToUi(s) {
  return {
    id: s.id,
    taskId: s.taskId,
    userId: s.studentId,
    // UI expects these names:
    code: s.sourceCode,
    status: s.status,
    finalGrade: s.score, // UI printira {finalGrade}% (mai trqbwa da se scale-ne po kusno)
    feedback: s.feedback,
    submissionTime: s.submittedAt,
    // optional for now:
    language: s.language,
  };
}

/**
 * API Service
 * Provides data access layer that can switch between mock data and real API
 * When Config.USE_MOCK_DATA is true, uses MockData
 * When false, makes actual HTTP requests to the backend
 */
const ApiService = {
    /**
     * Generic fetch wrapper for API calls
     * @param {string} endpoint - API endpoint
     * @param {object} options - Fetch options
     * @returns {Promise<any>} - Response data
     */
    async fetch(endpoint, options = {}) {
        const url = `${Config.API_BASE_URL}${endpoint}`;
        const defaultOptions = {
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
            },
        };

        const response = await fetch(url, { ...defaultOptions, ...options });

        if (!response.ok) {
            throw new Error(`API Error: ${response.status} ${response.statusText}`);
        }

        return response.json();
    },

    // ==================== AUTH ====================
    async login(username, password) {
    const res = await this.fetch(Config.ENDPOINTS.AUTH_LOGIN, {
        method: 'POST',
        body: JSON.stringify({ username, password }),
    });
    return res; // { success, user, error }
    },

    async logout() {
    await this.fetch(Config.ENDPOINTS.AUTH_LOGOUT, { method: 'POST' });
    },

    async me() {
    return this.fetch(Config.ENDPOINTS.AUTH_ME);
    },

    // ==================== USERS ====================

    /**
     * Get all users
     * @returns {Promise<Array>} - List of users
     */
    async getUsers() {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve([...MockData.users]);
        }
        return this.fetch(Config.ENDPOINTS.USERS);
    },

    /**
     * Get user by ID
     * @param {number} id - User ID
     * @returns {Promise<object>} - User object
     */
    async getUserById(id) {
        if (Config.USE_MOCK_DATA) {
            const user = MockData.users.find(u => u.id === id);
            return Promise.resolve(user ? { ...user } : null);
        }
        return this.fetch(`${Config.ENDPOINTS.USERS}/${id}`);
    },

    /**
     * Get users by role
     * @param {string} role - Role filter (Student, Teacher, Admin)
     * @returns {Promise<Array>} - Filtered users
     */
    async getUsersByRole(role) {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve(MockData.users.filter(u => u.role === role));
        }
        return this.fetch(`${Config.ENDPOINTS.USERS}?role=${role}`);
    },

    // ==================== COURSES ====================

    /**
     * Get all courses
     * @returns {Promise<Array>} - List of courses
     */
    async getCourses() {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve([...MockData.courses]);
        }
        return this.fetch(Config.ENDPOINTS.COURSES);
    },

    /**
     * Get course by ID
     * @param {number} id - Course ID
     * @returns {Promise<object>} - Course object
     */
    async getCourseById(id) {
        if (Config.USE_MOCK_DATA) {
            const course = MockData.courses.find(c => c.id === id);
            return Promise.resolve(course ? { ...course } : null);
        }
        return this.fetch(`${Config.ENDPOINTS.COURSES}/${id}`);
    },

    // ==================== TASKS ====================

    /**
     * Get all tasks
     * @returns {Promise<Array>} - List of tasks
     */
    async getTasks() {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve([...MockData.tasks]);
        }
        //return this.fetch(Config.ENDPOINTS.TASKS);
        return this.fetch(Config.ENDPOINTS.TASKS);
    },

    /**
     * Get task by ID
     * @param {number} id - Task ID
     * @returns {Promise<object>} - Task object
     */
    async getTaskById(id) {
        if (Config.USE_MOCK_DATA) {
            const task = MockData.tasks.find(t => t.id === id);
            return Promise.resolve(task ? { ...task } : null);
        }
        //return this.fetch(`${Config.ENDPOINTS.TASKS}/${id}`);
        return this.fetch(`${Config.ENDPOINTS.TASKS}/${id}`);
    },

    /**
     * Get tasks by course ID
     * @param {number} courseId - Course ID
     * @returns {Promise<Array>} - Tasks for the course
     */
    async getTasksByCourseId(courseId) {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve(MockData.tasks.filter(t => t.courseId === courseId));
        }
        return this.fetch(`${Config.ENDPOINTS.TASKS}?courseId=${courseId}`);
    },

    /**
     * Get active tasks
     * @returns {Promise<Array>} - Active tasks only
     */
    async getActiveTasks() {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve(MockData.tasks.filter(t => t.isActive));
        }
        return this.fetch(`${Config.ENDPOINTS.TASKS}?isActive=true`);
    },

    async createTask(payload) {
        return this.fetch(Config.ENDPOINTS.TASKS, {
            method: 'POST',
            body: JSON.stringify(payload),
        });
    },

    async updateTask(id,payload) {
        return this.fetch(`${Config.ENDPOINTS.TASKS}/${id}`, {
            method: 'PUT',
            body: JSON.stringify(payload),
        });
    },

    // ==================== SUBMISSIONS ====================

    /**
     * Get all submissions
     * @returns {Promise<Array>} - List of submissions
     */
    async getSubmissions() {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve([...MockData.submissions]);
        }
        //return this.fetch(Config.ENDPOINTS.SUBMISSIONS);
        const submissions = await this.fetch(Config.ENDPOINTS.SUBMISSIONS);
        return submissions.map(mapSubmissionDtoToUi);
    },

    /**
     * Get submission by ID
     * @param {number} id - Submission ID
     * @returns {Promise<object>} - Submission object
     */
    async getSubmissionById(id) {
        if (Config.USE_MOCK_DATA) {
            const submission = MockData.submissions.find(s => s.id === id);
            return Promise.resolve(submission ? { ...submission } : null);
        }
        //return this.fetch(`${Config.ENDPOINTS.SUBMISSIONS}/${id}`);
        const submissions = await this.fetch(`${Config.ENDPOINTS.SUBMISSIONS}/${id}`);
        return mapSubmissionDtoToUi(submissions);
    },

    /**
     * Get submissions by task ID
     * @param {number} taskId - Task ID
     * @returns {Promise<Array>} - Submissions for the task
     */
    async getSubmissionsByTaskId(taskId) {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve(MockData.submissions.filter(s => s.taskId === taskId));
        }
        return this.fetch(`${Config.ENDPOINTS.SUBMISSIONS}/GetSubmissionsByTaskId?taskId=${taskId}`);
    },

    /**
     * Get submissions by user ID
     * @param {number} userId - User ID
     * @returns {Promise<Array>} - Submissions by the user
     */
    async getSubmissionsByUserId(userId) {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve(MockData.submissions.filter(s => s.userId === userId));
        }
        return this.fetch(`${Config.ENDPOINTS.SUBMISSIONS}?userId=${userId}`);
    },

    /**
     * Get submissions by status
     * @param {string} status - Status filter
     * @returns {Promise<Array>} - Filtered submissions
     */
    async getSubmissionsByStatus(status) {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve(MockData.submissions.filter(s => s.status === status));
        }
        return this.fetch(`${Config.ENDPOINTS.SUBMISSIONS}?status=${status}`);
    },

    // ==================== TEST CASES ====================

    /**
     * Get test cases by task ID
     * @param {number} taskId - Task ID
     * @returns {Promise<Array>} - Test cases for the task
     */
    async getTestCasesByTaskId(taskId) {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve(MockData.testCases.filter(tc => tc.taskId === taskId));
        }
        //return this.fetch(`${Config.ENDPOINTS.TEST_CASES}?taskId=${taskId}`);
        const task = await this.getTaskById(taskId);
        return task?.testCases || [];
    },

    // ==================== TEST RESULTS ====================

    /**
     * Get test results by submission ID
     * @param {number} submissionId - Submission ID
     * @returns {Promise<Array>} - Test results for the submission
     */
    async getTestResultsBySubmissionId(submissionId) {
        if (Config.USE_MOCK_DATA) {
            return Promise.resolve(MockData.testResults.filter(tr => tr.submissionId === submissionId));
        }
        return this.fetch(`${Config.ENDPOINTS.TEST_RESULTS}?submissionId=${submissionId}`);
    },

    // ==================== AGGREGATED/VIEW DATA ====================

    /**
     * Get enriched submissions with user and task data
     * Used by the submissions view
     * @returns {Promise<Array>} - Submissions with related data
     */
    async getEnrichedSubmissions() {
        // const [submissions, users, tasks, testCases, testResults] = await Promise.all([
        //     this.getSubmissions(),
        //     this.getUsers(),
        //     this.getTasks(),
        //     Config.USE_MOCK_DATA ? Promise.resolve(MockData.testCases) : this.fetch(Config.ENDPOINTS.TEST_CASES),
        //     Config.USE_MOCK_DATA ? Promise.resolve(MockData.testResults) : this.fetch(Config.ENDPOINTS.TEST_RESULTS)
        // ]);

        // return submissions.map(submission => {
        //     const user = users.find(u => u.id === submission.userId);
        //     const task = tasks.find(t => t.id === submission.taskId);
        //     const taskTestCases = testCases.filter(tc => tc.taskId === submission.taskId);
        //     const submissionResults = testResults.filter(tr => tr.submissionId === submission.id);
        //     const passedTests = submissionResults.filter(tr => tr.status === 'Pass').length;

        //     return {
        //         ...submission,
        //         user,
        //         task,
        //         studentName: user ? `${user.firstName} ${user.lastName}` : 'Unknown',
        //         taskTitle: task ? task.title : 'Unknown',
        //         passedTests,
        //         totalTests: taskTestCases.length
        //     };
        // });
        const [submissions, tasks, users] = await Promise.all([
            this.getSubmissions(),
            this.getTasks(),
            this.getUsers()
        ]);

        return submissions.map(submission => {
            const user = users.find(u => u.id === submission.userId);
            const task = tasks.find(t => t.id === submission.taskId);

            return {
            ...submission,
            user,
            task,
            studentName: user ? `${user.firstName} ${user.lastName}` : `User #${submission.userId}`,
            taskTitle: task ? task.name : 'Unknown',
            passedTests: null,
            totalTests: task?.testCases?.length ?? 0,
            };
        });
    },

    /**
     * Get enriched tasks with course and submission count
     * Used by the tasks view
     * @returns {Promise<Array>} - Tasks with related data
     */
    async getEnrichedTasks() {
        // const [tasks, courses, submissions] = await Promise.all([
        //     this.getTasks(),
        //     this.getCourses(),
        //     this.getSubmissions()
        // ]);

        // return tasks.map(task => {
        //     const course = courses.find(c => c.id === task.courseId);
        //     const taskSubmissions = submissions.filter(s => s.taskId === task.id);

        //     return {
        //         ...task,
        //         course,
        //         courseName: course ? course.name : 'Unknown',
        //         submissionCount: taskSubmissions.length
        //     };
        // });
        const [tasks, submissions] = await Promise.all([
        this.getTasks(),
        this.getSubmissions()
        ]);

        return tasks.map(task => {
            const taskSubmissions = submissions.filter(s => s.taskId === task.id);

            return {
                ...task,
                course: null,
                courseName: '—',
                submissionCount: taskSubmissions.length
            };
        });
    },

    /**
     * Get enriched students with submission stats
     * Used by the students view
     * @returns {Promise<Array>} - Students with stats
     */
    async getEnrichedStudents() {
        const [users, submissions] = await Promise.all([
            this.getUsersByRole('Student'),
            this.getSubmissions()
        ]);

        return users.map(user => {
            const userSubmissions = submissions.filter(s => s.userId === user.id);
            const completedSubmissions = userSubmissions.filter(s => s.status === 'Completed' && s.finalGrade !== null);
            const avgGrade = completedSubmissions.length > 0
                ? completedSubmissions.reduce((sum, s) => sum + s.finalGrade, 0) / completedSubmissions.length
                : null;

            return {
                ...user,
                fullName: `${user.firstName} ${user.lastName}`,
                submissionCount: userSubmissions.length,
                avgGrade: avgGrade !== null ? Math.round(avgGrade * 100) / 100 : null
            };
        });
    },

    /**
     * Get dashboard statistics
     * @returns {Promise<object>} - Dashboard stats
     */
    async getDashboardStats() {
        const submissions = await this.getSubmissions();

        const totalSubmissions = submissions.length;
        const completedSubmissions = submissions.filter(s => s.status === 'Completed').length;
        const pendingSubmissions = submissions.filter(s => s.status === 'Pending' || s.status === 'Processing').length;
        const completedWithGrade = submissions.filter(s => s.status === 'Completed' && s.finalGrade !== null);
        const avgGrade = completedWithGrade.length > 0
            ? completedWithGrade.reduce((sum, s) => sum + s.finalGrade, 0) / completedWithGrade.length
            : 0;

        return {
            totalSubmissions,
            completedSubmissions,
            pendingSubmissions,
            avgGrade: Math.round(avgGrade * 10) / 10
        };
    }
};
