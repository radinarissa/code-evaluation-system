/**
 * Statistics View
 * Displays task statistics based on FR-3.2 requirements
 */
const StatisticsView = {
    taskId: 0,
    // Mock statistics data - matches backend Submission model
    // Grade is on Bulgarian scale: 2 (Слаб) to 6 (Отличен)
    mockTaskStatistics: {
        taskId: 1,
        taskTitle: 'Двоично търсене',
        courseName: 'Структури от данни',
        // Submissions data for calculating statistics
        submissions: [
            { id: 1, userId: 1, grade: 5.75, submittedAt: '2024-12-03T14:30:00', attemptNumber: 1 },
            { id: 2, userId: 1, grade: 6.00, submittedAt: '2024-12-03T16:00:00', attemptNumber: 2 },
            { id: 3, userId: 2, grade: 5.00, submittedAt: '2024-12-03T15:45:00', attemptNumber: 1 },
            { id: 4, userId: 3, grade: 3.50, submittedAt: '2024-12-03T16:00:00', attemptNumber: 1 },
            { id: 5, userId: 3, grade: 4.25, submittedAt: '2024-12-03T18:30:00', attemptNumber: 2 },
            { id: 6, userId: 4, grade: 6.00, submittedAt: '2024-12-04T08:30:00', attemptNumber: 1 },
            { id: 7, userId: 5, grade: 5.50, submittedAt: '2024-12-03T17:00:00', attemptNumber: 1 },
            { id: 8, userId: 6, grade: 2.00, submittedAt: '2024-12-04T10:00:00', attemptNumber: 1 },
            { id: 9, userId: 6, grade: 3.00, submittedAt: '2024-12-04T11:30:00', attemptNumber: 2 },
            { id: 10, userId: 7, grade: 6.00, submittedAt: '2024-12-04T10:30:00', attemptNumber: 1 },
            { id: 11, userId: 8, grade: 3.25, submittedAt: '2024-12-04T12:00:00', attemptNumber: 1 },
            { id: 12, userId: 8, grade: 4.00, submittedAt: '2024-12-04T14:00:00', attemptNumber: 2 },
            { id: 13, userId: 8, grade: 5.00, submittedAt: '2024-12-04T16:00:00', attemptNumber: 3 },
            { id: 14, userId: 9, grade: 2.00, submittedAt: '2024-12-04T09:00:00', attemptNumber: 1 },
            { id: 15, userId: 10, grade: 4.50, submittedAt: '2024-12-04T11:00:00', attemptNumber: 1 }
        ]
    },

    /**
     * Calculate statistics from submissions data
     * @param {Array} submissions - Array of submission objects
     * @returns {Object} - Calculated statistics
     */
    calculateStatistics(submissions, maxPoints) {
        if (!submissions || submissions.length === 0) {
            return this.getEmptyStatistics();
        }

        // A. General Information
        const totalSubmissions = submissions.length;
        const uniqueStudents = new Set(submissions.map(s => s.studentId)).size;
        const avgAttemptsPerStudent = (totalSubmissions / uniqueStudents).toFixed(2);

        // Calculate average time between first and last attempt per student
        const studentAttempts = {};
        submissions.forEach(s => {
            if (!studentAttempts[s.studentId]) {
                studentAttempts[s.studentId] = [];
            }
            studentAttempts[s.studentId].push(new Date(s.submittedAt));
        });

        let totalTimeDiff = 0;
        let studentsWithMultipleAttempts = 0;
        Object.values(studentAttempts).forEach(times => {
            if (times.length > 1) {
                times.sort((a, b) => a - b);
                const timeDiff = times[times.length - 1] - times[0];
                totalTimeDiff += timeDiff;
                studentsWithMultipleAttempts++;
            }
        });

        const avgTimeBetweenAttempts = studentsWithMultipleAttempts > 0
            ? totalTimeDiff / studentsWithMultipleAttempts
            : 0;

        // First and last submission
        const sortedByTime = [...submissions].sort(
            (a, b) => new Date(a.submittedAt) - new Date(b.submittedAt)
        );
        const firstSubmission = sortedByTime[0].submittedAt;
        const lastSubmission = sortedByTime[sortedByTime.length - 1].submittedAt;

        // B. Grade Distribution (Bulgarian scale: 2-6)
        const grades = submissions.map(s => (s.score / maxPoints) * 4 + 2).filter(g => g !== null);

        // Histogram ranges: 2-3, 3-4, 4-5, 5-6 (Bulgarian grading scale)
        const histogram = {
            '2-3': grades.filter(g => g >= 2 && g < 3).length,
            '3-4': grades.filter(g => g >= 3 && g < 4).length,
            '4-5': grades.filter(g => g >= 4 && g < 5).length,
            '5-6': grades.filter(g => g >= 5 && g <= 6).length
        };

        const avgGrade = grades.length > 0
            ? (grades.reduce((sum, g) => sum + g, 0) / grades.length).toFixed(2)
            : 0;

        const sortedGrades = [...grades].sort((a, b) => a - b);
        const medianGrade = this.calculateMedian(sortedGrades);
        const minGrade = Math.min(...grades);
        const maxGrade = Math.max(...grades);

        const perfectScorePercent = ((grades.filter(g => g >= 5.5).length / grades.length) * 100).toFixed(1);
        const failingScorePercent = ((grades.filter(g => g <= 2.5).length / grades.length) * 100).toFixed(1);

        return {
            general: {
                totalSubmissions,
                uniqueStudents,
                avgAttemptsPerStudent,
                avgTimeBetweenAttempts,
                firstSubmission,
                lastSubmission
            },
            grades: {
                histogram,
                avgGrade,
                medianGrade,
                minGrade,
                maxGrade,
                perfectScorePercent,
                failingScorePercent
            }
        };
    },

    /**
     * Calculate median of sorted array
     * @param {Array} sortedArr - Sorted array of numbers
     * @returns {number} - Median value
     */
    calculateMedian(sortedArr) {
        if (sortedArr.length === 0) return 0;
        const mid = Math.floor(sortedArr.length / 2);
        if (sortedArr.length % 2 === 0) {
            return ((sortedArr[mid - 1] + sortedArr[mid]) / 2).toFixed(2);
        }
        return sortedArr[mid].toFixed(2);
    },

    /**
     * Get empty statistics object
     * @returns {Object} - Empty statistics
     */
    getEmptyStatistics() {
        return {
            general: {
                totalSubmissions: 0,
                uniqueStudents: 0,
                avgAttemptsPerStudent: 0,
                avgTimeBetweenAttempts: 0,
                firstSubmission: null,
                lastSubmission: null
            },
            grades: {
                histogram: { '2-3': 0, '3-4': 0, '4-5': 0, '5-6': 0 },
                avgGrade: 0,
                medianGrade: 0,
                minGrade: 0,
                maxGrade: 0,
                perfectScorePercent: 0,
                failingScorePercent: 0
            }
        };
    },

    /**
     * Format milliseconds to human readable duration
     * @param {number} ms - Milliseconds
     * @returns {string} - Formatted duration
     */
    formatDuration(ms) {
        if (ms === 0) return I18n.t('notAvailable');

        const hours = Math.floor(ms / (1000 * 60 * 60));
        const minutes = Math.floor((ms % (1000 * 60 * 60)) / (1000 * 60));

        if (hours > 0) {
            return `${hours}${I18n.t('hours')} ${minutes}${I18n.t('minutes')}`;
        }
        return `${minutes}${I18n.t('minutes')}`;
    },

    /**
     * Render the statistics view
     * @returns {Promise<string>} - HTML content
     */
    async render() {;
        if (!this.taskId)
        {
            let allTasks = await ApiService.getTasks();

            this.taskId = allTasks.length > 0 ? allTasks[0].id : 0;
        }

        const [submissions, tasks, task] = await Promise.all([
            ApiService.getSubmissionsByTaskId(this.taskId),
            ApiService.getTasks(),
            ApiService.getTaskById(this.taskId)
        ]);

        const stats = this.calculateStatistics(submissions, task.maxPoints);

        return `
            <div class="space-y-6">
                <!-- Task Selector -->
                <div class="bg-white rounded-lg shadow p-6">
                    <div class="flex items-center justify-between">
                        <div>
                            <h3 class="text-lg font-semibold">${I18n.t('statisticsFor')}: ${Utils.escapeHtml(task.name)}</h3>
                        </div>
                        <select id="task-selector" class="border rounded-lg px-4 py-2" onchange="StatisticsView.changeTask(this.value)">
                            ${tasks.map(t => `<option value="${t.id}" ${t.id === this.taskId ? "selected" : ""}>${Utils.escapeHtml(t.name)}</option>`).join('')}
                        </select>
                    </div>
                </div>

                <!-- A. General Information Section -->
                <div class="bg-white rounded-lg shadow">
                    <div class="px-6 py-4 border-b">
                        <h3 class="text-lg font-semibold">${I18n.t('generalInfo')}</h3>
                    </div>
                    <div class="p-6 grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">
                        ${this.renderStatCard(I18n.t('totalSubmissions'), stats.general.totalSubmissions, 'bg-blue-50 text-blue-700')}
                        ${this.renderStatCard(I18n.t('uniqueStudents'), stats.general.uniqueStudents, 'bg-green-50 text-green-700')}
                        ${this.renderStatCard(I18n.t('avgAttempts'), stats.general.avgAttemptsPerStudent, 'bg-purple-50 text-purple-700')}
                        ${this.renderStatCard(I18n.t('avgTimeBetween'), this.formatDuration(stats.general.avgTimeBetweenAttempts), 'bg-orange-50 text-orange-700')}
                        ${this.renderStatCard(I18n.t('firstSubmission'), stats.general.firstSubmission ? I18n.formatDateTime(stats.general.firstSubmission) : I18n.t('notAvailable'), 'bg-gray-50 text-gray-700')}
                        ${this.renderStatCard(I18n.t('lastSubmission'), stats.general.lastSubmission ? I18n.formatDateTime(stats.general.lastSubmission) : I18n.t('notAvailable'), 'bg-gray-50 text-gray-700')}
                    </div>
                </div>

                <!-- B. Grade Distribution Section -->
                <div class="bg-white rounded-lg shadow">
                    <div class="px-6 py-4 border-b">
                        <h3 class="text-lg font-semibold">${I18n.t('gradeDistribution')}</h3>
                    </div>
                    <div class="p-6">
                        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
                            <!-- Histogram -->
                            <div>
                                <h4 class="text-sm font-medium text-gray-600 mb-4">${I18n.t('histogram')}</h4>
                                ${this.renderHistogram(stats.grades.histogram, stats.general.totalSubmissions)}
                            </div>

                            <!-- Grade Statistics -->
                            <div class="grid grid-cols-2 gap-4">
                                ${this.renderStatCard(I18n.t('averageGrade'), `${stats.grades.avgGrade}/6`, 'bg-blue-50 text-blue-700')}
                                ${this.renderStatCard(I18n.t('medianGrade'), `${stats.grades.medianGrade}/6`, 'bg-indigo-50 text-indigo-700')}
                                ${this.renderStatCard(I18n.t('minGrade'), `${stats.grades.minGrade}/6`, 'bg-red-50 text-red-700')}
                                ${this.renderStatCard(I18n.t('maxGrade'), `${stats.grades.maxGrade}/6`, 'bg-green-50 text-green-700')}
                                ${this.renderStatCard(I18n.t('perfectScore'), `${stats.grades.perfectScorePercent}%`, 'bg-emerald-50 text-emerald-700')}
                                ${this.renderStatCard(I18n.t('failingScore'), `${stats.grades.failingScorePercent}%`, 'bg-rose-50 text-rose-700')}
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Recent Submissions Preview -->
                <div class="bg-white rounded-lg shadow">
                    <div class="px-6 py-4 border-b">
                        <h3 class="text-lg font-semibold">${I18n.t('recentSubmissionsPreview')}</h3>
                    </div>
                    <div class="overflow-x-auto">
                        <table class="w-full">
                            <thead class="bg-gray-50">
                                <tr>
                                    <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">${I18n.t('student')}</th>
                                    <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">${I18n.t('grade')}</th>
                                    <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">${I18n.t('submitted')}</th>
                                </tr>
                            </thead>
                            <tbody class="divide-y">
                                ${await this.renderRecentSubmissions(submissions.slice(-5).reverse(), task.maxPoints)}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        `;
    },

    /**
     * Render a statistics card
     * @param {string} label - Card label
     * @param {string|number} value - Card value
     * @param {string} colorClasses - Tailwind color classes
     * @returns {string} - HTML
     */
    renderStatCard(label, value, colorClasses) {
        return `
            <div class="rounded-lg p-4 ${colorClasses}">
                <p class="text-xs font-medium opacity-75 mb-1">${label}</p>
                <p class="text-lg font-bold">${value}</p>
            </div>
        `;
    },

    /**
     * Render histogram chart
     * @param {Object} histogram - Histogram data
     * @param {number} total - Total submissions
     * @returns {string} - HTML
     */
    renderHistogram(histogram, total) {
        const maxCount = Math.max(...Object.values(histogram), 1);
        const colors = {
            '2-3': 'bg-red-500',
            '3-4': 'bg-orange-500',
            '4-5': 'bg-yellow-500',
            '5-6': 'bg-green-500'
        };

        return `
            <div class="space-y-3">
                ${Object.entries(histogram).map(([range, count]) => {
                    const percentage = total > 0 ? ((count / total) * 100).toFixed(1) : 0;
                    const barWidth = maxCount > 0 ? (count / maxCount) * 100 : 0;
                    return `
                        <div class="flex items-center gap-3">
                            <span class="w-12 text-sm text-gray-600 font-medium">${range}</span>
                            <div class="flex-1 bg-gray-200 rounded-full h-6 relative overflow-hidden">
                                <div class="${colors[range]} h-full rounded-full transition-all duration-500" style="width: ${barWidth}%"></div>
                                <span class="absolute inset-0 flex items-center justify-center text-xs font-medium ${barWidth > 50 ? 'text-white' : 'text-gray-700'}">
                                    ${count} (${percentage}%)
                                </span>
                            </div>
                        </div>
                    `;
                }).join('')}
            </div>
            <div class="mt-4 flex justify-between text-xs text-gray-500">
                <span>${I18n.t('poor')}</span>
                <span>${I18n.t('excellent')}</span>
            </div>
        `;
    },

    /**
     * Render recent submissions table rows
     * @param {Array} submissions - Submissions array
     * @returns {string} - HTML table rows
     */
    async renderRecentSubmissions(submissions, maxPoints) {
        // Map user IDs to names (mock)
        const students = await ApiService.getEnrichedStudents();

        return submissions.map(s => `
            <tr class="hover:bg-gray-50">
                <td class="px-6 py-4 whitespace-nowrap">
                    <span class="font-medium">${students.filter(s => s.id === s.studentId).length > 0 ? students.filter(s => s.id === s.studentId)[0].fullName : ""}</span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap">
                    <span class="font-medium ${(s.score / maxPoints) * 4 + 2 >= 5 ? 'text-green-600' : (s.score / maxPoints) * 4 + 2 >= 4 ? 'text-blue-600' : (s.score / maxPoints) * 4 + 2 >= 3 ? 'text-orange-600' : 'text-red-600'}">
                        ${((s.score / maxPoints) * 4 + 2).toFixed(2)}/6
                    </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    ${I18n.formatDateTime(s.submittedAt)}
                </td>
            </tr>
        `).join('');
    },

    /**
     * Handle task change from dropdown
     * @param {string} taskId - Selected task ID
     */
    async changeTask(taskId) {
        // In real implementation, this would fetch statistics for the selected task
        StatisticsView.taskId = parseInt(taskId);

        document.getElementById('content').innerHTML = await StatisticsView.render();
    }
};
