<template>
  <div class="col-md-3 card-container">
    <div class="card-flip">
      <!-- Card 2 Front -->
      <div class="card front">
        <img :src="imageUrl" class="card-img-top img-fluid">
        <div class="card-block">
        </div>
      </div>
      <!-- End Card 2 Front -->

      <!-- Card 2 Back -->
      <div class="card back">
        <div class="card-header">
          <i class="fas fa-star"></i>
          {{ imdbRating }}
        </div>
        <div class="card-block">
          <h4 class="card-title">{{ title }}</h4>
          <p class="card-text"><small class="text-muted">{{ director }} ({{ publishedYear }})</small></p>
          <p class="card-text">
            {{ description && description.length > 90 ? description.substring(0, 90) + '...' : description }}
          </p>
          <router-link :to="{ name: 'MovieDetails', params: { id: movieId } }" class="btn btn-primary">
            Details
          </router-link>
        </div>
      </div>
      <!-- End Card 2 Back -->
    </div>
  </div>
</template>

<script setup>
defineProps({
  movieId: {
    type: [String, Number],
    required: true
  },

  description: {
    type: String,
    default: 'No description available',
  },
  imageUrl: {
    type: String,
    required: true
  },
  title: {
    type: String,
    required: true
  },
  director: {
    type: String,
    required: true
  },
  publishedYear: {
    type: Number,
    required: true
  },
  imdbRating: {
    type: Number
  },
  link: {
    type: String,
    default: '#'
  },
  linkTitle: {
    type: String,
    default: 'View More'
  }
})
</script>

<style scoped>
.card {
  margin: 10px 10px;
}

/* Flip Cards CSS */
.card-container {
  display: grid;
  perspective: 750px;
}

.card-block {
  margin: 10px;
}

.card-flip {
  display: grid;
  grid-template: 1fr / 1fr;
  grid-template-areas: "frontAndBack";
  transform-style: preserve-3d;
  transition: all 0.9s ease;
}

.card-flip div {
  backface-visibility: hidden;
  transform-style: preserve-3d;
}

.front {
  grid-area: frontAndBack;
}

.back {
  grid-area: frontAndBack;
  transform: rotateY(-180deg);
}

.card-container:hover .card-flip {
  transform: rotateY(180deg);
}

img {
  height: 100%;
  width: 100%;
}
</style>
