<script setup>
import { ref, onMounted } from 'vue'
import emitter from "@/utils/emitter.js";
import jwtDecode from "jwt-decode";
import NewActorForm from '../admin-actor/NewActorForm.vue'
import NewDirectorForm from '../admin-director/NewDirectorForm.vue' // dodano

const emit = defineEmits(['submitted', 'cancel'])

const movie = ref({
  name: '',
  year: 0,
  imdbId: '',
  imdbRating: 0,
  poster: '',
  description: '',
  directorId: 0,
  editedById: 0,
  actorIds: [],
  genreIds: []
})

const directors = ref([])
const actors = ref([])
const genres = ref([])

const showActorForm = ref(false)
const showDirectorForm = ref(false) // dodano

const fetchDirectors = () => {
  fetch('http://localhost:5222/api/Director')
    .then(res => res.json())
    .then(data => directors.value = data)
}

const fetchActors = () => {
  fetch('http://localhost:5222/api/Actor')
    .then(res => res.json())
    .then(data => actors.value = data)
}

const fetchGenres = () => {
  fetch('http://localhost:5222/api/Genre')
    .then(res => res.json())
    .then(data => genres.value = data)
}

const handleActorSubmitted = () => {
  fetchActors()
  showActorForm.value = false
}

const handleDirectorSubmitted = () => { // dodano
  fetchDirectors()
  showDirectorForm.value = false
}

onMounted(() => {
  fetchDirectors()
  fetchActors()
  fetchGenres()
})

const submitMovie = () => {
  const token = localStorage.getItem('token')
  const decodedToken = jwtDecode(token)
  movie.value.editedById = decodedToken.UserId

  fetch('http://localhost:5222/api/Movie', {
    method: 'POST',
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token}`
    },
    body: JSON.stringify(movie.value)
  })
    .then(res => {
      if (!res.ok) throw new Error('Failed to submit')
      return res.json()
    })
    .then(() => {
      emit('submitted')
      emitter.emit('toast', {
        message: 'Movie added successfully.',
        type: 'success'
      })
    })
    .catch(err => emitter.emit('toast', {
      message: "Failed to add movie (" + err.message + ")",
      type: 'error'
    }))
}
</script>

<template>
  <div class="modal fade show d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);" role="dialog">
    <div class="modal-dialog modal-lg" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Add New Movie</h5>
          <button type="button" class="close-btn" @click="$emit('cancel')" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitMovie">
            <div class="form-group mb-3">
              <label>Title</label>
              <input type="text" v-model="movie.name" class="form-control" required />
            </div>
            <div class="form-group mb-3">
              <label>Year</label>
              <input type="number" v-model="movie.year" class="form-control" required />
            </div>
            <div class="form-group mb-3">
              <label>IMDb ID</label>
              <input type="text" v-model="movie.imdbId" class="form-control" required />
            </div>
            <div class="form-group mb-3">
              <label>IMDb Rating</label>
              <input type="number" step="0.1" v-model="movie.imdbRating" class="form-control" required />
            </div>
            <div>
              <label>Description</label>
              <input type="text" v-model="movie.description" class="form-control mb-3" />
            </div>
            <div class="form-group mb-3">
              <label>Poster URL</label>
              <input type="text" v-model="movie.poster" class="form-control" />
            </div>
            <div class="form-group mb-3">
              <label>Director</label>
              <select v-model="movie.directorId" class="form-control">
                <option disabled value="0">Select director</option>
                <option v-for="d in directors" :key="d.id" :value="d.id">
                  {{ d.name }}
                </option>
              </select>
              <button type="button" class="btn btn-warning mt-2" @click="showDirectorForm = true">
                Add new Director
              </button>
            </div>
            <div class="form-group mb-3">
              <label>Actors</label>
              <div v-for="a in actors" :key="a.id" class="form-check">
                <input class="form-check-input" type="checkbox" :id="'actor-' + a.id"
                       :value="a.id" v-model="movie.actorIds" />
                <label class="form-check-label" :for="'actor-' + a.id">
                  {{ a.name }}
                </label>
              </div>
              <button type="button" class="btn btn-warning mt-2" @click="showActorForm = true">
                Add new Actor
              </button>
            </div>
            <div class="form-group mb-3">
              <label>Genres</label>
              <div v-for="g in genres" :key="g.id" class="form-check">
                <input class="form-check-input" type="checkbox" :id="'genre-' + g.id"
                       :value="g.id" v-model="movie.genreIds" />
                <label class="form-check-label" :for="'genre-' + g.id">
                  {{ g.name }}
                </label>
              </div>
            </div>
          </form>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Cancel</button>
          <button type="button" class="btn btn-primary" @click="submitMovie">Submit</button>
        </div>
      </div>
    </div>
  </div>

  <NewActorForm
    v-if="showActorForm"
    @submitted="handleActorSubmitted"
    @cancel="showActorForm = false"
  />

  <NewDirectorForm
    v-if="showDirectorForm"
    @submitted="handleDirectorSubmitted"
    @cancel="showDirectorForm = false"
  />
</template>

<style scoped>
.modal-body {
  max-height: 70vh;
  overflow-y: auto;
}
</style>
