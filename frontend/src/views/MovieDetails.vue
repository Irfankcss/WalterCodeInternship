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
              <button class="btn btn-rent"
                @click="() => { if (!isLoggedIn) { emitter.emit('toast', { message: 'You must be logged in to rent movies.', type: 'error' }); return; } toggleShowCopies(); }"
                :disabled="!copies.length || !isLoggedIn"
                :class="{ 'opacity-50 cursor-not-allowed': !copies.length || !isLoggedIn }">
                {{ showCopies ? 'Cancel' : 'Rent movie' }}
              </button>

              <button class="btn btn-favorite"
                @click="() => { if (!isLoggedIn) { emitter.emit('toast', { message: 'You must be logged in to manage favorites.', type: 'error' }); return; } toggleFavorite(); }"
                :disabled="!isLoggedIn" :class="{ active: isFavorite, 'opacity-50 cursor-not-allowed': !isLoggedIn }">
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
            <div v-else-if="reviews.some(r => r.userId === currentUserId || r.userID === currentUserId)"
              class="alert alert-info">
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
import '@/features/movies/styles/MovieDetails.css'
import { ref } from 'vue'
import { useRoute } from 'vue-router'
import { useMovieDetails } from '@/features/movies/composables/useMovieDetails'

const route = useRoute()
const id = ref(route.params.id)

const {
  isLoggedIn, movie, copies, hasAnyCopies, newReview, selectedCopyId, returnDate,
  today, maxReturnDate, activeRentals, todayIso, isCopyRented, availableCopies,
  showCopies, toggleShowCopies, confirmRent, isFavorite, toggleFavorite,
  actors, genre, reviews, rental, fetchFavoriteStatus, fetchActiveRentals, fetchCopies,
  submitReview
} = useMovieDetails(id)
</script>
