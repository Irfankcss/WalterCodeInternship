<template>
  <div class="custom-toast-container">
    <div
      v-for="(toast, index) in toasts"
      :key="index"
      class="toast show align-items-center text-white mb-2"
      :class="toast.type === 'success' ? 'bg-success' : 'bg-danger'"
      role="alert"
    >
      <div class="d-flex">
        <div class="toast-body">{{ toast.message }}</div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import emitter from '../utils/emitter'

const toasts = ref([])

const remove = (index) => {
  toasts.value.splice(index, 1)
}

onMounted(() => {
  emitter.on('toast', ({ message, type }) => {
    toasts.value.push({ message, type })
    setTimeout(() => remove(0), 4000)
  })
})
</script>
<style scoped>
.custom-toast-container {
  position: fixed;
  top: 20px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 9999;
  padding: 1rem;
}
</style>
