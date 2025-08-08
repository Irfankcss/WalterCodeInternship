<template>
  <h1 class="text-center my-4">Movies</h1>
  <div class="container">
    <div class="row">
      <input type="text" v-model="searchQuery" placeholder="Search movies..." class="form-control mb-3" />

      <select v-model="selectedGenre" class="form-control mb-3">
        <option value="">All genres</option>
        <option v-for="genre in genres" :key="genre.id" :value="genre.name">{{ genre.name }}</option>
      </select>

      <select v-model="sortBy" class="form-control mb-3">
        <option value="name">Title</option>
        <option value="year">Year</option>
        <option value="rating">Rating</option>
      </select>

      <FlippingCard
        v-for="movie in filteredMovies"
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
import { ref, computed, onMounted } from 'vue';
import FlippingCard from '../components/FlippingCard.vue';

const searchQuery = ref('');
const sortBy = ref('name');
const selectedGenre = ref('');
const movies = ref([]);
const genres = ref([]);

const filteredMovies = computed(() => {
  let result = movies.value.filter(m => {
    const matchesSearch = m.name?.toLowerCase().includes(searchQuery.value.toLowerCase());
    const matchesGenre = !selectedGenre.value || m.genres?.some(g => g.name === selectedGenre.value);
    return matchesSearch && matchesGenre;
  });

  result.sort((a, b) => {
    if (sortBy.value === 'title') {
      return a.title.localeCompare(b.title);
    }
    if (sortBy.value === 'year') {
      return (b.year || 0) - (a.year || 0);
    }
    if (sortBy.value === 'rating') {
      return (b.imdbRating || 0) - (a.imdbRating || 0);
    }
    return 0;
  });

  return result;
});

onMounted(async () => {
  try {
    // Dohvati filmove
    const movieRes = await fetch('http://localhost:5222/api/Movie');
    const movieData = await movieRes.json();

    // Za svaki film dohvati žanrove
    for (const movie of movieData) {
      try {
        const genreRes = await fetch(`http://localhost:5222/api/Movie/${movie.id}/genres`);
        const genreData = await genreRes.json();
        movie.genres = genreData;
      } catch (err) {
        movie.genres = [];
      }
    }

    movies.value = movieData;

    // Dohvati listu svih žanrova
    const genreRes = await fetch('http://localhost:5222/api/Genre');
    const genreList = await genreRes.json();
    genres.value = genreList;
  } catch (err) {
    console.error('Error loading movies or genres:', err);
  }
});
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