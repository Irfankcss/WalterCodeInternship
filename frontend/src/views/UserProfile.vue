<template>
  <div class="container mt-5">
    <div class="card profile-card shadow-lg p-4">
      <div class="row align-items-center">
        <div class="col-md-4 profile-left text-center">
          <img :src="tempAvatar" alt="Avatar" class="rounded-circle avatar-img mb-3" />
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

            <div class="mb-3">
              <label class="form-label">Change Avatar</label>
              <input type="file" accept="image/*" @change="handleAvatarUpload" class="form-control" />
            </div>

            <div class="d-flex justify-content-end gap-2 mb-3">
              <button type="button" class="btn btn-outline-warning" @click="showPasswordFields = !showPasswordFields">
                {{ showPasswordFields ? 'Cancel Password Change' : 'Change Password' }}
              </button>
              <button type="submit" class="btn btn-success">Save Changes</button>
            </div>

            <div v-if="showPasswordFields">
              <div class="mb-3">
                <label class="form-label">Current Password</label>
                <input type="password" v-model="passwordForm.current" class="form-control" />
              </div>
              <div class="mb-3">
                <label class="form-label">New Password</label>
                <input type="password" v-model="passwordForm.new" class="form-control" />
              </div>
              <p v-if="passwordError" class="text-danger">{{ passwordError }}</p>
              <p v-if="passwordSuccess" class="text-success">{{ passwordSuccess }}</p>
            </div>
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
            <th>Serial Number</th>
            <th>Rented Until</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(r, i) in activeRentals" :key="'a' + i">
            <td>{{ r.title }}</td>
            <td>{{ r.serialNumber }}</td>
            <td>{{ r.rentedUntil }}</td>
          </tr>
        </tbody>
      </table>
      <p v-else class="text-muted">No active rentals.</p>

      <h4 class="mt-4 mb-3">Rental History</h4>
      <table v-if="pastRentals.length" class="table table-bordered">
        <thead>
          <tr>
            <th>Title</th>
            <th>Serial Number</th>
            <th>Returned On</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(r, i) in pastRentals" :key="'p' + i">
            <td>{{ r.title }}</td>
            <td>{{ r.serialNumber }}</td>
            <td>{{ r.rentedUntil }}</td>
          </tr>
        </tbody>
      </table>
      <p v-else class="text-muted">No rental history.</p>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref, computed, onMounted, onUnmounted } from 'vue'
import { useUserStore } from '@/stores/userStore'
import { mockMovies } from '@/seeders/userData'

const userStore = useUserStore()
const user = userStore.user

try {
  const rawUser = localStorage.getItem('userData');
  if (rawUser && rawUser !== "undefined") {
    const userData = JSON.parse(rawUser);
    Object.assign(user, userData);
  }
} catch (error) {
  console.error("Greška prilikom učitavanja userData:", error);
}

try {
  const favs = localStorage.getItem('favorites');
  if (favs && favs !== "undefined") {
    user.favoriteMovies = JSON.parse(favs);
  }
} catch (error) {
  console.error("Greška prilikom učitavanja favorites:", error);
}

if (!user.rentals) user.rentals = []; //safe option

const rentals = JSON.parse(localStorage.getItem('rentals'));
rentals.push({
  title: "The Dark Knight",
  serialNumber: "SN004",
  rentedUntil: "2024-07-30"
}, {
  title: "The Godfather",
  serialNumber: "SN003",
    rentedUntil: "2023-08-15"
})
localStorage.setItem('rentals', JSON.stringify(rentals));

const seedRentals = [
  {
    title: "The Godfather",
    serialNumber: "SN003",
    rentedUntil: "2025-08-15" 
  },
  {
    title: "The Dark Knight",
    serialNumber: "SN004",
    rentedUntil: "2024-07-30" 
  }
];

const rentalsFromStorage = localStorage.getItem('rentals');
if (!rentalsFromStorage || rentalsFromStorage === "undefined") {
  localStorage.setItem('rentals', JSON.stringify(seedRentals));
  user.rentals = seedRentals;
} else {
  try {
    const parsed = JSON.parse(rentalsFromStorage);
    user.rentals = parsed;
  } catch (error) {
    console.error("Neispravan JSON za 'rentals':", error);
    user.rentals = [];
  }
}

const refreshRentals = () => {
  const stored = localStorage.getItem('rentals');
  if (stored && stored !== "undefined") {
    try {
      user.rentals = JSON.parse(stored);
    } catch (e) {
      console.error("Greška u parsiranju rentals pri refreshu:", e);
    }
  }
};

onMounted(() => {
  window.addEventListener('storage', refreshRentals);
});

onUnmounted(() => {
  window.removeEventListener('storage', refreshRentals);
});

const isEditing = ref(false)
const showPasswordFields = ref(false)
const successMessage = ref('')
const passwordError = ref('')
const passwordSuccess = ref('')
const tempAvatar = ref(user.avatarUrl || '')
const showHistory = ref(false)

const today = new Date();
today.setHours(0, 0, 0, 0); 

const activeRentals = computed(() =>
  user.rentals?.filter(r => {
    const rentedDate = new Date(r.rentedUntil);
    rentedDate.setHours(0, 0, 0, 0);
    return rentedDate >= today;
  }) || []
);

const pastRentals = computed(() =>
  user.rentals?.filter(r => {
    const rentedDate = new Date(r.rentedUntil);
    rentedDate.setHours(0, 0, 0, 0);
    return rentedDate < today;
  }) || []
);

const formLabels = {
  name: 'Full Name',
  username: 'Username',
  email: 'Email',
  dob: 'Date of Birth'
}

const form = reactive({
  name: user.name,
  username: user.username,
  email: user.email,
  dob: user.dob
})

const passwordForm = reactive({ current: '', new: '' })

const toggleEdit = () => {
  if (isEditing.value) {
    Object.assign(form, user)
    tempAvatar.value = user.avatarUrl
    passwordForm.current = passwordForm.new = ''
    passwordError.value = passwordSuccess.value = successMessage.value = ''
    showPasswordFields.value = false
  }
  isEditing.value = !isEditing.value
}

const saveChanges = () => {
  if (!form.name || !form.username || !form.email || !form.dob) {
    alert('Please fill in all required fields.')
    return
  }

  if (showPasswordFields.value) {
    if (passwordForm.current !== user.password) {
      passwordError.value = 'Current password is incorrect.'
      return
    }
    if (!passwordForm.new.trim() || passwordForm.new === passwordForm.current) {
      passwordError.value = 'New password must be different and not empty.'
      return
    }
    user.password = passwordForm.new
    passwordSuccess.value = 'Password changed successfully.'
  }

  Object.assign(user, form)
  user.avatarUrl = tempAvatar.value
  localStorage.setItem('userData', JSON.stringify(user));
  localStorage.setItem('favorites', JSON.stringify(user.favoriteMovies)); 
  successMessage.value = 'Profile updated successfully.'
  isEditing.value = false
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

const favoriteMoviesList = computed(() => {
  return mockMovies.filter(m => user.favoriteMovies.includes(m.imdbID))
})
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
