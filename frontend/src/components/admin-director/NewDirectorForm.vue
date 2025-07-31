<template>
  <div class="modal fade show d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);" role="dialog">
    <div class="modal-dialog modal-lg" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Add New Director</h5>
          <button type="button" class="close-btn" @click="$emit('cancel')" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitDirector">
            <div class="form-group mb-3">
              <label>Name</label>
              <input type="text" v-model="director.name" class="form-control" required />
            </div>
            <div class="form-group mb-3">
              <label>Date of birth</label>
              <input type="date" v-model="director.dob" class="form-control" required />
            </div>
            <div class="form-group mb-3">
              <label>Biography</label>
              <textarea type="text" v-model="director.bio" class="form-control" rows="5" required />
            </div>
          </form>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Cancel</button>
          <button type="button" class="btn btn-primary" @click="submitDirector">Submit</button>
        </div>
      </div>
    </div>
  </div>
</template>


<script setup>
import {ref } from 'vue'
import emitter from "@/utils/emitter.js";

const emit = defineEmits(['submitted', 'cancel'])

const director = ref({
  name: '',
  dob: '',
  bio: '',
})

const submitDirector = () => {
  fetch('http://localhost:5222/api/Director', {
    method: 'POST',
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`
    },
    body: JSON.stringify(director.value)
  })
    .then(res => {
      if (!res.ok) throw new Error('Failed to submit')
      return res.json()
    })
    .then(() => {
      emitter.emit("toast",{
        message: 'Director added successfully',
        type: 'success'
      })
      emit('submitted')
    })
    .catch(err =>
      emitter.emit("toast",{message: "Failed to add director (" + err.message + ")", type: 'error'})
    )
}
</script>

<style scoped>
.modal-body {
  max-height: 70vh;
  overflow-y: auto;
}
</style>

