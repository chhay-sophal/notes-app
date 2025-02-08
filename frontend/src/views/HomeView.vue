<template>
  <main class="p-4">
    <h1 class="text-2xl font-bold mb-4">This is the home view</h1>
    <div v-if="loading" class="text-center">Loading...</div>
    <div v-else-if="error" class="text-red-500">{{ error }}</div>
    <div v-else>
      <ul v-if="notes.length > 0" class="list-disc pl-5">
        <li v-for="note in notes" :key="note.id" class="py-2 border-b border-gray-300">{{ note.title }}</li>
      </ul>
      <div v-else class="text-center text-gray-500">
        <p>You don't have any notes.</p>
      </div>
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axiosInstance from '@/services/axiosInstance';

interface Note {
  id: number;
  title: string;
}

const notes = ref<Note[]>([]);
const loading = ref(true);
const error = ref('');

const router = useRouter();

const fetchNotes = async () => {
  try {
    const response = await axiosInstance.get('note/all');
    notes.value = response.data;
    console.log('notes:', notes.value);
  } catch (err) {
    error.value = 'Failed to fetch notes';
    console.error('Fetch notes failed:', err);
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  const token = localStorage.getItem('token');
  if (!token) {
    router.push('/login');
  } else {
    fetchNotes();
  }
});
</script>
