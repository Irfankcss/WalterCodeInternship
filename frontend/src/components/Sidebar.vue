<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()
const sidebarFilteredRoutes = ['Profile', 'Login', 'Register', 'Movie Copies', 'ForgotPassword']
const routes = router.options.routes.filter(route => !sidebarFilteredRoutes.includes(route.name))
</script>

<template>
  <ul class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
    <a class="sidebar-brand d-flex align-items-center justify-content-center" href="/dashboard">
      <div class="sidebar-brand-icon rotate-n-15">
        <i class="fas fa-video"></i>
      </div>
      <div class="sidebar-brand-text mx-3">VideoLibrary <sup>0.0.1</sup></div>
    </a>

    <hr class="sidebar-divider my-0">

    <li v-for="route in routes" :key="route.path" class="nav-item">
      <RouterLink v-if="route.name" :to="route.path" class="nav-link">
        <span>{{ route.name }}</span>
      </RouterLink>
    </li>


    <div class="text-center d-none d-md-inline">
      <button class="rounded-circle border-0" id="sidebarToggle"></button>
    </div>
  </ul>
</template>

<script>
export default {
  name: 'Sidebar'
}
</script>

<style scoped>
/* Add any specific styles for the Sidebar component here */
</style>
