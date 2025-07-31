<script setup>
import {nextTick, onMounted, ref} from 'vue'
import $ from 'jquery'
import 'datatables.net-bs4'
import EditActorForm from './EditActorForm.vue'
import NewActorForm from "./NewActorForm.vue";
import emitter from "@/utils/emitter.js";



const showForm = ref(false)
const selectedActor = ref(null)
const actors = ref([])
const handleSubmitted = () => {
  fetchActors()
  showForm.value = false
}
const reloadList = () => {
  selectedActor.value = null
  fetchActors()
}

onMounted(() => {
  fetchActors()
})
const fetchActors = function () {
  fetch("http://localhost:5222/api/Actor")
    .then(response => response.json())
    .then(data => {
      actors.value = data
      nextTick(() => {
        $('#dataTable').DataTable()
      })
    })
};
const deleteActor = (actor) => {
  if(confirm(`Are you sure you want to delete this actor? (${actor.name})`)) {
    fetch(`http://localhost:5222/api/Actor/${actor.id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        "Authorization": `Bearer ${localStorage.getItem('token')}`
      }
    })
      .then(() => {
          fetchActors()
          emitter.emit('toast', {
            message: 'Actor deleted successfully.',
            type: 'success'
          })
        }
      )
      .catch((err) => {
        emitter.emit('toast', {
          message: 'Failed to delete actor (' + err.message + ')',
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
        <h6 class="m-0 font-weight-bold text-primary">Actors List</h6>
      </div>
      <div class="card-body">
        <button class="btn btn-success my-2 float-right" @click="showForm = true">New Actor</button>

        <NewActorForm v-if="showForm" @submitted="handleSubmitted" @cancel="showForm = false" />
        <EditActorForm v-if="selectedActor" :actor="selectedActor" @updated="reloadList" @cancel="selectedActor = null"
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
            <tr v-for="a in actors" :key="a.id">
              <td>{{ a.id }}</td>
              <td>{{ a.name }}</td>
              <td>{{ a.dob ? new Date(a.dob).toLocaleDateString('bs-BA') : 'N/A' }}</td>
              <td>{{ a.bio }}</td>
              <td>
                <button class="btn btn-primary" @click="selectedActor = a">Edit</button>
                <button class="btn btn-danger" @click="deleteActor(a)">Delete</button>
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
