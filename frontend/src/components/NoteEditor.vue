<template>
  <div class="w-full max-w-2xl mx-auto p-4">
    <!-- Title Input -->
    <input
      type="text"
      v-model="title"
      placeholder="Title"
      class="w-full text-2xl font-semibold border-b border-gray-300 focus:outline-none focus:border-blue-500 p-2"
      @input="debouncedSave"
    />

    <!-- Content Textarea -->
    <textarea
      v-model="content"
      placeholder="Write your note..."
      class="w-full h-64 mt-4 p-3 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400 resize-none"
      @input="debouncedSave"
    ></textarea>
  </div>
</template>

<script setup>
import { ref, watch, onMounted } from 'vue';
import { useDebounceFn } from '@vueuse/core';
import axiosInstance from '../services/axiosInstance';

const props = defineProps({
  noteId: String
});

const title = ref('');
const content = ref('');
const isNewNote = ref(!props.noteId);
const currentNoteId = ref(props.noteId || '');

const saveNote = async () => {
  if (!title.value.trim() || !content.value.trim()) return;

  const noteData = { title: title.value, content: content.value };

  try {
    if (isNewNote.value) {
      const response = await axiosInstance.post('/note/', noteData);
      currentNoteId.value = response.data.id;
      isNewNote.value = false; 
    } else {
      await axiosInstance.put(`/note/${currentNoteId.value}`, noteData);
    }
  } catch (error) {
    console.error('Error saving note:', error);
  }
};

// Auto-save when user stops typing
const debouncedSave = useDebounceFn(saveNote, 1000);

// Load existing note if noteId is provided
onMounted(async () => {
  if (props.noteId) {
    try {
      const response = await axiosInstance.get(`note/${props.noteId}`);
      title.value = response.data.title;
      content.value = response.data.content;
      isNewNote.value = false;
      currentNoteId.value = props.noteId;
    } catch (error) {
      console.error('Error loading note:', error);
    }
  }
});
</script>