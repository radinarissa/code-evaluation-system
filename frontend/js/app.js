/**
 * Main Application
 * Entry point and navigation controller
 */
const App = {
    currentView: 'dashboard',

    /**
     * Initialize the application
     */
    async init() {
        // Set current date
        this.updateDate();

        // Setup navigation
        this.setupNavigation();

        // Setup language switcher
        this.setupLanguageSwitcher();

        // Initialize with default language and view
        this.switchLanguage(Config.DEFAULT_LANGUAGE);
    },

    /**
     * Update the date display
     */
    updateDate() {
        const dateElement = document.getElementById('current-date');
        if (dateElement) {
            dateElement.textContent = new Date().toLocaleDateString(
                Config.DATE_FORMAT[I18n.currentLang],
                { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' }
            );
        }
    },

    /**
     * Setup navigation click handlers
     */
    setupNavigation() {
        document.querySelectorAll('.nav-link').forEach(link => {
            link.addEventListener('click', (e) => {
                e.preventDefault();
                this.navigateTo(link.dataset.view);
            });
        });
    },

    /**
     * Setup language switcher
     */
    setupLanguageSwitcher() {
        document.querySelectorAll('.lang-btn').forEach(btn => {
            btn.addEventListener('click', () => {
                this.switchLanguage(btn.dataset.lang);
            });
        });
    },

    /**
     * Navigate to a view
     * @param {string} view - View name
     */
    async navigateTo(view) {
        const content = document.getElementById('content');
        const pageTitle = document.getElementById('page-title');

        // Show loading state
        content.innerHTML = `
            <div class="flex items-center justify-center h-64">
                <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
            </div>
        `;

        try {
            // Render view
            let html = '';
            switch (view) {
                case 'dashboard':
                    html = await DashboardView.render();
                    break;
                case 'submissions':
                    html = await SubmissionsView.render();
                    break;
                case 'tasks':
                    html = await TasksView.render();
                    break;
                case 'students':
                    html = await StudentsView.render();
                    break;
                default:
                    html = '<p class="text-gray-500">View not found</p>';
            }

            content.innerHTML = html;
            pageTitle.textContent = I18n.t(view);
            this.currentView = view;

            // Setup view-specific handlers
            if (view === 'submissions') {
                SubmissionsView.setupFilters();
            }

            // Update active nav link
            this.updateActiveNavLink(view);

        } catch (error) {
            console.error('Error loading view:', error);
            content.innerHTML = `
                <div class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
                    <strong>Error:</strong> Failed to load view. Please try again.
                </div>
            `;
        }
    },

    /**
     * Update active navigation link styling
     * @param {string} view - Current view
     */
    updateActiveNavLink(view) {
        document.querySelectorAll('.nav-link').forEach(link => {
            if (link.dataset.view === view) {
                link.classList.add('bg-primary', 'text-white');
                link.classList.remove('hover:bg-gray-700', 'text-gray-300');
            } else {
                link.classList.remove('bg-primary', 'text-white');
                link.classList.add('hover:bg-gray-700', 'text-gray-300');
            }
        });
    },

    /**
     * Switch application language
     * @param {string} lang - Language code ('bg' or 'en')
     */
    switchLanguage(lang) {
        I18n.setLanguage(lang);

        // Update language button states
        document.querySelectorAll('.lang-btn').forEach(btn => {
            if (btn.dataset.lang === lang) {
                btn.classList.add('bg-primary', 'text-white');
                btn.classList.remove('text-gray-600', 'hover:bg-gray-200');
            } else {
                btn.classList.remove('bg-primary', 'text-white');
                btn.classList.add('text-gray-600', 'hover:bg-gray-200');
            }
        });

        // Update static elements
        document.getElementById('app-title').textContent = I18n.t('appTitle');
        document.getElementById('app-subtitle').textContent = I18n.t('teacherDashboard');
        document.getElementById('user-role').textContent = I18n.t('teacher');

        // Update navigation labels
        document.querySelectorAll('.nav-link').forEach(link => {
            const view = link.dataset.view;
            const span = link.querySelector('span');
            if (span) {
                span.textContent = I18n.t(view);
            }
        });

        // Update date
        this.updateDate();

        // Re-render current view
        this.navigateTo(this.currentView);
    }
};

// Initialize app when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    App.init();
});
