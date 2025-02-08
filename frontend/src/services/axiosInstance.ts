import axios from 'axios'
import { useRouter } from 'vue-router'

const axiosInstance = axios.create({
  baseURL: 'http://localhost:5250/api/'
})

// Function to check if the token is expired
const isTokenExpired = (token: string) => {
  const payload = JSON.parse(atob(token.split('.')[1]))
  const exp = payload.exp * 1000 // Convert to milliseconds
  return Date.now() > exp
}

// Add a request interceptor to include the token in the headers
axiosInstance.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token) {
    if (isTokenExpired(token)) {
      // Token is expired, log out the user
      localStorage.removeItem('token')
      const router = useRouter()
      router.push('/login')
      return Promise.reject(new Error('Token expired'))
    }
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
}, error => {
  return Promise.reject(error)
})

export default axiosInstance