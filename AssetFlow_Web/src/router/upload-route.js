export default [
    {
        path: "/Upload/Transporter",
        name: "UploadTransporter",
        component: () => import("@/components/Upload/Transporter.vue"),
        meta: {
            title:'Transporter',
            menuTitle: "Upload Transporter",
            name: "Transporter",
            requiresAuth: true,
        },
        props: true,
    }
];