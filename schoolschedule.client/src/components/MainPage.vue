<script setup>
import LoadingAnimation from '@/view/LoadingAnimation.vue';

import { week } from '@/core/week';

import { ref, onMounted} from 'vue';

onMounted(async () => {
    await getSchedule();
});

const loading = ref(false);
const error = ref(null);

async function prevWeek(){
    if(week.weekNumber > 1){
        week.weekNumber -= 1;
        await getSchedule();
    }
}

async function nextWeek(){
    if(week.weekNumber < 52){
        week.weekNumber += 1;
        await getSchedule();
    }
}

async function getSchedule(){
    loading.value = true;
    let response = await fetch('/api/schedule?week=' + week.weekNumber);
    if(response.ok){
        let data = await response.json();
        if(data.success){
            week.weekSchedule = data.week;
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
    <div class="table-container">
        <div v-if="!loading" class="table-nav">
            <button class="left-button custom-button" @click="prevWeek()" />
            <div class="week-header">Неделя {{ week.weekNumber }}</div>
            <button class="right-button custom-button" @click="nextWeek()"/>
        </div>
        <div v-if="loading" class="loading-animation">
            <loading-animation />
        </div>
        <div v-else-if="error" class="error-message">
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
                    <td>English</td>
                    <td>Physics</td>
                    <td>Maths</td>
                    <td>Tamil</td>
                    <td>Computer</td>
                    <td>Chemistry</td>
                </tr>
                <tr>
                    <th scope="row">Вторник</th>
                    <td>English</td>
                    <td>Physics</td>
                    <td>Maths</td>
                    <td>Tamil</td>
                    <td>Computer</td>
                    <td>Chemistry</td>
                </tr>
                <tr>
                    <th scope="row">Среда</th>
                    <td>English</td>
                    <td>Physics</td>
                    <td>Maths</td>
                    <td>Tamil</td>
                    <td>Computer Lab</td>
                    <td>Chemistry</td>
                </tr>
                <tr>
                    <th scope="row">Четверг</th>
                    <td>English</td>
                    <td>Physics</td>
                    <td>Maths</td>
                    <td>Tamil</td>
                    <td>Computer</td>
                    <td>Chemistry</td>
                </tr>
                <tr>
                    <th scope="row">Пятница</th>
                    <td>Science Lab</td>
                    <td>English</td>
                    <td>Computer</td>
                    <td>Chemistry</td>
                    <td>Maths</td>
                    <td>Physics</td>
                </tr>
            </tbody>
        </table>
        <router-link class="edit-button custom-button" :to="{name: 'Edit', params: {id: week.weekNumber}}" />
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