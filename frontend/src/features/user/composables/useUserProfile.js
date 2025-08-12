//This file manages user profile functionalities

import { ref, reactive, computed, onMounted } from 'vue'
import jwtDecode from 'jwt-decode'
import emitter from '@/utils/emitter'
import {
  getUser,
  updateUser,
  getUserRentals,
  getUserFavorites
} from '@/features/user/api/user.api'

export function useUserProfile() {
  const user = reactive({
    id: null,
    name: '',
    username: '',
    email: '',
    dob: '',
    avatarUrl: '',
    favoriteMovies: [],
    rentals: []
  })

  const UniversalImage = ref('static/sbadmin/img/undraw_profile.svg')
  const isEditing = ref(false)
  const successMessage = ref('')
  const tempAvatar = ref('')
  const showHistory = ref(false)
  const form = reactive({ name: '', username: '', email: '', dob: '' })
  const formLabels = {
    name: 'Full Name',
    username: 'Username',
    email: 'Email',
    dob: 'Date of Birth'
  }
  const favoriteMoviesList = ref([])
  const userId = ref(null)
  const formatDate = (dateStr) => new Date(dateStr).toLocaleDateString('en-GB')

  const toggleEdit = () => {
    if (isEditing.value) {
      Object.assign(form, user)
      tempAvatar.value = user.avatarUrl
    }
    isEditing.value = !isEditing.value
  }

  const saveChanges = async () => {
    if (!form.name || !form.username || !form.email || !form.dob) {
      alert('Please fill in all required fields.')
      return
    }
    const token = localStorage.getItem('token')
    if (!token) return

    const updatedData = {
      name: form.name,
      username: form.username,
      email: form.email,
      dob: new Date(form.dob).toISOString(),
      admin: user.admin
    }
    try {
      const updatedUser = await updateUser(userId.value, updatedData, token)
      Object.assign(user, updatedUser)
      Object.assign(form, updatedUser)
      tempAvatar.value = updatedUser.avatarUrl || ''
      emitter.emit('toast', { message: 'Profile updated successfully.', type: 'success' })
      isEditing.value = false
      await fetchUserProfile()
    } catch (err) {
      emitter.emit('toast', { message: 'Failed to update profile.' + err.message, type: 'error' })
    }
  }

  const fetchUserProfile = async () => {
    const token = localStorage.getItem('token')
    if (!token) return
    const decoded = jwtDecode(token)
    const id = Number(decoded.UserId)
    userId.value = id

    try {
      const data = await getUser(id, token)
      Object.assign(user, data)
      Object.assign(form, data)
      tempAvatar.value = data.avatarUrl || ''
    } catch (err) {
      console.error('Error fetching user:', err)
    }
  }

  const fetchRentals = async () => {
    try {
      const token = localStorage.getItem('token')
      const all = await getUserRentals(token)
      user.rentals = all.filter(r => Number(r.borrowedToId) === Number(userId.value))
    } catch (err) {
      console.error('Error fetching rentals:', err)
    }
  }

  const fetchFavorites = async () => {
    const token = localStorage.getItem('token')
    if (!token) return
    const decoded = jwtDecode(token)
    const uid = decoded.UserId || decoded.userId

    try {
      const data = await getUserFavorites(uid, token)
      favoriteMoviesList.value = data
    } catch (err) {
      console.error('Error fetching favorites:', err)
    }
  }

  const today = new Date(); today.setHours(0, 0, 0, 0)

  const activeRentals = computed(() =>
    user.rentals?.filter(r => {
      const d = new Date(r.returnDate)
      d.setHours(0, 0, 0, 0)
      return d >= today
    }) || []
  )

  const pastRentals = computed(() =>
    user.rentals?.filter(r => {
      const d = new Date(r.returnDate)
      d.setHours(0, 0, 0, 0)
      return d < today
    }) || []
  )

  onMounted(() => {
    const token = localStorage.getItem('token')
    if (!token) window.location.href = '/login'
    try {
      const decoded = jwtDecode(token)
      userId.value = Number(decoded.UserId)
    } catch {}
    fetchUserProfile()
    fetchRentals()
    fetchFavorites()
  })

  return {
    // state
    user, UniversalImage, isEditing, successMessage, tempAvatar, showHistory,
    form, formLabels, favoriteMoviesList,

    // methods
    toggleEdit, saveChanges, formatDate,

    // computed
    activeRentals, pastRentals
  }
}
