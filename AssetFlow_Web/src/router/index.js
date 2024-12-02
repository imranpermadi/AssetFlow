import { createWebHistory, createRouter } from "vue-router";
import Home from "@/components/Home.vue";
import BaseRoute from "./base-route";


import MasterDataRoute from "./masterdata-route";
import TransactionRoute from "./transaction-route";
import ReportRoute from "./report-route.js"
//import UploadRoute from "./upload-route.js";
//import LogRoute from "./log-route.js";

const routes = [
    {
        path: "/",
        name: "Home",
        component: Home,
        meta: {
            name: "Home",
            menuTitle: "Home",
            requiresAuth: true
        }
    },
    {
        path: "/:pathMatch(.*)*",
        name: "NotFound",
        component: () => import("@/components/Base/Helper/NotFound/Index.vue"),
        meta:{
            name: "PageNotFound",
            menuTitle : "Page Not Found",
            requiresAuth: true
        },
        props: true
    },
    ...BaseRoute,
    ...MasterDataRoute,
    ...TransactionRoute,
    ...ReportRoute,
    //...UploadRoute,
    //...LogRoute
    
    
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;