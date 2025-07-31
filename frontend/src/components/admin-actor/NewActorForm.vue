<template>
  <div class="modal fade show d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);" role="dialog">
    <div class="modal-dialog modal-lg" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Add New Actor</h5>
          <button type="button" class="close-btn" @click="$emit('cancel')" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitActor">
            <div class="form-group mb-3">
              <label>Name</label>
              <input type="text" v-model="actor.name" class="form-control" required />
            </div>
            <div class="form-group mb-3">
              <label>Date of birth</label>
              <input type="date" v-model="actor.dob" class="form-control" required />
            </div>
            <div class="form-group mb-3">
              <label>Biography</label>
              <textarea type="text" v-model="actor.bio" class="form-control" rows="5" required />
            </div>
          </form>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Cancel</button>
          <button type="button" class="btn btn-primary" @click="submitActor">Submit</button>
        </div>
      </div>
    </div>
  </div>
</template>


<script setup>
import {ref } from 'vue'
import emitter from "@/utils/emitter.js";

const emit = defineEmits(['submitted', 'cancel'])

const actor = ref({
  name: '',
  dob: '',
  bio: '',
})

const submitActor = () => {
  fetch('http://localhost:5222/api/Actor', {
    method: 'POST',
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${localStorage.getItem("token")}`
    },
    body: JSON.stringify(actor.value)
  })
    .then(res => {
      if (!res.ok) throw new Error('Failed to submit')
      return res.json()
    })
    .then(() => {
      emitter.emit('toast', {
        message: 'Actor added successfully',
        type: 'success'
      })
      emit('submitted')
    })
    .catch(err =>
      emitter.emit('toast', {
        message: 'Failed to add actor (' + err.message + ')',
        type: 'error'
      })
    )
}
</script>

<style scoped>
.modal-body {
  max-height: 70vh;
  overflow-y: auto;
}
</style>

