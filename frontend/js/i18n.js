/**
 * Internationalization (i18n) Module
 * Handles translations for Bulgarian and English
 */
const I18n = {
    currentLang: Config.DEFAULT_LANGUAGE,

    translations: {
        bg: {
            // App
            appTitle: 'Code Evaluator',
            teacherDashboard: 'Табло на преподавателя',

            // Login
            login: 'Вход',
            loggingIn: 'Влизане...',
            username: 'Потребителско име',
            password: 'Парола',
            enterUsername: 'Въведете потребителско име',
            enterPassword: 'Въведете парола',
            invalidCredentials: 'Невалидно потребителско име или парола',
            connectionError: 'Грешка при свързване. Моля, опитайте отново.',
            logout: 'Изход',

            // Navigation
            dashboard: 'Табло',
            submissions: 'Предавания',
            tasks: 'Задачи',
            students: 'Студенти',
            teacher: 'Преподавател',

            // Dashboard
            totalSubmissions: 'Общо предавания',
            completed: 'Завършени',
            pending: 'Изчакващи',
            averageGrade: 'Средна оценка',
            recentSubmissions: 'Последни предавания',
            activeTasks: 'Активни задачи',
            submissionsCount: 'предавания',
            due: 'Краен срок',

            // Submissions
            allSubmissions: 'Всички предавания',
            allTasks: 'Всички задачи',
            allStatuses: 'Всички статуси',
            student: 'Студент',
            task: 'Задача',
            status: 'Статус',
            tests: 'Тестове',
            grade: 'Оценка',
            submitted: 'Предадено',
            actions: 'Действия',
            viewCode: 'Виж код',

            // Statuses
            statusCompleted: 'Завършено',
            statusProcessing: 'Обработва се',
            statusPending: 'Изчакващо',
            statusError: 'Грешка',

            // Tasks
            programmingTasks: 'Програмни задачи',
            newTask: '+ Нова задача',
            view: 'Виж',
            edit: 'Редактирай',
            pts: 'точки',
            description: 'Описание',
            timeLimit: 'Лимит време',
            memoryLimit: 'Лимит памет',
            ms: 'мс',
            mb: 'МБ',

            // Students
            name: 'Име',
            email: 'Имейл',
            avgGrade: 'Средна оценка',
            viewDetails: 'Виж детайли',
            notAvailable: 'Н/Д',
            username: 'Потребител',
            role: 'Роля',

            // Roles
            roleStudent: 'Студент',
            roleTeacher: 'Преподавател',
            roleAdmin: 'Админ',

            // Code viewer
            viewingSubmission: 'Преглед на предаване',

            // Course
            course: 'Курс',
            academicYear: 'Учебна година',
            semester: 'Семестър',

            // Statistics
            statistics: 'Статистика',
            statisticsFor: 'Статистика за',
            generalInfo: 'Обща информация',
            uniqueStudents: 'Уникални студенти',
            avgAttempts: 'Ср. опити/студент',
            avgTimeBetween: 'Ср. време между опити',
            firstSubmission: 'Първо предаване',
            lastSubmission: 'Последно предаване',
            gradeDistribution: 'Разпределение на оценките',
            histogram: 'Хистограма по диапазони',
            medianGrade: 'Медиана',
            minGrade: 'Мин. оценка',
            maxGrade: 'Макс. оценка',
            perfectScore: '% с отличен (6)',
            failingScore: '% със слаб (2)',
            hours: 'ч',
            minutes: 'мин',
            poor: 'Слаб (2)',
            excellent: 'Отличен (6)',
            recentSubmissionsPreview: 'Последни предавания',
            attemptNumber: 'Опит №',
            loadingStatisticsFor: 'Зареждане на статистика за',
            mockDataNote: 'Това са примерни данни'
        },
        en: {
            // App
            appTitle: 'Code Evaluator',
            teacherDashboard: 'Teacher Dashboard',

            // Login
            login: 'Login',
            loggingIn: 'Logging in...',
            username: 'Username',
            password: 'Password',
            enterUsername: 'Enter username',
            enterPassword: 'Enter password',
            invalidCredentials: 'Invalid username or password',
            connectionError: 'Connection error. Please try again.',
            logout: 'Logout',

            // Navigation
            dashboard: 'Dashboard',
            submissions: 'Submissions',
            tasks: 'Tasks',
            students: 'Students',
            teacher: 'Teacher',

            // Dashboard
            totalSubmissions: 'Total Submissions',
            completed: 'Completed',
            pending: 'Pending',
            averageGrade: 'Average Grade',
            recentSubmissions: 'Recent Submissions',
            activeTasks: 'Active Tasks',
            submissionsCount: 'submissions',
            due: 'Due',

            // Submissions
            allSubmissions: 'All Submissions',
            allTasks: 'All Tasks',
            allStatuses: 'All Statuses',
            student: 'Student',
            task: 'Task',
            status: 'Status',
            tests: 'Tests',
            grade: 'Grade',
            submitted: 'Submitted',
            actions: 'Actions',
            viewCode: 'View Code',

            // Statuses
            statusCompleted: 'Completed',
            statusProcessing: 'Processing',
            statusPending: 'Pending',
            statusError: 'Error',

            // Tasks
            programmingTasks: 'Programming Tasks',
            newTask: '+ New Task',
            view: 'View',
            edit: 'Edit',
            pts: 'pts',
            description: 'Description',
            timeLimit: 'Time Limit',
            memoryLimit: 'Memory Limit',
            ms: 'ms',
            mb: 'MB',

            // Students
            name: 'Name',
            email: 'Email',
            avgGrade: 'Avg Grade',
            viewDetails: 'View Details',
            notAvailable: 'N/A',
            username: 'Username',
            role: 'Role',

            // Roles
            roleStudent: 'Student',
            roleTeacher: 'Teacher',
            roleAdmin: 'Admin',

            // Code viewer
            viewingSubmission: 'Viewing submission',

            // Course
            course: 'Course',
            academicYear: 'Academic Year',
            semester: 'Semester',

            // Statistics
            statistics: 'Statistics',
            statisticsFor: 'Statistics for',
            generalInfo: 'General Information',
            uniqueStudents: 'Unique Students',
            avgAttempts: 'Avg Attempts/Student',
            avgTimeBetween: 'Avg Time Between Attempts',
            firstSubmission: 'First Submission',
            lastSubmission: 'Last Submission',
            gradeDistribution: 'Grade Distribution',
            histogram: 'Histogram by Range',
            medianGrade: 'Median Grade',
            minGrade: 'Min Grade',
            maxGrade: 'Max Grade',
            perfectScore: '% Excellent (6)',
            failingScore: '% Failing (2)',
            hours: 'h',
            minutes: 'min',
            poor: 'Failing (2)',
            excellent: 'Excellent (6)',
            recentSubmissionsPreview: 'Recent Submissions',
            attemptNumber: 'Attempt #',
            loadingStatisticsFor: 'Loading statistics for',
            mockDataNote: 'This is mock data'
        }
    },

    /**
     * Get translation for a key
     * @param {string} key - Translation key
     * @returns {string} - Translated string
     */
    t(key) {
        return this.translations[this.currentLang][key]
            || this.translations['en'][key]
            || key;
    },

    /**
     * Get translated status text
     * @param {string} status - Status value from backend
     * @returns {string} - Translated status
     */
    getStatusText(status) {
        const statusMap = {
            'Completed': this.t('statusCompleted'),
            'Processing': this.t('statusProcessing'),
            'Pending': this.t('statusPending'),
            'Error': this.t('statusError')
        };
        return statusMap[status] || status;
    },

    /**
     * Get translated role text
     * @param {string} role - Role value from backend
     * @returns {string} - Translated role
     */
    getRoleText(role) {
        const roleMap = {
            'Student': this.t('roleStudent'),
            'Teacher': this.t('roleTeacher'),
            'Admin': this.t('roleAdmin')
        };
        return roleMap[role] || role;
    },

    /**
     * Set current language
     * @param {string} lang - Language code ('bg' or 'en')
     */
    setLanguage(lang) {
        if (this.translations[lang]) {
            this.currentLang = lang;
        }
    },

    /**
     * Format date according to current language
     * @param {Date|string} date - Date to format
     * @returns {string} - Formatted date string
     */
    formatDate(date) {
        const d = date instanceof Date ? date : new Date(date);
        return d.toLocaleDateString(Config.DATE_FORMAT[this.currentLang], {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
    },

    /**
     * Format datetime according to current language
     * @param {Date|string} date - Date to format
     * @returns {string} - Formatted datetime string
     */
    formatDateTime(date) {
        const d = date instanceof Date ? date : new Date(date);
        return d.toLocaleString(Config.DATE_FORMAT[this.currentLang], {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }
};
