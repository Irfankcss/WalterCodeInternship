<script setup>
import {nextTick, onMounted, ref} from 'vue'
import $ from 'jquery'
import 'datatables.net-bs4'

const users = ref([])

onMounted(() => {
  fetchUsers()
})
const fetchUsers = function () {
  fetch("http://localhost:5222/api/User",{  method: 'GET',
    headers: {
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    }})
    .then(response => response.json())
    .then(data => {
      users.value = data
      nextTick(() => {
        $('#dataTable').DataTable()
      })
    })
};

</script>

<template>
  <div class="container-fluid">
    <div class="card shadow mb-4">
      <div class="card-header py-3">
        <h6 class="m-0 font-weight-bold text-primary">Users List</h6>
      </div>
      <div class="card-body">
        <div class="table-responsive">
          <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
            <thead>
            <tr>
              <th>Id</th>
              <th>Username</th>
              <th>Name</th>
              <th>Email</th>
              <th>Date of birth</th>
              <th>Admin</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="u in users" :key="u.id">
              <td>{{ u.id }}</td>
              <td>{{ u.username }}</td>
              <td>{{ u.name }}</td>
              <td>{{ u.email }}</td>
              <td>{{ u.dob ? new Date(u.dob).toLocaleDateString('bs-BA') : 'N/A' }}</td>
              <td>{{ u.admin ? 'Yes' : 'No' }}</td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
