<script setup lang="ts">

</script>

<template>
    <div class="space-y-4">
        <div v-for="n in 3" :key="n" class="list-row flex items-center gap-3 p-4 border border-base-300 rounded-lg">
            <div class="skeleton w-6 h-6 rounded"></div>
            <div class="skeleton h-4 w-3/4"></div>
            <div class="skeleton w-8 h-8 rounded ml-auto"></div>
        </div>
    </div>
</template>