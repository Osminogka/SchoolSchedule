<script setup>
import LoadingAnimation from '@/view/LoadingAnimation.vue';

import { mainWeek, courses } from '@/core/week';

import { reactive, ref, onMounted } from 'vue';

const loading = ref(false);
const error = ref(null);
const result = ref(null);

const dayOfWeeks = ['monday', 'tuesday', 'wednesday', 'thursday', 'friday'];

const week = reactive({
    monday: ['', '', '', '', '', ''],
    tuesday: ['', '', '', '', '', ''],
    wednesday: ['', '', '', '', '', ''],
    thursday: ['', '', '', '', '', ''],
    friday: ['', '', '', '', '', ''],
    weekNumber: mainWeek.maxWeek
});

onMounted(async () => {
    await getCourses();
});

async function getCourses(){
    loading.value = true;
    let response = await fetch('/api/courses/get');
    if(response.ok){
        let data = await response.json();
        if(data.success){
            courses.value = data.courses;
        }
        else{
            error.value = data.message;
        }
    }
    else{
        error.value = 'Ошибка при загрузке курсов';
    }
    loading.value = false;
}

async function postNewSchedule(){
    loading.value = true;
    let response = await fetch('/api/schedule/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(week)
    });
    if(response.ok){
        let data = await response.json();
        if(data.success){
            result.value = data.message;
        }
        else{
            error.value = data.message;
        }
    }
    else{
        error.value = 'Ошибка при сохранении расписания';
    }
    loading.value = false;
}

</script>

<template>
    <div v-if="loading">
            <loading-animation />
    </div>
    <div v-else class="container">
        <div class="header-text">Новое расписание для недели {{ mainWeek.maxWeek }}</div>
        <table>
            <thead>
                <th>День недели</th>
                <th>Понедельник</th>
                <th>Вторник</th>
                <th>Среда</th>
                <th>Четверг</th>
                <th>Пятница</th>
            </thead>
            <tbody>
                <tr>
                    <td>8:00-8:40</td>
                    <td v-for="(dayNumber, index) of dayOfWeeks">
                        <div>
                            <select v-model="week[dayNumber][0]">
                                <option v-for="course in courses" :value="course">{{ course }}</option>
                            </select>
                        </div>
                    </td>                 
                </tr>
                <tr>
                    <td>8:50-9:30</td>
                    <td v-for="(dayNumber, index) of dayOfWeeks">
                        <div>
                            <select v-model="week[dayNumber][1]">
                                <option v-for="course in courses" :value="course">{{ course }}</option>
                            </select>
                        </div>
                    </td>   
                </tr>
                <tr>
                    <td>9:50-10:30</td>
                    <td v-for="(dayNumber, index) of dayOfWeeks">
                        <div>
                            <select v-model="week[dayNumber][2]">
                                <option v-for="course in courses" :value="course">{{ course }}</option>
                            </select>
                        </div>
                    </td>   
                </tr>
                <tr>
                    <td>11:50-12:30</td>
                    <td v-for="(dayNumber, index) of dayOfWeeks">
                        <div>
                            <select v-model="week[dayNumber][3]">
                                <option v-for="course in courses" :value="course">{{ course }}</option>
                            </select>
                        </div>
                    </td>   
                </tr>
                <tr>
                    <td>12:50-13:30</td>
                    <td v-for="(dayNumber, index) of dayOfWeeks">
                        <div>
                            <select v-model="week[dayNumber][4]">
                                <option v-for="course in courses" :value="course">{{ course }}</option>
                            </select>
                        </div>
                    </td>   
                </tr>
                <tr>
                    <td>13:40-14:20</td>
                    <td v-for="(dayNumber, index) of dayOfWeeks">
                        <div>
                            <select v-model="week[dayNumber][5]">
                                <option v-for="course in courses" :value="course">{{ course }}</option>
                            </select>
                        </div>
                    </td>   
                </tr>
            </tbody>
        </table>
        <button class="save-button" @click="postNewSchedule()">Сохранить</button>
    </div>
</template>

<style scoped>

.container{
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-top: 10vh;
    width: 100vw;
    height: 60vh;
    background-color: white;
    border: 3px solid black;
    border-radius: 1em;
}

table th,tr,td{
    padding:10px !important;
    border: 1px solid #ccc !important;
}

table{
    width: 80%;
    margin-top: 1em;
    border-collapse: collapse;
    border: 4px solid black;
    text-align: center;
}

th{
    width: 350px;
    border: 2px solid black;
}

td{
    border: 2px solid black;
}

.header-text{
    font-family: 'Lora', serif;
    font-size: 2rem;
}

.save-button{
    margin-top: 1em;
    padding: 0.5em 1em;
    background-color: orange;
    border: none;
    border-radius: 0.5em;
    font-size: 1.5rem;
    font-family: 'Lora', serif;
    cursor: pointer;
}

select{
    background-color: orange;
    border: 1px solid black;
    border-radius: 0.2em;
    width: 100%;
    height: 100%;
    font-size: 1rem;
    cursor: pointer;
}

option{
    background-color: orange;
    color: black;
}

</style>