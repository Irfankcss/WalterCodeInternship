import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../views/Dashboard.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/dashboard',
      name: 'Dashboard',
      component: Dashboard
    },
    {
      path: '/movies',
      name: 'Movies',
      component: () => import('../views/MoviesView.vue'),
    },
    {
      path: '/table',
      name: 'Table',
      component: () => import('../views/TableView.vue'),
    }
  ],
})

export default router
