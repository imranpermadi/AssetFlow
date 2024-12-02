
export default [
    {
        path: "/Approval/Index",
        name: "ApprovalIndex",
        component: () => import("@/components/Approval/Index.vue"),
        meta: {
            name: "ApprovalIndex",
            menuTitle: "Approval",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/Example/Index",
        name: "ExampleIndex",
        component: () => import("@/components/Example/Index.vue"),
        meta:{
            name: "ExampleIndex",
            menuTitle : "Example",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/Example/Editor",
        name: "ExampleEditor",
        component: () => import("@/components/Example/Editor.vue"),
        meta:{
            name: "ExampleEditor",
            menuTitle : "Example",
            requiresAuth: true
        },
        props: true
    },
    
]