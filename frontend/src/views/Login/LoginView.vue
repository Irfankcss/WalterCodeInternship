<template>
    <div class="container">

        <!-- Outer Row -->
        <div class="row justify-content-center">

            <div class="col-xl-10 col-lg-12 col-md-9">

                <div class="card o-hidden border-0 shadow-lg my-5">
                    <div class="card-body p-0">
                        <!-- Nested Row within Card Body -->
                        <div class="row">
                            <div class="col-lg-6 d-none d-lg-block bg-login-image"></div>
                            <div class="col-lg-6">
                                <div class="p-5">
                                    <div class="text-center">
                                        <h1 class="h4 text-gray-900 mb-4">Welcome Back!</h1>
                                    </div>
                                    <form class="user" @submit.prevent="handleLogin">
                                        <div class="form-group">
                                            <input type="email" class="form-control form-control-user"
                                                id="exampleInputEmail" aria-describedby="emailHelp"
                                                placeholder="Enter Email Address..."
                                                v-model="email">
                                            <div v-if="emailError" class="text-danger small mt-1">{{ emailError }}</div>
                                        </div>
                                        <div class="form-group">
                                            <input type="password" class="form-control form-control-user"
                                                id="exampleInputPassword" placeholder="Password"
                                                v-model="password">
                                            <div v-if="passwordError" class="text-danger small mt-1">{{ passwordError }}</div>
                                        </div>
                                        <div class="form-group">
                                            <div class="custom-control custom-checkbox small">
                                                <input type="checkbox" class="custom-control-input" id="customCheck" v-model="rememberMe">
                                                <label class="custom-control-label" for="customCheck">Remember
                                                    Me</label>
                                            </div>
                                        </div>
                                        <button type="submit" class="btn btn-primary btn-user btn-block">
                                            Login
                                        </button>
                                        <hr>
                                        <router-link to="/login-google" class="btn btn-google btn-user btn-block">
                                            <i class="fab fa-google fa-fw"></i> Login with Google
                                        </router-link>
                                        <router-link to="/login-facebook" class="btn btn-facebook btn-user btn-block">
                                            <i class="fab fa-facebook-f fa-fw"></i> Login with Facebook
                                        </router-link>
                                    </form>
                                    <hr>
                                    <div class="text-center">
                                        <router-link class="small" to="/forgot-password">Forgot Password?</router-link>
                                    </div>
                                    <div class="text-center">
                                        <router-link class="small" to="/register">Create an Account!</router-link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>

    </div>
</template>

<script>
export default {
    data() {
        return {
            email: '',
            password: '',
            rememberMe: false,
            emailError: '',
            passwordError: ''
        };
    },
    methods: {
        handleLogin() {
            this.emailError = '';
            this.passwordError = '';
            let valid = true;

            // Basic email validation
            if (!this.email) {
                this.emailError = 'Email is required.';
                valid = false;
            } else if (!/\S+@\S+\.\S+/.test(this.email)) {
                this.emailError = 'Email is invalid.';
                valid = false;
            }

            // Basic password validation
            if (!this.password) {
                this.passwordError = 'Password is required.';
                valid = false;
            }

            if (valid) {
                // Proceed with login logic, e.g., API call or navigation
                this.$router.push('/dashboard');
            }
        }
    }
}
</script>