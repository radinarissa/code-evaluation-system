const TaskFormView = {
  mode: 'create',   // 'create' | 'edit'
  taskId: null,
  task: null,

  async openCreate() {
    this.mode = 'create';
    this.taskId = null;
    this.task = {
      name: '',
      description: '',
      maxExecutionTimeMs: 15000,
      memoryLimitKb: 262144,
      maxDiskUsageMb: 256,
      maxPoints: 100,
      testCases: [],
    };

    await App.navigateTo('taskForm');
  },

  async openEdit(id) {
    this.mode = 'edit';
    this.taskId = id;

    const task = await ApiService.getTaskById(id);
    if (!task) {
      alert('Task not found');
      return;
    }
    this.task = task;

    await App.navigateTo('taskForm');
  },

  async render() {
    const t = this.task;
    if (!t) return `<div>Loading...</div>`;

    return `
      <div class="bg-white rounded-lg shadow p-6 max-w-4xl">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-lg font-semibold">
            ${this.mode === 'create' ? 'Create task' : `Edit task #${this.taskId}`}
          </h3>
          <button class="px-3 py-1 border rounded hover:bg-gray-50" onclick="App.navigateTo('tasks')">
            Back
          </button>
        </div>

        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium mb-1">Name</label>
            <input id="task_name" class="w-full border rounded px-3 py-2"
              value="${Utils.escapeHtml(t.name || '')}" />
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">Description</label>
            <textarea id="task_desc" class="w-full border rounded px-3 py-2" rows="5">${Utils.escapeHtml(t.description || '')}</textarea>
            <p class="text-xs text-gray-500 mt-1">
              Note: Moodle descriptions may contain HTML (&lt;p&gt;...).
            </p>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-4 gap-3">
            <div>
              <label class="block text-sm font-medium mb-1">Max points</label>
              <input id="task_points" type="number" class="w-full border rounded px-3 py-2"
                value="${Number(t.maxPoints ?? 0)}" />
            </div>
            <div>
              <label class="block text-sm font-medium mb-1">Time limit (ms)</label>
              <input id="task_time" type="number" class="w-full border rounded px-3 py-2"
                value="${Number(t.maxExecutionTimeMs ?? 0)}" />
            </div>
            <div>
              <label class="block text-sm font-medium mb-1">Memory (KB)</label>
              <input id="task_mem" type="number" class="w-full border rounded px-3 py-2"
                value="${Number(t.memoryLimitKb ?? 0)}" />
            </div>
            <div>
              <label class="block text-sm font-medium mb-1">Disk (MB)</label>
              <input id="task_disk" type="number" class="w-full border rounded px-3 py-2"
                value="${Number(t.maxDiskUsageMb ?? 0)}" />
            </div>
          </div>

          <div class="pt-4 border-t">
            <div class="flex items-center justify-between mb-2">
              <h4 class="font-semibold">Test cases</h4>
              <button class="px-3 py-1 border rounded hover:bg-gray-50" onclick=" TaskFormView.addTestCase()">
                + Add test
              </button>
            </div>

            <div id="tc_list" class="space-y-3">
              ${this.renderTestCases(t.testCases || [])}
            </div>
          </div>

          <div class="pt-4 border-t flex justify-end gap-2">
            <button class="px-4 py-2 border rounded hover:bg-gray-50" onclick="App.navigateTo('tasks')">
              Cancel
            </button>
            <button class="px-4 py-2 bg-primary text-white rounded hover:bg-blue-600"
              onclick="TaskFormView.submit()">
              Save
            </button>
          </div>
        </div>
      </div>
    `;
  },

  renderTestCases(list) {
    if (!list.length) {
      return `<div class="text-sm text-gray-500">No test cases yet.</div>`;
    }

    return list.map((tc, idx) => `
      <div class="border rounded p-3">
        <div class="flex items-center justify-between mb-2">
          <div class="font-semibold">Test #${idx + 1}</div>
          <button class="text-sm px-2 py-1 border rounded hover:bg-gray-50"
            onclick="TaskFormView.removeTestCase(${idx})">Remove</button>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-3">
          <div>
            <label class="block text-xs font-medium mb-1">Name</label>
            <input class="w-full border rounded px-2 py-1" data-tc-name="${idx}"
              value="${Utils.escapeHtml(tc.name || '')}" />
          </div>
          <div>
            <label class="block text-xs font-medium mb-1">Execution order</label>
            <input type="number" class="w-full border rounded px-2 py-1" data-tc-order="${idx}"
              value="${Number(tc.executionOrder ?? (idx + 1))}" />
          </div>
          <div class="flex items-center gap-2 pt-5">
            <input type="checkbox" data-tc-public="${idx}" ${tc.isPublic ? 'checked' : ''} />
            <span class="text-sm">Public (shown to student)</span>
          </div>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-3 mt-3">
          <div>
            <label class="block text-xs font-medium mb-1">Input</label>
            <textarea class="w-full border rounded px-2 py-1" rows="3" data-tc-input="${idx}">${Utils.escapeHtml(tc.input || '')}</textarea>
          </div>
          <div>
            <label class="block text-xs font-medium mb-1">Expected output</label>
            <textarea class="w-full border rounded px-2 py-1" rows="3" data-tc-out="${idx}">${Utils.escapeHtml(tc.expectedOutput || '')}</textarea>
          </div>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-1 gap-3 mt-3">
          <div>
            <label class="block text-xs font-medium mb-1">Points</label>
            <input type="number" class="w-full border rounded px-2 py-1" data-tc-points="${idx}"
              value="${Number(tc.points ?? 0)}" />
          </div>
        </div>
      </div>
    `).join('');
  },

  addTestCase() {
    this.task.testCases = this.task.testCases || [];
    this.task.testCases.push({
      name: `Test ${this.task.testCases.length + 1}`,
      input: '',
      expectedOutput: '',
      isPublic: false,
      executionOrder: this.task.testCases.length + 1,
      points: 0,
    });
    App.renderCurrentView();
  },

  removeTestCase(idx) {
    this.task.testCases.splice(idx, 1);
    App.renderCurrentView();
  },

  collectForm() {
    const t = this.task;

    const name = document.getElementById('task_name').value;
    const description = document.getElementById('task_desc').value;

    const maxPoints = Number(document.getElementById('task_points').value || 0);
    const maxExecutionTimeMs = Number(document.getElementById('task_time').value || 0);
    const memoryLimitKb = Number(document.getElementById('task_mem').value || 0);
    const maxDiskUsageMb = Number(document.getElementById('task_disk').value || 0);

    const testCases = (t.testCases || []).map((_, idx) => ({
      name: document.querySelector(`[data-tc-name="${idx}"]`).value,
      input: document.querySelector(`[data-tc-input="${idx}"]`).value,
      expectedOutput: document.querySelector(`[data-tc-out="${idx}"]`).value,
      isPublic: document.querySelector(`[data-tc-public="${idx}"]`).checked,
      executionOrder: Number(document.querySelector(`[data-tc-order="${idx}"]`).value || (idx + 1)),
      points: Number(document.querySelector(`[data-tc-points="${idx}"]`).value || 0),
    }));

    return {
      ...t,
      name,
      description,
      maxPoints,
      maxExecutionTimeMs,
      memoryLimitKb,
      maxDiskUsageMb,
      testCases,
    };
  },

  async submit() {
    try {
      const payload = this.collectForm();

      const requestDto = {
        title: payload.name,
        description: payload.description,
        maxExecutionTimeMs: payload.maxExecutionTimeMs,
        memoryLimitKb: payload.memoryLimitKb,
        maxDiskUsageMb: payload.maxDiskUsageMb,
        maxPoints: payload.maxPoints,
        testCases: payload.testCases.map(tc => ({
            name: tc.name,
            input: tc.input,
            expectedOutput: tc.expectedOutput,
            isPublic: tc.isPublic,
            executionOrder: tc.executionOrder,
            points: tc.points,
        }))
      };

      if (this.mode === 'create') {
        await ApiService.createTask(requestDto);
      } else {
        await ApiService.updateTask(this.taskId, requestDto);
      }

      await App.navigateTo('tasks');
    } catch (e) {
      console.error(e);
      alert(`Save failed: ${e.message}`);
    }
  }
};
