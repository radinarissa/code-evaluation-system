/**
 * Students View
 */
const StudentsView = {
    /**
     * Render the students view
     * @returns {Promise<string>} - HTML content
     */
    async render() {
        const students = await ApiService.getEnrichedStudents();

        return `
            <div class="bg-white rounded-lg shadow">
                <div class="px-6 py-4 border-b">
                    <h3 class="text-lg font-semibold">${I18n.t('students')}</h3>
                </div>
                <div class="overflow-x-auto">
                    <table class="w-full">
                        <thead class="bg-gray-50">
                            <tr>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('name')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('username')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('email')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('submissions')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('avgGrade')}</th>
                                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">${I18n.t('actions')}</th>
                            </tr>
                        </thead>
                        <tbody class="bg-white divide-y divide-gray-200">
                            ${this.renderTableRows(students)}
                        </tbody>
                    </table>
                </div>
            </div>
        `;
    },

    /**
     * Render table rows
     * @param {Array} students - Students to render
     * @returns {string} - HTML rows
     */
    renderTableRows(students) {
        if (students.length === 0) {
            return `
                <tr>
                    <td colspan="6" class="px-6 py-8 text-center text-gray-500">
                        No students found
                    </td>
                </tr>
            `;
        }

        return students.map(s => `
            <tr class="hover:bg-gray-50">
                <td class="px-6 py-4 whitespace-nowrap">
                    <div class="flex items-center">
                        <div class="w-10 h-10 rounded-full bg-gray-200 flex items-center justify-center">
                            <span class="text-sm font-medium text-gray-600">${Utils.getInitials(s.fullName)}</span>
                        </div>
                        <span class="ml-3 font-medium">${Utils.escapeHtml(s.fullName)}</span>
                    </div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${Utils.escapeHtml(s.username)}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${Utils.escapeHtml(s.email)}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm">${s.submissionCount}</td>
                <td class="px-6 py-4 whitespace-nowrap">
                    ${s.avgGrade !== null
                        ? `<span class="text-sm font-medium ${Utils.getGradeClass(s.avgGrade)}">${s.avgGrade}%</span>`
                        : `<span class="text-sm text-gray-400">${I18n.t('notAvailable')}</span>`}
                </td>
                <td class="px-6 py-4 whitespace-nowrap">
                    <button class="text-primary hover:text-blue-700 text-sm" onclick="StudentsView.viewStudent(${s.id})">
                        ${I18n.t('viewDetails')}
                    </button>
                </td>
            </tr>
        `).join('');
    },

    /**
     * View student details
     * @param {number} id - Student ID
     */
    async viewStudent(id) {
        const user = await ApiService.getUserById(id);
        if (!user) {
            alert('Student not found');
            return;
        }

        const submissions = await ApiService.getSubmissionsByUserId(id);

        alert(`${I18n.t('student')}: ${user.firstName} ${user.lastName}\n${I18n.t('email')}: ${user.email}\n${I18n.t('username')}: ${user.username}\n\n${I18n.t('submissions')}: ${submissions.length}`);
    }
};
