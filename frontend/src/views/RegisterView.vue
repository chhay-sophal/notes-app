<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="bg-white p-8 rounded shadow-md w-full max-w-md">
      <h2 class="text-blue-600 text-2xl font-bold mb-6 text-center">Register</h2>
      <form @submit.prevent="register">
        <div class="mb-4 text-gray-700">
          <label for="fullName" class="block">Full Name:</label>
          <input type="text" v-model="fullName" class="w-full px-3 py-2 border rounded" required />
        </div>
        <div class="mb-4 text-gray-700">
          <label for="email" class="block">Email:</label>
          <input type="email" v-model="email" class="w-full px-3 py-2 border rounded" required />
        </div>
        <div class="mb-4 text-gray-700">
          <label for="password" class="block">Password:</label>
          <input type="password" v-model="password" class="w-full px-3 py-2 border rounded" required />
        </div>
        <div class="mb-6 text-gray-700">
          <label for="confirmPassword" class="block">Confirm Password:</label>
          <input type="password" v-model="confirmPassword" class="w-full px-3 py-2 border rounded" required />
        </div>
        <button type="submit" class="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600">Register</button>
      </form>
      <p class="mt-4 text-center text-gray-600">
        Already have an account? 
        <RouterLink to="/login" class="text-blue-500 hover:underline">Login</RouterLink>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { RouterLink } from 'vue-router'

const fullName = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')

const register = async () => {
  if (password.value !== confirmPassword.value) {
    console.error('Passwords do not match')
    return
  }

  try {
    const response = await axios.post('http://localhost:5389/api/auth/register', {
      fullName: fullName.value,
      email: email.value,
      password: password.value
    })
    console.log('Registration successful:', response.data)
  } catch (error) {
    console.error('Registration failed:', error)
  }
}
</script>