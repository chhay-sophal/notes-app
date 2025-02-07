<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="bg-white p-8 rounded shadow-md w-full max-w-md">
      <h2 class="text-blue-600 text-2xl font-bold mb-6 text-center">Login</h2>
      <form @submit.prevent="login">
        <div class="mb-4 text-gray-700">
          <label for="email" class="block">Email:</label>
          <input type="email" v-model="email" class="w-full px-3 py-2 border rounded" required />
        </div>
        <div class="mb-6 text-gray-700">
          <label for="password" class="block">Password:</label>
          <input type="password" v-model="password" class="w-full px-3 py-2 border rounded" required />
        </div>
        <button type="submit" class="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600">Login</button>
      </form>
      <p class="mt-4 text-center text-gray-600">
        Don't have an account? 
        <RouterLink to="/register" class="text-blue-500 hover:underline">Register</RouterLink>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'

const email = ref('')
const password = ref('')

const login = async () => {
  try {
    const response = await axios.post('http://localhost:5389/api/auth/login', {
      email: email.value,
      password: password.value
    })
    console.log('Login successful:', response.data)
  } catch (error) {
    console.error('Login failed:', error)
  }
}
</script>