// This file manages movie-related API calls

export async function getMovieById(id) {
  const res = await fetch(`http://localhost:5222/api/Movie/${id}`);
  if (!res.ok) throw new Error(`Failed to load movie ${id}`);
  return await res.json();
}

export async function getMovieCopies(movieId) {
  const data = await getMovieById(movieId);
  return data.movieCopies || [];
}

export async function getMovieGenres(movieId) {
  const res = await fetch(`http://localhost:5222/api/Movie/${movieId}/genres`);
  if (!res.ok) throw new Error(`API error: ${res.status}`);
  const text = await res.text();
  const data = text ? JSON.parse(text) : [];
  return Array.isArray(data) ? data : [];
}

export async function getActorsByMovieId(movieId) {
  const res = await fetch(`http://localhost:5222/api/Actor/GetByMovieID?MovieId=${movieId}`);
  if (!res.ok) throw new Error('Failed to load actors');
  return await res.json();
}

export async function getAllMovieRatings() {
  const res = await fetch('http://localhost:5222/api/MovieRatings');
  if (!res.ok) throw new Error('Failed to load ratings');
  return await res.json();
}

export async function getMovieRatingsByMovieId(movieId) {
  const all = await getAllMovieRatings();
  return all.filter(r => r.movieId === Number(movieId));
}

export async function postMovieRating(reviewPayload) {
  const response = await fetch('http://localhost:5222/api/MovieRatings', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(reviewPayload)
  });

  const responseText = await response.text();

  if (!response.ok) {
    const err = new Error(responseText || `Server error: ${response.status}`);
    err.status = response.status;
    throw err;
  }
  return JSON.parse(responseText);
}

export async function getAllRentals() {
  const res = await fetch('http://localhost:5222/api/Rental');
  if (!res.ok) throw new Error('Failed to load rentals');
  return await res.json();
}

export async function getRentalsByMovieId(movieId) {
  const all = await getAllRentals();
  return all.filter(r => r.movieId === Number(movieId));
}

export async function postRental(payload) {
  const res = await fetch('http://localhost:5222/api/Rental', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });

  const responseText = await res.text();

  if (!res.ok) {
    const err = new Error(responseText || 'Rent failed');
    err.status = res.status;
    throw err;
  }
  return responseText;
}