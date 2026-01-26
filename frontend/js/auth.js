/**
 * Authentication Module
 * Handles user authentication state using sessionStorage
 */
const Auth = {
    STORAGE_KEY: 'auth_user',

    /**
     * Check if user is logged in
     * @returns {boolean}
     */
    isLoggedIn() {
        return sessionStorage.getItem(this.STORAGE_KEY) !== null;
    },

    /**
     * Get current user data
     * @returns {object|null}
     */
    getUser() {
        const data = sessionStorage.getItem(this.STORAGE_KEY);
        return data ? JSON.parse(data) : null;
    },

    /**
     * Login user and store data
     * @param {object} userData - User data to store
     */
    login(userData) {
        sessionStorage.setItem(this.STORAGE_KEY, JSON.stringify(userData));
    },

    /**
     * Logout user and clear data
     */
    logout() {
        sessionStorage.removeItem(this.STORAGE_KEY);
    },

    /**
     * Get user display name
     * @returns {string}
     */
    getDisplayName() {
        const user = this.getUser();
        if (user) {
            return `${user.firstName} ${user.lastName}`;
        }
        return '';
    },

    /**
     * Get user initials for avatar
     * @returns {string}
     */
    getInitials() {
        const user = this.getUser();
        if (user) {
            return `${user.firstName.charAt(0)}${user.lastName.charAt(0)}`.toUpperCase();
        }
        return '';
    }
};
