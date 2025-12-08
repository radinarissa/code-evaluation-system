/**
 * Submissions View
 */
const SubmissionsView = {
    submissions: [],
    tasks: [],

    /**
     * Render the submissions view
     * @returns {Promise<string>} - HTML content
     */
    async render() {
        const [submissions, tasks] = await Promise.all([
            ApiService.getEnrichedSubmissions(),
            ApiService.getTasks()
        ]);

        this.submissions = submissions;
        this.tasks = tasks;

        return `
            <div class="bg-white rounded-lg shadow">
                <div class="px-6 py-4 border-b flex items-center justify-between">
                    <h3 class="text-lg font-semibold">${I18n.t('allSubmissions')}</h3>
                    <div class="flex space-x-2">
                        <select id="filter-task" class="px-3 py-2 border rounded-lg text-sm">
                            <option value="">${I18n.t('allTasks')}</option>
                            ${tasks.map(t => `<option value="${t.id}">${Utils.escapeHtml(t.title)}</option>`).join('')}
                        </select>
                        <select id="filter-status" class="px-3 py-2 border rounded-lg text-sm">
                            <option value="">${I18n.t('allStatuses')}</option>
                            <option value="Completed">${I18n.t('statusCompleted')}</option>
                            <option value="Processing">${I18n.t('statusProcessing')}</option>
                            <option value="Pending">${I18n.t('statusPending')}</option>
                            <option value="Error">${I18n.t('statusError')}</option>
                        </select>
                    </div>
                </div>
                <div class="overflow-x-auto">
                    <table class="w-full">
                        <thead class="bg-gray-50">
                            <tr>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('student')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('task')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('status')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('tests')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('grade')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('submitted')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('actions')}</th>
                            </tr>
                        </thead>
                        <tbody id="submissions-table" class="bg-white divide-y divide-gray-200">
                            ${this.renderTableRows(submissions)}
                        </tbody>
                    </table>
                </div>
            </div>
        `;
    },

    /**
     * Render table rows
     * @param {Array} submissions - Submissions to render
     * @returns {string} - HTML rows
     */
    renderTableRows(submissions) {
        if (submissions.length === 0) {
            return `
                <tr>
                    <td colspan="7" class="px-6 py-8 text-center text-gray-500">
                        No submissions found
                    </td>
                </tr>
            `;
        }

        return submissions.map(s => `
            <tr class="hover:bg-gray-50">
                <td class="px-6 py-4 whitespace-nowrap">
                    <div class="flex items-center">
                        <div class="w-8 h-8 rounded-full bg-gray-200 flex items-center justify-center">
                            <span class="text-xs font-medium text-gray-600">${Utils.getInitials(s.studentName)}</span>
                        </div>
                        <span class="ml-3 text-sm font-medium">${Utils.escapeHtml(s.studentName)}</span>
                    </div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm">${Utils.escapeHtml(s.taskTitle)}</td>
                <td class="px-6 py-4 whitespace-nowrap">
                    <span class="px-2 py-1 text-xs rounded-full ${Utils.getStatusClass(s.status)}">${I18n.getStatusText(s.status)}</span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm">
                    ${s.totalTests > 0 ? `${s.passedTests}/${s.totalTests}` : '-'}
                </td>
                <td class="px-6 py-4 whitespace-nowrap">
                    ${s.finalGrade !== null
                        ? `<span class="text-sm font-medium ${Utils.getGradeClass(s.finalGrade)}">${s.finalGrade}%</span>`
                        : '<span class="text-sm text-gray-400">-</span>'}
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${Utils.formatDate(s.submissionTime)}</td>
                <td class="px-6 py-4 whitespace-nowrap">
                    <button class="text-primary hover:text-blue-700 text-sm" onclick="SubmissionsView.viewSubmission(${s.id})">${I18n.t('viewCode')}</button>
                </td>
            </tr>
        `).join('');
    },

    /**
     * Setup filter event listeners
     */
    setupFilters() {
        const taskFilter = document.getElementById('filter-task');
        const statusFilter = document.getElementById('filter-status');

        if (taskFilter && statusFilter) {
            const applyFilters = () => {
                const taskId = taskFilter.value ? parseInt(taskFilter.value) : null;
                const status = statusFilter.value || null;

                let filtered = this.submissions;

                if (taskId) {
                    filtered = filtered.filter(s => s.taskId === taskId);
                }
                if (status) {
                    filtered = filtered.filter(s => s.status === status);
                }

                document.getElementById('submissions-table').innerHTML = this.renderTableRows(filtered);
            };

            taskFilter.addEventListener('change', applyFilters);
            statusFilter.addEventListener('change', applyFilters);
        }
    },

    /**
     * View submission details
     * @param {number} id - Submission ID
     */
    async viewSubmission(id) {
        const submission = await ApiService.getSubmissionById(id);
        if (!submission) {
            alert('Submission not found');
            return;
        }

        const user = await ApiService.getUserById(submission.userId);
        const studentName = user ? `${user.firstName} ${user.lastName}` : 'Unknown';

        // For now, show a simple alert. In production, this would open a modal
        alert(`${I18n.t('viewingSubmission')} #${id}\n\n${I18n.t('student')}: ${studentName}\n${I18n.t('status')}: ${submission.status}\n\n--- Code ---\n${submission.code}`);
    }
};
