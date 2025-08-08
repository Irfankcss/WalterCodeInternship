<script setup>
import { ref } from 'vue'
import MovieManager from '../components/admin-movie/MovieManager.vue'
import GenreManager from "@/components/admin-genre/GenreManager.vue";
import ActorManager from "@/components/admin-actor/ActorManager.vue";
import DirectorManager from "@/components/admin-director/DirectorManager.vue";
import UserManager from "@/components/admin-user/UserManager.vue";

const selectedModel = ref('movies')
</script>

<template>
  <div class="container-fluid">
    <ul class="nav nav-tabs mb-4">
      <li class="nav-item">
        <a class="nav-link" :class="{ active: selectedModel === 'movies' }" @click="selectedModel = 'movies'">Movies</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" :class="{ active: selectedModel === 'genres' }" @click="selectedModel = 'genres'">Genres</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" :class="{ active: selectedModel === 'actors' }" @click="selectedModel = 'actors'">Actors</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" :class="{ active: selectedModel === 'directors' }"
          @click="selectedModel = 'directors'">Directors</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" :class="{ active: selectedModel === 'users' }" @click="selectedModel = 'users'">Users</a>
      </li>
    </ul>

    <MovieManager v-if="selectedModel === 'movies'" />
    <GenreManager v-if="selectedModel === 'genres'" />
    <ActorManager v-if="selectedModel === 'actors'" />
    <DirectorManager v-if="selectedModel === 'directors'" />
    <UserManager v-if="selectedModel === 'users'" />
  </div>
</template>
<style scoped>
li {
  cursor: pointer;
}
</style>
