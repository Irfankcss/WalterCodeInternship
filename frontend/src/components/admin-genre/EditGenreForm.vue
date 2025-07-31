<template>
  <div class="modal fade show d-block" tabindex="-1" style="background: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-lg modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Edit Genre</h5>
          <button type="button" class="btn-close" @click="$emit('cancel')"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitEdit">
            <div class="mb-3">
              <label class="form-label">Name</label>
              <input type="text" class="form-control" v-model="editedGenre.name" />
            </div>
            <div class="mb-3">
              <label class="form-label">Description</label>
              <textarea class="form-control" v-model="editedGenre.description" rows="3"></textarea>
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
  genre: Object
})

const emit = defineEmits(['updated', 'cancel'])

const editedGenre = ref({
  name: props.genre.name,
  description: props.genre.description
})

const submitEdit = async () => {
  try {
    const response = await fetch(`http://localhost:5222/api/Genre/${props.genre.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify(editedGenre.value)
    })

    if (!response.ok) throw new Error('Failed to update genre.')
    emitter.emit('toast', {
      message: 'Genre updated successfully.',
      type: 'success'
    })
    emit('updated')
  } catch (error) {
    emitter.emit('toast', {
      message: "Failed to update genre (" + error.message + ")",
      type: 'error'
    })
  }
}
</script>
