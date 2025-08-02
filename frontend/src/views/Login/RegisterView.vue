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
                                            placeholder="Full name" v-model="form.name">
                                        <div v-if="errors.name" class="text-danger small">{{ errors.name }}</div>
                                    </div>
                                    <div class="col-sm-6">
                                        <input type="text" class="form-control form-control-user" id="exampleLastName"
                                            placeholder="Username" v-model="form.username">
                                        <div v-if="errors.username" class="text-danger small">{{ errors.username }}</div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <input type="email" class="form-control form-control-user" id="exampleInputEmail"
                                        placeholder="Email Address" v-model="form.email">
                                    <div v-if="errors.email" class="text-danger small">{{ errors.email }}</div>
                                </div>
                              <div class="form-group">
                                <input type="date" class="form-control form-control-user" id="exampleDateOfBirth"
                                       placeholder="Date of birth" v-model="form.dob">
                                <div v-if="errors.dob" class="text-danger small">{{ errors.dob }}</div>
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

                            </form>
                            <hr>
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
import emitter from "@/utils/emitter.js";

export default {
    data() {
        return {
            form: {
                name: '',
                username: '',
                email: '',
                password: '',
                repeatPassword: '',
                dob: ''
            },
            errors: {}
        }
    },
    methods: {
        handleRegister() {
          this.errors = {};
          if (!this.form.name) {
            this.errors.name = 'Full name is required';
          }
          if (!this.form.username) {
            this.errors.username = 'Username is required';
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
          if (!this.form.dob) {
            this.errors.dob = 'Date of birth is required';
          }
          if (Object.keys(this.errors).length === 0) {
              fetch("http://localhost:5222/api/User/register", {
                method: 'POST',
                headers: {
                  "Content-Type": "application/json"
                },
                body: JSON.stringify({
                  username: this.form.username,
                  password: this.form.password,
                  email: this.form.email,
                  name: this.form.name,
                  dob: this.form.dob
                })
              })
                .then(async res => {
                  const responseText = await res.text();
                  if (!res.ok) {
                    emitter.emit("toast", {
                      message: responseText,
                      type: "error"
                    })
                    throw new Error(responseText);
                  } else {
                    emitter.emit("toast", {
                      message: "Registration successful",
                      type: "success"});
                    this.$router.push('/login');
                  }
                })

          }
        }
    }
}
</script>
