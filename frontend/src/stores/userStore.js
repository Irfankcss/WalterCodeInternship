import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useUserStore = defineStore('user', () => {
  const user = ref({
    id: 1,
    name: "Pera Perić",
    username: "pera_peric",
    email: "pera@example.com",
    dob: "1990-05-15",
    password: "123456",
    avatarUrl: "/avatars/avatar.jpg",
    favoriteMovies: [],
    rentals: []
  });

  const toggleFavorite = (movieId) => {
    const index = user.value.favoriteMovies.indexOf(movieId);
    if (index === -1) {
      user.value.favoriteMovies.push(movieId);
    } else {
      user.value.favoriteMovies.splice(index, 1);
    }
  };

  return { user, toggleFavorite };
});
