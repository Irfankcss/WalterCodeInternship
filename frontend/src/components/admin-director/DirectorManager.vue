<script setup>
import {nextTick, onMounted, ref} from 'vue'
import $ from 'jquery'
import 'datatables.net-bs4'
import EditDirectorForm from './EditDirectorForm.vue'
import NewDirectorForm from "@/components/admin-director/NewDirectorForm.vue";
import emitter from "@/utils/emitter.js";



const showForm = ref(false)
const selectedDirector = ref(null)
const directors = ref([])
const handleSubmitted = () => {
  fetchDirectors()
  showForm.value = false
}
const reloadList = () => {
  selectedDirector.value = null
  fetchDirectors()
}

onMounted(() => {
  fetchDirectors()
})
const fetchDirectors = function () {
  fetch("http://localhost:5222/api/Director")
    .then(response => response.json())
    .then(data => {
      directors.value = data
      nextTick(() => {
        $('#dataTable').DataTable()
      })
    })
};
const deleteDirector = (d) => {
  if(confirm(`Are you sure you want to delete this director? (${d.name})`)) {
    fetch(`http://localhost:5222/api/Director/${d.id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        "Authorization": `Bearer ${localStorage.getItem('token')}`
      }
    })
      .then(() => {
          fetchDirectors()
        emitter.emit('toast', {
          message: 'Director deleted successfully.',
          type: 'success'
        })
        }
      )
      .catch((err) => {
        emitter.emit('toast', {
          message: 'Failed to delete director (' + err.message + ')',
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
        <h6 class="m-0 font-weight-bold text-primary">Directors List</h6>
      </div>
      <div class="card-body">
        <button class="btn btn-success my-2 float-right" @click="showForm = true">New Director</button>

        <NewDirectorForm v-if="showForm" @submitted="handleSubmitted" @cancel="showForm = false" />
        <EditDirectorForm v-if="selectedDirector" :director="selectedDirector" @updated="reloadList" @cancel="selectedDirector = null"
        />

        <div class="table-responsive">
          <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
            <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Date of birth</th>
              <th>Biography</th>
              <th>Actions</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="d in directors" :key="d.id">
              <td>{{ d.id }}</td>
              <td>{{ d.name }}</td>
              <td>{{ d.dob ? new Date(d.dob).toLocaleDateString('bs-BA') : 'N/A' }}</td>
              <td>{{ d.bio }}</td>
              <td>
                <button class="btn btn-primary" @click="selectedDirector = d">Edit</button>
                <button class="btn btn-danger" @click="deleteDirector(d)">Delete</button>
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
