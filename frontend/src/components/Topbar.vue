<template>
  <nav class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">
    <!-- Sidebar Toggle (Topbar) -->
    <button id="sidebarToggleTop" class="btn btn-link d-md-none rounded-circle mr-3">
      <i class="fa fa-bars"></i>
    </button>

    <!-- Topbar Search
    <form class="d-none d-sm-inline-block form-inline mr-auto ml-md-3 my-2 my-md-0 mw-100 navbar-search">
      <div class="input-group">
        <input type="text" class="form-control bg-light border-0 small" placeholder="Search for..." aria-label="Search" aria-describedby="basic-addon2">
        <div class="input-group-append">
          <button class="btn btn-primary" type="button">
            <i class="fas fa-search fa-sm"></i>
          </button>
        </div>
      </div>
    </form>
-->
    <!-- Topbar Navbar -->
    <ul class="navbar-nav ml-auto">
      <!-- Alerts -->
      <li class="nav-item dropdown no-arrow mx-1">
        <a class="nav-link dropdown-toggle" href="#" id="alertsDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <i class="fas fa-bell fa-fw"></i>
          <span class="badge badge-danger badge-counter">3+</span>
        </a>
        <div class="dropdown-list dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="alertsDropdown">
          <h6 class="dropdown-header">Alerts Center</h6>
          <a class="dropdown-item d-flex align-items-center" href="#">
            <div class="mr-3">
              <div class="icon-circle bg-primary">
                <i class="fas fa-file-alt text-white"></i>
              </div>
            </div>
            <div>
              <div class="small text-gray-500">December 12, 2019</div>
              <span class="font-weight-bold">A new monthly report is ready to download!</span>
            </div>
          </a>
          <a class="dropdown-item text-center small text-gray-500" href="#">Show All Alerts</a>
        </div>
      </li>

      <!-- User Info -->
      <li v-if="!isLoggedin" class="nav-item d-flex align-items-center justify-content-center gap-2">
        <button class="btn btn-primary me-2" type="button" @click="$router.push('/login')">Login</button>
        <button class="btn btn-success" type="button" @click="$router.push('/register')">Register</button>
      </li>

      <li v-else class="nav-item dropdown no-arrow">
        <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <span class="mr-2 d-none d-lg-inline text-gray-600 small">{{ username }}</span>
          <img class="img-profile rounded-circle" src="../../static/sbadmin/img/undraw_profile.svg">
        </a>
        <div class="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
          <router-link class="dropdown-item" to="/profile">
            <i class="fas fa-user fa-sm fa-fw mr-2 text-gray-400"></i>
            Profile
          </router-link>
          <a class="dropdown-item" href="#" @click.prevent="logout">
            <i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>
            Logout
          </a>
        </div>
      </li>
    </ul>
  </nav>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import jwtDecode from "jwt-decode"
import emitter from "@/utils/emitter.js"

const username = ref("")
const isLoggedin = ref(false)

onMounted(() => {
  isLoggedin.value = !!localStorage.getItem('token')
  if (isLoggedin.value) {
    const token = localStorage.getItem('token')
    const decodedToken = jwtDecode(token)
    username.value = decodedToken.unique_name
  }
})

emitter.on("userLoggedIn", () => {
  const token = localStorage.getItem('token')
  if (token) {
    const decodedToken = jwtDecode(token)
    username.value = decodedToken.unique_name
    isLoggedin.value = true
  }
})

const logout = () => {
  localStorage.removeItem('token')
  isLoggedin.value = false
  window.location.reload()
}
</script>

<style scoped>
.img-profile {
  width: 40px;
  height: 40px;
  object-fit: cover;
}
</style>
