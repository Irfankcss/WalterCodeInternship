<script setup>
import {nextTick, onMounted, ref} from 'vue'
import $ from 'jquery'
import 'datatables.net-bs4'
import EditGenreForm from './EditGenreForm.vue'
import NewGenreForm from "@/components/admin-genre/NewGenreForm.vue";
import emitter from "@/utils/emitter.js";



const showForm = ref(false)
const selectedGenre = ref(null)
const genres = ref([])
const handleSubmitted = () => {
  fetchGenres()
  showForm.value = false
}
const reloadList = () => {
  selectedGenre.value = null
  fetchGenres()
}

onMounted(() => {
  fetchGenres()
})
const fetchGenres = function () {
  fetch("http://localhost:5222/api/Genre")
    .then(response => response.json())
    .then(data => {
      genres.value = data
      nextTick(() => {
        $('#dataTable').DataTable()
      })
    })
};
const deleteGenre = (genre) => {
  if(confirm(`Are you sure you want to delete this genre? (${genre.name})`)) {
    fetch(`http://localhost:5222/api/Genre/${genre.id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        "Authorization": `Bearer ${localStorage.getItem('token')}`
      }
    })
      .then(() => {
          fetchGenres()
        emitter.emit('toast', {
          message: 'Genre deleted successfully.',
          type: 'success'
        })
        }
      )
      .catch((err) => {
        emitter.emit('toast', {
          message: 'Failed to delete genre (' + err.message + ')',
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
        <h6 class="m-0 font-weight-bold text-primary">Genre List</h6>
      </div>
      <div class="card-body">
        <button class="btn btn-success my-2 float-right" @click="showForm = true">New Genre</button>

        <NewGenreForm v-if="showForm" @submitted="handleSubmitted" @cancel="showForm = false" />
        <EditGenreForm v-if="selectedGenre" :genre="selectedGenre" @updated="reloadList" @cancel="selectedGenre = null"
        />

        <div class="table-responsive">
          <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
            <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Description</th>
              <th>Actions</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="g in genres" :key="g.id">
              <td>{{ g.id }}</td>
              <td>{{ g.name }}</td>
              <td>{{ g.description }}</td>
              <td>
                <button class="btn btn-primary" @click="selectedGenre = g">Edit</button>
                <button class="btn btn-danger" @click="deleteGenre(g)">Delete</button>
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
