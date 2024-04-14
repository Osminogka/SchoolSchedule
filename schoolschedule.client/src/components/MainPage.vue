<script setup>
import LoadingAnimation from '@/view/LoadingAnimation.vue';

import { mainWeek } from '@/core/week';

import { ref, onMounted} from 'vue';

onMounted(async () => {
    await getSchedule();
});

const loading = ref(false);
const error = ref(null);

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

</script>

<template>
<div class="container">
    <div v-if="loading" class="loading-animation">
            <loading-animation />
        </div>
    <div v-else class="table-container">
        <div class="table-nav">
            <button class="left-button custom-button" @click="prevWeek()" />
            <div class="week-header">Неделя {{ mainWeek.weekNumber }}</div>
            <button class="right-button custom-button" @click="nextWeek()"/>
        </div>
        <div v-if="error" class="error-message">
            {{ error }}
        </div>
        <table v-else class="table table-striped text-center">
            <thead class="table-purple">
                <tr>
                    <th class="time" scope="col">Время</th>
                    <th class="time" scope="col">8:00-8:40</th>
                    <th class="time" scope="col">8:50-9:30</th>
                    <th class="time" scope="col">9:50-10:30</th>
                    <th class="time" scope="col">11:50-12:30</th>
                    <th class="time" scope="col">12:50-13:30</th>
                    <th class="time" scope="col">13:40-14:20</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th scope="row">Понедельник</th>
                    <td v-for="(course, index) of mainWeek.weekSchedule.monday" :key="index">{{ course }}</td>
                </tr>
                <tr>
                    <th scope="row">Вторник</th>
                    <td v-for="(course, index) of mainWeek.weekSchedule.tuesday" :key="index">{{ course }}</td>
                </tr>
                <tr>
                    <th scope="row">Среда</th>
                    <td v-for="(course, index) of mainWeek.weekSchedule.wednesday" :key="index">{{ course }}</td>
                </tr>
                <tr>
                    <th scope="row">Четверг</th>
                    <td v-for="(course, index) of mainWeek.weekSchedule.thursday" :key="index">{{ course }}</td>
                </tr>
                <tr>
                    <th scope="row">Пятница</th>
                    <td v-for="(course, index) of mainWeek.weekSchedule.friday" :key="index">{{ course }}</td>
                </tr>
            </tbody>
        </table>
        <router-link class="edit-button custom-button" :to="{name: 'Edit', params: {id: mainWeek.weekNumber}}" />
    </div>   
</div>
</template>

<style scoped>

.container{
    display: flex;
    width: 100vw;
    height: 60vh;
    align-items: center;
    justify-content: center;
}

.table-container{
    border: 3px solid black;
    border-radius: 0.5em;
    padding: 20px;
    background-color: white;
    margin-top: auto;
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
    padding:15px !important;
    border: 1px solid #ccc !important;
}

.table-purple{
    background:#6c7ae0 !important;color:#fff;
}

.time{
    background-color: black;
}

.error-message{
    color: red;
    font-size: 1.5rem;
    text-align: center;
}

</style>