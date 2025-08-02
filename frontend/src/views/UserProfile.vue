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
            <h5 class="mb-0">Lični podaci</h5>
            <button class="btn btn-sm btn-outline-primary" @click="toggleEdit">
              {{ isEditing ? 'Otkaži' : 'Uredi profil' }}
            </button>
          </div>

          <form v-if="isEditing" @submit.prevent="saveChanges">
            <div class="mb-3" v-for="(label, key) in formLabels" :key="key">
              <label class="form-label">{{ label }} *</label>
              <input :type="key === 'email' ? 'email' : key === 'dob' ? 'date' : 'text'"
                     v-model="form[key]" class="form-control" required />
            </div>

            <div class="mb-3">
              <label class="form-label">Promijeni avatar</label>
              <input type="file" accept="image/*" @change="handleAvatarUpload" class="form-control" />
            </div>

            <div class="d-flex justify-content-end gap-2 mb-3">
              <button type="button" class="btn btn-outline-warning" @click="showPasswordFields = !showPasswordFields">
                {{ showPasswordFields ? 'Otkaži promjenu lozinke' : 'Promijeni lozinku' }}
              </button>
              <button type="submit" class="btn btn-success">Sačuvaj promjene</button>
            </div>

            <div v-if="showPasswordFields">
              <div class="mb-3">
                <label class="form-label">Trenutna lozinka</label>
                <input type="password" v-model="passwordForm.current" class="form-control" />
              </div>
              <div class="mb-3">
                <label class="form-label">Nova lozinka</label>
                <input type="password" v-model="passwordForm.new" class="form-control" />
              </div>
              <p v-if="passwordError" class="text-danger">{{ passwordError }}</p>
              <p v-if="passwordSuccess" class="text-success">{{ passwordSuccess }}</p>
            </div>
          </form>

          <div v-else>
            <ul class="list-group list-group-flush">
              <li class="list-group-item"><strong>Email:</strong> {{ user.email }}</li>
              <li class="list-group-item"><strong>Datum rođenja:</strong> {{ formatDate(user.dob) }}</li>
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
      <h4 class="mb-3">Omiljeni filmovi</h4>
      <div class="row">
        <div class="col-md-3 mb-4" v-for="movie in favoriteMoviesList" :key="movie.imdbID">
          <div class="card h-100 shadow-sm">
            <img :src="movie.imageUrl" class="card-img-top" alt="Poster" />
            <div class="card-body">
              <h5 class="card-title">{{ movie.title }}</h5>
              <p class="card-text text-muted">{{ movie.director }}</p>
              <router-link :to="{ name: 'MovieDetails', params: { imdbID: movie.imdbID } }" class="btn btn-primary btn-sm">
                Detalji
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref, computed } from 'vue'
import { useUserStore } from '@/stores/userStore'
import { mockMovies } from '@/seeders/userData'

const userStore = useUserStore()
const user = userStore.user

const isEditing = ref(false)
const showPasswordFields = ref(false)
const successMessage = ref('')
const passwordError = ref('')
const passwordSuccess = ref('')
const tempAvatar = ref(user.avatarUrl || '')

const formLabels = {
  name: 'Ime i prezime',
  username: 'Korisničko ime',
  email: 'Email',
  dob: 'Datum rođenja'
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
    alert('Molimo popunite sva obavezna polja.')
    return
  }

  if (showPasswordFields.value) {
    if (passwordForm.current !== user.password) {
      passwordError.value = 'Trenutna lozinka nije tačna.'
      return
    }
    if (!passwordForm.new.trim() || passwordForm.new === passwordForm.current) {
      passwordError.value = 'Nova lozinka mora biti različita i ne smije biti prazna.'
      return
    }
    user.password = passwordForm.new
    passwordSuccess.value = 'Lozinka je uspješno promijenjena.'
  }

  Object.assign(user, form)
  user.avatarUrl = tempAvatar.value
  localStorage.setItem('avatarUrl', user.avatarUrl)
  successMessage.value = 'Profil je uspješno ažuriran.'
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

const formatDate = (dateStr) => new Date(dateStr).toLocaleDateString('sr-Latn-BA')

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