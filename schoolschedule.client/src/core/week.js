import { reactive, ref } from 'vue';

export const mainWeek = reactive({
    weekNumber: 1,
    maxWeek: 1,
    weekSchedule:{
        monday: [],
        tuesday: [],
        wednesday: [],
        thursday: [],
        friday: []
    }
});

export const courses = ref([]);