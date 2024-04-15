<script setup>
import { courses } from '@/core/week';

import LoadingAnimation from '@/view/LoadingAnimation.vue';

import { ref, onMounted } from 'vue';

const loading = ref(false);
const error = ref(null);

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

async function deleteCourse(courseName){
    loading.value = true;
    let response = await fetch('/api/courses/delete?courseName=' + courseName);
    if(response.ok){
        let data = await response.json();
        if(data.success){
            courses.value = courses.value.filter(course => course !== courseName);
        }
        else{
            error.value = data.message;
        }
    }
    else{
        error.value = 'Ошибка при удалении курса';
    }
    loading.value = false;
}

async function postNewCourse(){
    loading.value = true;
    let response = await fetch('/api/courses/new?course='+newCourseName.value);
    if(response.ok){
        let data = await response.json();
        if(data.success){
            courses.value.push(newCourseName.value);
            newCourseName.value = '';
        }
        else{
            error.value = data.message;
        }
    }
    else{
        error.value = 'Ошибка при добавлении курса';
    }
    loading.value = false;
}

const newCourseName = ref('');

</script>

<template>
    <div v-if="loading">
        <loading-animation />
    </div>
    <div v-else class="container">
        <div class="container-new-course">
        <div>
            <div class="existing-couse-header">Добавить новый курс</div>
            <div class="new-course-container">
                <input v-model="newCourseName" type="text" placeholder="Название курса" />
                <button class="add-course-button" @click="postNewCourse()">Добавить курс</button>
                <div v-if="error" class="error-message">
                    {{ error }}
            </div>
        </div>
        </div>
        <div>
            <div class="existing-couse-header">Существующие курсы</div>
            <div class="existing-courses-container">
                <div class="existing-course-entity" v-for="(course,index) in courses" :key="index">
                    {{ course }}
                    <button class="delete-course-button custom-button" @click="deleteCourse(course)"/>
                </div>
            </div>
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

.container-new-course{
    border: 3px solid black;
    border-radius: 0.5em;
    padding: 20px;
    background-color: white;
    width: 90%;
    left: 1;
    right: 1;
    margin-top: 10em;
}

.new-course-container{
    display: flex;
    flex-direction: column;
    border: 3px solid black;
    border-radius: 1em;
    padding: 1em;
    height: 150px;
}

.existing-courses-container{
    display: flex;
    flex-direction: column;
    border: 3px solid black;
    border-radius: 1em;
    padding: 1em;
    margin: 1em;
    font-size: 1.5rem;
    overflow-y: auto;
    max-height: 450px;
}

.existing-course-entity{
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    padding: 10px;
    border-bottom: 1px solid black;
}

.existing-couse-header{
    font-size: 2rem;
    font-weight: bold;
    text-align: center;
}

.delete-course-button{
    background-image: url('../assets/svg/delete.svg');
    background-color: white;
}

.add-course-button{
    background-color: orange;
    border: 2px solid black;
    border-radius: 0.5em;
    margin-top: 1em;
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
</style>