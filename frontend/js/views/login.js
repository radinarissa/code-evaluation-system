const LoginView = {
    render() {
        return `
            <div class="fixed inset-0 bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center z-50">
                <div class="max-w-md w-full mx-4">
                    <!-- Login Card -->
                    <div class="bg-white rounded-2xl shadow-2xl p-8 transform transition-all hover:scale-[1.01]">
                        <!-- Logo/Header -->
                        <div class="text-center mb-8">
                            <div class="inline-flex items-center justify-center w-16 h-16 bg-primary rounded-full mb-4">
                                <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 20l4-16m4 4l4 4-4 4M6 16l-4-4 4-4"></path>
                                </svg>
                            </div>
                            <h1 class="text-3xl font-bold text-gray-800 mb-2">Code Evaluator</h1>
                            <p class="text-gray-600">${I18n.t('teacherDashboard')}</p>
                        </div>

                        <!-- Login Form -->
                        <form id="login-form" class="space-y-5">
                            <!-- Username Field -->
                            <div>
                                <label for="username" class="block text-sm font-semibold text-gray-700 mb-2">
                                    ${I18n.t('username')}
                                </label>
                                <div class="relative">
                                    <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <svg class="h-5 w-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path>
                                        </svg>
                                    </div>
                                    <input
                                        type="text"
                                        id="username"
                                        name="username"
                                        class="w-full pl-10 pr-4 py-3 border-2 border-gray-200 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent outline-none transition-all"
                                        placeholder="${I18n.t('enterUsername')}"
                                        required
                                        autocomplete="username"
                                    >
                                </div>
                            </div>

                            <!-- Password Field -->
                            <div>
                                <label for="password" class="block text-sm font-semibold text-gray-700 mb-2">
                                    ${I18n.t('password')}
                                </label>
                                <div class="relative">
                                    <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <svg class="h-5 w-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path>
                                        </svg>
                                    </div>
                                    <input
                                        type="password"
                                        id="password"
                                        name="password"
                                        class="w-full pl-10 pr-12 py-3 border-2 border-gray-200 rounded-lg focus:ring-2 focus:ring-primary focus:border-transparent outline-none transition-all"
                                        placeholder="${I18n.t('enterPassword')}"
                                        required
                                        autocomplete="current-password"
                                    >
                                    <button
                                        type="button"
                                        id="toggle-password"
                                        class="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600 transition-colors"
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

                            <!-- Error Message -->
                            <div id="login-error" class="hidden">
                                <div class="bg-red-50 border-l-4 border-red-500 p-4 rounded-r-lg">
                                    <div class="flex items-center">
                                        <svg class="w-5 h-5 text-red-500 mr-2" fill="currentColor" viewBox="0 0 20 20">
                                            <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clip-rule="evenodd"></path>
                                        </svg>
                                        <p class="text-sm text-red-700 font-medium">${I18n.t('invalidCredentials')}</p>
                                    </div>
                                </div>
                            </div>

                            <!-- Loading State -->
                            <div id="login-loading" class="hidden">
                                <div class="bg-blue-50 border-l-4 border-blue-500 p-4 rounded-r-lg">
                                    <div class="flex items-center">
                                        <svg class="animate-spin h-5 w-5 text-blue-500 mr-2" fill="none" viewBox="0 0 24 24">
                                            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                                            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                                        </svg>
                                        <p class="text-sm text-blue-700 font-medium">Authenticating with Moodle...</p>
                                    </div>
                                </div>
                            </div>

                            <!-- Submit Button -->
                            <button
                                type="submit"
                                id="login-btn"
                                class="w-full bg-gradient-to-r from-blue-500 to-blue-600 hover:from-blue-600 hover:to-blue-700 text-white font-semibold py-3 px-6 rounded-lg shadow-lg transform transition-all duration-150 hover:scale-[1.02] active:scale-[0.98] focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
                            >
                                <span class="flex items-center justify-center">
                                    <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1"></path>
                                    </svg>
                                    ${I18n.t('login')}
                                </span>
                            </button>
                        </form>

                        <!-- Divider -->
                        <div class="relative my-6">
                            <div class="absolute inset-0 flex items-center">
                                <div class="w-full border-t border-gray-200"></div>
                            </div>
                            <div class="relative flex justify-center text-sm">
                                <span class="px-4 bg-white text-gray-500">Moodle Authentication</span>
                            </div>
                        </div>

                        <!-- Language Switcher -->
                        <div class="flex justify-center">
                            <div class="inline-flex rounded-lg border border-gray-200 bg-gray-50 p-1">
                                <button data-lang="bg" class="login-lang-btn px-4 py-2 text-sm font-medium rounded-md transition-all ${I18n.currentLang === 'bg' ? 'bg-white text-primary shadow-sm' : 'text-gray-600 hover:text-gray-900'}">
                                    🇧🇬 BG
                                </button>
                                <button data-lang="en" class="login-lang-btn px-4 py-2 text-sm font-medium rounded-md transition-all ${I18n.currentLang === 'en' ? 'bg-white text-primary shadow-sm' : 'text-gray-600 hover:text-gray-900'}">
                                    🇬🇧 EN
                                </button>
                            </div>
                        </div>
                    </div>

                    <!-- Footer Info -->
                    <div class="text-center mt-6">
                        <p class="text-sm text-gray-600">
                            <svg class="w-4 h-4 inline-block mr-1" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd"></path>
                            </svg>
                            Use your Moodle credentials to login
                        </p>
                    </div>
                </div>
            </div>
        `;
    },

    setupHandlers() {
        const form = document.getElementById('login-form');
        if (form) {
            form.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleLogin();
            });
        }

        const toggleBtn = document.getElementById('toggle-password');
        if (toggleBtn) {
            toggleBtn.addEventListener('click', () => {
                this.togglePasswordVisibility();
            });
        }

        document.querySelectorAll('.login-lang-btn').forEach(btn => {
            btn.addEventListener('click', () => {
                this.switchLoginLanguage(btn.dataset.lang);
            });
        });
    },

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

    async handleLogin() {
        const username = document.getElementById('username').value.trim();
        const password = document.getElementById('password').value;
        const errorDiv = document.getElementById('login-error');
        const loadingDiv = document.getElementById('login-loading');
        const loginBtn = document.getElementById('login-btn');

        errorDiv.classList.add('hidden');
        loadingDiv.classList.remove('hidden');
        loginBtn.disabled = true;
        loginBtn.classList.add('opacity-50', 'cursor-not-allowed');

        try {
            const response = await fetch(`${Config.API_BASE_URL}/auth/moodle/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, password })
            });

            if (response.ok) {
                const userData = await response.json();
                
                Auth.login({
                    username: userData.username,
                    firstName: userData.firstName,
                    lastName: userData.lastName,
                    role: userData.role,
                    moodleId: userData.moodleId
                });
                
                App.showMainApp();
            } else {
                throw new Error('Invalid credentials');
            }
        } catch (error) {
            console.error('Login error:', error);
            
            loadingDiv.classList.add('hidden');
            errorDiv.classList.remove('hidden');
            
            errorDiv.classList.add('animate-pulse');
            setTimeout(() => errorDiv.classList.remove('animate-pulse'), 500);
        } finally {
            loginBtn.disabled = false;
            loginBtn.classList.remove('opacity-50', 'cursor-not-allowed');
        }
    },

    switchLoginLanguage(lang) {
        I18n.setLanguage(lang);

        document.querySelectorAll('.login-lang-btn').forEach(btn => {
            if (btn.dataset.lang === lang) {
                btn.classList.add('bg-white', 'text-primary', 'shadow-sm');
                btn.classList.remove('text-gray-600', 'hover:text-gray-900');
            } else {
                btn.classList.remove('bg-white', 'text-primary', 'shadow-sm');
                btn.classList.add('text-gray-600', 'hover:text-gray-900');
            }
        });

        document.getElementById('login-container').innerHTML = this.render();
        this.setupHandlers();
    }
};