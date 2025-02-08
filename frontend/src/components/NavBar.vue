<template>
  <nav class="space-x-4">
    <RouterLink to="/" class="hover:underline">Home</RouterLink>
    <RouterLink to="/about" class="hover:underline">About</RouterLink>
    <RouterLink v-if="!isLoggedIn" to="/login" class="hover:underline">Login</RouterLink>
    <RouterLink v-if="!isLoggedIn" to="/register" class="hover:underline">Register</RouterLink>
    <button v-if="isLoggedIn" @click="logout" class="hover:underline text-red-500">Logout</button>
  </nav>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink, useRouter } from 'vue-router'

const router = useRouter()
const isLoggedIn = ref(false)

const checkAuthStatus = () => {
  const token = localStorage.getItem('token')
  isLoggedIn.value = !!token
}

const logout = () => {
  if (confirm('Are you sure you want to log out?')) {
    localStorage.removeItem('token')
    isLoggedIn.value = false
    router.push('/login')
  }
}

onMounted(() => {
  checkAuthStatus()
})
</script>