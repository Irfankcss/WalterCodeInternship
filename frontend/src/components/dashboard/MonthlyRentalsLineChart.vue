<script setup>
import { onMounted, ref } from 'vue'
import { Chart } from 'chart.js/auto'

const canvasRef = ref(null)
let chartInstance = null

const fetchRentals = async () => {
  const headers = {}
  const token = localStorage.getItem('token')
  if (token) headers['Authorization'] = `Bearer ${token}`

  const res = await fetch('http://localhost:5222/api/Rental', { headers })
  if (!res.ok) {
    return []
  }
  return await res.json()
}

const groupByMonth = (rentals) => {
  const counts = new Map()
  rentals.forEach(r => {
    const d = new Date(r.date)
    if (isNaN(d)) return
    const key = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`
    counts.set(key, (counts.get(key) || 0) + 1)
  })
  const keys = Array.from(counts.keys()).sort()
  const labels = keys.map(k => {
    const [y, m] = k.split('-')
    const dt = new Date(Number(y), Number(m) - 1, 1)
    return dt.toLocaleDateString('bs-BA', { month: 'long', year: 'numeric' })
  })
  const data = keys.map(k => counts.get(k))
  return { labels, data }
}

const renderChart = (labels, data) => {
  if (chartInstance) {
    chartInstance.destroy()
    chartInstance = null
  }
  const ctx = canvasRef.value.getContext('2d')
  chartInstance = new Chart(ctx, {
    type: 'line',
    data: {
      labels,
      datasets: [{
        label: 'Rents',
        data,
        fill: true,
        tension: 0.3,
        backgroundColor: 'rgba(78,115,223,0.08)',
        borderColor: 'rgba(78,115,223,1)',
        pointRadius: 4,
        pointBackgroundColor: 'rgba(78,115,223,1)',
        pointBorderWidth: 1
      }]
    },
    options: {
      maintainAspectRatio: false,
      scales: {
        x: {
          grid: { display: false }
        },
        y: {
          beginAtZero: true,
          ticks: { precision: 0 },
          grid: {
            color: 'rgba(234,236,244,0.8)',
            drawBorder: false
          }
        }
      },
      plugins: {
        legend: { display: false },
        tooltip: {
          backgroundColor: 'rgba(255,255,255,0.9)',
          titleColor: '#343a40',
          bodyColor: '#6c757d',
          borderColor: '#dee2e6',
          borderWidth: 1
        }
      }
    }
  })
}

onMounted(async () => {
  const rentals = await fetchRentals()
  const { labels, data } = groupByMonth(rentals)
  renderChart(labels, data)
})
</script>

<template>
  <div class="card shadow mb-4">
    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
      <h6 class="m-0 font-weight-bold text-primary">Rentals per month</h6>
    </div>
    <div class="card-body">
      <div class="chart-area">
        <canvas ref="canvasRef"></canvas>
      </div>
    </div>
  </div>
</template>

<style scoped>
.chart-area {
  position: relative;
  height: 320px;
}
</style>
