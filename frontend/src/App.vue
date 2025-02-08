<script setup lang="ts">
import { RouterView, useRoute } from 'vue-router'
import NavBar from './components/NavBar.vue'

const route = useRoute()
</script>

<template>
  <header class="bg-blue-600 text-white p-4 shadow-md">
    <div class="container mx-auto flex justify-between items-center">
      <h1 class="text-2xl font-bold">Notes App</h1>
      <NavBar v-if="route.name !== 'login' && route.name !== 'register'" />
    </div>
  </header>

  <main class="container mx-auto p-4">
    <RouterView />
  </main>
</template>
