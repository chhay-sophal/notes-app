<template>
  <main class="p-4">
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">All</h1>
      <svg
        xmlns="http://www.w3.org/2000/svg"
        class="h-6 w-6 text-gray-500 cursor-pointer hover:text-gray-700"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
        stroke-width="2"
        @click="goToNoteEditor"
      >
        <path stroke-linecap="round" stroke-linejoin="round" d="M12 4v16m8-8H4" />
      </svg>
    </div>
    <div v-if="loading" class="text-center">Loading...</div>
    <div v-else-if="error" class="text-red-500">{{ error }}</div>
    <div v-else>
      <ul v-if="notes.length > 0" class="list-disc pl-5">
        <li
          v-for="note in notes"
          :key="note.id"
          class="py-2 border-b border-gray-300 cursor-pointer hover:bg-gray-100"
          @click="goToEditNote(note.id)"
        >
          {{ note.title }}
        </li>
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

const goToNoteEditor = () => {
  router.push('/note');
};

const goToEditNote = (id: number) => {
  router.push(`/note/${id}`);
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
