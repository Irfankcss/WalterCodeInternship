<template>
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
      <ColorSystem />
    </div>
    
    <div class="col-lg-6 mb-4">
      <Illustrations />
      <Approach />
  </div>
  </div>
  </div>
</template>

<script setup>
import DashboardCards from '../components/DashboardCards.vue'
import AreaChart from '../components/AreaChart.vue'
import PieChart from '../components/PieChart.vue'
import Projects from '../components/Projects.vue'
import ColorSystem from '../components/ColorSystem.vue'
import Illustrations from '../components/Illustrations.vue'
import Approach from '../components/Approach.vue'
import FlippingCard from '../components/FlippingCard.vue'
</script>