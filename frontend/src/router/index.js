import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../views/Dashboard.vue'
import isAdmin from "@/utils/auth.js"

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/dashboard'
    },
    {
      path: '/dashboard',
      name: 'Dashboard',
      meta: { requiresAdmin: true },
      component: Dashboard
    },
    {
      path: '/movies',
      name: 'Movies',
      component: () => import('../views/MoviesView.vue'),
      meta: { title: 'Filmovi' }
    },
    {
      path: '/admin-panel',
      name: 'Admin Panel',
      meta: { requiresAdmin: true },
      component: () => import('../views/AdminPanelView.vue'),
    },
    {
      path: '/movies/:id/copies',
      name: 'Movie Copies',
      meta: { requiresAdmin: true },
      component: () => import('../components/admin-moviecopies/MovieCopiesManager.vue'),
    },
    {
      path: '/login',
      name: 'Login',
      component: () => import('../views/Login/LoginView.vue'),
    },
    {
      path: '/register',
      name: 'Register',
      component: () => import('../views/Login/RegisterView.vue'),
    },
    {
      path: '/forgot-password',
      name: 'ForgotPassword',
      component: () => import('../views/Login/ForgotPasswordView.vue'),
    },
    {
      path: '/movie/:id',
      name: 'MovieDetails',
      component: () => import('../views/MovieDetails.vue'),
      meta: { title: 'Detalji filma' },
    },
    {
      path: '/profile',
      name: 'UserProfile',
      component: () => import('../views/UserProfile.vue'),
    },
    {
      path: '/:pathMatch(.*)*',
      redirect: '/dashboard'
    }
  ]
})

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAdmin && !isAdmin()) {
    return next('/movies')
  }

  document.title = to.meta.title || 'Video Klub'
  next()
})

export default router
