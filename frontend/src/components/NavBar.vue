<template>
  <nav class="space-x-4">
    <RouterLink to="/" class="hover:underline">Home</RouterLink>
    <RouterLink to="/about" class="hover:underline">About</RouterLink>
    <RouterLink to="/login" class="hover:underline">Login</RouterLink>
    <RouterLink to="/register" class="hover:underline">Register</RouterLink>
  </nav>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>