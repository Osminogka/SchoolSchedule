import { createRouter, createWebHistory } from "vue-router";

const routes = [
    {
        path: "/",
        name: "Schedule",
        component: () => import('@/components/MainPage.vue'),
        meta:{
            title: 'Расписание'
        }
    },
    {
        path: "/new",
        name: "New",
        component: () => import('@/components/NewSchedule.vue'),
        meta:{
            title: 'Новое расписание'
        }
    },
    {
        path: "/edit/:id",
        name: "Edit",
        component: () => import('@/components/EditSchedule.vue'),
        meta:{
            title: 'Редактирование расписания'
        }
    },
    {
        path: "/newcourse",
        name: "NewCourse",
        component: () => import('@/components/NewCourse.vue'),
        meta:{
            title: 'Новый курс'
        }
    },
    {
        path: "/:pathMatch(.*)*",
        name: "NotFound",
        component: () => import('@/components/NotFound.vue'),
        meta:{
            title: 'Страница не найдена'
        }
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach((to, from, next) => {
    document.title = to.meta.title || 'Default Title';
    next();
});

export default router;