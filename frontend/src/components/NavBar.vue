<template>
  <nav class="space-x-4">
    <RouterLink to="/" class="hover:underline">Home</RouterLink>
    <RouterLink to="/about" class="hover:underline">About</RouterLink>
    <RouterLink to="/login" class="hover:underline">Login</RouterLink>
    <RouterLink to="/register" class="hover:underline">Register</RouterLink>
    <button @click="logout" class="hover:underline text-red-500">Logout</button>
  </nav>
</template>

<script setup lang="ts">
import { RouterLink, useRouter } from 'vue-router'

const router = useRouter()

const logout = () => {
  if (confirm('Are you sure you want to log out?')) {
    localStorage.removeItem('token')
    router.push('/login')
  }
}
</script>