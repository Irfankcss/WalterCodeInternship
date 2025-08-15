<template>
  <h1 class="text-center my-4">Movies</h1>
  <div class="filters-row mb-3">
  <input type="text" v-model="searchQuery" placeholder="Search movies..." class="form-control" />

  <select v-model="selectedGenre" class="form-select">
    <option value="">All genres</option>
    <option v-for="genre in genres" :key="genre.id" :value="genre.name">{{ genre.name }}</option>
  </select>

  <select v-model="sortBy" class="form-select">
    <option value="name">Title</option>
    <option value="year">Year</option>
    <option value="rating">Rating</option>
  </select>
</div>

<div class="flipping-cards-container">
      <FlippingCard v-for="movie in filteredMovies" :key="movie.id" :movieId="movie.id" :imageUrl="movie.poster"
        :title="movie.name" :director="movie.director.name" :publishedYear="movie.year" :imdbRating="movie.imdbRating"
        :description="movie.description" :link="movie.link" :linkTitle="movie.linkTitle" />
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
    const movieRes = await fetch('http://localhost:5222/api/Movie');
    const movieData = await movieRes.json();

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

.filters-row {
  display: flex;
  gap: 10px;
  align-items: center;
}

.filters-row .form-control,
.filters-row .form-select {
  flex: 1;
  min-width: 140px;
}

.flipping-cards-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}
</style>