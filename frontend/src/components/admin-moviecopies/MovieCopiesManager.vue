<template>
  <div class="container-fluid">
    <div class="card shadow mb-4">
      <div class="card-header py-3 d-flex justify-content-between align-items-center">
        <h6 class="m-0 font-weight-bold text-primary">Movie Copies for "{{ movieTitle }}"</h6>
        <button class="btn btn-secondary" @click="$router.back()">← Back</button>
      </div>
      <div class="card-body">
        <form class="mb-3" @submit.prevent="addCopy">
          <div class="row g-3 align-items-center">
            <div class="col-auto">
              <label class="col-form-label">Serial number:</label>
            </div>
            <div class="col-auto">
              <input type="text" class="form-control" v-model="newCopy.serialNumber" required />
            </div>
            <div class="col-auto">
              <label class="col-form-label">Description:</label>
            </div>
            <div class="col-auto">
              <input type="text" class="form-control" v-model="newCopy.description" required />
            </div>
            <div class="col-auto">
              <button class="btn btn-success" type="submit">Add Copy</button>
            </div>
          </div>
        </form>

        <div class="table-responsive">
          <table class="table table-bordered" id="copiesTable">
            <thead>
            <tr>
              <th>ID</th>
              <th>Serial Number</th>
              <th>Description</th>
              <th>Actions</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="copy in movieCopies" :key="copy.id">
              <td>{{ copy.id }}</td>
              <td>{{ copy.serialNumber }}</td>
              <td>{{ copy.description }}</td>
              <td>
                <button class="btn btn-danger btn-sm" @click="deleteCopy(copy.id)">Delete</button>
              </td>
            </tr>
            <tr v-if="movieCopies.length === 0">
              <td colspan="4" class="text-center">No copies found.</td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import emitter from "@/utils/emitter.js";
const route = useRoute()
const movieId = route.params.id

const movieCopies = ref([])
const movieTitle = ref('')
const newCopy = ref({
  movieId: movieId,
  serialNumber: '',
  description: ''
})

const fetchMovieCopies = async () => {
  const res = await fetch(`http://localhost:5222/api/MovieCopy/by-movie/${movieId}`)
  if (res.status === 404) {
       movieCopies.value = []
    return
  }
  const data = await res.json()
  movieCopies.value = data
}

const fetchMovieTitle = async () => {
  const res = await fetch(`http://localhost:5222/api/Movie/${movieId}`)
  const data = await res.json()
  movieTitle.value = data.name
}

const addCopy = async () => {
  try {
    const response = await fetch(`http://localhost:5222/api/MovieCopy`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify(newCopy.value)
    })
    if (!response.ok) throw new Error('Failed to add copy')
    emitter.emit('toast', {
      message: 'Copy added successfully.',
      type: 'success'
    })
    newCopy.value.serialNumber = ''
    newCopy.value.description = ''
    await fetchMovieCopies()

  } catch (error) {
    emitter.emit('toast', {
      message:'Failed to add copy' + " (" + error.message + ")",
      type: 'error'
    })
  }
}

const deleteCopy = async (copyId) => {
  if (!confirm('Are you sure you want to delete this copy?')) return

  try {
    const response = await fetch(`http://localhost:5222/api/MovieCopy/${copyId}`, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    })

    if (!response.ok) {
      throw new Error(await response.text())
    }

    emitter.emit('toast', {
      message: 'Copy deleted successfully.',
      type: 'success'
    })

    await fetchMovieCopies()
  } catch (error) {
    emitter.emit('toast', {
      message: 'Failed to delete copy (' + error.message + ')',
      type: 'error'
    })
  }
}

onMounted(() => {
  fetchMovieTitle()
  fetchMovieCopies()
})
</script>
