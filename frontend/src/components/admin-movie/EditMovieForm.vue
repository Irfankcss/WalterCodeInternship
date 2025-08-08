<template>
  <div class="modal fade show d-block" tabindex="-1" style="background: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-lg modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Edit Movie</h5>
          <button type="button" class="btn-close" @click="$emit('cancel')"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitEdit">
            <div class="mb-3">
              <label class="form-label">Title</label>
              <input type="text" class="form-control" :value="movie.name" readonly />
            </div>
            <div class="mb-3">
              <label class="form-label">Year</label>
              <input type="number" class="form-control" :value="movie.year" readonly />
            </div>
            <div class="mb-3">
              <label class="form-label">Last edited by</label>
              <input type="text" class="form-control" :value="movie.editedByUsername" readonly />
            </div>
            <div class="mb-3">
              <label class="form-label">IMDb ID</label>
              <input type="text" class="form-control" :value="movie.imdbId" readonly />
            </div>
            <div class="mb-3">
              <label class="form-label">IMDb Rating</label>
              <input type="number" step="0.1" class="form-control" v-model="editedMovie.imdbRating" />
            </div>
            <div class="mb-3">
              <label class="form-label">Description</label>
              <input type="text" class="form-control" v-model="editedMovie.description" />
            </div>
        <div class="mb-3">
          <label class="form-label">Poster URL</label>
          <input type="text" class="form-control" v-model="editedMovie.poster" />
        </div>

        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Cancel</button>
          <button type="submit" class="btn btn-primary">Save Changes</button>
        </div>
        </form>
      </div>
    </div>
  </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import emitter from "@/utils/emitter.js";
const props = defineProps({
  movie: Object
})

const emit = defineEmits(['updated', 'cancel'])
const editedMovie = ref({
  imdbRating: props.movie.imdbRating,
  poster: props.movie.poster
})

const submitEdit = async () => {
  try {
    const response = await fetch(`http://localhost:5222/api/Movie/${props.movie.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify(editedMovie.value)
    })
    if (!response.ok) throw new Error('Failed to update movie.')

    emitter.emit('toast', {
      message: 'Movie updated successfully.',
      type: 'success'
    })

    emit('updated')
  } catch (error) {
    emitter.emit('toast', {
      message: 'Failed to update movie' + " (" + error.message + ")",
      type: 'error'
    })
  }
}
</script>
