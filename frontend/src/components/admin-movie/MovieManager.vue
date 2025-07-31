<script setup>
import {nextTick, onMounted, ref} from 'vue'
import $ from 'jquery'
import 'datatables.net-bs4'
import NewMovieForm from './NewMovieForm.vue'
import EditMovieForm from './EditMovieForm.vue'
import emitter from "@/utils/emitter.js";


const showForm = ref(false)
const selectedMovie = ref(null)
const filmovi = ref([])
const handleSubmitted = () => {
  fetchMovies()
  showForm.value = false
}
const reloadMovies = () => {
  selectedMovie.value = null
  fetchMovies()
}

onMounted(() => {
  fetchMovies()
})
const fetchMovies = function () {
  fetch("http://localhost:5222/api/Movie")
    .then(response => response.json())
    .then(data => {
      filmovi.value = data
      nextTick(() => {
        $('#dataTable').DataTable()
      })
    })
};
const deleteMovie = (movie) => {
  if(confirm(`Are you sure you want to delete this movie? (${movie.name})`)) {
    fetch(`http://localhost:5222/api/Movie/${movie.id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        "Authorization": `Bearer ${localStorage.getItem('token')}`
      }
    })
    .then(() => {
        emitter.emit('toast', {
          message: 'Movie deleted successfully.',
          type: 'success'
        })
        fetchMovies()
      }
      )
      .catch((err)=>{
        emitter.emit('toast', {
          message: 'Failed to delete movie (' + err.message + ')',
          type: 'error'
        })
      })
  }
}

</script>

<template>
  <div class="container-fluid">
    <div class="card shadow mb-4">
      <div class="card-header py-3">
        <h6 class="m-0 font-weight-bold text-primary">Movie List</h6>
      </div>
      <div class="card-body">
        <button class="btn btn-success my-2 float-right" @click="showForm = true">New Movie</button>

        <NewMovieForm v-if="showForm" @submitted="handleSubmitted" @cancel="showForm = false" />
        <EditMovieForm v-if="selectedMovie" :movie="selectedMovie" @updated="reloadMovies" @cancel="selectedMovie = null"
        />

        <div class="table-responsive">
          <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
            <thead>
              <tr>
                <th>Title</th>
                <th>Director</th>
                <th>Year</th>
                <th>IMDb Rating</th>
                <th>Poster</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="movie in filmovi" :key="movie.id">
                <td>{{ movie.name }}</td>
                <td>{{ movie.director.name }}</td>
                <td>{{ movie.year }}</td>
                <td>{{ movie.imdbRating }}</td>
                <td>
                  <a :href="movie.poster" target="_blank" rel="noopener">Image</a>
                </td>
                <td>
                  <button class="btn btn-primary" @click="selectedMovie = movie">Edit</button>
                  <button class="btn btn-danger" @click="deleteMovie(movie)">Delete</button>
                  <router-link :to="`/movies/${movie.id}/copies`" class="btn btn-info">Manage Copies</router-link>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
