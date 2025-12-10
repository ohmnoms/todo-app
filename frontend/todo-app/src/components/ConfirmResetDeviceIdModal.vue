<script setup lang="ts">
defineProps({ open: Boolean });
const emit = defineEmits<{
  (e: 'confirm'): void;
  (e: 'cancel'): void;
}>();
</script>

<template>
  <dialog class="modal" :open="open">
    <form method="dialog" class="modal-box">
      <h3 class="font-bold text-lg mb-4">Reset Device ID</h3>
      <p class="mb-4">Resetting will generate a new device identifier and remove the old one from local storage.  Existing todos associated with the old identifier will no longer be visible from this device. Do you want to continue?</p>
      <div class="flex justify-end gap-2">
        <button type="button" class="btn" @click="emit('cancel')">Cancel</button>
        <button type="button" class="btn btn-error" @click="emit('confirm')">Confirm</button>
      </div>
    </form>
  </dialog>
</template>
