<template>
  <nav class="flex space-x-6 items-center">
    <RouterLink to="/" class="hover:text-gray-200 transition">Home</RouterLink>
    
    <template v-if="!isLoggedIn">
      <RouterLink to="/login" class="hover:text-gray-200 transition">Login</RouterLink>
      <RouterLink to="/register" class="hover:text-gray-200 transition">Register</RouterLink>
    </template>

    <button 
      v-else 
      @click="logout" 
      class="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600 transition"
    >
      Logout
    </button>
  </nav>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const isLoggedIn = ref(false)

const checkAuthStatus = () => {
  isLoggedIn.value = !!localStorage.getItem('token')
}

const logout = () => {
  if (confirm('Are you sure you want to log out?')) {
    localStorage.removeItem('token')
    isLoggedIn.value = false
    router.push('/login')
  }
}

onMounted(checkAuthStatus)
</script>
