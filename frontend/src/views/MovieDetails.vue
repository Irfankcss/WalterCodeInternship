<template>
  <div class="movie-details-container" v-if="movie">
    <div class="movie-header">
      <div class="container">
        <div class="row">
          <div class="col-md-4">
            <img :src="movie.imageUrl" :alt="'Poster za ' + movie.title" class="movie-poster">
          </div>
          <div class="col-md-8 movie-info">
            <h1 class="movie-title">{{ movie.title }}</h1>
            <p class="movie-director">{{ movie.director }} ({{ movie.publishedYear }})</p>

            <div class="movie-meta">
              <div class="meta-item">
                <span class="meta-label">IMDb ocena:</span>
                <span class="meta-value">{{ movie.imdbRating }}/10</span>
              </div>
              <div class="meta-item">
                <span class="meta-label">Žanr:</span>
                <span class="meta-value">{{ movie.genre }}</span>
              </div>
              <div class="meta-item">
                <span class="meta-label">Trajanje:</span>
                <span class="meta-value">142 min</span>
              </div>
            </div>

            <p class="movie-description">{{ movie.backText }}</p>

            <div class="action-buttons mb-3">
              <button class="btn btn-rent" @click="toggleShowCopies">
                {{ showCopies ? 'Otkaži' : 'Iznajmi film' }}
              </button>
              <button class="btn btn-favorite" :class="{ active: isFavorite }" @click="toggleFavorite">
                {{ isFavorite ? '★ Ukloni iz omiljenih' : '☆ Dodaj u omiljene' }}
              </button>
              <router-link to="/movies" class="btn btn-back">
                Nazad na listu filmova
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- BLOK ZA IZNAJMLJIVANJE KOPIJA (premješten ovdje) -->
    <div v-if="showCopies && copies.length" class="container mt-4">
      <h5>Odaberite kopiju i datum vraćanja (maksimalno 14 dana):</h5>

      <div class="d-flex flex-wrap gap-3">
        <div v-for="copy in copies" :key="copy.id" class="card"
          :class="{ 'border-primary': selectedCopyId === copy.id }"
          style="width: 18rem; cursor: pointer;" @click="selectedCopyId = copy.id">
          <div class="card-body">
            <h5 class="card-title">Serial Number: {{ copy.serialNumber }}</h5>
            <p class="card-text">{{ copy.description || 'No description.' }}</p>
          </div>
        </div>
      </div>

      <div class="mt-3">
        <label>Datum vraćanja:</label>
        <input type="date" v-model="returnDate" :min="today" :max="maxReturnDate"
          class="form-control w-auto d-inline-block ms-2">
        <button class="btn btn-success ms-3" @click="confirmRent"
          :disabled="!selectedCopyId || !returnDate">
          Potvrdi iznajmljivanje
        </button>
      </div>
    </div>
    <!-- KRAJ BLOKA ZA KOPIJE -->

    <div class="movie-content container">
      <div class="row">
        <div class="col-md-8">
          <section class="movie-section">
            <h2 class="section-title">Opis</h2>
            <p>{{ movie.frontText }}</p>
          </section>

          <section class="movie-section">
            <h2 class="section-title">Glumci</h2>
            <div class="cast-grid">
              <div class="cast-member" v-for="actor in mainCast" :key="actor">
                <div class="actor-avatar"></div>
                <span class="actor-name">{{ actor }}</span>
              </div>
            </div>
          </section>

          <section class="movie-section">
            <h2 class="section-title">Komentari i ocjene</h2>
            <div class="mb-3">
              <textarea v-model="newReview.comment" class="form-control mb-2"
                placeholder="Napišite komentar..."></textarea>
              <select v-model.number="newReview.rating" class="form-control mb-2">
                <option disabled value="">Ocijeni (1-5)</option>
                <option v-for="n in 5" :key="n" :value="n">{{ n }}</option>
              </select>
              <button class="btn btn-success" @click="submitReview">Dodaj recenziju</button>
            </div>

            <div v-if="movie.reviews.length">
              <div v-for="(review, index) in movie.reviews" :key="index" class="mb-3 border p-3 rounded">
                <p class="mb-1"><strong>Ocjena:</strong> {{ review.rating }}/5</p>
                <p class="mb-0"><em>{{ review.comment }}</em></p>
              </div>
            </div>
            <div v-else class="text-muted">Još nema komentara za ovaj film.</div>
          </section>
        </div>

        <div class="col-md-4">
          <section class="movie-section">
            <h2 class="section-title">Ocene</h2>
            <div class="rating-box">
              <div class="rating-value">{{ movie.imdbRating }}</div>
              <div class="rating-stars">
                <span class="star" v-for="n in 5" :key="n">★</span>
              </div>
              <p class="rating-source">IMDb ocena</p>
            </div>
          </section>
        </div>
      </div>
    </div>
  </div>

  <div v-else class="container mt-4">
    <div class="alert alert-warning">Film nije pronađen</div>
  </div>
</template>



<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import { useUserStore } from '@/stores/userStore';

const userStore = useUserStore();
const route = useRoute();
const imdbID = route.params.imdbID;
const movie = ref(null);
const newReview = ref({ comment: '', rating: '' });

// DODANO: seed podaci za kopije
const allMovieCopies = [
  { id: 1, imdbID: 'tt0111161', serialNumber: "SN001", description: "Blu-ray copy" },
  { id: 2, imdbID: 'tt0111161', serialNumber: "SN002", description: "DVD copy" },
  { id: 3, imdbID: 'tt0068646', serialNumber: "SN003", description: "Collector's edition" },
  { id: 4, imdbID: 'tt0468569', serialNumber: "SN004", description: "4K UHD" },
  { id: 5, imdbID: 'tt0108052', serialNumber: "SN005", description: "Digital release" }
];

const selectedCopyId = ref(null)
const returnDate = ref(null)

const today = new Date().toISOString().split('T')[0]
const maxReturnDate = new Date(Date.now() + 14 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]


const copies = ref([]);
const showCopies = ref(false);
const toggleShowCopies = () => {
  showCopies.value = !showCopies.value;
  selectedCopyId.value = null;
  returnDate.value = null;
};

const confirmRent = () => {
  const copy = copies.value.find(c => c.id === selectedCopyId.value)
  if (copy && returnDate.value) {
    alert(`DEMO: Izabrana kopija '${copy.serialNumber}' iznajmljena do ${returnDate.value}`)
  }
}

const savedFavorites = localStorage.getItem('favorites');
if (savedFavorites) {
  userStore.user.favoriteMovies = JSON.parse(savedFavorites);
}

const isFavorite = computed(() => {
  return movie.value && userStore.user.favoriteMovies.includes(movie.value.imdbID);
});

const toggleFavorite = () => {
  if (movie.value) {
    userStore.toggleFavorite(movie.value.imdbID);
    localStorage.setItem('favorites', JSON.stringify(userStore.user.favoriteMovies));
  }
};

const allMovies = [
  {
    id: 1,
    imdbID: 'tt0111161',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMDFkYTc0MGEtZmNhMC00ZDIzLWFmNTEtODM1ZmRlYWMwMWFmXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_SX300.jpg',
    title: 'The Shawshank Redemption',
    director: 'Frank Darabont',
    publishedYear: 1994,
    imdbRating: 9.3,
    frontText: 'A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict.',
    backText: 'Maintaining his innocence and trying to remain hopeful through simple compassion.',
    genre: 'Drama',
    reviews: []
  },
  {
    id: 2,
    imdbID: 'tt0068646',
    imageUrl: 'https://preview.redd.it/whats-the-most-famous-movie-youve-never-seen-and-really-v0-rw4x7o2pby5d1.jpeg?auto=webp&s=c94d8dcba68000e75702362e0a05e173933e84b9',
    title: 'The Godfather',
    director: 'Francis Ford Coppola',
    publishedYear: 1972,
    imdbRating: 9.2,
    frontText: 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.',
    backText: 'A classic mafia drama about power, family, and betrayal.',
    genre: 'Crime',
    reviews: []
  },
  {
    id: 3,
    imdbID: 'tt0468569',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_FMjpg_UX1000_.jpg',
    title: 'The Dark Knight',
    director: 'Christopher Nolan',
    publishedYear: 2008,
    imdbRating: 9.0,
    frontText: 'Batman faces the Joker, a criminal mastermind who plunges Gotham into chaos.',
    backText: 'A gripping superhero thriller with legendary performances.',
    genre: 'Action',
    reviews: []
  },
  {
    id: 4,
    imdbID: 'tt0108052',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BNjM1ZDQxYWUtMzQyZS00MTE1LWJmZGYtNGUyNTdlYjM3ZmVmXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: "Schindler's List",
    director: 'Steven Spielberg',
    publishedYear: 1993,
    imdbRating: 8.9,
    frontText: 'In German-occupied Poland during World War II, Oskar Schindler gradually becomes concerned for his Jewish workforce.',
    backText: 'A powerful story of courage and humanity.',
    genre: 'History',
    reviews: []
  },
  {
    id: 5,
    imdbID: 'tt0133093',
    imageUrl: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRfSjSWOCaw5dnDL2GT1zFd9RMCgUGw5Q2Cfg&s',
    title: 'The Matrix',
    director: 'Lana Wachowski, Lilly Wachowski',
    publishedYear: 1999,
    imdbRating: 8.7,
    frontText: 'A computer hacker learns about the true nature of his reality and his role in the war against its controllers.',
    backText: 'A revolutionary sci-fi action film.',
    genre: 'Sci-Fi',
    reviews: []
  }
];

const mainCast = ref([
  'Tim Robbins',
  'Morgan Freeman',
  'Bob Gunton',
  'William Sadler',
  'Clancy Brown'
]);

// Dodavanje komentara/ocene i snimanje u localStorage
const submitReview = () => {
  if (!newReview.value.comment || !newReview.value.rating) {
    alert('Molimo unesite komentar i ocjenu.');
    return;
  }

  movie.value.reviews.push({
    comment: newReview.value.comment,
    rating: newReview.value.rating
  });

  localStorage.setItem(`reviews_${movie.value.imdbID}`, JSON.stringify(movie.value.reviews));

  newReview.value.comment = '';
  newReview.value.rating = '';
};

const handleRent = () => {
  if (movie.value) {
    alert(`DEMO: Film "${movie.value.title}" je iznajmljen`);
  }
};

onMounted(() => {
  movie.value = allMovies.find(m => m.imdbID === imdbID);

  if (movie.value) {
    copies.value = allMovieCopies.filter(c => c.imdbID === movie.value.imdbID);
  }

  const storedReviews = localStorage.getItem(`reviews_${imdbID}`);
  if (storedReviews && movie.value) {
    movie.value.reviews = JSON.parse(storedReviews);
  }
});

</script>
<style scoped>
.movie-details-container {
  background-color: #f8f9fa;
  min-height: 100vh;
}

.movie-header {
  background: linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7)),
    url('https://via.placeholder.com/1920x400') center/cover;
  color: white;
  padding: 3rem 0;
}

.movie-poster {
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(17, 17, 17, 0.251);
}

.movie-info {
  padding-left: 2rem;
}

.movie-title {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.movie-director {
  font-size: 1.2rem;
  opacity: 0.8;
  margin-bottom: 1.5rem;
}

.movie-meta {
  display: flex;
  gap: 2rem;
  margin-bottom: 1.5rem;
}

.meta-item {
  display: flex;
  flex-direction: column;
}

.meta-label {
  font-size: 0.9rem;
  opacity: 0.7;
}

.meta-value {
  font-size: 1.1rem;
  font-weight: 600;
}

.movie-description {
  font-size: 1.1rem;
  line-height: 1.6;
  margin-bottom: 2rem;
}

.action-buttons {
  display: flex;
  gap: 1rem;
}

.btn {
  padding: 0.6rem 1.2rem;
  border-radius: 4px;
  font-weight: 600;
  transition: all 0.3s;
}

.btn-rent {
  background-color: #007bff;
  color: white;
  border: none;
}

.btn-rent:hover {
  background-color: #0069d9;
}

.btn-favorite {
  background-color: #ffc107;
  color: #000;
  border: none;
}

.btn-favorite:hover {
  background-color: #e0a800;
}

.btn-favorite.active {
  background-color: #dc3545;
  color: white;
}

.btn-favorite.active:hover {
  background-color: #c82333;
}

.btn-back {
  background-color: transparent;
  color: white;
  border: 1px solid white;
}

.btn-back:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

.movie-content {
  padding: 3rem 0;
}

.movie-section {
  margin-bottom: 3rem;
}

.section-title {
  font-size: 1.5rem;
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #dee2e6;
}

.cast-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
  gap: 1.5rem;
}

.cast-member {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.actor-avatar {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background-color: #ddd;
  margin-bottom: 0.5rem;
}

.actor-name {
  text-align: center;
  font-weight: 500;
}

.rating-box {
  background-color: white;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  text-align: center;
  margin-bottom: 1.5rem;
}

.rating-value {
  font-size: 2.5rem;
  font-weight: 700;
  color: #007bff;
}

.rating-stars {
  color: #ffc107;
  font-size: 1.5rem;
  margin: 0.5rem 0;
}

.rating-source {
  font-size: 0.9rem;
  color: #6c757d;
}

.btn-rate {
  width: 100%;
  background-color: #28a745;
  color: white;
  border: none;
}

.btn-rate:hover {
  background-color: #218838;
}
</style>
