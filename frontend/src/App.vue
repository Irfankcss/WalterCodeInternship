<script setup>
import Sidebar from './components/Sidebar.vue'
import Topbar from './components/Topbar.vue'
import Footer from './components/Footer.vue'
import { RouterLink, RouterView } from 'vue-router'
import Toast from "@/components/Toast.vue";
</script>

<template>
    <div id="wrapper" class="d-flex">

      <Sidebar />
      <div id="content-wrapper" class="d-flex flex-column w-100">
        <div id="content">
          <Topbar />
          <div class="container-fluid">
            <Toast />
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
