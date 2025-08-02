<script setup>
import { useRouter } from 'vue-router'
import isAdmin from '@/utils/auth'

const router = useRouter()
const sidebarFilteredRoutes = ['Profile', 'Login', 'Register', 'Movie Copies', 'ForgotPassword']

const routes = router.options.routes.filter(route => {
  if (sidebarFilteredRoutes.includes(route.name))
    return false;
  if (route.meta?.requiresAdmin && !isAdmin())
    return false;

  return true;
})
</script>

<template>
  <ul class="navbar-nav sidebar sidebar-dark accordion"
      :style="{ backgroundColor: isAdmin() ? '#0a1c21' : '#4d72dd' }"
      id="accordionSidebar">
    <a class="sidebar-brand d-flex align-items-center justify-content-center" href="/movies">
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
