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
                                            <input type="text" class="form-control form-control-user"
                                                id="exampleInputUsername" aria-describedby="usernameHelp"
                                                placeholder="Enter Username..."
                                                v-model="username">
                                            <div v-if="usernameError" class="text-danger small mt-1">{{
                                                usernameError
                                              }}</div>
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

                                    </form>
                                    <hr>

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
import emitter from "@/utils/emitter.js";
export default {
    data() {
        return {
            username: '',
            password: '',
            rememberMe: false,
            usernameError: '',
            passwordError: ''
        };
    },
    methods: {
        handleLogin() {
            this.usernameError = '';
            this.passwordError = '';
            let valid = true;

            if (!this.username) {
                this.usernameError = 'Username is required.';
                valid = false;
            }

            if (!this.password) {
                this.passwordError = 'Password is required.';
                valid = false;
            }

          if (valid) {
            fetch("http://localhost:5222/api/User/login", {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
              },
              body: JSON.stringify({
                username: this.username,
                password: this.password,
              }),
            })
              .then(async (response) => {
                const data = await response.json();
                if (!response.ok) {
                  throw new Error(data.message || "Invalid credentials");
                } else if(response.status === 200){
                  localStorage.setItem("token", data.token);
                  emitter.emit("userLoggedIn");
                  emitter.emit("toast", {
                    message: "Welcome back!",
                    type: "success",
                  })
                  this.$router.push("/movies");
                }
              })
              .catch((error) => {
                console.log("Toast emit:", error.message);
                emitter.emit("toast", {
                  message: "Invalid login credentials",
                  type: "error",
                });
              });
          }
        }
    }
}
</script>
