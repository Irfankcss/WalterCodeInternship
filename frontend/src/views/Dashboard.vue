<template>
  <h1 class="text-center my-4">Dashboard</h1>
  <div>
    <DashboardCards />
    <div class="row">
      <div class="col-xl-8 col-lg-7">
        <AreaChart />
      </div>
      <div class="col-xl-4 col-lg-5">
        <PieChart />
      </div>
    </div>
    <div class="row">
      <div class="col-lg-6 mb-4">
        <Projects />
      </div>

    <div class="col-lg-6 mb-4">
  </div>
  </div>
  </div>
</template>

<script setup>
import DashboardCards from '../components/dashboard/DashboardCards.vue'
import AreaChart from '../components/dashboard/MonthlyRentalsLineChart.vue'
import PieChart from '../components/dashboard/GenreRentalsPieChart.vue'
import Projects from '../components/dashboard/TopMoviesBarChart.vue'
</script>
