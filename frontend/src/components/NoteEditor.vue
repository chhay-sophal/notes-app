<template>
  <div class="w-full max-w-2xl mx-auto p-6 shadow-lg rounded-lg mt-8 min-h-screen flex flex-col dark:border dark:border-gray-700">
    <!-- Title Input -->
    <input
      type="text"
      v-model="title"
      placeholder="Enter your note title"
      class="w-full text-3xl font-bold border-b border-gray-300 focus:outline-none focus:border-blue-500 p-3 mb-4"
      @input="debouncedSave"
    />

    <!-- Content Textarea -->
    <textarea
      ref="contentTextarea"
      v-model="content"
      placeholder="Write your note here..."
      class="w-full flex-grow mt-4 p-4 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400 resize-none leading-relaxed overflow-hidden"
      @input="adjustHeight, debouncedSave"
    ></textarea>

    <p class="mt-3">Created at: {{ createdAt }}</p>
    <p v-if="updatedAt">Edited at: {{ updatedAt }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick } from "vue";
import axiosInstance from "@/services/axiosInstance";
import { useRoute, useRouter } from "vue-router";
import debounce from "lodash/debounce";

const title = ref("");
const content = ref("");
const contentTextarea = ref(null);
const createdAt = ref("");
const updatedAt = ref("");
const route = useRoute();
const router = useRouter();
const saving = ref(false);
let noteId = ref(null);

const adjustHeight = () => {
  if (contentTextarea.value) {
    contentTextarea.value.style.height = "auto"; // Reset height
    contentTextarea.value.style.height = `${contentTextarea.value.scrollHeight}px`; // Set to scroll height
  }
};

const debouncedSave = debounce(async () => {
  saving.value = true;
  try {
    if (noteId.value) {
      await axiosInstance.put(`note/${noteId.value}`, { title: title.value, content: content.value });
      updatedAt.value = new Date().toLocaleString();
    } else {
      const response = await axiosInstance.post("note", { title: title.value, content: content.value });
      noteId.value = response.data.id;
      createdAt.value = new Date().toLocaleString();
    }
    saving.value = false;
  } catch (error) {
    console.error("Failed to save note:", error);
    saving.value = false;
  }
}, 2000); // Save after 2 seconds of inactivity

const loadNote = async () => {
  const id = route.params.id;
  if (id) {
    noteId.value = id;
    try {
      const response = await axiosInstance.get(`note/${id}`);
      title.value = response.data.title;
      content.value = response.data.content;
      createdAt.value = new Date(response.data.createdAt).toLocaleString();
      if (response.data.updatedAt) {
        updatedAt.value = new Date(response.data.updatedAt).toLocaleString();
      }
      await nextTick(); // Wait for DOM update
      adjustHeight(); // Adjust height after setting content
    } catch (error) {
      console.error("Failed to load note:", error);
    }
  }
};

onMounted(() => {
  loadNote();
});
</script>
