import { reactive, ref } from 'vue';

export const week = reactive({
    weekNumber: 1,
    weekSchedule:{
        monday: [],
        tuesday: [],
        wednesday: [],
        thursday: [],
        friday: []
    }
});

export const courses = ref([]);