<template>
  <div class="container mt-5">
    <div class="card profile-card shadow-lg p-4">
      <div class="row align-items-center">
        <div class="col-md-4 profile-left text-center">
          <img :src= UniversalImage alt="Avatar" class="rounded-circle avatar-img mb-3" />
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
              <input :type="key === 'email' ? 'email' : key === 'dob' ? 'date' : 'text'"
                     v-model="form[key]" class="form-control" required />
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
        <div class="col-md-3 mb-4" v-for="movie in favoriteMoviesList" :key="movie.imdbID">
          <div class="card h-100 shadow-sm">
            <img :src="movie.imageUrl" class="card-img-top" alt="Poster" />
            <div class="card-body">
              <h5 class="card-title">{{ movie.title }}</h5>
              <p class="card-text text-muted">{{ movie.director }}</p>
              <router-link :to="{ name: 'MovieDetails', params: { imdbID: movie.imdbID } }" class="btn btn-primary btn-sm">
                Details
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Rental History Section -->
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
import { ref, reactive, computed, onMounted } from 'vue'
import jwtDecode from 'jwt-decode'

const user = reactive({
  id: null,
  name: '',
  username: '',
  email: '',
  dob: '',
  avatarUrl: '',
  favoriteMovies: [],
  rentals: []
})

const UniversalImage = ref('https://upload.wikimedia.org/wikipedia/commons/9/99/Sample_User_Icon.png')
const isEditing = ref(false)
const successMessage = ref('')
const tempAvatar = ref('')
const showHistory = ref(false)
let userId = null


const form = reactive({
  name: '',
  username: '',
  email: '',
  dob: ''
})


const toggleEdit = () => {
  if (isEditing.value) {
    Object.assign(form, user)
    tempAvatar.value = user.avatarUrl
  }
  isEditing.value = !isEditing.value
}

const saveChanges = async () => {
  if (!form.name || !form.username || !form.email || !form.dob) {
    alert('Please fill in all required fields.')
    return
  }

  const token = localStorage.getItem('token')
  if (!token) return



  const updatedData = {
    name: form.name,
    username: form.username,
    email: form.email,
    dob: new Date(form.dob).toISOString(),
    admin: user.admin
  }

  try {
    const response = await fetch(`http://localhost:5222/api/User/${userId}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(updatedData)
    })

    if (!response.ok) throw new Error('Failed to update user')

    let updatedUser = {}
    const text = await response.text()
    if (text) updatedUser = JSON.parse(text)

    Object.assign(user, updatedUser)
    Object.assign(form, updatedUser)
    tempAvatar.value = updatedUser.avatarUrl || ''
    emitter.emit("toast", {message: "Profile updated successfully.", type: "success"})
    isEditing.value = false
    await fetchUserProfile()
  } catch (err) {
    emitter.emit("toast", {message: "Failed to update profile." + err.message, type: "error"})
  }
}

const handleAvatarUpload = (e) => {
  const file = e.target.files[0]
  if (!file) return
  const reader = new FileReader()
  reader.onload = (event) => {
    tempAvatar.value = event.target.result
  }
  reader.readAsDataURL(file)
}

const formatDate = (dateStr) => new Date(dateStr).toLocaleDateString('en-GB')

const formLabels = {
  name: 'Full Name',
  username: 'Username',
  email: 'Email',
  dob: 'Date of Birth'
}

const fetchUserProfile = async () => {
  const token = localStorage.getItem('token')
  if (!token) return
  const decoded = jwtDecode(token)
  const userId = Number(decoded.UserId)

  try {
    const response = await fetch(`http://localhost:5222/api/User/${userId}`, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    })

    if (!response.ok) throw new Error('Failed to fetch user')

    const data = await response.json()
    Object.assign(user, data)
    Object.assign(form, data)
    tempAvatar.value = data.avatarUrl || ''
  } catch (err) {
    console.error('Error fetching user:', err)
  }
}

onMounted(() => {

  const token = localStorage.getItem('token')
  if(!token)window.location.href = "/login"
  const decoded = jwtDecode(token)
  userId = Number(decoded.UserId)

  fetchUserProfile()
  fetchRentals()
})

const fetchRentals = async () => {
  try {
    const response = await fetch('http://localhost:5222/api/Rental', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    })
    if (!response.ok) throw new Error('Failed to fetch rentals')

    const data = await response.json()
    user.rentals = await data.filter(r => r.borrowedToId === userId)
  } catch (err) {
    console.error('Error fetching rentals:', err)
  }
}

import { mockMovies } from '@/seeders/userData'
import emitter from "@/utils/emitter.js";

const favoriteMoviesList = computed(() => {
  return mockMovies.filter(m => user.favoriteMovies.includes(m.imdbID))
})

const today = new Date()
today.setHours(0, 0, 0, 0)

const activeRentals = computed(() =>
  user.rentals?.filter(r => {
    const rentedDate = new Date(r.returnDate)
    rentedDate.setHours(0, 0, 0, 0)
    return rentedDate >= today
  }) || []
)

const pastRentals = computed(() =>
  user.rentals?.filter(r => {
    const rentedDate = new Date(r.returnDate)
    rentedDate.setHours(0, 0, 0, 0)
    return rentedDate < today
  }) || []
)
</script>



<style scoped>
.profile-card {
  background-color: #ffffff;
  border-radius: 1rem;
  padding: 2rem;
  min-height: 300px;
}

.profile-left {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.avatar-img {
  width: 180px;
  height: 180px;
  object-fit: cover;
  border: 4px solid #dee2e6;
}

.card-img-top {
  height: 350px;
  object-fit: cover;
}
</style>
