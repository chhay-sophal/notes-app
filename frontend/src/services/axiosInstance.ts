import axios from 'axios'

const axiosInstance = axios.create({
  baseURL: 'http://localhost:5389/api/'
})

export default axiosInstance