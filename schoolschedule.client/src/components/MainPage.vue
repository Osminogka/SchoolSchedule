<script setup>
import LoadingAnimation from '@/view/LoadingAnimation.vue';

import { mainWeek } from '@/core/week';

import { ref, onMounted, computed} from 'vue';

const dayOfWeeks = ['monday', 'tuesday', 'wednesday', 'thursday', 'friday'];
const loading = ref(false);
const error = ref(null);


onMounted(async () => {
    await getSchedule();
});

async function prevWeek(){
    if(mainWeek.weekNumber > 1){
        mainWeek.weekNumber -= 1;
        await getSchedule();
    }
}

async function nextWeek(){
    if(mainWeek.weekNumber < 52){
        mainWeek.weekNumber += 1;
        await getSchedule();
    }
}

async function getSchedule(){
    loading.value = true;
    let response = await fetch('/api/schedule/get?week=' + mainWeek.weekNumber);
    if(response.ok){
        let data = await response.json();
        if(data.success){
            mainWeek.weekSchedule = data.week;
            mainWeek.maxWeek = data.week.weekNumber + 1;
            error.value = null;
        }
        else
            error.value = data.message;
    } else {
        //error.value = 'Ошибка при загрузке расписания';
    }
    loading.value = false;
}

const checkIfThereAreLessons = computed(() =>{
    if(mainWeek.weekSchedule.monday.length > 0 && mainWeek.weekSchedule.tuesday.length > 0 && mainWeek.weekSchedule.wednesday.length > 0 && 
        mainWeek.weekSchedule.thursday.length > 0 && mainWeek.weekSchedule.friday.length > 0){
        return true;
    }
    return false;
});

function getCourse(day, time){
    return mainWeek.weekSchedule[day][time]
}

</script>

<template>
<div v-if="loading" class="loading-animation">
    <loading-animation />
</div>
<div v-else class="container">
    <div class="table-container">
        <div class="table-nav">
            <button class="left-button custom-button" @click="prevWeek()" />
            <div class="week-header">Неделя {{ mainWeek.weekNumber }}</div>
            <button class="right-button custom-button" @click="nextWeek()"/>
        </div>
        <div v-if="error" class="error-message">
            {{ error }}
        </div>
        <div v-else-if="!checkIfThereAreLessons" class="no-schedule">
            Нет расписания на эту неделю
        </div>
        <div v-else class="table-overflow">
            <table class="table table-striped text-center">
                <thead class="table-purple">
                    <tr>
                        <th class="time" scope="col">Время</th>
                        <th class="time" scope="col">Пн</th>
                        <th class="time" scope="col">Вт</th>
                        <th class="time" scope="col">Ср</th>
                        <th class="time" scope="col">Чт</th>
                        <th class="time" scope="col">Пт</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <th scope="row">8:00-8:40</th>
                        <td v-for="(course, index) of dayOfWeeks" :key="index">{{ getCourse(course, 0) == "No" ? "Нет занятий" : getCourse(course, 0) }}</td>
                    </tr>
                    <tr>
                        <th scope="row">8:50-9:30</th>
                        <td v-for="(course, index) of dayOfWeeks" :key="index">{{ getCourse(course, 1) == "No" ? "Нет занятий" : getCourse(course, 1) }}</td>
                    </tr>
                    <tr>
                        <th scope="row">9:50-10:30</th>
                        <td v-for="(course, index) of dayOfWeeks" :key="index">{{ getCourse(course, 2) == "No" ? "Нет занятий" : getCourse(course, 2) }}</td>
                    </tr>
                    <tr>
                        <th scope="row">11:50-12:30</th>
                        <td v-for="(course, index) of dayOfWeeks" :key="index">{{ getCourse(course, 3) == "No" ? "Нет занятий" : getCourse(course, 3) }}</td>
                    </tr>
                    <tr>
                        <th scope="row">12:50-13:30</th>
                        <td v-for="(course, index) of dayOfWeeks" :key="index">{{ getCourse(course, 4) == "No" ? "Нет занятий" : getCourse(course, 4) }}</td>
                    </tr>
                    <tr>
                        <th scope="row">13:40-14:20</th>
                        <td v-for="(course, index) of dayOfWeeks" :key="index">{{ getCourse(course, 5) == "No" ? "Нет занятий" : getCourse(course, 5) }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>   
</div>
</template>

<style scoped>

.container{
    display: flex;
    width: 100vw;
    height: 90vh;
    align-items: flex-start;
    justify-content: center;
}

.table-container{
    border: 3px solid black;
    border-radius: 0.5em;
    padding: 20px;
    background-color: white;
    width: 90%;
    position: fixed;
    left: 1;
    right: 1;
    margin-top: 13em;
}

.table-overflow{
    overflow-y: auto;
}

.table-nav{
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
}

.week-header{
    font-size: 2rem;
    font-weight: bold;
    text-align: center;
}

.left-button{
    background-image: url('../assets/svg/arrowleft.svg');
}

.right-button{
    background-image: url('../assets/svg/arrowright.svg');
}

.edit-button{
    background-image: url('../assets/svg/edit.svg');
    display: inline-block;
    margin-left: 95%;
}

.custom-button{
    background-position: center;
    background-repeat: no-repeat;
    background-size: contain;
    width: 2em;
    height: 2em;
    background-color: white;
    border: none;
}

table{
    background-color: rgb(255, 251, 251);
    border: 1px solid #ccc;
}

table th,tr,td{
    padding:15px;
    border: 1px solid #ccc !important;
}

.table-purple{
    background:#6c7ae0 !important;color:#fff;
}

.time{
    background-color: black;
}

.no-schedule{
    font-size: 2rem;
    text-align: center;
}

.error-message{
    color: red;
    font-size: 1.5rem;
    text-align: center;
}

@media screen and (max-width: 768px){
    .container{
        height: 85vh;
    }

    .table-container{
        min-width: 300px;
        min-height: 300px;
    }

    table th,tr,td{
        padding: 1px;
        width: 20px;
    }

}
</style>