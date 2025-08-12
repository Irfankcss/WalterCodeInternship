<template>
  <div class="container mt-5">
    <div class="card profile-card shadow-lg p-4">
      <div class="row align-items-center">
        <div class="col-md-4 profile-left text-center">
          <img :src=UniversalImage alt="Avatar" class="rounded-circle avatar-img mb-3" />
          <h4 class="text-primary fw-bold mb-0">{{ user.name }}</h4>
          <p class="text-muted">@{{ user.username }}</p>
        </div>

        <div class="col-md-8">
          <div class="d-flex justify-content-between align-items-center mb-3">
            <h5 class="mb-0">Personal Info</h5>
            <button class="btn btn-sm btn-outline-primary" @click="toggleEdit">
              {{ isEditing ? 'Cancel' : 'Edit Profile' }}
            </button>
          </div>

          <form v-if="isEditing" @submit.prevent="saveChanges">
            <div class="mb-3" v-for="(label, key) in formLabels" :key="key">
              <label class="form-label">{{ label }} *</label>
              <input :type="key === 'email' ? 'email' : key === 'dob' ? 'date' : 'text'" v-model="form[key]"
                class="form-control" required />
            </div>
            <button type="submit" class="btn btn-success">Save Changes</button>
          </form>

          <div v-else>
            <ul class="list-group list-group-flush">
              <li class="list-group-item"><strong>Email:</strong> {{ user.email }}</li>
              <li class="list-group-item"><strong>Date of Birth:</strong> {{ formatDate(user.dob) }}</li>
              <li class="list-group-item">
                <button class="btn btn-outline-secondary btn-sm mt-2" @click="showHistory = !showHistory">
                  {{ showHistory ? 'Hide Rental History' : 'View Rental History' }}
                </button>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>

    <div v-if="successMessage" class="alert alert-success mt-3">
      {{ successMessage }}
    </div>

    <!-- Favorite Movies -->
    <div class="mt-5">
      <h4 class="mb-3">Favorite Movies</h4>
      <div class="row">
        <div class="col-md-3 mb-4" v-for="movie in favoriteMoviesList" :key="movie.movieId">
          <div class="card h-100 shadow-sm">
            <img :src="movie.moviePoster" class="card-img-top" alt="Poster" />
            <div class="card-body">
              <h5 class="card-title">{{ movie.movieTitle }}</h5>
              <router-link :to="{ name: 'MovieDetails', params: { id: movie.movieId } }" class="btn btn-primary btn-sm">
                Details
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="showHistory" class="mt-5">
      <h4 class="mb-3">Currently Rented</h4>
      <table v-if="activeRentals.length" class="table table-bordered">
        <thead>
          <tr>
            <th>Title</th>
            <th>Rent date</th>
            <th>Borrowed by</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(r, i) in activeRentals" :key="r.id">
            <td>{{ r.movieTitle }}</td>
            <td>{{ r.date }}</td>
            <td>{{ r.borrowedByName }}</td>
          </tr>
        </tbody>
      </table>
      <p v-else class="text-muted">No active rentals.</p>

      <h4 class="mt-4 mb-3">Rental History</h4>
      <table v-if="pastRentals.length" class="table table-bordered">
        <thead>
          <tr>
            <th>Title</th>
            <th>Rented date</th>
            <th>Returned On</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(r, i) in pastRentals" :key="r.id">
            <td>{{ r.movieTitle }}</td>
            <td>{{ r.date }}</td>
            <td>{{ r.returnDate }}</td>
          </tr>
        </tbody>
      </table>
      <p v-else class="text-muted">No rental history.</p>
    </div>
  </div>
</template>

<script setup>
import '@/features/user/styles/UserProfile.css'
import { useUserProfile } from '@/features/user/composables/useUserProfile'

const {
  user, UniversalImage, isEditing, successMessage, tempAvatar, showHistory,
  form, formLabels, favoriteMoviesList,
  toggleEdit, saveChanges, formatDate,
  activeRentals, pastRentals
} = useUserProfile()
</script>

