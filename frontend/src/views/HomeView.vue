<template>
  <main class="p-4 max-w-3xl mx-auto">
    <!-- Header with Search and Sorting -->
    <div class="flex flex-col md:flex-row justify-between items-center mb-4 gap-4">
      <h1 class="text-2xl font-bold">All Notes</h1>
      <div class="flex items-center gap-2 w-full md:w-auto">
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Search notes..."
          class="w-full md:w-64 p-2 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400"
        />
        <select
          v-model="sortOrder"
          class="p-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
        >
          <option value="newest">Newest First</option>
          <option value="oldest">Oldest First</option>
          <option value="alpha">Alphabetical</option>
        </select>
        <button
          @click="goToNoteEditor"
          class="bg-blue-500 text-white px-3 py-2 rounded-lg shadow hover:bg-blue-600 transition"
          title="Create Note"
        >
          ✏️
        </button>
      </div>
    </div>

    <!-- Notes List -->
    <div v-if="loading" class="text-center">Loading...</div>
    <div v-else-if="error" class="text-red-500">{{ error }}</div>
    <div v-else>
      <ul v-if="filteredAndSortedNotes.length > 0" class="space-y-4">
        <li
          v-for="note in filteredAndSortedNotes"
          :key="note.id"
          class="flex justify-between items-center p-4 border border-gray-300 rounded-lg shadow-lg cursor-pointer hover:shadow-xl dark:hover:bg-gray-700 transition-all"
        >
          <span @click="goToEditNote(note.id)" class="flex-grow text-lg font-medium">
            {{ note.title }}
          </span>
          <button
            @click="deleteNote(note.id)"
            class="text-red-500 hover:text-red-700 transition"
            title="Delete Note"
          >
            🗑️
          </button>
        </li>
      </ul>
      <div v-else class="text-center text-gray-500">
        <p>No matching notes found.</p>
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
  createdAt: string;
  isDeleted?: boolean;
}

const notes = ref<Note[]>([]);
const loading = ref(true);
const error = ref('');
const searchQuery = ref('');
const sortOrder = ref('newest'); // Default sort order

const router = useRouter();

// Filtered and sorted notes
const filteredAndSortedNotes = computed(() => {
  let filtered = notes.value.filter(note =>
    note.title.toLowerCase().includes(searchQuery.value.toLowerCase()) && !note.isDeleted
  );

  return filtered.sort((a, b) => {
    if (sortOrder.value === 'newest') return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
    if (sortOrder.value === 'oldest') return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
    if (sortOrder.value === 'alpha') return a.title.localeCompare(b.title);
    return 0;
  });
});

// Fetch notes from API
const fetchNotes = async () => {
  try {
    const response = await axiosInstance.get('note/all');
    notes.value = response.data;
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
    // Optimistically remove from UI
    notes.value = notes.value.filter(note => note.id !== id);
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
