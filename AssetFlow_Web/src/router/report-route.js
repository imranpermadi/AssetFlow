export default [
{
    path: "/Report/DocumentReport",
    name: "DocumentReport",
    component:  () => import("@/components/Report/DocumentReport.vue"),
    meta: {
        name: "DocumentReport",
        menuTitle: "Document Report",
    },
    props: true
},
{
    path: "/Report/DocumentMovementReport",
    name: "DocumentMovementReport",
    component:  () => import("@/components/Report/MovementReport.vue"),
    meta: {
        name: "DocumentMovementReport",
        menuTitle: "Document Movement Report",
    },
    props: true
},
];
