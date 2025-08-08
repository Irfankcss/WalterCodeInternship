<template>
<h1 class="text-center my-4">Movies</h1>
  <div class="container">
    <div class="row">
      <FlippingCard
        v-for="movie in filmovi"
        :key="movie.id"
        :movieId="movie.id"
        :imageUrl="movie.poster"
        :title="movie.name"
        :director="movie.director.name"
        :publishedYear="movie.year"
        :imdbRating="movie.imdbRating"
        :description="movie.description"
        :link="movie.link"
        :linkTitle="movie.linkTitle"
      />
    </div>
  </div>
</template>

<script setup>
import FlippingCard from '../components/FlippingCard.vue'
import {onMounted, ref} from "vue";
const filmovi = ref([])
onMounted(() => {
  fetch('http://localhost:5222/api/Movie')
    .then(response => response.json())
    .then(data => filmovi.value = data)
})
</script>

<style scoped>
.container {
  margin-top: 2rem;
}
.row {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}
</style>
 