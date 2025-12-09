/**
 * Utility Functions
 */
const Utils = {
    /**
     * Get CSS class for submission status
     * @param {string} status - Submission status
     * @returns {string} - Tailwind CSS classes
     */
    getStatusClass(status) {
        const classes = {
            'Completed': 'bg-green-100 text-green-800',
            'Processing': 'bg-blue-100 text-blue-800',
            'Pending': 'bg-yellow-100 text-yellow-800',
            'Error': 'bg-red-100 text-red-800'
        };
        return classes[status] || 'bg-gray-100 text-gray-800';
    },

    /**
     * Get CSS class for grade display
     * @param {number} grade - Grade value
     * @returns {string} - Tailwind CSS class
     */
    getGradeClass(grade) {
        if (grade >= 70) return 'text-green-600';
        if (grade >= 50) return 'text-yellow-600';
        return 'text-red-600';
    },

    /**
     * Get initials from a name
     * @param {string} name - Full name
     * @returns {string} - Initials (max 2 characters)
     */
    getInitials(name) {
        if (!name) return '??';
        return name.split(' ')
            .map(n => n[0])
            .join('')
            .substring(0, 2)
            .toUpperCase();
    },

    /**
     * Format a date string for display
     * @param {string} dateString - ISO date string
     * @returns {string} - Formatted date
     */
    formatDate(dateString) {
        if (!dateString) return '-';
        return I18n.formatDateTime(dateString);
    },

    /**
     * Format a short date (no time)
     * @param {string} dateString - ISO date string
     * @returns {string} - Formatted date
     */
    formatShortDate(dateString) {
        if (!dateString) return '-';
        return I18n.formatDate(dateString);
    },

    /**
     * Debounce a function
     * @param {Function} func - Function to debounce
     * @param {number} wait - Wait time in ms
     * @returns {Function} - Debounced function
     */
    debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    },

    /**
     * Escape HTML to prevent XSS
     * @param {string} str - String to escape
     * @returns {string} - Escaped string
     */
    escapeHtml(str) {
        if (!str) return '';
        const div = document.createElement('div');
        div.textContent = str;
        return div.innerHTML;
    },

    /**
     * Truncate text with ellipsis
     * @param {string} text - Text to truncate
     * @param {number} maxLength - Maximum length
     * @returns {string} - Truncated text
     */
    truncate(text, maxLength = 50) {
        if (!text || text.length <= maxLength) return text;
        return text.substring(0, maxLength) + '...';
    },

    /**
     * Format bytes to human readable
     * @param {number} bytes - Bytes
     * @returns {string} - Formatted string
     */
    formatBytes(bytes) {
        if (bytes === 0) return '0 B';
        const k = 1024;
        const sizes = ['B', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    },

    /**
     * Format milliseconds to human readable
     * @param {number} ms - Milliseconds
     * @returns {string} - Formatted string
     */
    formatMs(ms) {
        if (ms < 1000) return `${ms}ms`;
        return `${(ms / 1000).toFixed(2)}s`;
    }
};
