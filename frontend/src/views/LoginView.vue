<template>
  <div class="flex items-center justify-center min-h-screen">
    <div class="bg-white p-8 rounded shadow-md w-full max-w-md">
      <h2 class="text-blue-600 text-2xl font-bold mb-6 text-center">Login</h2>
      <form @submit.prevent="login">
        
        <!-- Email Input -->
        <div class="mb-4 text-gray-700">
          <label for="email" class="block">Email:</label>
          <input 
            type="email" 
            v-model="email" 
            class="w-full px-3 py-2 border rounded" 
            required 
          />
        </div>

        <!-- Password Input with Toggle -->
        <div class="mb-6 text-gray-700 relative">
          <label for="password" class="block">Password:</label>
          <div class="relative">
            <input 
              :type="showPassword ? 'text' : 'password'"
              v-model="password" 
              class="w-full px-3 py-2 border rounded pr-10" 
              required
            />
            <!-- Eye Icon -->
            <button 
              type="button"
              @click="togglePassword" 
              class="absolute inset-y-0 right-2 flex items-center text-gray-500 hover:text-gray-700"
            >
              <span v-if="showPassword">👁️</span>
              <span v-else>🙈</span>
            </button>
          </div>
        </div>

        <!-- Error Message -->
        <p v-if="errorMessage" class="text-red-500 text-sm mb-4">{{ errorMessage }}</p>

        <!-- Submit Button -->
        <button type="submit" class="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600">
          Login
        </button>
      </form>

      <p class="mt-4 text-center text-gray-600">
        Don't have an account? 
        <RouterLink to="/register" class="text-blue-500 hover:underline">Register</RouterLink>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import axiosInstance from '../services/axiosInstance'
import { useRouter } from 'vue-router'

const email = ref('')
const password = ref('')
const showPassword = ref(false)
const errorMessage = ref<string | null>(null)  // To store the error message
const router = useRouter()

// Toggle password visibility
const togglePassword = () => {
  showPassword.value = !showPassword.value
}

const login = async () => {
  try {
    const response = await axiosInstance.post('auth/login', {
      email: email.value,
      password: password.value
    })
    const token = response.data.token
    localStorage.setItem('token', token)
    console.log('Login successful:', response.data)
    router.push('/')
  } catch (error: any) {
    // Handle backend error message
    if (error.response && error.response.data && error.response.data.message) {
      errorMessage.value = error.response.data.message  // Set the error message from the backend
    } else {
      errorMessage.value = 'An error occurred during login. Please try again later.'
    }
    console.error('Login failed:', error)
  }
}

// Check if the user is already logged in
onMounted(() => {
  const token = localStorage.getItem('token')
  if (token) {
    router.push('/')
  }
})
</script>
