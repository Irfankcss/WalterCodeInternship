<script setup>
import {nextTick, onMounted, ref} from 'vue'
import { Chart } from 'chart.js/auto'
import emitter from "@/utils/emitter.js";

const chartRendered = ref(false)

const renderChart = async () => {
  try {
    const response = await fetch('http://localhost:5222/api/Rental')
    if (!response.ok) {
      throw new Error('Failed to fetch rentals: ' + response.status + response.message)
    }
    const rentals = await response.json()


    const genreCounts = {}
    const fetchPromises = []

    for (const rental of rentals) {
      const movieId = rental?.movieId
      if (!movieId) continue

      const promise = fetch(`http://localhost:5222/api/Movie/${movieId}/genres`)
        .then(res => res.json())
        .then(genres => {
          genres.forEach(g => {
            const genreName = g.name
            if (genreName) {
              genreCounts[genreName] = (genreCounts[genreName] || 0) + 1
            }
          })
        })

      fetchPromises.push(promise)
    }

    function generateColors(count) {
      const colors = []
      for (let i = 0; i < count; i++) {
        const hue = (i * 360 / count) % 360
        colors.push(`hsl(${hue}, 70%, 60%)`)
      }
      return colors
    }


    await Promise.all(fetchPromises)

    const labels = Object.keys(genreCounts)
    const data = Object.values(genreCounts)
    const backgroundColors = generateColors(labels.length)
    const hoverColors = generateColors(labels.length).map(c => c.replace('60%', '50%'))

    const ctx = document.getElementById('myPieChart')?.getContext('2d')
    new Chart(ctx, {
      type: 'pie',
      data: {
        labels,
        datasets: [{
          data,
          backgroundColor: backgroundColors,
          hoverBackgroundColor: hoverColors,
          hoverBorderColor: 'rgba(234, 236, 244, 1)'
        }]
      },
      options: {
        maintainAspectRatio: false,
        layout: {
          padding: { left: 10, right: 10, top: 10, bottom: 10 }
        },
        scales: {
          y: { beginAtZero: true }
        },
        legend: { display: true, position: 'top' },
        title: { display: true, text: 'Revenue Sources' },
        animation: {
          animateScale: true,
          animateRotate: true
        }
      }
    })

    chartRendered.value = true
  } catch (err) {
    emitter.emit('toast', {
      message: err.message,
      type: 'error'
    })
  }
}

onMounted(async () => {
  await nextTick()
  await renderChart()
})
</script>

<template>
    <div class="card shadow mb-4">
      <div class="card-header py-3">
        <h6 class="m-0 font-weight-bold text-primary">Rentals per genre</h6>
      </div>
      <div class="card-body">
        <div class="chart-pie pt-4 pb-2">
          <canvas id="myPieChart"></canvas>
        </div>
        <div v-if="!chartRendered" class="text-center text-muted mt-3">
          Loading data...
        </div>
      </div>
    </div>
</template>

<style scoped>
.chart-pie {
  position: relative;
  height: 300px;
}
</style>
