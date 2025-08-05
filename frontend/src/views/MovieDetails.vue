<template>
  <div class="movie-details-container" v-if="movie">
    <div class="movie-header">
      <div class="container">
        <div class="row">
          <div class="col-md-4">
            <img :src="movie.imageUrl" :alt="'Poster za ' + movie.title" class="movie-poster">
          </div>
          <div class="col-md-8 movie-info">
            <h1 class="movie-title">{{ movie.title }}</h1>
            <p class="movie-director">{{ movie.director }} ({{ movie.publishedYear }})</p>

            <div class="movie-meta">
              <div class="meta-item">
                <span class="meta-label">IMDb rating:</span>
                <span class="meta-value">{{ movie.imdbRating }}/10</span>
              </div>
              <div class="meta-item">
                <span class="meta-label">Žanr:</span>
                <span class="meta-value">{{ movie.genre }}</span>
              </div>
              <div class="meta-item">
                <span class="meta-label">Trajanje:</span>
                <span class="meta-value">142 min</span>
              </div>
            </div>

            <p class="movie-description">{{ movie.backText }}</p>

            <div class="action-buttons mb-3">
              <button class="btn btn-rent" @click="toggleShowCopies">
                {{ showCopies ? 'Cancel' : 'Rent movie' }}
              </button>
              <button class="btn btn-favorite" :class="{ active: isFavorite }" @click="toggleFavorite">
                {{ isFavorite ? '★ Remove from favorites' : '☆ Add to favorites' }}
              </button>
              <router-link to="/movies" class="btn btn-back">
                Back to movie list
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="showCopies && copies.length" class="container mt-4">
      <h5>Choose a copy and return date (maksimalno 14 dana):</h5>

      <div class="d-flex flex-wrap gap-3">
        <div v-for="copy in copies" :key="copy.id" class="card"
          :class="{ 'border-primary': selectedCopyId === copy.id }" style="width: 18rem; cursor: pointer;"
          @click="selectedCopyId = copy.id">
          <div class="card-body">
            <h5 class="card-title">Serial Number: {{ copy.serialNumber }}</h5>
            <p class="card-text">{{ copy.description || 'No description.' }}</p>
          </div>
        </div>
      </div>

      <div class="mt-3">
        <label>Return date:</label>
        <input type="date" v-model="returnDate" :min="today" :max="maxReturnDate"
          class="form-control w-auto d-inline-block ms-2">
        <button class="btn btn-success ms-3" @click="confirmRent" :disabled="!selectedCopyId || !returnDate">
          Confirm rental
        </button>
      </div>
    </div>

    <div class="movie-content container">
      <div class="row">
        <div class="col-md-8">
          <section class="movie-section">
            <h2 class="section-title">Description</h2>
            <p>{{ movie.frontText }}</p>
          </section>

          <section class="movie-section">
            <h2 class="section-title">Cast</h2>
            <div class="cast-grid">
              <div class="cast-member" v-for="actor in mainCast" :key="actor.name">
                <img :src="actor.image" :alt="actor.name" class="actor-avatar" />
                <span class="actor-name">{{ actor.name }}</span>
              </div>
            </div>
          </section>

          <section class="movie-section">
            <h2 class="section-title">Comments and Ratings</h2>
            <div class="mb-3">
              <textarea v-model="newReview.comment" class="form-control mb-2"
                placeholder="Write a comment..."></textarea>
              <select v-model.number="newReview.rating" class="form-control mb-2">
                <option disabled value="">Rate (1-5)</option>
                <option v-for="n in 5" :key="n" :value="n">{{ n }}</option>
              </select>
              <button class="btn btn-success" @click="submitReview">Add review</button>
            </div>

            <div v-if="movie.reviews.length">
              <div v-for="(review, index) in movie.reviews" :key="index" class="mb-3 border p-3 rounded">
                <p class="mb-1"><strong>Rating:</strong> {{ review.rating }}/5</p>
                <p class="mb-0"><em>{{ review.comment }}</em></p>
              </div>
            </div>
            <div v-else class="text-muted">No comments yet for this movie.</div>
          </section>
        </div>

        <div class="col-md-4">
          <section class="movie-section">
            <h2 class="section-title">Ratings</h2>
            <div class="rating-box">
              <div class="rating-value">{{ movie.imdbRating }}</div>
              <div class="rating-stars">
                <span class="star" v-for="n in 5" :key="n">★</span>
              </div>
              <p class="rating-source">IMDb rating</p>
            </div>
          </section>
        </div>
      </div>
    </div>
  </div>

  <div v-else class="container mt-4">
    <div class="alert alert-warning">Movie not found</div>
  </div>
</template>



<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import { useUserStore } from '@/stores/userStore';

const userStore = useUserStore();
const route = useRoute();
const imdbID = route.params.imdbID;
const movie = ref(null);
const newReview = ref({ comment: '', rating: '' });

const selectedCopyId = ref(null);
const returnDate = ref(null);
const today = new Date().toISOString().split('T')[0];
const maxReturnDate = new Date(Date.now() + 14 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];

const copies = ref([]);
const showCopies = ref(false);
const toggleShowCopies = () => {
  showCopies.value = !showCopies.value;
  selectedCopyId.value = null;
  returnDate.value = null;
};

const activeRentals = computed(() => {
  const todayDate = new Date();
  todayDate.setHours(0, 0, 0, 0);
  return userStore.user.rentals?.filter(r => {
    const rentedDate = new Date(r.rentedUntil);
    rentedDate.setHours(0, 0, 0, 0);
    return rentedDate >= todayDate;
  }) || [];
});

const confirmRent = () => {
  const copy = copies.value.find(c => c.id === selectedCopyId.value);
  if (copy && returnDate.value) {
    if (activeRentals.value.length >= 3) {
      alert("⚠️ You can't rent more than 3 movies at the same time.");
      return;
    }

    const newRental = {
      title: movie.value.title,
      serialNumber: copy.serialNumber,
      rentedUntil: returnDate.value
    };

    userStore.user.rentals.push(newRental);
    localStorage.setItem('rentals', JSON.stringify(userStore.user.rentals));
    window.dispatchEvent(new Event('storage'));
    alert(`✅ Rental confirmed for '${copy.serialNumber}' until ${returnDate.value}`);

    showCopies.value = false;
    selectedCopyId.value = null;
    returnDate.value = null;
  }
};

const savedFavorites = localStorage.getItem('favorites');
if (savedFavorites) {
  userStore.user.favoriteMovies = JSON.parse(savedFavorites);
}

const isFavorite = computed(() => {
  return movie.value && userStore.user.favoriteMovies.includes(movie.value.imdbID);
});

const toggleFavorite = () => {
  if (movie.value) {
    userStore.toggleFavorite(movie.value.imdbID);
    localStorage.setItem('favorites', JSON.stringify(userStore.user.favoriteMovies));
  }
};

const allMovieCopies = [
  { id: 1, imdbID: 'tt0111161', serialNumber: "SN001", description: "Blu-ray copy" },
  { id: 2, imdbID: 'tt0111161', serialNumber: "SN002", description: "DVD copy" },
  { id: 3, imdbID: 'tt0068646', serialNumber: "SN003", description: "Collector's edition" },
  { id: 4, imdbID: 'tt0468569', serialNumber: "SN004", description: "4K UHD" },
  { id: 5, imdbID: 'tt0108052', serialNumber: "SN005", description: "Digital release" }
];
const allMovies = [
  {
    id: 1,
    imdbID: 'tt0111161',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMDAyY2FhYjctNDc5OS00MDNlLThiMGUtY2UxYWVkNGY2ZjljXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: 'The Shawshank Redemption',
    director: 'Frank Darabont',
    publishedYear: 1994,
    imdbRating: 9.3,
    frontText: 'A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict.',
    backText: 'Maintaining his innocence and trying to remain hopeful through simple compassion.',
    genre: 'Drama',
    reviews: []
  },
  {
    id: 2,
    imdbID: 'tt0068646',
    imageUrl: 'https://preview.redd.it/whats-the-most-famous-movie-youve-never-seen-and-really-v0-rw4x7o2pby5d1.jpeg?auto=webp&s=c94d8dcba68000e75702362e0a05e173933e84b9',
    title: 'The Godfather',
    director: 'Francis Ford Coppola',
    publishedYear: 1972,
    imdbRating: 9.2,
    frontText: 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.',
    backText: 'A classic mafia drama about power, family, and betrayal.',
    genre: 'Crime',
    reviews: []
  },
  {
    id: 3,
    imdbID: 'tt0468569',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_FMjpg_UX1000_.jpg',
    title: 'The Dark Knight',
    director: 'Christopher Nolan',
    publishedYear: 2008,
    imdbRating: 9.0,
    frontText: 'Batman faces the Joker, a criminal mastermind who plunges Gotham into chaos.',
    backText: 'A gripping superhero thriller with legendary performances.',
    genre: 'Action',
    reviews: []
  },
  {
    id: 4,
    imdbID: 'tt0108052',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BNjM1ZDQxYWUtMzQyZS00MTE1LWJmZGYtNGUyNTdlYjM3ZmVmXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg',
    title: "Schindler's List",
    director: 'Steven Spielberg',
    publishedYear: 1993,
    imdbRating: 8.9,
    frontText: 'In German-occupied Poland during World War II, Oskar Schindler gradually becomes concerned for his Jewish workforce.',
    backText: 'A powerful story of courage and humanity.',
    genre: 'History',
    reviews: []
  },
  {
    id: 5,
    imdbID: 'tt0133093',
    imageUrl: 'https://m.media-amazon.com/images/M/MV5BZGM1NDM3MTAtMmI0ZC00ZDAwLWEwY2EtNDdhYjZmMjJkNzM0XkEyXkFqcGc@._V1_.jpg',
    title: 'The Matrix',
    director: 'Lana Wachowski, Lilly Wachowski',
    publishedYear: 1999,
    imdbRating: 8.7,
    frontText: 'A computer hacker learns about the true nature of his reality and his role in the war against its controllers.',
    backText: 'A revolutionary sci-fi action film.',
    genre: 'Sci-Fi',
    reviews: []
  }
];

const mainCast = ref([
  { name: 'Tim Robbins', image: 'https://ntvb.tmsimg.com/assets/assets/680595_v9_bc.jpg' },
  { name: 'Morgan Freeman', image: 'https://m.media-amazon.com/images/M/MV5BMTc0MDMyMzI2OF5BMl5BanBnXkFtZTcwMzM2OTk1MQ@@._V1_FMjpg_UX1000_.jpg' },
  { name: 'Bob Gunton', image: 'data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxITEhUSExMWFhUVFxcVGBgXFxUXFRcVFRUXFxcXFRUYHSggGBolHRUXITEhJSkrLi4uFx8zODMtNygtLisBCgoKDg0OGhAQGi0dHx0rLS0tLS0tLS0tKy0tKy0tKy0tLS0tKy0tLS0tLS0tLS0tLTctKy0rLS0tLS0tLS0tK//AABEIARMAtwMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAABAAIEBQYDBwj/xAA6EAABAwEGAwYFBAAGAwEAAAABAAIRAwQFEiExQVFhcQYigZGhsRMywdHwI0Lh8QcUJDNSYnKSskP/xAAZAQADAQEBAAAAAAAAAAAAAAAAAQMCBAX/xAAoEQEBAAICAgEDAwUBAAAAAAAAAQIRAyESMUEiUWFCcaEEEzNDgRT/2gAMAwEAAhEDEQA/AKvCiQrOjdDnU3uBnAYjjGqrwETKX18FZYYAipH+WIZiPHLouROScsvo7jZ7NCMoJFNk4ppShJzUAEHJTCr7deTWGARI15JGnymkrPVL4qO+UQOJ+ya21Vz+6PJGw0UoOVGLTVGZcM11F5vGoBRsaXFJhcYGqltumqdGqrsNuDiCMirtl/1RpHkpZ3P9OlMfDX1IlrsFSn8w8vqolWmRqI38FtbwYTZGvd8zy31KhdsqAbTpZZ8fBS4/6i2yWe9/wMsJ3YyUJVKZGRU26aOKoB4oXp/uHkref1+I8Po8vygELjatB4qSo9s0C2mgkJKTZbI+o7Axpc7WBySSD0+52f6es7/s9ZQWd/8Axd5FT7lvg0HGRiaduZ3VrW7VsOlL1C55OTjyupvanWXuqy8aeGi3oFUFyvL9pywP0xZxtmqIqnBd4bPnv1T9iCTnIIK6LS9jLnp2mo5tSYDZyMZypvaa6bHTAbSJxh3ezJyG2ab/AIbVQLQ8THc+qq+118l9ofEANJaDwAME8zEeYR8GzN7VpkMbhbplm7xKpqV3Fxz5wJ8yeSsLVbcRyEDQfU9ciub6xOQ8fDQdFi09H0rCwczxhSn0mxk0T0BPmVCp1c8pOepmPIZn0CsaJcRAmTwAB9NB1KW2vGqW02d4/sewUF9FxmPQLUts0nvR0Lp9AFFtbqTZmMtIny0T2WmXe17SCHQehHqAtf2MqUq7vhWh/wAN+WEyIfyz0PuqStWZGTR5KNUqjhHD+JSyls1LoR6h2ivemGsoM73w3NJO0NVhXFmtbG1KjwIHyzELzG77didhcZPGZn7qeXKP/nmpq6s+T8mxbYKDDNEy6OMrKW8nG6dZVn2X+Z55Krtrpe48ylxY3Hkst2rnd8cv5cIXG2DILsVwthMBdLnK7bwfQf8AEpnvQR4FJRCggNCUAimrRLy+qoNNkHh7KlckSeOQS2U+PDwmlOTPyuzZSqOgTqn0WSYGpPlxXa10gGxMAZuO/TqtsKn/ADrxm04dpGRPRcrRUL9TM6lNtZJg6DYbgaSeaVKgTkpZ56X4+Lfs6zUQYEK2o2Nvj/KVjskfnRWlGh9lzXOu/DhmkOjdbRkfNSqdjbxjgIj3Ur4JTXUDG6x51ScU+yBaLCI1y8gqS1XZy8ZWlDDpK51aQIghOZ0suHG/DH1rMRP55rjRomYMRwjL0V/arL+fdQqVGct9ir4cn3cXLwa7ivr3cWODm5HUbg/wp9GtiGcTurKy2c1KZY4ZtEtPPkfofNUJJY+Dxgg8fv7q7kq3slqdTMt3yK5OdJJTQEEam9i5XWiXK2jujqupXC2aBMkRJBBINEUAkShiWyI8E4lMahWfDSUGn3W2ZPDIdTsm3y2O7wznntKl9nGDCzKZBefEZT4KvvIl1WOcnqs301jO1XWZLwOAVhZKABz1UQf7iu7FTHBcedenxY9J1mo5ZDJS6dBDHER/PQqxosJzgD8/tSdUiLTondPfRgZKT8Jw8dkcJ3SaVr7PGwHOFCrU491eubkRv6KrtAkx6oKxRuEzsq2qIeIWktlEgdMlQWhvenmt41DOdad7A4h4B0O/PYHxVf2qsUOxcRPlseYVpZx3h+f2j2ldNMTqIz5Rr5g+a7cLuPK5JqqKx1MTAfzJdiVXWG0AHB+ZqfK0wco9r0C7NC52zQIJCSRQSDQJq0lg7F2mqzH3Wg6BxzK70ew1YsccQD2z3OPCDzW9BlFzrtlpHJaO/wDs0bLSY99QF7v2R5rPjRAT7hrwCJ2DfCJ+q72ykBJjM97pJMHyhUlCuGvMbR7fwrS1WiWjoPLCIlYyvSmE3VWw949VoLO7IZaa/wBKhAEk/kAyryy2poHePkuOzb0sMpFqxvJWlmqHh6/wqqx16ciX66c+EequrO8YcvyFjVdEyl9Gted/4SJ/MlJ+CO7zlB1nBdEaZjkYjLzRo9xDqMP5r5qFWs8hWlTQZ/mf54KNaHNwyCJ4SJ8kaFsUtppQCs/W1PgtBaLW2YyjMZ5QqC2jC8jY5jpuPZOTSOWcvpNs9MFuIbbKJfYmmHAyNPA5gnySu6097DqCPoulqypun9xOXCHD7ldfHennc81WJY0muxg1dktCLO9oMgw04Sdp6rM3tWNO0Mc3Vuf2Wtsl8vNndSIEPdjJ3nVUc6M0oW5oDW8TKcAuVsOQQEFJOY2TCSWzktfQN7hzv8u6nJAqNJj/AIwdeSm1W/rNcDAa04/HReSWDtTaqLMDKhw7TnHRJvam1YXM+Jk+ZO+fNU2y0X+JljeXMrgzTjDyB/lYSVYVb9tDqXwHPJpjY8lXApGq7dLaod5qzrOPww6cjAHPIfdR7wpuc3IZCAR10Vha6Yp0KbN8uuWv0U8/VU49zKIVdxDTxMepE59D6qVYaZe7DkBlmJPjrkmmz4yIBzE5RsDi9QE6pbfgMAAJc45RlJ4TtzK5t6dut3td1LlAzNUTzIB9Cpdl+JSzJLm8RmPNZinbLSTh7uHXuAuy5uJG07bDjKuabnshuPHiAJwtdkdwQfoVnKX5Uwyxt1jGko2qcEFSaryHeCzNitJbUg7EiOmY8YPorC+7f3ZGWgWHRL0baXPrS1pyGROgUFlw1Bm2sBoYIJHujjqik3BuJyjfmTn+Zqtt4tIg06lUy3vS5ggznAzkdfNax3flHk1Juy10vOy1G/O1vVp2PKNOSp3OnTPCcukfwF1NrtDYBmpPzENDXDPPTJ4/IU1ljnPfoYHGAtb0nrfpBsrYe07E+it6dJpD8YkgEtHPPP2VNaX4QDwOh5HJWNqtI7pH7ob4GB9fRW4rqVz82O7Iwd8jBaGkxt0gGPotE+ux7pYO7A84zVL/AIkU2stNNrdqQJ6ucSpNzH9MKmOW5tz8mPjlZ9liFztYyCe1crWch1WmHOyjvDxQXIOI0SU8sbavx8mOM1YuClKBSCqgUohNKUoCbYSA2pOwDv8A1n7hQbztWL4eXpC60XQTza4HyP2CXauo34lJrYhtPbiXGfZc+d1lZ93Zx4zLjmX26TbvAOf5/anGyZkcfE+Sq7lqyNdPcrR2WkNXKFdnHjLHCz2IMzwj1HjCVXITMHXp/Kl1jAgKHarM4tk/LOZ6LG1vDSvYf1GcdTzJU69Z+H0UKyOGM8tOitrY8Fmu0Jlj3Ki3PXLmlhJgacQOnDNT2UANHD3VNdNNznnCc2k+I+qu6tFwMkfaUU8JuGusn4BPoFx+DgExI8/6VjSeCM9VDttQZkHIpbGWMjKX00RGvPcjTzC63JYhXs1So5xL6RwtGwAAM9fso97O7wGx+gz+nkm9nrW6n8VoEhxaD1Mj86Kvfi5Zrzm2Q7b2r4lteTs2m3yYD9Vb3O2KYWZv/K1VeT48gB9Fp7od+mF04eo87ku8rfynAplrHd8U8Lna9FthDSQKSQXBKQTqtJzcnAjqmgJ7BFIFBEJh1sz4e0kTnpxnKFHvmiRVbM5synYBzmx6LpSfhcHcCD5FNvivicx407wnaZmPD7rn5fbr4L9Nn5Pud8DqT6LQWeqT7LOXSJcBzPurmxVYcQdjChm7eC9NPZLM2Mbjsq69rUSwgZNkNgaZlMfayQGDX25oVGhzMGo9Z4rLovcVtnZ+oYyk+34Ff3jY8NEOkZ/bNU7LumCHuBGkwRlxylSqlirvbD3Na0a4SS48swI9VpKWzpX3ZVhzdjjiRwdt6StVZLVPcqCeZ2ndUNjsLGOEAk5xJn+FMq1ZgaOGh+izW8MdTtNtVMNBgqjtlTdTX2guHMZEKttEnEI0SayvSmvfVvM+kFd+zlIE150Jpt/+jKh3nUkUxwPsCqh/aQWdlVtMk1HmCP2gADCZ4jNVktnThuWOOW6zF8N/1NUTP6js+Oa1d1shg6LGWeXVBJkudmeJJzK9Hq0AxlMAbLp3qyOHxuUuX2cAudp+XxXaVxtLu6tpoSSSSQei3rSDrM2o4d5xb4SdAonauyNY2m4CCRB55KX2jvGmymygDLmObMaQ1TbXZqVsYx5fhDdpE+K4MLcPHK+t1e3y3GMu+nieG+PkjeR/UIHJaQXTSonEx+J3CQVmbwJNR0iCujDkmee560dnjxa/KMVT3jaiyqwT3SdNpjVW7lmu1M5EbK9m4jLq7aq4X/qj/wAj6gFaYWMfGPB4xDroR+cVgLkt/wAj+MTyI1HuvSmEOptqN1Zn1B1+/guPOar1ODKXEypYcLJEy7gJJKrG3g2m4sqS08x7LS064LJGxy8VQ37SxQ7fYhZi1m/STZrQ0HEQ8DKTgcBnzjgp9e3WcN7tQEnKBJJUW6r9tLSASHZgnEOAiAR9irx97VmA1XUKbQ6Mw4YtzMRMLfTGXnL3j/LNOtTWZnEIEyWuGUa6aKPYq5rPinmNSTkPArpelvrVxDzDQMJABAgf0pN00AxsgQFm6ikxy/V0mUbJ3iSP2558zChVacNqHy8FcfFAYqK9LRgpOWDvUYi/LUG5naT6QsS55dJOpMqff9vxvIGgOfgqkFdmOOo8flz8sk66qE1WdQvRLyfOEAHIbrAdnqLn16Y5r0W/zDmjg1L/AGSHP8V/dVkplYd0p0ptb5SrIOFlqBrgXCQJy8ElySSC1c4k667pBx4ldrwpMa8hjsTdio8py7mzsXXZz/cJ4NVfeTpqu6rnZbW6mZaeS51KkknjmpzC/wBy5fhS5z+34/k1wVdfFmDmEnQCZU20VmtGJxgDcrM3vexqDA0Qz1PCeSok4XTWwuwbO06j89F6T2Ovb/8AJ5zGnMFeUF5aQRqFqLstWICpTMPbqPp0UeTHbp4OTxr0is74ZwftcZb7wur6QqMj8yVOy9RXoZfOyHAbyNR5SrG7LYHQ/UELm1p6OOUt0lWWiR4Ka/EREnpP0UT4+cAKdhIEEGfZOVS46QK1jB146BPLQIA2zTKtQg6R6qDeNtwNLj0WfdOzxm0xlbEI2GvkvP8At7fkfpMOZ9Fd2y+fhUf+zpMdV51eoOT3Zvf3jyH7Qq8eHe64f6jm+nUVT2wlTbJAXdzAV1sAa14LtF06ee09w2X4Ra+Mxmre8bX8R2KIVZQt9IgAPb0Jg+RUmUeM3s/K68fgk9z4Y4cY90xJ/wApWmUNJBFILJKU3FGuiqrbftNmTe+eXy+f2TC2cqy333Tp5Dvu4DQdSqC2XnVqamG8BkPHcqIGpBItVrfVOJ56DYdAuUJIoNzqhdLHXcxwc0wUHDJc6aA2d3WgVe8w4ag+Zuk8wrK7b0+C4h04Cc8s2u5jgsPZ6paQ5pII3C0llvNlYBtTuv0xbHqFLPB08fJ/yt5d95UzBDgYjcFW7r0aYP54BeWupmk6S0OB/MiNVcWS0UHDfpjePqo3HXp2Y81vVjSXle9Nu4k/mQWXt9t+K4ahjdBuTxI26JlvtbRlTaB01PUqudMaokZ5OS5dI94VcbgOPsFRXzVxP6ZeStba/AC7fQLOVXSVfCOLkvwcCiuTF0VERXSlWc35XEdD9FyTkBZUL7qD5od6HzCsaV903Agy0njp5hZwhCEBqGuBEgz0SWZp1HN0JHRFBOttt9Sr8xy/4jT+VGARASQZIpIhMCkikUgS5kZroE16Ydqea6hR6SlNKCWt23thGCoMTPUdCrH/AC7T3mGdxzH3WahdaNqdTMg+GxU8uP5i+HNrrJoHObEhMZmVFpXhSdmSGHcHTwO6g3heUjAw93d255DkpTG70veTGTe0e+bZicQ3Qb8VTld6hXBXk1048svK7osC6kJjF0jJMjE4IIhAEJIIoBFFBFAABJGE9rUwYiAnOamwgHIFCUYQBQcEkUAGlSGOUYLswoDsUx7ss06clFqVAdZjZBNLc932Rwa6q9znHVrWkgF0wNRw9kL6uOizC2iape7RrwwADnnIyBKobuthY4cJ8PFbux2r4tJxydpLZBIkxInwyy16LcssZu3nlqouYS12oUZWF6U3Cs5p2MeCgubBU62dSC6tTKOie1AMKKJQCAScEAigEkkkgCiHIJJgSUEkkAkUEQgCkkkgAU9pTXBFpQBDpMbe6LqYXOo2DIXVlSeqCcHtWi7MWsthsSXEany99VSYVbUbMaFP4rjDwe63I94ZwRyy8XRsUBx7Q3a+m8PeZxHbb75Kpr05V52tc8uY97sQIEHKMjBAjz6EKocihwpDJEap5TSgwchCL0EgIQSRKACKACSYdEEUEA1GUkAgEiEEkA5FCUUApSYc0kCgOpbIUeFJaUyoM5QEux1qTQTUa44oa0gwGzq7mYOSl9oHgvDW5taAGgZiPmzjr6LjZmEmk0AyZdwEnIQeP8rZ2WwiIwAuhxOmjJ0BmJg6DWNFrHHbNumLve11KoYx7C0sbOhALRlPpqoDdFpe1oMtqCCWtLd850IiZG+ay1GtPJGXVEPITKi6PTH6LJg5NTimpGSKCSASKQSQD0ESgmAQKcggAUglKSQOCMpoRTAoFFJAGm5di2Qow1UimUE1lz2QYLPDS4yHamQDImOEn8la+1gtECASMpIiMoETnx2GZ1Vb2VogWZrwwl5ZABgRhOEmec9YJ0yi+qGz/CaHAVC5p02JaAcMgGDGg0V56TrzntHWrgy7TaCQI2AiBHmsm/WePvv7LcXzQDQQzSNBEeR10/M1iLY6HEH7KWbeLs10hMIyKVHbmnFZacxogi3RBIEEUCiUAgkgimDpQQlJBCEikESgAkkUkGATpTUQgHJSgigGuXSm5MhJhQHoHZW0VKdBuGP3kECSMTjx3E7cVe1ar3sc4DvEQ3MZESB4ZbcFQdlrWRQpzMd4fMYyeci0g5EZaK2rDCXYicIzbGsmcQGLYYZz4iN1Xeonrtm75tbnDEab2vJDYzDRIzOE5QJjQSsdaSMxqePElba/LQ7CcQw7RJJkzmTGWQ0yWMtLp04n39NlKtxyoHIDddn8Vys7A4gRuu7hqgODUE4JpSMIRlJAoApISkmBCQKKSAQTkkkECQSSQZJJJIIUSkkgEmjUJJINv+xp/wBO3k50cs9uCtahxNdOwJ8hKSS3fTM9sjfQik2N3EnwBWcf8zkklgxs474/N13qjIHjPp/aSSfwPlE4ppRSSMAgUkkgaSkkkmH/2Q==' },
  { name: 'William Sadler', image: 'https://images.entertainment.ie/person/rWeb2kjYCA7V9MC9kRwRpm57YoY.jpg' },
  { name: 'Clancy Brown', image: 'https://m.media-amazon.com/images/M/MV5BMTUxODY3NjAzMF5BMl5BanBnXkFtZTcwMTQ5MjYwNg@@._V1_FMjpg_UX1000_.jpg' }
]);

const submitReview = () => {
  if (!newReview.value.comment || !newReview.value.rating) {
    alert('Please enter a comment and rating.');
    return;
  }

  movie.value.reviews.push({
    comment: newReview.value.comment,
    rating: newReview.value.rating
  });

  localStorage.setItem(`reviews_${movie.value.imdbID}`, JSON.stringify(movie.value.reviews));
  newReview.value.comment = '';
  newReview.value.rating = '';
};

const handleRent = () => {
  if (movie.value) {
    alert(`DEMO: Film "${movie.value.title}" je iznajmljen`);
  }
};

onMounted(() => {
  movie.value = allMovies.find(m => m.imdbID === imdbID);
  if (movie.value) {
    copies.value = allMovieCopies.filter(c => c.imdbID === movie.value.imdbID);
  }
  const storedReviews = localStorage.getItem(`reviews_${imdbID}`);
  if (storedReviews && movie.value) {
    movie.value.reviews = JSON.parse(storedReviews);
  }
});
</script>
<style scoped>
.movie-details-container {
  background-color: #f8f9fa;
  min-height: 100vh;
}

.movie-header {
  background: linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7)),
    url('https://via.placeholder.com/1920x400') center/cover;
  color: white;
  padding: 3rem 0;
}

.movie-poster {
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(17, 17, 17, 0.251);
}

.movie-info {
  padding-left: 2rem;
}

.movie-title {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.movie-director {
  font-size: 1.2rem;
  opacity: 0.8;
  margin-bottom: 1.5rem;
}

.movie-meta {
  display: flex;
  gap: 2rem;
  margin-bottom: 1.5rem;
}

.meta-item {
  display: flex;
  flex-direction: column;
}

.meta-label {
  font-size: 0.9rem;
  opacity: 0.7;
}

.meta-value {
  font-size: 1.1rem;
  font-weight: 600;
}

.movie-description {
  font-size: 1.1rem;
  line-height: 1.6;
  margin-bottom: 2rem;
}

.action-buttons {
  display: flex;
  gap: 1rem;
}

.btn {
  padding: 0.6rem 1.2rem;
  border-radius: 4px;
  font-weight: 600;
  transition: all 0.3s;
}

.btn-rent {
  background-color: #007bff;
  color: white;
  border: none;
}

.btn-rent:hover {
  background-color: #0069d9;
}

.btn-favorite {
  background-color: #ffc107;
  color: #000;
  border: none;
}

.btn-favorite:hover {
  background-color: #e0a800;
}

.btn-favorite.active {
  background-color: #dc3545;
  color: white;
}

.btn-favorite.active:hover {
  background-color: #c82333;
}

.btn-back {
  background-color: transparent;
  color: white;
  border: 1px solid white;
}

.btn-back:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

.movie-content {
  padding: 3rem 0;
}

.movie-section {
  margin-bottom: 3rem;
}

.section-title {
  font-size: 1.5rem;
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #dee2e6;
}

.cast-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
  gap: 1.5rem;
}

.cast-member {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.actor-avatar {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  object-fit: cover;
  margin-bottom: 0.5rem;
}



.actor-name {
  text-align: center;
  font-weight: 500;
}

.rating-box {
  background-color: white;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  text-align: center;
  margin-bottom: 1.5rem;
}

.rating-value {
  font-size: 2.5rem;
  font-weight: 700;
  color: #007bff;
}

.rating-stars {
  color: #ffc107;
  font-size: 1.5rem;
  margin: 0.5rem 0;
}

.rating-source {
  font-size: 0.9rem;
  color: #6c757d;
}

.btn-rate {
  width: 100%;
  background-color: #28a745;
  color: white;
  border: none;
}

.btn-rate:hover {
  background-color: #218838;
}
</style>