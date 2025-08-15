// This file manages movie details functionalities

import { ref, computed, onMounted } from 'vue'
import jwtDecode from 'jwt-decode'
import emitter from '@/utils/emitter'

import {
  getMovieById,
  getMovieCopies,
  getMovieGenres,
  getActorsByMovieId,
  getMovieRatingsByMovieId,
  getRentalsByMovieId,
  getAllRentals,
  postMovieRating,
  postRental
} from '@/features/movies/api/movies.api'

import {
  getUserFavorites,
  addFavorite,
  removeFavorite
} from '@/features/user/api/user.api'

export function useMovieDetails(movieIdRef) {
  const isLoggedIn = computed(() => !!localStorage.getItem('token'))
  const movie = ref(null)
  const copies = ref([])
  const hasAnyCopies = computed(() => (copies.value?.length || 0) > 0)
  const newReview = ref({ comment: '', rating: '' })
  const selectedCopyId = ref(null)
  const returnDate = ref(null)
  const today = new Date().toLocaleDateString('en-CA')
  const maxReturnDate = new Date(Date.now() + 14 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]

  const activeRentals = ref([])
  const todayIso = () => new Date().toISOString().slice(0, 10)

  const isCopyRented = (copyId) => {
    return activeRentals.value.some(
      r => r.movieCopyId === copyId && new Date(r.returnDate) >= new Date()
    )
  }

  const availableCopies = computed(() => copies.value.filter(c => !isCopyRented(c.id)))

  const showCopies = ref(false)

  async function fetchActiveRentals() {
    try {
      const all = await getAllRentals()
      activeRentals.value = all.filter(r =>
        copies.value.some(c => c.id === r.movieCopyId) &&
        new Date(r.returnDate) >= new Date() &&
        !r.isDeleted
      )
    } catch (err) {
      console.error('Error loading rentals', err)
    }
  }

  const toggleShowCopies = async () => {
    showCopies.value = !showCopies.value
    selectedCopyId.value = null
    returnDate.value = null
    if (showCopies.value) {
      await fetchActiveRentals()
    }
  }

  const fetchCopies = async () => {
    try {
      copies.value = await getMovieCopies(movieIdRef.value)
    } catch (error) {
      console.error('Error fetching movie copies:', error)
    }
  }

  const isFavorite = ref(false)

  const fetchFavoriteStatus = async () => {
    const token = localStorage.getItem('token')
    if (!token) return

    const decodedToken = jwtDecode(token)
    const userId = decodedToken.UserId || decodedToken.userId

    try {
      const favs = await getUserFavorites(userId)
      isFavorite.value = favs.some(f => f.movieId === Number(movieIdRef.value))
    } catch (err) {
      console.error('Error checking favorite status:', err)
    }
  }

  const toggleFavorite = async () => {
    const token = localStorage.getItem('token')
    if (!token) {
      emitter.emit('toast', {
        message: 'You must be logged in to use favorites.',
        type: 'error'
      })
      return
    }

    const decodedToken = jwtDecode(token)
    const userId = decodedToken.UserId || decodedToken.userId

    if (isFavorite.value) {
      try {
        await removeFavorite(userId, movie.value.id)
        isFavorite.value = false
        emitter.emit('toast', { message: 'Removed from favorites.', type: 'success' })
      } catch (err) {
        console.error('Error removing favorite:', err)
        emitter.emit('toast', { message: 'Failed to remove from favorites.', type: 'error' })
      }
    } else {
      const payload = {
        userId,
        movieId: movie.value.id,
        movieTitle: movie.value.title,
        moviePoster: movie.value.imageUrl
      }
      try {
        await addFavorite(payload)
        isFavorite.value = true
        emitter.emit('toast', { message: 'Added to favorites.', type: 'success' })
      } catch (err) {
        console.error('Error adding favorite:', err)
        emitter.emit('toast', { message: 'Failed to add to favorites.', type: 'error' })
      }
    }
  }

  const confirmRent = async () => {
    const copy = copies.value.find(c => c.id === selectedCopyId.value)
    if (!copy || !returnDate.value) {
      emitter.emit('toast', { message: 'Please select a copy and return date.', type: 'error' })
      return
    }

    const token = localStorage.getItem('token')
    const decodedToken = jwtDecode(token)
    const userId = decodedToken.UserId

    const payload = {
      movieId: Number(movieIdRef.value),
      movieCopyId: copy.id,
      borrowedById: 1,
      borrowedToId: userId,
      returnDate: returnDate.value
    }

    try {
      const responseText = await postRental(payload)
      emitter.emit('toast', {
        message: `Movie rented successfully until ${returnDate.value}`,
        type: 'success'
      })

      await fetchCopies()
      await fetchActiveRentals()

      showCopies.value = false
      selectedCopyId.value = null
      returnDate.value = null
    } catch (err) {
      console.error('Rent request failed:', err)
      emitter.emit('toast', {
        message: `Rent failed: ${err.message || 'Could not process rental. Try again later.'}`,
        type: 'error'
      })
    }
  }

  const actors = ref([])
  const genre = ref('')
  const reviews = ref([])
  const rental = ref([])

  const submitReview = async () => {
    if (!newReview.value.comment || !newReview.value.rating) {
      alert('Please enter a comment and rating.')
      return
    }

    const token = localStorage.getItem('token')
    const decodedToken = jwtDecode(token)
    const userId = decodedToken.UserId || decodedToken.userId

    const alreadyExists = reviews.value.some(
      r => r.userId === userId || r.userID === userId
    )
    if (alreadyExists) {
      emitter.emit('toast', { message: '⚠️ You have already reviewed this movie.', type: 'error' })
      return
    }

    const reviewPayload = {
      movieID: Number(movieIdRef.value),
      userID: userId,
      rating: Number(newReview.value.rating),
      comment: newReview.value.comment
    }

    try {
      const savedReview = await postMovieRating(reviewPayload) 
      reviews.value.push({
        rating: savedReview.rating,
        comment: savedReview.comment,
        userId: currentUserId.value
      })
      newReview.value.comment = ''
      newReview.value.rating = ''
      emitter.emit('toast', { message: '✅ Review submitted successfully!', type: 'success' })
    } catch (error) {
      console.error('❌ Error submitting review:', error)
      const msg = (error.message || '').includes('Duplicate entry')
        ? '⚠️ You have already reviewed this movie.'
        : `❌ Error submitting review: ${error.message}`
      emitter.emit('toast', { message: msg, type: 'error' })
    }
  }
  onMounted(async () => {
    await fetchFavoriteStatus()
    try {
      reviews.value = await getMovieRatingsByMovieId(movieIdRef.value)
    } catch (err) {
      console.error('Error fetching reviews:', err)
    }

    try {
      rental.value = await getRentalsByMovieId(movieIdRef.value)
    } catch (err) {
      console.error('Error fetching rentals:', err)
    }
    try {
      actors.value = await getActorsByMovieId(movieIdRef.value)
    } catch (err) {
      console.error('Error fetching actors:', err)
    }
    try {
      const g = await getMovieGenres(movieIdRef.value)
      genre.value = g.map(x => x.name).join(', ')
    } catch (err) {
      console.error('Error fetching genres:', err)
    }
    try {
      const data = await getMovieById(movieIdRef.value)
      movie.value = {
        ...data,
        title: data.name,
        imageUrl: data.poster,
        publishedYear: data.year,
        description: data.description || data.decs || data.backText || data.frontText || 'No description available',
        backText: data.backText || '',
        frontText: data.frontText || '',
        genre: data.genre || '',
        imdbRating: data.imdbRating,
        director: data.director?.name || '',
        reviews: JSON.parse(localStorage.getItem(`reviews_${data.id}`)) || []
      }
    } catch (err) {
      console.error('Error fetching movie:', err)
    }

    await fetchCopies()
  })

  return {
    // computed
    isLoggedIn,
    hasAnyCopies,
    availableCopies,

    // state
    movie,
    copies,
    newReview,
    selectedCopyId,
    returnDate,
    today,
    maxReturnDate,
    activeRentals,
    showCopies,
    isFavorite,
    actors,
    genre,
    reviews,
    rental,

    // methods
    todayIso,
    isCopyRented,
    fetchActiveRentals,
    toggleShowCopies,
    fetchCopies,
    fetchFavoriteStatus,
    confirmRent,
    toggleFavorite,
    submitReview
  }
}
