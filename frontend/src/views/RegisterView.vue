<template>
  <div class="flex items-center justify-center min-h-screen">
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
          <!-- Error Message for Existing User -->
          <p v-if="errorMessage" class="text-red-500 text-sm mt-2">{{ errorMessage }}</p>
        </div>

        <!-- Password Input with Toggle -->
        <div class="mb-4 text-gray-700 relative">
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
          <p v-if="passwordComplexityMessage" class="text-red-500 text-sm mt-1">{{ passwordComplexityMessage }}</p>
        </div>

        <!-- Confirm Password Input with Toggle -->
        <div class="mb-6 text-gray-700 relative">
          <label for="confirmPassword" class="block">Confirm Password:</label>
          <div class="relative">
            <input 
              :type="showConfirmPassword ? 'text' : 'password'"
              v-model="confirmPassword" 
              class="w-full px-3 py-2 border rounded pr-10" 
              required
            />
            <!-- Eye Icon for Confirm Password -->
            <button 
              type="button"
              @click="toggleConfirmPassword" 
              class="absolute inset-y-0 right-2 flex items-center text-gray-500 hover:text-gray-700"
            >
              <span v-if="showConfirmPassword">👁️</span>
              <span v-else>🙈</span>
            </button>
          </div>
          <p v-if="passwordNotMatched" class="text-red-500 text-sm mt-1">{{ passwordNotMatched }}</p>
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
import { ref, computed } from 'vue'
import axiosInstance from '../services/axiosInstance'
import { RouterLink } from 'vue-router'
import { useRouter } from 'vue-router'
import { AxiosError } from 'axios'

const fullName = ref('')
const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const errorMessage = ref('')
const showPassword = ref(false)
const showConfirmPassword = ref(false)
const router = useRouter()

// Toggle password visibility
const togglePassword = () => {
  showPassword.value = !showPassword.value
}

// Toggle confirm password visibility
const toggleConfirmPassword = () => {
  showConfirmPassword.value = !showConfirmPassword.value
}

// Password complexity validation
const passwordComplexityMessage = computed(() => {
  if (password.value.length < 8) {
    return 'Password must be at least 8 characters long.'
  }
  if (!/[A-Z]/.test(password.value)) {
    return 'Password must contain at least one uppercase letter.'
  }
  if (!/[a-z]/.test(password.value)) {
    return 'Password must contain at least one lowercase letter.'
  }
  if (!/[0-9]/.test(password.value)) {
    return 'Password must contain at least one number.'
  }
  if (!/[!@#$%^&*]/.test(password.value)) {
    return 'Password must contain at least one special character.'
  }
  return ''
})

const passwordNotMatched = computed(() => {
  if (password.value !== confirmPassword.value) {
    return 'Passwords do not match'
  }
})

const register = async () => {
  if (password.value !== confirmPassword.value) {
    console.error('Passwords do not match')
    return
  }

  if (passwordComplexityMessage.value) {
    console.error('Password does not meet complexity requirements')
    return
  }

  try {
    const response = await axiosInstance.post('auth/register', {
      fullName: fullName.value,
      email: email.value,
      password: password.value
    })
    console.log('Registration successful:', response.data)
    router.push('/login')
  } catch (error: unknown) {
    if (error instanceof AxiosError) {
      if (error.response && error.response.data && error.response.data.message) {
        errorMessage.value = error.response.data.message
      } else {
        errorMessage.value = 'An error occurred. Please try again.'
      }
    } else {
      errorMessage.value = 'An unexpected error occurred.'
    }
  }
}
</script>