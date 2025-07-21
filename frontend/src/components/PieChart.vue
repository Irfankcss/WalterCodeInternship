<script setup>
import { Chart } from 'chart.js/auto'
</script>
<template>
  <div class="card shadow mb-4">
    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
      <h6 class="m-0 font-weight-bold text-primary">Revenue Sources</h6>
      <div class="dropdown no-arrow">
        <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400"></i>
        </a>
        <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in" aria-labelledby="dropdownMenuLink">
          <div class="dropdown-header">Dropdown Header:</div>
          <a class="dropdown-item" href="#">Action</a>
          <a class="dropdown-item" href="#">Another action</a>
          <div class="dropdown-divider"></div>
          <a class="dropdown-item" href="#">Something else here</a>
        </div>
      </div>
    </div>
    <div class="card-body">
      <div class="chart-pie pt-4 pb-2">
        <canvas id="myPieChart"></canvas>
      </div>
      <div class="mt-4 text-center small">
        <span class="mr-2">
          <i class="fas fa-circle text-primary"></i> Direct
        </span>
        <span class="mr-2">
          <i class="fas fa-circle text-success"></i> Social
        </span>
        <span class="mr-2">
          <i class="fas fa-circle text-info"></i> Referral
        </span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'PieChart',
  mounted() {
    this.renderChart();
  },
  methods: {
    renderChart() {
      const ctx = document.getElementById('myPieChart').getContext('2d');
      const myPieChart = new Chart(ctx, {
        type: 'pie',
        data: {
          labels: ['Direct', 'Social', 'Referral'],
          datasets: [{
            data: [300, 50, 100],
            backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc'],
            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf'],
            hoverBorderColor: 'rgba(234, 236, 244, 1)',
          }],
        },
        options: {
          maintainAspectRatio: false,
          layout: {
            padding: {
              left: 10,
              right: 10,
              top: 10,
              bottom: 10,
            },
          },
          scales: {
            y: {
              beginAtZero: true,
            },
          },
          legend: {
            display: true,
            position: 'top',
          },
          title: {
            display: true,
            text: 'Revenue Sources',
          },
          animation: {
            animateScale: true,
            animateRotate: true,
          },
        },
      });
    },
  },
};
</script>

<style scoped>
.chart-pie {
  position: relative;

}
</style>