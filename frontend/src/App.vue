<script setup>
import Sidebar from './components/Sidebar.vue'
import Topbar from './components/Topbar.vue'
import Footer from './components/Footer.vue'
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  
    <div id="wrapper" class="d-flex">
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
</style>