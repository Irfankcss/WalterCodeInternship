<script setup>
import Sidebar from './components/Sidebar.vue'
import Topbar from './components/Topbar.vue'
import Footer from './components/Footer.vue'
import { RouterView, useRoute } from 'vue-router'
import Toast from "@/components/Toast.vue";
const route = useRoute()
</script>

<template>
  <Toast />
  <div v-if="route.name === 'Login' || route.name === 'Register' || route.name === 'ForgotPassword'">
    <RouterView />
  </div>
  <div v-else id="wrapper" class="d-flex">
    <Sidebar />
    <div id="content-wrapper" class="d-flex flex-column w-100">
      <div id="content">
        <Topbar />
        <div class="container-fluid">

          <RouterView />
        </div>
      </div>
      <Footer />
    </div>
  </div>
</template>

<style scoped>
#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}
</style>
