/**
 * Tasks View
 */
const TasksView = {
    /**
     * Render the tasks view
     * @returns {Promise<string>} - HTML content
     */
    async render() {
        const tasks = await ApiService.getEnrichedTasks();

        return `
            <div class="bg-white rounded-lg shadow">
                <div class="px-6 py-4 border-b flex items-center justify-between">
                    <h3 class="text-lg font-semibold">${I18n.t('programmingTasks')}</h3>
                    <button class="px-4 py-2 bg-primary text-white rounded-lg hover:bg-blue-600 transition" onclick="TaskFormView.openCreate()"> ${I18n.t('newTask')} </button>
                </div>
                <div class="p-6 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    ${this.renderTaskCards(tasks)}
                </div>
            </div>
        `;
    },

    /**
     * Render task cards
     * @param {Array} tasks - Tasks to render
     * @returns {string} - HTML cards
     */
    renderTaskCards(tasks) {
        if (tasks.length === 0) {
            return `
                <div class="col-span-full text-center py-8 text-gray-500">
                    No tasks found
                </div>
            `;
        }

        return tasks.map(t => `
            <div class="border rounded-lg p-4 hover:shadow-md transition ${!t.isActive ? 'opacity-60' : ''}">
                <div class="flex items-start justify-between">
                    <h4 class="font-semibold text-lg">${Utils.escapeHtml(t.name)}</h4>
                    ${!t.isActive ? '<span class="px-2 py-1 text-xs bg-gray-200 rounded">Inactive</span>' : ''}
                </div>
                <p class="text-sm text-gray-500 mb-3">${Utils.escapeHtml(t.courseName)}</p>

                <p class="text-sm text-gray-600 mb-3 line-clamp-2">${Utils.escapeHtml(Utils.truncate(t.description, 100))}</p>

                <div class="flex justify-between text-sm mb-2">
                    <span class="text-gray-600">${t.submissionCount} ${I18n.t('submissionsCount')}</span>
                    <span class="text-gray-600">${t.maxPoints} ${I18n.t('pts')}</span>
                </div>

                <div class="text-xs text-gray-500 mb-3 space-y-1">
                    <div class="flex justify-between">
                        <span>${I18n.t('timeLimit')}:</span>
                        <span>${Utils.formatMs(t.maxExecutionTimeMs)}</span>
                    </div>
                    <div class="flex justify-between">
                        <span>${I18n.t('memoryLimit')}:</span>
                        <span>${Math.round((t.memoryLimitKb ?? 0) / 1024)} ${I18n.t('mb')}</span>
                    </div>
                </div>
                <div class="flex justify-between">
                    <span>Disk:</span>
                    <span>${t.maxDiskUsageMb ?? 0} ${I18n.t('mb')}</span>
                </div>
                <div class="pt-3 border-t">
                    <p class="text-xs text-gray-500">${I18n.t('due')}: ${Utils.formatShortDate(t.dueDate)}</p>
                </div>

                <div class="mt-3 flex space-x-2">
                    <button class="flex-1 px-3 py-1 text-sm border rounded hover:bg-gray-50" onclick="TasksView.viewTask(${t.id})">
                        ${I18n.t('view')}
                    </button>
                    <button class="flex-1 px-3 py-1 text-sm border rounded hover:bg-gray-50" onclick="TasksView.openEdit(${t.id})">
                        ${I18n.t('edit')}
                    </button>
                </div>
            </div>
        `).join('');
    },

    /**
     * View task details
     * @param {number} id - Task ID
     */
    async viewTask(id) {
        const task = await ApiService.getTaskById(id);
        if (!task) {
            alert('Task not found');
            return;
        }

        const testCases = await ApiService.getTestCasesByTaskId(id);
        const publicTests = testCases.filter(tc => tc.isPublic).length;

        alert(`${I18n.t('task')}: ${task.name}\n\n${I18n.t('description')}:\n${task.description}\n\n${I18n.t('tests')}: ${testCases.length} (${publicTests} public)`);
    },

    /**
     * Edit task
     * @param {number} id - Task ID
     */
    editTask(id) {
        //alert(`Edit task #${id}\n\nThis would open an edit form.`);
        TaskFormView.openEdit(id);
    },

    /**
     * Create new task
     */
    createTask() {
        //alert(`${I18n.t('newTask')}\n\nThis would open a create form.`);
        TaskFormView.openCreate();
    }
};
