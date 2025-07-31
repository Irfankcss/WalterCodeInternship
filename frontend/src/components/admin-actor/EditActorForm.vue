<template>
  <div class="modal fade show d-block" tabindex="-1" style="background: rgba(0,0,0,0.5);">
    <div class="modal-dialog modal-lg modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Edit Actor</h5>
          <button type="button" class="btn-close" @click="$emit('cancel')"></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitEdit">
            <div class="mb-3">
              <label class="form-label">Name</label>
              <input type="text" class="form-control" v-model="editedActor.name" />
            </div>
            <div class="mb-3">
              <label class="form-label">Date of birth</label>
                <input type="date" class="form-control" v-model="editedActor.dob" />
            </div>
            <div class="mb-3">
              <label class="form-label">Biography</label>
              <textarea class="form-control" v-model="editedActor.bio" rows="5"></textarea>
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
  actor: Object
})

const emit = defineEmits(['updated', 'cancel'])

const editedActor = ref({
  name: props.actor.name,
  dob: props.actor.dob,
  bio: props.actor.bio
})

const submitEdit = async () => {
  try {
    const response = await fetch(`http://localhost:5222/api/Actor/${props.actor.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify(editedActor.value)
    })

    if (!response.ok) throw new Error('Failed to update actor.')
    emitter.emit('toast', {
      message: 'Actor updated successfully.',
      type: 'success'
    })

    emit('updated')
  } catch (error) {
    emitter.emit('toast', {
      message:'Failed to update actor' + " (" + error.message + ")",
      type: 'error'
    })
  }
}
</script>
