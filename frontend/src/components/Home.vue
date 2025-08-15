<script setup>
import { useRouter } from "vue-router";

const router = useRouter();

function goToMovies() {
  router.push("/movies");
}
</script>

<template>
  <div class="home-wrapper d-flex flex-column justify-content-center align-items-center text-center">
    <h1 class="mb-4">Welcome to Video Library</h1>
    <button class="btn btn-dark btn-lg" @click="goToMovies">
      Browse Movies
    </button>
  </div>
</template>

<style scoped>
:global(.container-fluid),
:global(.container) {
  margin: 0 !important;
  padding: 0 !important;
}

:global(body) {
  margin: 0;
  padding: 0;
}

.home-wrapper {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  text-align: center;
  width: 100vw;
  height: 100vh;
  background:
    linear-gradient(rgba(0,0,0,0.5), rgba(0,0,0,0.5)),
    url("../assets/homebg.jpg") center/cover no-repeat;
}

button {
  margin-bottom: 50px;
}

h1 {
  font-size: 4rem;
  font-weight: bold;
  background-color: rgba(0, 0, 0, 0.6);
  color: white;
  padding: 10px 20px;
  border-radius: 8px;
}
</style>
