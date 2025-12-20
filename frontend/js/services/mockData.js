/**
 * Mock Data Service
 * Data structure matches backend entities exactly
 * This file simulates what the API would return
 */
const MockData = {
    // Users - matches User.cs entity
    users: [
        {
            id: 1,
            moodleId: 1001,
            username: 'aivanova',
            email: 'alisa.ivanova@example.com',
            firstName: 'Алиса',
            lastName: 'Иванова',
            role: 'Student',
            createdAt: '2024-09-01T10:00:00',
            updatedAt: '2024-09-01T10:00:00'
        },
        {
            id: 2,
            moodleId: 1002,
            username: 'bpetrov',
            email: 'boris.petrov@example.com',
            firstName: 'Борис',
            lastName: 'Петров',
            role: 'Student',
            createdAt: '2024-09-01T10:00:00',
            updatedAt: '2024-09-01T10:00:00'
        },
        {
            id: 3,
            moodleId: 1003,
            username: 'vdimitrova',
            email: 'vanya.dimitrova@example.com',
            firstName: 'Ваня',
            lastName: 'Димитрова',
            role: 'Student',
            createdAt: '2024-09-01T10:00:00',
            updatedAt: '2024-09-01T10:00:00'
        },
        {
            id: 4,
            moodleId: 1004,
            username: 'gstoyanov',
            email: 'georgi.stoyanov@example.com',
            firstName: 'Георги',
            lastName: 'Стоянов',
            role: 'Student',
            createdAt: '2024-09-01T10:00:00',
            updatedAt: '2024-09-01T10:00:00'
        },
        {
            id: 5,
            moodleId: 1005,
            username: 'enikolova',
            email: 'elena.nikolova@example.com',
            firstName: 'Елена',
            lastName: 'Николова',
            role: 'Student',
            createdAt: '2024-09-01T10:00:00',
            updatedAt: '2024-09-01T10:00:00'
        },
        {
            id: 6,
            moodleId: 1006,
            username: 'fgeorgiev',
            email: 'filip.georgiev@example.com',
            firstName: 'Филип',
            lastName: 'Георгиев',
            role: 'Student',
            createdAt: '2024-09-01T10:00:00',
            updatedAt: '2024-09-01T10:00:00'
        },
        {
            id: 7,
            moodleId: 1007,
            username: 'gtodorova',
            email: 'gabriela.todorova@example.com',
            firstName: 'Габриела',
            lastName: 'Тодорова',
            role: 'Student',
            createdAt: '2024-09-01T10:00:00',
            updatedAt: '2024-09-01T10:00:00'
        },
        {
            id: 8,
            moodleId: 1008,
            username: 'hmarinov',
            email: 'hristo.marinov@example.com',
            firstName: 'Христо',
            lastName: 'Маринов',
            role: 'Student',
            createdAt: '2024-09-01T10:00:00',
            updatedAt: '2024-09-01T10:00:00'
        },
        {
            id: 9,
            moodleId: 2001,
            username: 'ipetrov',
            email: 'ivan.petrov@example.com',
            firstName: 'Иван',
            lastName: 'Петров',
            role: 'Teacher',
            createdAt: '2024-08-01T10:00:00',
            updatedAt: '2024-08-01T10:00:00'
        }
    ],

    // Courses - matches Course.cs entity
    courses: [
        {
            id: 1,
            moodleCourseId: 101,
            name: 'Структури от данни',
            academicYear: '2024/2025',
            semester: 'Зимен',
            createdAt: '2024-09-01T08:00:00',
            updatedAt: '2024-09-01T08:00:00'
        },
        {
            id: 2,
            moodleCourseId: 102,
            name: 'Алгоритми',
            academicYear: '2024/2025',
            semester: 'Зимен',
            createdAt: '2024-09-01T08:00:00',
            updatedAt: '2024-09-01T08:00:00'
        }
    ],

    // Tasks - matches Task.cs entity
    tasks: [
        {
            id: 1,
            courseId: 1,
            title: 'Двоично търсене',
            description: 'Имплементирайте алгоритъм за двоично търсене в сортиран масив.',
            maxPoints: 100,
            timeLimitS: 5000,
            memoryLimitMb: 256,
            diskLimitMb: 256,
            createdBy: 9,
            creationDate: '2024-11-01T10:00:00',
            dueDate: '2024-12-10T23:59:59',
            moodleAssignmentId: 1001,
            moodleAssignmentName: 'Binary Search',
            isActive: true,
            updatedAt: '2024-11-01T10:00:00'
        },
        {
            id: 2,
            courseId: 1,
            title: 'Свързан списък',
            description: 'Имплементирайте едносвързан списък с основни операции.',
            maxPoints: 100,
            timeLimitS: 5000,
            memoryLimitMb: 256,
            diskLimitMb: 256,
            createdBy: 9,
            creationDate: '2024-11-05T10:00:00',
            dueDate: '2024-12-15T23:59:59',
            moodleAssignmentId: 1002,
            moodleAssignmentName: 'Linked List',
            isActive: true,
            updatedAt: '2024-11-05T10:00:00'
        },
        {
            id: 3,
            courseId: 2,
            title: 'Бързо сортиране',
            description: 'Имплементирайте алгоритъм за бързо сортиране (QuickSort).',
            maxPoints: 100,
            timeLimitS: 10000,
            memoryLimitMb: 512,
            diskLimitMb: 256,
            createdBy: 9,
            creationDate: '2024-11-10T10:00:00',
            dueDate: '2024-12-20T23:59:59',
            moodleAssignmentId: 1003,
            moodleAssignmentName: 'Quick Sort',
            isActive: true,
            updatedAt: '2024-11-10T10:00:00'
        }
    ],

    // TestCases - matches TestCase.cs entity
    testCases: [
        // Binary Search test cases
        { id: 1, taskId: 1, name: 'Тест 1 - Основен', input: '[1,2,3,4,5]\n3', expectedOutput: '2', isPublic: true, points: 10, executionOrder: 1, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 2, taskId: 1, name: 'Тест 2 - Първи елемент', input: '[1,2,3,4,5]\n1', expectedOutput: '0', isPublic: true, points: 10, executionOrder: 2, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 3, taskId: 1, name: 'Тест 3 - Последен елемент', input: '[1,2,3,4,5]\n5', expectedOutput: '4', isPublic: false, points: 10, executionOrder: 3, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 4, taskId: 1, name: 'Тест 4 - Не съществува', input: '[1,2,3,4,5]\n6', expectedOutput: '-1', isPublic: false, points: 10, executionOrder: 4, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 5, taskId: 1, name: 'Тест 5 - Празен масив', input: '[]\n1', expectedOutput: '-1', isPublic: false, points: 10, executionOrder: 5, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 6, taskId: 1, name: 'Тест 6 - Голям масив', input: '[1..1000]\n500', expectedOutput: '499', isPublic: false, points: 10, executionOrder: 6, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 7, taskId: 1, name: 'Тест 7 - Един елемент', input: '[5]\n5', expectedOutput: '0', isPublic: false, points: 10, executionOrder: 7, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 8, taskId: 1, name: 'Тест 8 - Дублирани', input: '[1,2,2,2,3]\n2', expectedOutput: '1', isPublic: false, points: 10, executionOrder: 8, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 9, taskId: 1, name: 'Тест 9 - Negative', input: '[-5,-3,-1,0,2]\n-3', expectedOutput: '1', isPublic: false, points: 10, executionOrder: 9, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        { id: 10, taskId: 1, name: 'Тест 10 - Edge case', input: '[1,3]\n2', expectedOutput: '-1', isPublic: false, points: 10, executionOrder: 10, createdAt: '2024-11-01T10:00:00', updatedAt: '2024-11-01T10:00:00' },
        // Linked List test cases
        { id: 11, taskId: 2, name: 'Тест 1 - Insert', input: 'insert 1\ninsert 2\nprint', expectedOutput: '1->2', isPublic: true, points: 20, executionOrder: 1, createdAt: '2024-11-05T10:00:00', updatedAt: '2024-11-05T10:00:00' },
        { id: 12, taskId: 2, name: 'Тест 2 - Delete', input: 'insert 1\ninsert 2\ndelete 1\nprint', expectedOutput: '2', isPublic: true, points: 20, executionOrder: 2, createdAt: '2024-11-05T10:00:00', updatedAt: '2024-11-05T10:00:00' },
        { id: 13, taskId: 2, name: 'Тест 3 - Search', input: 'insert 1\ninsert 2\nsearch 2', expectedOutput: 'true', isPublic: false, points: 20, executionOrder: 3, createdAt: '2024-11-05T10:00:00', updatedAt: '2024-11-05T10:00:00' },
        { id: 14, taskId: 2, name: 'Тест 4 - Empty', input: 'print', expectedOutput: 'empty', isPublic: false, points: 20, executionOrder: 4, createdAt: '2024-11-05T10:00:00', updatedAt: '2024-11-05T10:00:00' },
        { id: 15, taskId: 2, name: 'Тест 5 - Complex', input: 'insert 1\ninsert 2\ninsert 3\ndelete 2\nprint', expectedOutput: '1->3', isPublic: false, points: 20, executionOrder: 5, createdAt: '2024-11-05T10:00:00', updatedAt: '2024-11-05T10:00:00' },
        // Quick Sort test cases
        { id: 16, taskId: 3, name: 'Тест 1 - Basic', input: '[3,1,4,1,5,9,2,6]', expectedOutput: '[1,1,2,3,4,5,6,9]', isPublic: true, points: 12.5, executionOrder: 1, createdAt: '2024-11-10T10:00:00', updatedAt: '2024-11-10T10:00:00' },
        { id: 17, taskId: 3, name: 'Тест 2 - Sorted', input: '[1,2,3,4,5]', expectedOutput: '[1,2,3,4,5]', isPublic: true, points: 12.5, executionOrder: 2, createdAt: '2024-11-10T10:00:00', updatedAt: '2024-11-10T10:00:00' },
        { id: 18, taskId: 3, name: 'Тест 3 - Reverse', input: '[5,4,3,2,1]', expectedOutput: '[1,2,3,4,5]', isPublic: false, points: 12.5, executionOrder: 3, createdAt: '2024-11-10T10:00:00', updatedAt: '2024-11-10T10:00:00' },
        { id: 19, taskId: 3, name: 'Тест 4 - Single', input: '[1]', expectedOutput: '[1]', isPublic: false, points: 12.5, executionOrder: 4, createdAt: '2024-11-10T10:00:00', updatedAt: '2024-11-10T10:00:00' },
        { id: 20, taskId: 3, name: 'Тест 5 - Empty', input: '[]', expectedOutput: '[]', isPublic: false, points: 12.5, executionOrder: 5, createdAt: '2024-11-10T10:00:00', updatedAt: '2024-11-10T10:00:00' },
        { id: 21, taskId: 3, name: 'Тест 6 - Duplicates', input: '[3,3,3,1,1]', expectedOutput: '[1,1,3,3,3]', isPublic: false, points: 12.5, executionOrder: 6, createdAt: '2024-11-10T10:00:00', updatedAt: '2024-11-10T10:00:00' },
        { id: 22, taskId: 3, name: 'Тест 7 - Negative', input: '[-3,1,-5,2]', expectedOutput: '[-5,-3,1,2]', isPublic: false, points: 12.5, executionOrder: 7, createdAt: '2024-11-10T10:00:00', updatedAt: '2024-11-10T10:00:00' },
        { id: 23, taskId: 3, name: 'Тест 8 - Large', input: '[...1000 elements...]', expectedOutput: '[sorted]', isPublic: false, points: 12.5, executionOrder: 8, createdAt: '2024-11-10T10:00:00', updatedAt: '2024-11-10T10:00:00' },
    ],

    // Submissions - matches Submission.cs entity
    submissions: [
        {
            id: 1,
            taskId: 1,
            userId: 1,
            submissionTime: '2024-12-03T14:30:00',
            code: 'def binary_search(arr, target):\n    left, right = 0, len(arr) - 1\n    while left <= right:\n        mid = (left + right) // 2\n        if arr[mid] == target:\n            return mid\n        elif arr[mid] < target:\n            left = mid + 1\n        else:\n            right = mid - 1\n    return -1',
            status: 'Completed',
            finalGrade: 95.00,
            feedback: 'Отлично решение!',
            compilationOutput: null,
            evaluationStartedAt: '2024-12-03T14:30:05',
            evaluationCompletedAt: '2024-12-03T14:30:15',
            moodleSubmissionId: 5001,
            moodleSyncStatus: 'Synced',
            moodleSyncOutput: null,
            moodleSyncCreatedAt: '2024-12-03T14:30:20'
        },
        {
            id: 2,
            taskId: 1,
            userId: 2,
            submissionTime: '2024-12-03T15:45:00',
            code: 'def binary_search(arr, target):\n    for i, v in enumerate(arr):\n        if v == target:\n            return i\n    return -1',
            status: 'Completed',
            finalGrade: 80.00,
            feedback: 'Работи, но не е оптимално O(n) вместо O(log n)',
            compilationOutput: null,
            evaluationStartedAt: '2024-12-03T15:45:05',
            evaluationCompletedAt: '2024-12-03T15:45:12',
            moodleSubmissionId: 5002,
            moodleSyncStatus: 'Synced',
            moodleSyncOutput: null,
            moodleSyncCreatedAt: '2024-12-03T15:45:15'
        },
        {
            id: 3,
            taskId: 2,
            userId: 3,
            submissionTime: '2024-12-04T09:15:00',
            code: 'class Node:\n    def __init__(self, data):\n        self.data = data\n        self.next = None\n\nclass LinkedList:\n    def __init__(self):\n        self.head = None',
            status: 'Processing',
            finalGrade: null,
            feedback: null,
            compilationOutput: null,
            evaluationStartedAt: '2024-12-04T09:15:05',
            evaluationCompletedAt: null,
            moodleSubmissionId: null,
            moodleSyncStatus: null,
            moodleSyncOutput: null,
            moodleSyncCreatedAt: null
        },
        {
            id: 4,
            taskId: 1,
            userId: 4,
            submissionTime: '2024-12-03T16:00:00',
            code: 'def binary_search(arr, target):\n    if target in arr:\n        return arr.index(target)\n    return -1',
            status: 'Completed',
            finalGrade: 60.00,
            feedback: 'Некоректна имплементация за някои случаи',
            compilationOutput: null,
            evaluationStartedAt: '2024-12-03T16:00:05',
            evaluationCompletedAt: '2024-12-03T16:00:10',
            moodleSubmissionId: 5004,
            moodleSyncStatus: 'Synced',
            moodleSyncOutput: null,
            moodleSyncCreatedAt: '2024-12-03T16:00:15'
        },
        {
            id: 5,
            taskId: 2,
            userId: 5,
            submissionTime: '2024-12-04T08:30:00',
            code: 'class Node:\n    def __init__(self, data):\n        self.data = data\n        self.next = None\n\nclass LinkedList:\n    def __init__(self):\n        self.head = None\n    def insert(self, data):\n        new_node = Node(data)\n        if not self.head:\n            self.head = new_node\n        else:\n            current = self.head\n            while current.next:\n                current = current.next\n            current.next = new_node',
            status: 'Completed',
            finalGrade: 100.00,
            feedback: 'Перфектно решение!',
            compilationOutput: null,
            evaluationStartedAt: '2024-12-04T08:30:05',
            evaluationCompletedAt: '2024-12-04T08:30:12',
            moodleSubmissionId: 5005,
            moodleSyncStatus: 'Synced',
            moodleSyncOutput: null,
            moodleSyncCreatedAt: '2024-12-04T08:30:15'
        },
        {
            id: 6,
            taskId: 3,
            userId: 6,
            submissionTime: '2024-12-04T10:00:00',
            code: 'def quicksort(arr):\n    syntax error here',
            status: 'Error',
            finalGrade: null,
            feedback: 'Compilation error',
            compilationOutput: 'SyntaxError: invalid syntax at line 2',
            evaluationStartedAt: '2024-12-04T10:00:05',
            evaluationCompletedAt: '2024-12-04T10:00:06',
            moodleSubmissionId: null,
            moodleSyncStatus: 'Failed',
            moodleSyncOutput: 'Compilation failed',
            moodleSyncCreatedAt: null
        },
        {
            id: 7,
            taskId: 3,
            userId: 7,
            submissionTime: '2024-12-04T10:30:00',
            code: 'def quicksort(arr):\n    if len(arr) <= 1:\n        return arr\n    pivot = arr[len(arr) // 2]\n    left = [x for x in arr if x < pivot]\n    middle = [x for x in arr if x == pivot]\n    right = [x for x in arr if x > pivot]\n    return quicksort(left) + middle + quicksort(right)',
            status: 'Pending',
            finalGrade: null,
            feedback: null,
            compilationOutput: null,
            evaluationStartedAt: null,
            evaluationCompletedAt: null,
            moodleSubmissionId: null,
            moodleSyncStatus: null,
            moodleSyncOutput: null,
            moodleSyncCreatedAt: null
        },
        {
            id: 8,
            taskId: 1,
            userId: 8,
            submissionTime: '2024-12-03T17:00:00',
            code: 'def binary_search(arr, target):\n    left, right = 0, len(arr) - 1\n    while left <= right:\n        mid = (left + right) // 2\n        if arr[mid] == target:\n            return mid\n        elif arr[mid] < target:\n            left = mid + 1\n        else:\n            right = mid - 1\n    return -1',
            status: 'Completed',
            finalGrade: 90.00,
            feedback: 'Много добро решение',
            compilationOutput: null,
            evaluationStartedAt: '2024-12-03T17:00:05',
            evaluationCompletedAt: '2024-12-03T17:00:12',
            moodleSubmissionId: 5008,
            moodleSyncStatus: 'Synced',
            moodleSyncOutput: null,
            moodleSyncCreatedAt: '2024-12-03T17:00:15'
        }
    ],

    // TestResults - matches TestResult.cs entity
    testResults: [
        // Submission 1 (Alisa - Binary Search) - 10/10 passed
        { id: 1, testCaseId: 1, submissionId: 1, status: 'Pass', executionTime: 0.05, memoryUsage: 12.5, diskUsedMb: null, output: '2', errorMessage: null, judge0Token: 'token-001', createdAt: '2024-12-03T14:30:10' },
        { id: 2, testCaseId: 2, submissionId: 1, status: 'Pass', executionTime: 0.04, memoryUsage: 12.3, diskUsedMb: null, output: '0', errorMessage: null, judge0Token: 'token-002', createdAt: '2024-12-03T14:30:10' },
        { id: 3, testCaseId: 3, submissionId: 1, status: 'Pass', executionTime: 0.04, memoryUsage: 12.4, diskUsedMb: null, output: '4', errorMessage: null, judge0Token: 'token-003', createdAt: '2024-12-03T14:30:10' },
        { id: 4, testCaseId: 4, submissionId: 1, status: 'Pass', executionTime: 0.05, memoryUsage: 12.5, diskUsedMb: null, output: '-1', errorMessage: null, judge0Token: 'token-004', createdAt: '2024-12-03T14:30:10' },
        { id: 5, testCaseId: 5, submissionId: 1, status: 'Pass', executionTime: 0.03, memoryUsage: 12.1, diskUsedMb: null, output: '-1', errorMessage: null, judge0Token: 'token-005', createdAt: '2024-12-03T14:30:10' },
        { id: 6, testCaseId: 6, submissionId: 1, status: 'Pass', executionTime: 0.15, memoryUsage: 15.2, diskUsedMb: null, output: '499', errorMessage: null, judge0Token: 'token-006', createdAt: '2024-12-03T14:30:10' },
        { id: 7, testCaseId: 7, submissionId: 1, status: 'Pass', executionTime: 0.03, memoryUsage: 12.0, diskUsedMb: null, output: '0', errorMessage: null, judge0Token: 'token-007', createdAt: '2024-12-03T14:30:10' },
        { id: 8, testCaseId: 8, submissionId: 1, status: 'Pass', executionTime: 0.04, memoryUsage: 12.2, diskUsedMb: null, output: '1', errorMessage: null, judge0Token: 'token-008', createdAt: '2024-12-03T14:30:10' },
        { id: 9, testCaseId: 9, submissionId: 1, status: 'Pass', executionTime: 0.04, memoryUsage: 12.3, diskUsedMb: null, output: '1', errorMessage: null, judge0Token: 'token-009', createdAt: '2024-12-03T14:30:10' },
        { id: 10, testCaseId: 10, submissionId: 1, status: 'Fail', executionTime: 0.04, memoryUsage: 12.2, diskUsedMb: null, output: '1', errorMessage: 'Expected -1, got 1', judge0Token: 'token-010', createdAt: '2024-12-03T14:30:10' },

        // Submission 2 (Boris - Binary Search) - 8/10 passed
        { id: 11, testCaseId: 1, submissionId: 2, status: 'Pass', executionTime: 0.10, memoryUsage: 12.5, diskUsedMb: null, output: '2', errorMessage: null, judge0Token: 'token-011', createdAt: '2024-12-03T15:45:10' },
        { id: 12, testCaseId: 2, submissionId: 2, status: 'Pass', executionTime: 0.08, memoryUsage: 12.3, diskUsedMb: null, output: '0', errorMessage: null, judge0Token: 'token-012', createdAt: '2024-12-03T15:45:10' },
        { id: 13, testCaseId: 3, submissionId: 2, status: 'Pass', executionTime: 0.09, memoryUsage: 12.4, diskUsedMb: null, output: '4', errorMessage: null, judge0Token: 'token-013', createdAt: '2024-12-03T15:45:10' },
        { id: 14, testCaseId: 4, submissionId: 2, status: 'Pass', executionTime: 0.08, memoryUsage: 12.5, diskUsedMb: null, output: '-1', errorMessage: null, judge0Token: 'token-014', createdAt: '2024-12-03T15:45:10' },
        { id: 15, testCaseId: 5, submissionId: 2, status: 'Pass', executionTime: 0.05, memoryUsage: 12.1, diskUsedMb: null, output: '-1', errorMessage: null, judge0Token: 'token-015', createdAt: '2024-12-03T15:45:10' },
        { id: 16, testCaseId: 6, submissionId: 2, status: 'Timeout', executionTime: 5000, memoryUsage: 50.0, diskUsedMb: null, output: null, errorMessage: 'Time limit exceeded', judge0Token: 'token-016', createdAt: '2024-12-03T15:45:10' },
        { id: 17, testCaseId: 7, submissionId: 2, status: 'Pass', executionTime: 0.05, memoryUsage: 12.0, diskUsedMb: null, output: '0', errorMessage: null, judge0Token: 'token-017', createdAt: '2024-12-03T15:45:10' },
        { id: 18, testCaseId: 8, submissionId: 2, status: 'Pass', executionTime: 0.06, memoryUsage: 12.2, diskUsedMb: null, output: '1', errorMessage: null, judge0Token: 'token-018', createdAt: '2024-12-03T15:45:10' },
        { id: 19, testCaseId: 9, submissionId: 2, status: 'Pass', executionTime: 0.06, memoryUsage: 12.3, diskUsedMb: null, output: '1', errorMessage: null, judge0Token: 'token-019', createdAt: '2024-12-03T15:45:10' },
        { id: 20, testCaseId: 10, submissionId: 2, status: 'Fail', executionTime: 0.06, memoryUsage: 12.2, diskUsedMb: null, output: '0', errorMessage: 'Expected -1, got 0', judge0Token: 'token-020', createdAt: '2024-12-03T15:45:10' },

        // Submission 5 (Elena - Linked List) - 5/5 passed
        { id: 21, testCaseId: 11, submissionId: 5, status: 'Pass', executionTime: 0.08, memoryUsage: 13.0, diskUsedMb: null, output: '1->2', errorMessage: null, judge0Token: 'token-021', createdAt: '2024-12-04T08:30:10' },
        { id: 22, testCaseId: 12, submissionId: 5, status: 'Pass', executionTime: 0.07, memoryUsage: 12.8, diskUsedMb: null, output: '2', errorMessage: null, judge0Token: 'token-022', createdAt: '2024-12-04T08:30:10' },
        { id: 23, testCaseId: 13, submissionId: 5, status: 'Pass', executionTime: 0.06, memoryUsage: 12.5, diskUsedMb: null, output: 'true', errorMessage: null, judge0Token: 'token-023', createdAt: '2024-12-04T08:30:10' },
        { id: 24, testCaseId: 14, submissionId: 5, status: 'Pass', executionTime: 0.05, memoryUsage: 12.2, diskUsedMb: null, output: 'empty', errorMessage: null, judge0Token: 'token-024', createdAt: '2024-12-04T08:30:10' },
        { id: 25, testCaseId: 15, submissionId: 5, status: 'Pass', executionTime: 0.09, memoryUsage: 13.2, diskUsedMb: null, output: '1->3', errorMessage: null, judge0Token: 'token-025', createdAt: '2024-12-04T08:30:10' },
    ]
};
