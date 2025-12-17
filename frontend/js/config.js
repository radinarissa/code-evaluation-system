/**
 * Application Configuration
 * Toggle USE_MOCK_DATA to switch between mock data and real API
 */
const Config = {
    // Set to false when API is ready
    USE_MOCK_DATA: false,

    // API Configuration
    API_BASE_URL: 'http://localhost:5218/api',

    // API Endpoints
    ENDPOINTS: {
        USERS: '/users',
        COURSES: '/courses',
        TASKS: '/tasks',
        SUBMISSIONS: '/submissions',
        TEST_CASES: '/testcases',
        TEST_RESULTS: '/testresults',
    },

    // Default language
    DEFAULT_LANGUAGE: 'bg',

    // Date formats
    DATE_FORMAT: {
        bg: 'bg-BG',
        en: 'en-US'
    }
};

// Freeze config to prevent accidental modifications
Object.freeze(Config);
Object.freeze(Config.ENDPOINTS);
Object.freeze(Config.DATE_FORMAT);
