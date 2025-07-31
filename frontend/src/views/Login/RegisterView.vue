<template>
    <div class="container">

        <!-- Outer Row -->
        <div class="row justify-content-center">

            <div class="col-xl-10 col-lg-12 col-md-9">

                <div class="card o-hidden border-0 shadow-lg my-5">
                    <div class="card-body p-0">
                        <!-- Nested Row within Card Body -->
<div class="row">
    <div class="col-lg-5 d-none d-lg-block bg-register-image"></div>
    <div class="col-lg-7">
        <div class="p-5">
                            <div class="text-center">
                                <h1 class="h4 text-gray-900 mb-4">Create an Account!</h1>
                            </div>
                            <form class="user" @submit.prevent="handleRegister">
                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <input type="text" class="form-control form-control-user" id="exampleFirstName"
                                            placeholder="First Name" v-model="form.firstName">
                                        <div v-if="errors.firstName" class="text-danger small">{{ errors.firstName }}</div>
                                    </div>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control form-control-user" id="exampleLastName"
                                            placeholder="Last Name" v-model="form.lastName">
                                        <div v-if="errors.lastName" class="text-danger small">{{ errors.lastName }}</div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <input type="email" class="form-control form-control-user" id="exampleInputEmail"
                                        placeholder="Email Address" v-model="form.email">
                                    <div v-if="errors.email" class="text-danger small">{{ errors.email }}</div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <input type="password" class="form-control form-control-user"
                                            id="exampleInputPassword" placeholder="Password" v-model="form.password">
                                        <div v-if="errors.password" class="text-danger small">{{ errors.password }}</div>
                                    </div>
                                    <div class="col-sm-6">
                                        <input type="password" class="form-control form-control-user"
                                            id="exampleRepeatPassword" placeholder="Repeat Password" v-model="form.repeatPassword">
                                        <div v-if="errors.repeatPassword" class="text-danger small">{{ errors.repeatPassword }}</div>
                                    </div>
                                </div>
                                <button type="submit" class="btn btn-primary btn-user btn-block">
                                    Register Account
                                </button>
                                <hr>
                                <router-link to="/auth/google" class="btn btn-google btn-user btn-block">
                                    <i class="fab fa-google fa-fw"></i> Register with Google
                                </router-link>
                                <router-link to="/auth/facebook" class="btn btn-facebook btn-user btn-block">
                                    <i class="fab fa-facebook-f fa-fw"></i> Register with Facebook
                                </router-link>
                            </form>
                            <hr>
                            <div class="text-center">
                                <a class="small" href="forgot-password">Forgot Password?</a>
                            </div>
                            <div class="text-center">
                                <a class="small" href="login">Already have an account? Login!</a>
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
            form: {
                firstName: '',
                lastName: '',
                email: '',
                password: '',
                repeatPassword: ''
            },
            errors: {}
        }
    },
    methods: {
        handleRegister() {
            this.errors = {};
            if (!this.form.firstName) {
                this.errors.firstName = 'First name is required';
            }
            if (!this.form.lastName) {
                this.errors.lastName = 'Last name is required';
            }
            if (!this.form.email) {
                this.errors.email = 'Email is required';
            } else if (!/\S+@\S+\.\S+/.test(this.form.email)) {
                this.errors.email = 'Email is invalid';
            }
            if (!this.form.password) {
                this.errors.password = 'Password is required';
            }
            if (!this.form.repeatPassword) {
                this.errors.repeatPassword = 'Repeat password is required';
            } else if (this.form.password !== this.form.repeatPassword) {
                this.errors.repeatPassword = 'Passwords do not match';
            }
            if (Object.keys(this.errors).length === 0) {
                // Submit form or call API here
                alert('Registration successful!');
            }
        }
    }
}
</script>