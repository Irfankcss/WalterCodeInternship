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
      path: '/admin-panel',
      name: 'Admin Panel',
      component: () => import('../views/AdminPanelView.vue'),
    },
    {
      path: '/movies/:id/copies',
      name: 'Movie Copies',
      component: () => import('../components/admin-moviecopies/MovieCopiesManager.vue'),
    }
  ],
})

export default router
