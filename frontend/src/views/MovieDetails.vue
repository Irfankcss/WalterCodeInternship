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
                <span class="meta-label">IMDb rating:</span>
                <span class="meta-value">{{ movie.imdbRating }}/10</span>
              </div>
              <div class="meta-item">
                <span class="meta-label">Genres:</span>
                <span class="meta-value">{{ genre }}</span>
              </div>
              <div class="meta-item">
                <span class="meta-label">Duration:</span>
                <span class="meta-value">142 min</span>
              </div>
            </div>

            <p class="movie-description">{{ movie.backText }}</p>

            <div class="action-buttons mb-3">
              <button class="btn btn-rent" @click="toggleShowCopies">
                {{ showCopies ? 'Cancel' : 'Rent movie' }}
              </button>
              <button class="btn btn-favorite" :class="{ active: isFavorite }" @click="toggleFavorite">
                {{ isFavorite ? '★ Remove from favorites' : '☆ Add to favorites' }}
              </button>
              <router-link to="/movies" class="btn btn-back">
                Back to movie list
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="showCopies && copies.length" class="container mt-4">
      <h5>Choose a copy and return date (max 14 days):</h5>

      <div class="d-flex flex-wrap gap-3">
        <div v-for="copy in copies" :key="copy.id" class="card"
          :class="{ 'border-primary': selectedCopyId === copy.id }" style="width: 18rem; cursor: pointer;"
          @click="selectedCopyId = copy.id">
          <div class="card-body">
            <h5 class="card-title">Serial Number: {{ copy.serialNumber }}</h5>
            <p class="card-text">{{ copy.description || 'No description.' }}</p>
          </div>
        </div>
      </div>

      <div class="mt-3">
        <label>Return date:</label>
        <input type="date" v-model="returnDate" :min="today" :max="maxReturnDate"
          class="form-control w-auto d-inline-block ms-2">
        <button class="btn btn-success ms-3" @click="confirmRent" :disabled="!selectedCopyId || !returnDate">
          Confirm rental
        </button>
      </div>
    </div>

    <div class="movie-content container">
      <div class="row">
        <div class="col-md-8">
          <section class="movie-section">
            <h2 class="section-title">Description</h2>
            <p>{{ movie.frontText }}</p>
          </section>

          <section class="movie-section">
            <h2 class="section-title">Cast</h2>
            <div class="cast-grid">
              <div class="cast-member" v-for="actor in mainCast" :key="actor.name">
                <img :src="actor.image" :alt="actor.name" class="actor-avatar" />
                <span class="actor-name">{{ actor.name }}</span>
              </div>
            </div>
          </section>

          <section class="movie-section">
            <h2 class="section-title">Comments and Ratings</h2>
            <div class="mb-3">
              <textarea v-model="newReview.comment" class="form-control mb-2"
                placeholder="Write a comment..."></textarea>
              <select v-model.number="newReview.rating" class="form-control mb-2">
                <option disabled value="">Rate (1-5)</option>
                <option v-for="n in 5" :key="n" :value="n">{{ n }}</option>
              </select>
              <button class="btn btn-success" @click="submitReview">Add review</button>
            </div>

            <div v-if="movie.reviews.length">
              <div v-for="(review, index) in movie.reviews" :key="index" class="mb-3 border p-3 rounded">
                <p class="mb-1"><strong>Rating:</strong> {{ review.rating }}/5</p>
                <p class="mb-0"><em>{{ review.comment }}</em></p>
              </div>
            </div>
            <div v-else class="text-muted">No comments yet for this movie.</div>
          </section>
        </div>

        <div class="col-md-4">
          <section class="movie-section">
            <h2 class="section-title">Ratings</h2>
            <div class="rating-box">
              <div class="rating-value">{{ movie.imdbRating }}</div>
              <div class="rating-stars">
                <span class="star" v-for="n in 5" :key="n">★</span>
              </div>
              <p class="rating-source">IMDb rating</p>
            </div>
          </section>
        </div>
      </div>
    </div>
  </div>

  <div v-else class="container mt-4">
    <div class="alert alert-warning">Movie not found</div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import { useUserStore } from '@/stores/userStore';

const userStore = useUserStore();
const route = useRoute();
const id = route.params.id;
const praviid = route.params.id;

const movie = ref(null);
const copies = ref([]);
const newReview = ref({ comment: '', rating: '' });

const selectedCopyId = ref(null);
const returnDate = ref(null);
const today = new Date().toISOString().split('T')[0];
const maxReturnDate = new Date(Date.now() + 14 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];

const showCopies = ref(false);
const toggleShowCopies = () => {
  showCopies.value = !showCopies.value;
  selectedCopyId.value = null;
  returnDate.value = null;
};

const activeRentals = computed(() => {
  const todayDate = new Date();
  todayDate.setHours(0, 0, 0, 0);
  return userStore.user.rentals?.filter(r => {
    const rentedDate = new Date(r.rentedUntil);
    rentedDate.setHours(0, 0, 0, 0);
    return rentedDate >= todayDate;
  }) || [];
});

const confirmRent = () => {
  const copy = copies.value.find(c => c.id === selectedCopyId.value);
  if (copy && returnDate.value) {
    if (activeRentals.value.length >= 3) {
      alert("⚠️ You can't rent more than 3 movies at the same time.");
      return;
    }

    const newRental = {
      title: movie.value.name,
      serialNumber: copy.serialNumber,
      rentedUntil: returnDate.value
    };

    userStore.user.rentals.push(newRental);
    localStorage.setItem('rentals', JSON.stringify(userStore.user.rentals));
    window.dispatchEvent(new Event('storage'));
    alert(`✅ Rental confirmed for '${copy.serialNumber}' until ${returnDate.value}`);

    showCopies.value = false;
    selectedCopyId.value = null;
    returnDate.value = null;
  }
};

const savedFavorites = localStorage.getItem('favorites');
if (savedFavorites) {
  userStore.user.favoriteMovies = JSON.parse(savedFavorites);
}

const isFavorite = computed(() => {
  return movie.value && userStore.user.favoriteMovies.includes(movie.value.id);
});

const toggleFavorite = () => {
  if (movie.value) {
    userStore.toggleFavorite(movie.value.id);
    localStorage.setItem('favorites', JSON.stringify(userStore.user.favoriteMovies));
  }
};

const mainCast = ref([
  { name: 'Tim Robbins', image: 'https://ntvb.tmsimg.com/assets/assets/680595_v9_bc.jpg' },
  { name: 'Morgan Freeman', image: 'https://m.media-amazon.com/images/M/MV5BMTc0MDMyMzI2OF5BMl5BanBnXkFtZTcwMzM2OTk1MQ@@._V1_FMjpg_UX1000_.jpg' },
  { name: 'Bob Gunton', image: '...' },
  { name: 'Clancy Brown', image: '...' }
]);

const submitReview = () => {
  if (!newReview.value.comment || !newReview.value.rating) {
    alert('Please enter a comment and rating.');
    return;
  }

  movie.value.reviews.push({
    comment: newReview.value.comment,
    rating: newReview.value.rating
  });

  localStorage.setItem(`reviews_${movie.value.id}`, JSON.stringify(movie.value.reviews));
  newReview.value.comment = '';
  newReview.value.rating = '';
};
const genre = ref('');

onMounted(() => {
  fetch(`http://localhost:5222/api/Movie/${praviid}/genres`)
    .then(async res => {
      if (!res.ok) throw new Error(`API error: ${res.status}`);
      const text = await res.text();
      return text ? JSON.parse(text) : {};
    })
    .then(data => {
      genre.value = data.map(g => g.name).join(', ');
    });

  fetch(`http://localhost:5222/api/Movie/${id}`)
    .then(res => res.json())
    .then(data => {
      movie.value = {
        ...data,
        title: data.name,
        imageUrl: data.poster,
        publishedYear: data.year,
        backText: data.backText || '',
        frontText: data.frontText || '',
        genre: data.genre || '',
        imdbRating: data.imdbRating,
        director: data.director?.name || '',
        reviews: JSON.parse(localStorage.getItem(`reviews_${data.id}`)) || []
      };
      copies.value = data.movieCopies || [];
    });
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
  object-fit: cover;
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