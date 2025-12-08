/**
 * Dashboard View
 */
const DashboardView = {
    /**
     * Render the dashboard view
     * @returns {Promise<string>} - HTML content
     */
    async render() {
        const [stats, enrichedSubmissions, enrichedTasks] = await Promise.all([
            ApiService.getDashboardStats(),
            ApiService.getEnrichedSubmissions(),
            ApiService.getEnrichedTasks()
        ]);

        const recentSubmissions = enrichedSubmissions
            .sort((a, b) => new Date(b.submissionTime) - new Date(a.submissionTime))
            .slice(0, 5);

        const activeTasks = enrichedTasks.filter(t => t.isActive);

        return `
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
                ${this.renderStatCard('totalSubmissions', stats.totalSubmissions, 'blue', this.getDocumentIcon())}
                ${this.renderStatCard('completed', stats.completedSubmissions, 'green', this.getCheckIcon())}
                ${this.renderStatCard('pending', stats.pendingSubmissions, 'yellow', this.getClockIcon())}
                ${this.renderStatCard('averageGrade', `${stats.avgGrade}%`, 'purple', this.getChartIcon())}
            </div>

            <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
                ${this.renderRecentSubmissions(recentSubmissions)}
                ${this.renderActiveTasks(activeTasks)}
            </div>
        `;
    },

    /**
     * Render a stat card
     */
    renderStatCard(labelKey, value, color, icon) {
        return `
            <div class="bg-white rounded-lg shadow p-6">
                <div class="flex items-center">
                    <div class="p-3 rounded-full bg-${color}-100 text-${color}-600">
                        ${icon}
                    </div>
                    <div class="ml-4">
                        <p class="text-sm text-gray-500">${I18n.t(labelKey)}</p>
                        <p class="text-2xl font-semibold">${value}</p>
                    </div>
                </div>
            </div>
        `;
    },

    /**
     * Render recent submissions panel
     */
    renderRecentSubmissions(submissions) {
        const items = submissions.map(s => `
            <div class="flex items-center justify-between">
                <div class="flex items-center">
                    <div class="w-10 h-10 rounded-full bg-gray-200 flex items-center justify-center">
                        <span class="text-sm font-medium text-gray-600">${Utils.getInitials(s.studentName)}</span>
                    </div>
                    <div class="ml-3">
                        <p class="text-sm font-medium">${Utils.escapeHtml(s.studentName)}</p>
                        <p class="text-xs text-gray-500">${Utils.escapeHtml(s.taskTitle)}</p>
                    </div>
                </div>
                <span class="px-2 py-1 text-xs rounded-full ${Utils.getStatusClass(s.status)}">${I18n.getStatusText(s.status)}</span>
            </div>
        `).join('');

        return `
            <div class="bg-white rounded-lg shadow">
                <div class="px-6 py-4 border-b">
                    <h3 class="text-lg font-semibold">${I18n.t('recentSubmissions')}</h3>
                </div>
                <div class="p-6">
                    <div class="space-y-4">
                        ${items || '<p class="text-gray-500 text-sm">No submissions yet</p>'}
                    </div>
                </div>
            </div>
        `;
    },

    /**
     * Render active tasks panel
     */
    renderActiveTasks(tasks) {
        const items = tasks.map(t => `
            <div class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
                <div>
                    <p class="font-medium">${Utils.escapeHtml(t.title)}</p>
                    <p class="text-sm text-gray-500">${Utils.escapeHtml(t.courseName)}</p>
                </div>
                <div class="text-right">
                    <p class="text-sm font-medium">${t.submissionCount} ${I18n.t('submissionsCount')}</p>
                    <p class="text-xs text-gray-500">${I18n.t('due')}: ${Utils.formatShortDate(t.dueDate)}</p>
                </div>
            </div>
        `).join('');

        return `
            <div class="bg-white rounded-lg shadow">
                <div class="px-6 py-4 border-b">
                    <h3 class="text-lg font-semibold">${I18n.t('activeTasks')}</h3>
                </div>
                <div class="p-6">
                    <div class="space-y-4">
                        ${items || '<p class="text-gray-500 text-sm">No active tasks</p>'}
                    </div>
                </div>
            </div>
        `;
    },

    // Icons
    getDocumentIcon() {
        return `<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
        </svg>`;
    },

    getCheckIcon() {
        return `<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path>
        </svg>`;
    },

    getClockIcon() {
        return `<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path>
        </svg>`;
    },

    getChartIcon() {
        return `<svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 3.055A9.001 9.001 0 1020.945 13H11V3.055z"></path>
        </svg>`;
    }
};
