import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { requiresAuth: true },
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue'),
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue'),
    },
    {
      path: '/note',
      name: 'create-note',
      component: () => import('../views/NoteEditorView.vue'),
    },
    {
      path: '/note/:id',
      name: 'edit-note',
      component: () => import('../views/NoteEditorView.vue'),
    }
  ],
})

export default router
