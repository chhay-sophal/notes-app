<template>
  <main class="p-4">
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">All Notes</h1>
      <span
        class="cursor-pointer"
        @click="goToNoteEditor"
        title="Create Note"
      >
        ✏️
      </span>
    </div>
    <div v-if="loading" class="text-center">Loading...</div>
    <div v-else-if="error" class="text-red-500">{{ error }}</div>
    <div v-else>
      <ul v-if="notes.length > 0" class="space-y-4">
        <li
          v-for="note in filteredNotes"
          :key="note.id"
          class="flex justify-between items-center p-4 bg-white border border-gray-300 rounded-lg shadow-lg cursor-pointer hover:shadow-xl hover:bg-gray-50 transition-all"
        >
          <span @click="goToEditNote(note.id)" class="flex-grow text-lg font-medium text-gray-800">
            {{ note.title }}
          </span>
          <span
            class="cursor-pointer"
            @click="deleteNote(note.id)"
            title="Delete Note"
          >
            🗑️
          </span>
        </li>
      </ul>
      <div v-else class="text-center text-gray-500">
        <p>You don't have any notes.</p>
      </div>
    </div>
  </main>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import axiosInstance from '@/services/axiosInstance';

interface Note {
  id: number;
  title: string;
  isDeleted?: boolean; // Optional field to track if a note is deleted
}

const notes = ref<Note[]>([]);
const loading = ref(true);
const error = ref('');

const router = useRouter();

// Filtered notes to exclude deleted ones
const filteredNotes = computed(() => {
  return notes.value.filter(note => !note.isDeleted);
});

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

const deleteNote = async (id: number) => {
  try {
    await axiosInstance.delete(`note/${id}`);
    // Mark the note as deleted in the frontend (filtering it out)
    const noteToDelete = notes.value.find(note => note.id === id);
    if (noteToDelete) {
      noteToDelete.isDeleted = true;
    }
    console.log('Note deleted');
  } catch (err) {
    error.value = 'Failed to delete note';
    console.error('Delete note failed:', err);
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
