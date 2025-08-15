<template>
  <footer class="sticky-footer" :style="{ backgroundColor: isAdmin() ? '#0a1c21' : '#162146' }">
    <div class="container my-auto">
      <div class="copyright text-center my-auto">
        <span>Copyright &copy; Video Library 2025</span>
      </div>
    </div>
  </footer>
</template>

<script>
import isAdmin from "@/utils/auth.js";

export default {
  name: 'Footer',
  methods: {isAdmin}
}
</script>

<style scoped>
/* Add any specific styles for the footer here */
</style>
