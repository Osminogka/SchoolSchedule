<script setup>
import {reactive, computed} from 'vue';

const monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
];

const todayDate = new Date();

const currentDay = computed(() => todayDate.getDate());
const currentMonth = computed(() => monthNames[todayDate.getMonth()]);
const currentYear = computed(() => todayDate.getFullYear());


const showInterface = reactive({
    sidebar: false
})

function openSidebar(isExplicit = false){
    let tempProp = showInterface.sidebar;
    if(!isExplicit) tempProp = !tempProp;
    showInterface.sidebar = tempProp;
    let new_value = tempProp ? 'rotate(90deg)' : "rotate(0deg)";
    document.getElementById('open-sidebar').style.transform = new_value;
}

function closeSidebar(){
    if(showInterface.sidebar && event.target.id != 'open-sidebar'){
        showInterface.sidebar = false;
        openSidebar(true);
    }
}

</script>

<template>
<div class="container-header">
    <button id="open-sidebar" class="open-sidebar-button" @click="openSidebar()"/>
    <div class="info">
        <div class="header-text">Расписание</div>
        <div>{{ currentDay }} {{ currentMonth }} {{ currentYear }}</div>
    </div>
</div>
<Transition name="sidebar">
    <div v-if="showInterface.sidebar" class="container-sidebar">
        <div class="router-links">
            <router-link class="link" :to="{name: 'Schedule'}" @click="closeSidebar()">Расписание</router-link>
            <router-link class="link" :to="{name: 'New'}" @click="closeSidebar()">Новое расписание</router-link>
            <router-link class="link" :to="{name: 'NewCourse'}" @click="closeSidebar()">Новый курс</router-link>
        </div>
    </div>
</Transition>
</template>

<style scoped>

.container-header{
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100vw;
    height: 10vh;
    background-color: orange;
}

.container-sidebar{
    display: flex;
    width: 20vw;
    height: 90vh;
    background-color: orange;
    border-right: 5px solid black;
    border-top: 5px solid black;
    position: absolute;
    z-index: 1;
}

.header-text{
    font-family: 'Schedule', sans-serif;
    font-size: 2rem;
    margin-right: 1em;
    color: black;
}

.open-sidebar-button{
    align-self: center;
    margin-left: 1em;
    background-color: orange;
    border:none;
    background-image: url('@/assets/svg/sidebar.svg');
    background-size: 100%;
    background-repeat: no-repeat;
    background-position: center;
    width: 2.5em;
    height: 2.5em;
    cursor: pointer;
    transition: transform 0.3s ease;
    --rotation: 0deg;
    transform: rotate(var(--rotation, 0deg));
}

.link{
    font-family: 'Roboto', sans-serif;
    font-size: 1.5rem;
    margin: 5px;
    text-decoration: none;
    color: black;
    width: 100%;
    padding: 10px;
}

.link:hover{
    background-color: rgb(251, 195, 93);
    border: 1px solid orange;
    border-radius: 0.2em;
}

.router-links{
    display: flex;
    flex-direction: column;
    width: 80%;
}

.sidebar{
    width: 20vw;
    height: 90vh;
    background-color: #f0f0f0;
    border-right: 1px solid #d0d0d0;
}

.sidebar-enter-active, .sidebar-leave-active {
    transition: all 0.3s ease-out;
}

.sidebar-leave-active {
    transition: all 0.3 ease-out;
}

.sidebar-enter-from,
.sidebar-leave-to {
    transform: translateX(-100px);
    opacity: 0;
}

@font-face {
    font-family: 'Schedule';
    src: url('@/assets/font/schedule.ttf') format('truetype');
}

</style>