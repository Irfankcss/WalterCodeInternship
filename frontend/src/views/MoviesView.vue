<template>
  <div class="container-fluid">
    <!-- Filter & Sorting Section -->
    <div class="row mb-4">
      <div class="col-md-4">
        <select v-model="selectedGenre" class="form-control bg-light border-0 small">
          <option value="">All genres</option>
          <option v-for="genre in staticGenres" :key="genre" :value="genre">{{ genre }}</option>
        </select>
      </div>

      <div class="col-md-4">
        <input v-model="searchQuery" type="text" class="form-control bg-light border-0 small"
          placeholder="Search movies...">
      </div>

      <div class="col-md-4">
        <select v-model="sortOption" class="form-control bg-light border-0 small">
          <option value="">No sorting</option>
          <option value="title-asc">Title A–Z</option>
          <option value="title-desc">Title Z–A</option>
          <option value="rating-asc">Rating 1–10</option>
          <option value="rating-desc">Rating 10–1</option>
          <option value="director-asc">Director A–Z</option>
          <option value="director-desc">Director Z–A</option>
          <option value="publishedYear-asc">Year Ascending</option>
          <option value="publishedYear-desc">Year Descending</option>
        </select>
      </div>
    </div>

    <!-- Movie Cards Grid -->
    <div class="row">D
      <FlippingCard
        v-for="movie in filmovi"
        :key="movie.id"
        :imageUrl="movie.poster"
        :title="movie.name"
        :director="movie.director.name"
        :publishedYear="movie.year"
        :imdbRating="movie.imdbRating"
        :frontText="movie.desc"
        :backText="movie.backText"
        :link="movie.link"
        :linkTitle="movie.linkTitle"
        class="col-xl-3 col-lg-4 col-md-6 mb-4"
      />
    </div>
  </div>
</template>


<script setup>
import { ref, computed } from 'vue'
import FlippingCard from '../components/FlippingCard.vue'
import {onMounted, ref} from "vue";
const filmovi = ref([])
onMounted(() => {
  fetch('http://localhost:5222/api/Movie')
    .then(response => response.json())
    .then(data => filmovi.value = data)
})
const movies = [
  {
    imdbID: 'tt0111161',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMDFkYTc0MGEtZmNhMC00ZDIzLWFmNTEtODM1ZmRlYWMwMWFmXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg',
    title: 'The Shawshank Redemption',
    director: 'Frank Darabont',
    publishedYear: 1994,
    imdbRating: 9.3,
    frontText: 'A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict.',
    backText: 'Maintaining his innocence and trying to remain hopeful through simple compassion.',
    link: 'https://www.imdb.com/title/tt0111161/',
    linkTitle: 'View on IMDb',
    genre: 'Drama'
  },
  {
    imdbID: 'tt0068646',
    imageUrl: 'https://preview.redd.it/whats-the-most-famous-movie-youve-never-seen-and-really-v0-rw4x7o2pby5d1.jpeg?auto=webp&s=c94d8dcba68000e75702362e0a05e173933e84b9',
    title: 'The Godfather',
    director: 'Francis Ford Coppola',
    publishedYear: 1972,
    imdbRating: 9.2,
    frontText: 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.',
    backText: 'A classic mafia drama about power, family, and betrayal.',
    link: 'https://www.imdb.com/title/tt0068646/',
    linkTitle: 'View on IMDb',
    genre: 'Crime'
  },
  {
    imdbID: 'tt0468569',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_FMjpg_UX1000_.jpg',
    title: 'The Dark Knight',
    director: 'Christopher Nolan',
    publishedYear: 2008,
    imdbRating: 9.0,
    frontText: 'Batman faces the Joker, a criminal mastermind who plunges Gotham into chaos.',
    backText: 'A gripping superhero thriller with legendary performances.',
    link: 'https://www.imdb.com/title/tt0468569/',
    linkTitle: 'View on IMDb',
    genre: 'Action'
  },
  {
    imdbID: 'tt0108052',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BNjM1ZDQxYWUtMzQyZS00MTE1LWJmZGYtNGUyNTdlYjM3ZmVmXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: 'Schindler\'s List',
    director: 'Steven Spielberg',
    publishedYear: 1993,
    imdbRating: 8.9,
    frontText: 'In German-occupied Poland during World War II, Oskar Schindler gradually becomes concerned for his Jewish workforce.',
    backText: 'A powerful story of courage and humanity.',
    link: 'https://www.imdb.com/title/tt0108052/',
    linkTitle: 'View on IMDb',
    genre: 'History'
  },
  {
    imdbID: 'tt0133093',
    imageUrl: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRfSjSWOCaw5dnDL2GT1zFd9RMCgUGw5Q2Cfg&s',
    title: 'The Matrix',
    director: 'Lana Wachowski, Lilly Wachowski',
    publishedYear: 1999,
    imdbRating: 8.7,
    frontText: 'A computer hacker learns about the true nature of his reality and his role in the war against its controllers.',
    backText: 'A revolutionary sci-fi action film.',
    link: 'https://www.imdb.com/title/tt0133093/',
    linkTitle: 'View on IMDb',
    genre: 'Sci-Fi'
  }
]

const staticGenres = ['Drama', 'Crime', 'Action', 'History', 'Sci-Fi']
const selectedGenre = ref('')
const searchQuery = ref('')
const sortOption = ref('')

const filteredMovies = computed(() => {
  return movies.filter(movie => {
    const genreMatch = !selectedGenre.value || movie.genre.toLowerCase() === selectedGenre.value.toLowerCase()
    const searchMatch = !searchQuery.value || movie.title.toLowerCase().includes(searchQuery.value.toLowerCase())
    return genreMatch && searchMatch
  })
})

const sortedMovies = computed(() => {
  const result = [...filteredMovies.value]

  switch (sortOption.value) {
    case 'title-asc':
      return result.sort((a, b) => a.title.localeCompare(b.title))
    case 'title-desc':
      return result.sort((a, b) => b.title.localeCompare(a.title))
    case 'rating-asc':
      return result.sort((a, b) => a.imdbRating - b.imdbRating)
    case 'rating-desc':
      return result.sort((a, b) => b.imdbRating - a.imdbRating)
    case 'director-asc':
      return result.sort((a, b) => a.director.localeCompare(b.director))
    case 'director-desc':
      return result.sort((a, b) => b.director.localeCompare(a.director))
    case 'publishedYear-asc': 
      return result.sort((a, b) => a.publishedYear - b.publishedYear)
    case 'publishedYear-desc':
      return result.sort((a, b) => b.publishedYear - a.publishedYear)
    default:
      return result
  }
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

