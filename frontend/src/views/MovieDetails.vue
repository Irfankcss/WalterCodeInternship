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

            <p class="movie-description">{{ movie.description }}</p>

            <div class="action-buttons mb-3">
<button class="btn btn-rent" @click="() => { if (!isLoggedIn) { emitter.emit('toast', { message: 'You must be logged in to rent movies.', type: 'error' }); return; } toggleShowCopies(); }" :disabled="!copies.length || !isLoggedIn"
  :class="{ 'opacity-50 cursor-not-allowed': !copies.length || !isLoggedIn }">
  {{ showCopies ? 'Cancel' : 'Rent movie' }}
</button>

<button class="btn btn-favorite" @click="() => { if (!isLoggedIn) { emitter.emit('toast', { message: 'You must be logged in to manage favorites.', type: 'error' }); return; } toggleFavorite(); }" :disabled="!isLoggedIn"
  :class="{ active: isFavorite, 'opacity-50 cursor-not-allowed': !isLoggedIn }">
  {{ isFavorite ? '★ Remove from favorites' : '☆ Add to favorites' }}
</button>
              <router-link to="/movies" class="btn btn-back">
                Back to movie list
              </router-link>
            </div>
            <div v-if="!isLoggedIn" class="alert alert-warning">
              🔒 Please log in to leave a comment and rating.
            </div>
          </div>
        </div>
      </div>
    </div>


    <div v-if="showCopies" class="container mt-4">
      <h5>Choose a copy and return date (maximum 14 days):</h5>

      <div class="d-flex flex-wrap gap-3">
        <div v-for="copy in availableCopies" :key="copy.id" class="card"
          :class="{ 'border-primary': selectedCopyId === copy.id }" @click="selectedCopyId = copy.id">
          <div class="card-body">
            <h5 class="card-title">Serial Number: {{ copy.serialNumber }}</h5>
            <p class="card-text">{{ copy.description || 'No description.' }}</p>
          </div>
        </div>
      </div>

      <div v-if="!availableCopies.length" class="alert alert-info mt-3">
        All copies are currently rented out.
      </div>
    </div>

    <div v-if="showCopies && availableCopies.length" class="mt-3">
      <label>Return date:</label>
      <input type="date" v-model="returnDate" :min="today" :max="maxReturnDate"
        class="form-control w-auto d-inline-block ms-2">
      <button class="btn btn-success ms-3" @click="confirmRent" :disabled="!selectedCopyId || !returnDate">
        Confirm rental
      </button>
    </div>

    <div class="movie-content container">
      <div class="row">
        <div class="col-md-8">
          <section class="movie-section">
            <h2 class="section-title">Description</h2>
            <p>{{ movie.description }}</p>
          </section>

          <section class="movie-section">
            <h2 class="section-title">Cast</h2>
            <div class="cast-grid">
              <div class="cast-member" v-for="actor in actors" :key="actor.id">
                <!-- <img :src="actor.image" :alt="actor.name" class="actor-avatar" /> -->
                <span class="actor-name">{{ actor.name }}</span>
              </div>
            </div>
          </section>

          <section class="movie-section">
            <h2 class="section-title">Comments and Ratings</h2>
            <div v-if="isLoggedIn && !reviews.some(r => r.userId === currentUserId || r.userID === currentUserId)"
              class="mb-3">
              <textarea v-model="newReview.comment" class="form-control mb-2"
                placeholder="Write a comment..."></textarea>
              <select v-model.number="newReview.rating" class="form-control mb-2">
                <option disabled value="">Rate (1-5)</option>
                <option v-for="n in 5" :key="n" :value="n">{{ n }}</option>
              </select>
              <button class="btn btn-success" @click="submitReview">Add review</button>
            </div>
            <div v-if="!isLoggedIn" class="alert alert-warning">
              🔒 Please log in to leave a comment and rating.
            </div>
            <div v-else class="alert alert-info">
              You have already submitted a review for this movie.
            </div>

            <div v-if="reviews.length">
              <div v-for="(review, index) in reviews" :key="index" class="mb-3 border p-3 rounded">
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
import jwtDecode from 'jwt-decode';
import emitter from '@/utils/emitter';

const userStore = useUserStore();
const route = useRoute();
const id = route.params.id;
const isLoggedIn = computed(() => !!localStorage.getItem('token'));
const movie = ref(null);
const copies = ref([]);
const hasAnyCopies = computed(() => (copies.value?.length || 0) > 0)
const newReview = ref({ comment: '', rating: '' });
const selectedCopyId = ref(null);
const returnDate = ref(null);
const today = new Date().toLocaleDateString('en-CA');
const maxReturnDate = new Date(Date.now() + 14 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];

const activeRentals = ref([]);
const todayIso = () => new Date().toISOString().slice(0, 10);

const isCopyRented = (copyId) => {
  return activeRentals.value.some(r => r.movieCopyId === copyId && new Date(r.returnDate) >= new Date());
};

const availableCopies = computed(() => copies.value.filter(c => !isCopyRented(c.id)));

async function fetchActiveRentals() {
  try {
    const res = await fetch(`http://localhost:5222/api/Rental`);
    if (!res.ok) throw new Error('Failed to load rentals');
    const data = await res.json();

    activeRentals.value = data.filter(r =>
      copies.value.some(c => c.id === r.movieCopyId) &&
      new Date(r.returnDate) >= new Date() &&
      !r.isDeleted
    );
  } catch (err) {
    console.error('Error loading rentals', err);
  }
}

const showCopies = ref(false);

const toggleShowCopies = async () => {
  showCopies.value = !showCopies.value;
  selectedCopyId.value = null;
  returnDate.value = null;

  if (showCopies.value) {
    await fetchActiveRentals();
  }
};
const fetchCopies = async () => {
  try {
    const res = await fetch(`http://localhost:5222/api/Movie/${id}`);
    const data = await res.json();
    copies.value = data.movieCopies || [];
  } catch (error) {
    console.error('Error fetching movie copies:', error);
  }
};
const fetchFavoriteStatus = async () => {
  const token = localStorage.getItem('token');
  if (!token) return;

  const decodedToken = jwtDecode(token);
  const userId = decodedToken.UserId || decodedToken.userId;

  try {
    const res = await fetch(`http://localhost:5222/api/UserFavoriteMovies/${userId}`);
    const data = await res.json();
    isFavorite.value = data.some(f => f.movieId === Number(id));
  } catch (err) {
    console.error("Error checking favorite status:", err);
  }
};

const confirmRent = async () => {
  const copy = copies.value.find(c => c.id === selectedCopyId.value);
  if (!copy || !returnDate.value) {
    emitter.emit("toast", {
      message: "Please select a copy and return date.",
      type: "error"
    });
    return;
  }

  const token = localStorage.getItem('token');
  const decodedToken = jwtDecode(token);
  const userId = decodedToken.UserId;

  const payload = {
    movieId: Number(id),
    movieCopyId: copy.id,
    borrowedById: 1,
    borrowedToId: userId,
    returnDate: returnDate.value
  };

  try {
    const res = await fetch("http://localhost:5222/api/Rental", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(payload)
    });

    const responseText = await res.text();

    if (!res.ok) {
      console.error('Rent error:', responseText);
      emitter.emit("toast", {
        message: "Rent failed: " + responseText,
        type: "error"
      });
      return;
    }

    emitter.emit("toast", {
      message: `Movie rented successfully until ${returnDate.value}`,
      type: "success"
    });

    await fetchCopies();
    await fetchActiveRentals();

    showCopies.value = false;
    selectedCopyId.value = null;
    returnDate.value = null;

  } catch (err) {
    console.error("Rent request failed:", err);
    emitter.emit("toast", {
      message: "Could not process rental. Try again later.",
      type: "error"
    });
  }

};

const isFavorite = ref(false);

const toggleFavorite = async () => {
  const token = localStorage.getItem('token');
  if (!token) {
    emitter.emit("toast", {
      message: "You must be logged in to use favorites.",
      type: "error"
    });
    return;
  }

  const decodedToken = jwtDecode(token);
  const userId = decodedToken.UserId || decodedToken.userId;

  if (isFavorite.value) {
    try {
      const res = await fetch(`http://localhost:5222/api/UserFavoriteMovies?userId=${userId}&movieId=${movie.value.id}`, {
        method: "DELETE"
      });

      if (!res.ok) throw new Error("Failed to remove favorite");

      isFavorite.value = false;
      emitter.emit("toast", {
        message: "Removed from favorites.",
        type: "success"
      });

    } catch (err) {
      console.error("Error removing favorite:", err);
      emitter.emit("toast", {
        message: "Failed to remove from favorites.",
        type: "error"
      });
    }

  } else {
    // POST
    const payload = {
      userId: userId,
      movieId: movie.value.id,
      movieTitle: movie.value.title,
      moviePoster: movie.value.imageUrl
    };

    try {
      const res = await fetch(`http://localhost:5222/api/UserFavoriteMovies`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(payload)
      });

      if (!res.ok) throw new Error("Failed to add favorite");

      isFavorite.value = true;
      emitter.emit("toast", {
        message: "Added to favorites.",
        type: "success"
      });

    } catch (err) {
      console.error("Error adding favorite:", err);
      emitter.emit("toast", {
        message: "Failed to add to favorites.",
        type: "error"
      });
    }
  }
};



const submitReview = async () => {
  if (!newReview.value.comment || !newReview.value.rating) {
    alert('Please enter a comment and rating.');
    return;
  }

  const token = localStorage.getItem('token');
  const decodedToken = jwtDecode(token);
  const userId = decodedToken.UserId || decodedToken.userId;

  const alreadyExists = reviews.value.some(
    r => r.userId === userId || r.userID === userId
  );

  if (alreadyExists) {
    emitter.emit("toast", {
      message: "⚠️ You have already reviewed this movie.",
      type: "error"
    });
    return;
  }

  const reviewPayload = {
    movieID: Number(id),
    userID: userId,
    rating: Number(newReview.value.rating),
    comment: newReview.value.comment
  };

  try {
    const response = await fetch('http://localhost:5222/api/MovieRatings', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(reviewPayload)
    });

    const responseText = await response.text();

    if (!response.ok) {
      console.error('❌ Server response:', responseText);

      if (responseText.includes("Duplicate entry")) {
        emitter.emit("toast", {
          message: "⚠️ You have already reviewed this movie.",
          type: "error"
        });
        return;
      }

      throw new Error(`Server error: ${response.status}`);
    }

    const savedReview = JSON.parse(responseText);

    reviews.value.push({
      rating: savedReview.rating,
      comment: savedReview.comment,
      userId: userId
    });

    newReview.value.comment = '';
    newReview.value.rating = '';

    emitter.emit("toast", {
      message: "✅ Review submitted successfully!",
      type: "success"
    });
  } catch (error) {
    console.error('❌ Error submitting review:', error);

    emitter.emit("toast", {
      message: `❌ Error submitting review: ${error.message}`,
      type: "error"
    });
  }
};

const actors = ref([]);
const genre = ref('');
const reviews = ref([]);
const rental = ref([]);

onMounted(async () => {
  fetchFavoriteStatus()
  fetch(`http://localhost:5222/api/MovieRatings`)
    .then(res => res.json())
    .then(data => {
      reviews.value = data.filter(r => r.movieId === Number(id));
    })
    .catch(err => console.error('Error fetching reviews:', err));

  fetch(`http://localhost:5222/api/Rental`)
    .then(res => res.json())
    .then(data => {
      rental.value = data.filter(r => r.movieId === Number(id));
    })
    .catch(err => console.error('Error fetching rentals:', err));

  fetch(`http://localhost:5222/api/Actor/GetByMovieID?MovieId=${id}`)
    .then(res => res.json())
    .then(data => {
      actors.value = data;
    })
    .catch(err => console.error('Error fetching actors:', err));

  fetch(`http://localhost:5222/api/Movie/${id}/genres`)
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
        description: data.description || data.decs || data.backText || data.frontText || 'No description available',
        backText: data.backText || '',
        frontText: data.frontText || '',
        genre: data.genre || '',
        imdbRating: data.imdbRating,
        director: data.director?.name || '',
        reviews: JSON.parse(localStorage.getItem(`reviews_${data.id}`)) || []
      };
    });

  await fetchCopies();
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

.cursor-pointer {
  cursor: pointer;
}

.cursor-not-allowed {
  cursor: not-allowed;
}
</style>
