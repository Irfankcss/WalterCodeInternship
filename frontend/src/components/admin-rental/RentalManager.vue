<template>
  <div class="container-fluid">
    <div class="card shadow mb-4">
      <div class="card-header py-3 d-flex justify-content-between align-items-center">
        <h6 class="m-0 font-weight-bold text-primary">Rentals</h6>
        <button class="btn btn-secondary" @click="$router.back()">← Back</button>
      </div>
      <div class="card-body">
        <div class="table-responsive">
          <input
            type="text"
            v-model="searchQuery"
            class="form-control mb-3"
            placeholder="Search by Borrowed To..."
          >

          <table class="table table-bordered" id="copiesTable">
            <thead>
            <tr>
              <th>ID</th>
              <th>Rent date</th>
              <th>Return date</th>
              <th>Borrowed by</th>
              <th>Borrowed to</th>
              <th>Movie</th>
              <th>is Returned?</th>
              <th>Actions</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="r in filteredRentals" :key="r.id">
              <th>{{r.id}}</th>
              <td>{{ new Date(r.date).toLocaleString('en-GB', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' }) }}</td>
              <td>
                {{ r.returnDate
                ? new Date(r.returnDate).toLocaleString('en-GB', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' })
                : '--' }}
              </td>
              <td>{{ r.borrowedByName }}</td>
              <td>{{ r.borrowedToName }}</td>
              <td>{{ r.movieTitle }}</td>
              <td>{{ r.isDeleted ? 'Yes' : 'No' }}</td>
              <td>
                <button class="btn btn-danger btn-sm" @click="deleteRental(r.id)">Return (delete)</button>
              </td>
            </tr>
            <tr v-if="filteredRentals.length === 0">
              <td colspan="8" class="text-center">No copies found.</td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import emitter from "@/utils/emitter.js";
const route = useRoute()

const rentals = ref([])
const searchQuery = ref("")

const filteredRentals = computed(() => {
  return rentals.value.filter(r =>
    r.borrowedToName.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
})

const fetchRentals = async () => {
  const res = await fetch(`http://localhost:5222/api/Rental`)
  if (res.status === 404) {
    rentals.value = []
    return
  }
  const data = await res.json()
  rentals.value = data
}

const deleteRental = async (rentalId) => {
  if (!confirm('Are you sure you want to delete this copy?')) return

  try {
    const response = await fetch(`http://localhost:5222/api/Rental/${rentalId}`, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    })

    if (!response.ok) {
      throw new Error(await response.text())
    }

    emitter.emit('toast', {
      message: 'Rental deleted successfully.',
      type: 'success'
    })

    await fetchRentals()
  } catch (error) {
    emitter.emit('toast', {
      message: 'Failed to delete rental (' + error.message + ')',
      type: 'error'
    })
  }
}
const returnRental = async (rental) => {
  try {
    const response = await fetch(`http://localhost:5222/api/Rental/${rental.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify({
        date: rental.date,
        returnDate: new Date().toISOString()
      })
    });

    if (!response.ok) {
      throw new Error(`Greška: ${response.status}`);
    }

    const data = await response.json();
    console.log("Uspješno vraćeno:", data);

  } catch (error) {
    console.error("Greška prilikom vraćanja:", error);
  }
};


onMounted(() => {
  fetchRentals()
})
</script>
