<script setup>
import { Chart } from 'chart.js/auto'
</script>

<template>
  <div class="card shadow mb-4">
    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
      <h6 class="m-0 font-weight-bold text-primary">Earnings Overview</h6>
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
      <div class="chart-area">
        <canvas id="myAreaChart"></canvas>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'AreaChart',
  mounted() {
    this.renderChart();
  },
  methods: {
    renderChart() {
      const ctx = document.getElementById('myAreaChart').getContext('2d');
      const myLineChart = new Chart(ctx, {
        type: 'line',
        data: {
          labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
          datasets: [{
            label: 'Earnings',
            lineTension: 0.3,
            backgroundColor: 'rgba(78, 115, 223, 0.05)',
            borderColor: 'rgba(78, 115, 223, 1)',
            pointRadius: 5,
            pointBackgroundColor: 'rgba(78, 115, 223, 1)',
            pointBorderColor: 'rgba(255, 255, 255, 0.8)',
            pointHoverRadius: 5,
            pointHoverBackgroundColor: 'rgba(78, 115, 223, 1)',
            pointHoverBorderColor: 'rgba(255, 255, 255, 0.8)',
            pointHitRadius: 50,
            pointBorderWidth: 2,
            data: [0, 10000, 5000, 15000, 10000, 20000, 25000],
          }],
        },
        options: {
          maintainAspectRatio: false,
          layout: {
            padding: {
              left: 10,
              right: 25,
              top: 25,
              bottom: 0
            }
          },
          scales: {
            xAxes: [{
              time: {
                unit: 'date'
              },
              gridLines: {
                display: false,
                drawBorder: false
              },
              ticks: {
                maxTicksLimit: 7
              }
            }],
            yAxes: [{
              ticks: {
                min: 0,
                max: 30000,
                maxTicksLimit: 5,
                padding: 10,
                callback: function(value) {
                  return '$' + value;
                }
              },
              gridLines: {
                color: "rgba(234, 236, 244, .8)",
                zeroLineColor: "rgba(234, 236, 244, .8)",
                drawBorder: false
              }
            }],
          },
          legend: {
            display: false
          },
          tooltips: {
            titleMarginBottom: 10,
            titleFontColor: '#6e707e',
            titleFontSize: 14,
            backgroundColor: 'rgba(255, 255, 255, 0.8)',
            bodyFontColor: '#858796',
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            caretPadding: 10,
          }
        }
      });
    }
  }
}
</script>

<style scoped>
.chart-area {
  position: relative;
  
}
</style>