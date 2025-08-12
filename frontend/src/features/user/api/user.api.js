//This file manages user-related API calls

export async function getUser(id, token) {
  const res = await fetch(`http://localhost:5222/api/User/${id}`, {
    headers: token ? { 'Authorization': `Bearer ${token}` } : {}
  })
  if (!res.ok) throw new Error('Failed to load user')
  return await res.json()
}

export async function updateUser(id, payload, token) {
  const res = await fetch(`http://localhost:5222/api/User/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { 'Authorization': `Bearer ${token}` } : {})
    },
    body: JSON.stringify(payload)
  })
  if (!res.ok) throw new Error('Failed to update user')
  const text = await res.text()
  return text ? JSON.parse(text) : {}
}


export async function getUserRentals(token) {
  const res = await fetch('http://localhost:5222/api/Rental', {
    headers: token ? { 'Authorization': `Bearer ${token}` } : {}
  })
  if (!res.ok) throw new Error('Failed to fetch rentals')
  return await res.json()
}

// FAVORITES (već imaš, ostavljam radi kompletnosti)
export async function getUserFavorites(userId, token) {
  const res = await fetch(`http://localhost:5222/api/UserFavoriteMovies/${userId}`, {
    headers: token ? { 'Authorization': `Bearer ${token}` } : {}
  })
  if (!res.ok) throw new Error('Failed to fetch favorites')
  return await res.json()
}

export async function addFavorite(payload, token) {
  const res = await fetch('http://localhost:5222/api/UserFavoriteMovies', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { 'Authorization': `Bearer ${token}` } : {})
    },
    body: JSON.stringify(payload)
  })
  if (!res.ok) throw new Error('Failed to add favorite')
  return true
}

export async function removeFavorite(userId, movieId, token) {
  const res = await fetch(
    `http://localhost:5222/api/UserFavoriteMovies?userId=${userId}&movieId=${movieId}`,
    {
      method: 'DELETE',
      headers: token ? { 'Authorization': `Bearer ${token}` } : {}
    }
  )
  if (!res.ok) throw new Error('Failed to remove favorite')
  return true}