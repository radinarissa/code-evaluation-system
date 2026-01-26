/**
 * Login View
 * Handles user authentication via backend API
 */
const LoginView = {

    /**
     * Render the login form
     * @returns {string} HTML string
     */
    render() {
        return `
            <div class="min-h-screen bg-gray-100 flex items-center justify-center">
                <div class="max-w-md w-full bg-white rounded-lg shadow-lg p-8">
                    <div class="text-center mb-8">
                        <h1 class="text-3xl font-bold text-gray-800">Code Evaluator</h1>
                        <p class="text-gray-600 mt-2">${I18n.t('teacherDashboard')}</p>
                    </div>

                    <form id="login-form" class="space-y-6">
                        <div>
                            <label for="username" class="block text-sm font-medium text-gray-700 mb-2">
                                ${I18n.t('username')}
                            </label>
                            <input
                                type="text"
                                id="username"
                                name="username"
                                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent outline-none transition"
                                placeholder="${I18n.t('enterUsername')}"
                                required
                                autocomplete="username"
                            >
                        </div>

                        <div>
                            <label for="password" class="block text-sm font-medium text-gray-700 mb-2">
                                ${I18n.t('password')}
                            </label>
                            <div class="relative">
                                <input
                                    type="password"
                                    id="password"
                                    name="password"
                                    class="w-full px-4 py-2 pr-10 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent outline-none transition"
                                    placeholder="${I18n.t('enterPassword')}"
                                    required
                                    autocomplete="current-password"
                                >
                                <button
                                    type="button"
                                    id="toggle-password"
                                    class="absolute inset-y-0 right-0 flex items-center px-3 text-gray-500 hover:text-gray-700"
                                >
                                    <svg id="eye-icon" class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"></path>
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"></path>
                                    </svg>
                                    <svg id="eye-off-icon" class="w-5 h-5 hidden" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21"></path>
                                    </svg>
                                </button>
                            </div>
                        </div>

                        <div id="login-error" class="hidden bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded text-sm">
                            ${I18n.t('invalidCredentials')}
                        </div>

                        <button
                            type="submit"
                            class="w-full bg-primary text-white py-2 px-4 rounded-lg hover:bg-blue-600 transition font-medium"
                        >
                            ${I18n.t('login')}
                        </button>
                    </form>

                    <!-- Language Switcher -->
                    <div class="mt-6 flex justify-center">
                        <div class="flex items-center space-x-1 bg-gray-100 rounded-lg p-1">
                            <button data-lang="bg" class="login-lang-btn px-3 py-1 text-sm rounded-md ${I18n.currentLang === 'bg' ? 'bg-primary text-white' : 'text-gray-600 hover:bg-gray-200'} transition">
                                BG
                            </button>
                            <button data-lang="en" class="login-lang-btn px-3 py-1 text-sm rounded-md ${I18n.currentLang === 'en' ? 'bg-primary text-white' : 'text-gray-600 hover:bg-gray-200'} transition">
                                EN
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;
    },

    /**
     * Setup login form handlers
     */
    setupHandlers() {
        const form = document.getElementById('login-form');
        if (form) {
            form.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleLogin();
            });
        }

        // Setup password toggle
        const toggleBtn = document.getElementById('toggle-password');
        if (toggleBtn) {
            toggleBtn.addEventListener('click', () => {
                this.togglePasswordVisibility();
            });
        }

        // Setup language switcher on login page
        document.querySelectorAll('.login-lang-btn').forEach(btn => {
            btn.addEventListener('click', () => {
                this.switchLoginLanguage(btn.dataset.lang);
            });
        });
    },

    /**
     * Toggle password visibility
     */
    togglePasswordVisibility() {
        const passwordInput = document.getElementById('password');
        const eyeIcon = document.getElementById('eye-icon');
        const eyeOffIcon = document.getElementById('eye-off-icon');

        if (passwordInput.type === 'password') {
            passwordInput.type = 'text';
            eyeIcon.classList.add('hidden');
            eyeOffIcon.classList.remove('hidden');
        } else {
            passwordInput.type = 'password';
            eyeIcon.classList.remove('hidden');
            eyeOffIcon.classList.add('hidden');
        }
    },

    /**
     * Handle login form submission
     */
    async handleLogin() {
        const username = document.getElementById('username').value.trim();
        const password = document.getElementById('password').value;
        const errorDiv = document.getElementById('login-error');
        const submitBtn = document.querySelector('#login-form button[type="submit"]');

        // Disable button during request
        submitBtn.disabled = true;
        submitBtn.textContent = I18n.t('loggingIn') || 'Logging in...';

        try {
            const response = await fetch(`${Config.API_BASE_URL}${Config.ENDPOINTS.AUTH_LOGIN}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                credentials: 'include',
                body: JSON.stringify({ username, password })
            });

            const data = await response.json();

            if (response.ok && data.success) {
                // Success - store auth state and redirect to app
                Auth.login({
                    id: data.user.id,
                    username: data.user.username,
                    firstName: data.user.firstName,
                    lastName: data.user.lastName,
                    role: data.user.role
                });
                App.showMainApp();
            } else {
                // Show error
                errorDiv.textContent = data.error || I18n.t('invalidCredentials');
                errorDiv.classList.remove('hidden');
                errorDiv.classList.add('animate-pulse');
                setTimeout(() => errorDiv.classList.remove('animate-pulse'), 500);
            }
        } catch (error) {
            console.error('Login error:', error);
            errorDiv.textContent = I18n.t('connectionError') || 'Connection error. Please try again.';
            errorDiv.classList.remove('hidden');
            errorDiv.classList.add('animate-pulse');
            setTimeout(() => errorDiv.classList.remove('animate-pulse'), 500);
        } finally {
            // Re-enable button
            submitBtn.disabled = false;
            submitBtn.textContent = I18n.t('login');
        }
    },

    /**
     * Switch language on login page
     * @param {string} lang - Language code
     */
    switchLoginLanguage(lang) {
        I18n.setLanguage(lang);

        // Update language buttons
        document.querySelectorAll('.login-lang-btn').forEach(btn => {
            if (btn.dataset.lang === lang) {
                btn.classList.add('bg-primary', 'text-white');
                btn.classList.remove('text-gray-600', 'hover:bg-gray-200');
            } else {
                btn.classList.remove('bg-primary', 'text-white');
                btn.classList.add('text-gray-600', 'hover:bg-gray-200');
            }
        });

        // Re-render login page
        document.getElementById('login-container').innerHTML = this.render();
        this.setupHandlers();
    }
};
