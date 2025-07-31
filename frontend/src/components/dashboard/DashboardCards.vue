<template>
  <div class="row">
    <!-- Earnings (Monthly) Card Example -->
    <div class="col-xl-3 col-md-6 mb-4">
      <div class="card border-left-primary shadow h-100 py-2">
        <div class="card-body">
          <div class="row no-gutters align-items-center">
            <div class="col mr-2">
              <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                Rentals Today
              </div>
              <div class="h5 mb-0 font-weight-bold text-gray-800">{{ todayRentals }}</div>
            </div>
            <div class="col-auto">
              <i class="fas fa-calendar fa-2x text-gray-300"></i>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="col-xl-3 col-md-6 mb-4">
      <div class="card border-left-primary shadow h-100 py-2">
        <div class="card-body">
          <div class="row no-gutters align-items-center">
            <div class="col mr-2">
              <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                All time rentals
              </div>
              <div class="h5 mb-0 font-weight-bold text-gray-800">{{ allTimeRentals }}</div>
            </div>
            <div class="col-auto">
              <i class="fas fa-infinity fa-2x text-gray-300"></i>
            </div>
          </div>
        </div>
      </div>
    </div>


    <div class="col-xl-3 col-md-6 mb-4">
      <div class="card border-left-success shadow h-100 py-2">
        <div class="card-body">
          <div class="row no-gutters align-items-center">
            <div class="col mr-2">
              <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                Earnings (All time)
              </div>
              <div class="h5 mb-0 font-weight-bold text-gray-800">$ {{earnings.toFixed(2)}}</div>
            </div>
            <div class="col-auto">
              <i class="fas fa-dollar-sign fa-2x text-gray-300"></i>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Earnings (Annual) Card Example
  <div class="col-xl-3 col-md-6 mb-4">
    <div class="card border-left-info shadow h-100 py-2">
      <div class="card-body">
        <div class="row no-gutters align-items-center">
          <div class="col mr-2">
            <div class="text-xs font-weight-bold text-info text-uppercase mb-1">Tasks</div>
            <div class="row no-gutters align-items-center">
              <div class="col-auto">
                <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">50%</div>
              </div>
              <div class="col">
                <div class="progress progress-sm mr-2">
                  <div class="progress-bar bg-info" role="progressbar" style="width: 50%" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-auto">
            <i class="fas fa-clipboard-list fa-2x text-gray-300"></i>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div class="col-xl-3 col-md-6 mb-4">
    <div class="card border-left-warning shadow h-100 py-2">
      <div class="card-body">
        <div class="row no-gutters align-items-center">
          <div class="col mr-2">
            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">Pending Requests</div>
            <div class="h5 mb-0 font-weight-bold text-gray-800">18</div>
          </div>
          <div class="col-auto">
            <i class="fas fa-comments fa-2x text-gray-300"></i>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
-->
  </template>

<script>
import { ref, onMounted } from 'vue';
import {allowErrorProps} from "superjson";

export default {
  name: 'DashboardCards',
  methods: {allowErrorProps},
  setup() {
    const rentals = ref([]);
    const rentalsToday = ref(0);
    const rentalsAllTime = ref(0);
    const earnings = ref(0.0);

    onMounted(async () => {
      const res = await fetch('http://localhost:5222/api/Rental');
      const data = await res.json();
      rentals.value = data;

      rentalsAllTime.value = rentals.value.length;
      earnings.value = rentalsAllTime.value * 7.5;
      const today = new Date().toISOString().split('T')[0];

      rentalsToday.value = rentals.value.filter(rental => {
        const rentalDate = rental.date?.split('T')[0];
        return rentalDate === today;
      }).length;
    });

    return {
      rentals,
      todayRentals: rentalsToday,
      allTimeRentals: rentalsAllTime,
      earnings: earnings,

    };
  }
};
</script>


<style scoped>
</style>
