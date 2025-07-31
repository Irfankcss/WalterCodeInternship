<template>
  <div class="modal fade show d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);" role="dialog">
    <div class="modal-dialog modal-lg" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Add New Genre</h5>
          <button type="button" class="close-btn" @click="$emit('cancel')" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitGenre">
            <div class="form-group mb-3">
              <label>Name</label>
              <input type="text" v-model="genre.name" class="form-control" required />
            </div>
            <div class="form-group mb-3">
              <label>Description</label>
              <input type="text" v-model="genre.description" class="form-control" required />
            </div>
          </form>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Cancel</button>
          <button type="button" class="btn btn-primary" @click="submitGenre">Submit</button>
        </div>
      </div>
    </div>
  </div>
</template>


<script setup>
import {ref } from 'vue'
import emitter from "@/utils/emitter.js";
const emit = defineEmits(['submitted', 'cancel'])

const genre = ref({
  name: '',
  description: '',
})

const submitGenre = () => {
  fetch('http://localhost:5222/api/Genre', {
    method: 'POST',
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`
    },
    body: JSON.stringify(genre.value)
  })
    .then(res => {
      if (!res.ok) throw new Error('Failed to submit')
      return res.json()
    })
    .then(() => {
      emitter.emit("toast",{
        message: 'Genre added successfully',
        type: 'success'
      })
      emit('submitted')
    })
    .catch(err =>
      emitter.emit("toast",{message: "Failed to add genre (" + err.message + ")", type: 'error'}))
}
</script>

<style scoped>
.modal-body {
  max-height: 70vh;
  overflow-y: auto;
}
</style>

