<template>
  <div class="card shadow mb-4">
    <div class="card-header py-3">
      <h6 class="m-0 font-weight-bold text-primary">Top 5 Most Rented Movies</h6>
    </div>
    <div class="card-body">
      <div class="chart-bar pt-4 pb-2">
        <canvas id="movieBarChart"></canvas>
      </div>
      <div v-if="!chartReady" class="text-center text-muted mt-3">
        Loading data...
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, nextTick } from 'vue'
import { Chart } from 'chart.js/auto'
import emitter from "@/utils/emitter.js";

const chartReady = ref(false)

const renderChart = async () => {
  try {
    const rentalRes = await fetch('http://localhost:5222/api/Rental')
    const rentals = await rentalRes.json()

    const movieCountMap = {}

    for (const rental of rentals) {
      const movieTitle = rental.movieTitle
      if (!movieTitle) continue

      movieCountMap[movieTitle] = (movieCountMap[movieTitle] || 0) + 1
    }

    const topMovies = Object.entries(movieCountMap)
      .sort((a, b) => b[1] - a[1])
      .slice(0, 5)


    const labels = topMovies.map(entry => entry[0])
    const data = topMovies.map(entry => entry[1])

    await nextTick()
    const ctx = document.getElementById('movieBarChart')?.getContext('2d')

    new Chart(ctx, {
      type: 'bar',
      data: {
        labels,
        datasets: [{
          label: 'Number of rentals',
          data,
          backgroundColor: '#4e73df',
          borderColor: '#2e59d9',
          borderWidth: 1
        }]
      },
      options: {
        indexAxis: 'x',
        responsive: true,
        plugins: {
          legend: { display: false },
          title: {
            display: false
          }
        },
        scales: {
          x: {
            title: {
              display: true,
              text: 'Movies'
            },
            ticks: {
              autoSkip: false,
              maxRotation: 45,
              minRotation: 0
            }
          },
          y: {
            title: {
              display: true,
              text: 'Number of rentals'
            },
            beginAtZero: true
          }
        }
      }
    })

    chartReady.value = true
  } catch (err) {
    emitter.emit('toast', {
      message: err.message,
      type: 'error'
    })
  }
}

onMounted(() => {
  renderChart()
})
</script>

<style scoped>
.chart-bar {
  position: relative;
  height: 400px;
}
</style>
