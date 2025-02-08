<template>
  <div class="p-4">
    <!-- Stylish Back Button -->
    <button
      @click="goBack"
      class="cursor-pointer flex items-center gap-2 text-blue-600 font-medium hover:text-blue-700 transition-all duration-200 ease-in-out"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
        <polyline points="15 18 9 12 15 6"></polyline>
      </svg>
      Back
    </button>

    <NoteEditor :noteId="noteId" />
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from "vue-router";
import { computed } from "vue";
import NoteEditor from "../components/NoteEditor.vue";

const route = useRoute();
const router = useRouter();
const noteId = computed(() => route.params.id || null);

const goBack = () => {
  router.back();
};
</script>
