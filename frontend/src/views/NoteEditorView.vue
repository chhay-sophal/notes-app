<template>
  <div class="p-4">
    <button @click="goBack" class="text-blue-500 hover:underline mb-4">
      ← Back
    </button>
    <NoteEditor :noteId="noteId" />
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router';
import { ref, computed } from 'vue';
import NoteEditor from '../components/NoteEditor.vue';

const route = useRoute();
const router = useRouter();

// Get noteId from the route params (if available)
const noteId = computed(() => route.params.id || null);

const goBack = () => {
  router.back();
};
</script>
