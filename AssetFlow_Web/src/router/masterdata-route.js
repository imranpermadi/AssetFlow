export default [
    {
        path: "/Company/Index",
        name: "CompanyIndex",
        component: () => import("@/components/Company/Index.vue"),
        meta: {
            name: "CompanyIndex",
            menuTitle: "Company",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/Company/Editor",
        name: "CompanyEditor",
        component:  () => import("@/components/Company/Editor.vue"),
        meta: {
            name: "CompanyEditor",
            menuTitle: "Company",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/LocationSAP/Index",
        name: "LocationSAPIndex",
        component: () => import("@/components/LocationSAP/Index.vue"),
        meta: {
            name: "LocationSAPIndex",
            menuTitle: "LocationSAP",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/LocationSAP/Editor",
        name: "LocationSAPEditor",
        component: () => import("@/components/LocationSAP/Editor.vue"),
        meta: {
            name: "LocationSAPEditor",
            menuTitle: "LocationSAP",
            requiresAuth: true
        },
        props: true
    },        
    {
        path: "/ApprovalMatrix/Index",
        name: "ApprovalMatrixIndex",
        component: () => import("@/components/ApprovalMatrix/Index.vue"),
        meta: {
            name: "ApprovalMatrixIndex",
            menuTitle: "Approval Matrix",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/ApprovalMatrix/Editor",
        name: "ApprovalMatrixEditor",
        component:  () => import("@/components/ApprovalMatrix/Editor.vue"),
        meta: {
            name: "ApprovalMatrixEditor",
            menuTitle: "Approval Matrix",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/RunningNumber/Index",
        name: "RunningNumberIndex",
        component: () => import("@/components/RunningNumber/Index.vue"),
        meta: {
            name: "RunningNumberIndex",
            menuTitle: "Running Number",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/RunningNumber/Editor",
        name: "RunningNumberEditor",
        component:  () => import("@/components/RunningNumber/Editor.vue"),
        meta: {
            name: "RunningNumberEditor",
            menuTitle: "Running Number",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/Department/Index",
        name: "DepartmentIndex",
        component: () => import("@/components/Department/Index.vue"),
        meta: {
            name: "DepartmentIndex",
            menuTitle: "Department",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/Department/Editor",
        name: "DepartmentEditor",
        component:  () => import("@/components/Department/Editor.vue"),
        meta: {
            name: "DepartmentEditor",
            menuTitle: "Department",
            requiresAuth: true
        },
        props: true
    },
    {   
        path: "/StorageLocation/Editor",
        name: "StorageLocationEditor",
        component:  () => import("@/components/StorageLocation/Editor.vue"),
        meta: {
            name: "StorageLocationEditor",
            menuTitle: "Storage Location",
            requiresAuth: true
        },
        props: true
    },
    {   
        path: "/ReleaseMatrix/Index",
        name: "ReleaseMatrixIndex",
        component:  () => import("@/components/ReleaseMatrix/Index.vue"),
        meta: {
            name: "ReleaseMatrixIndex",
            menuTitle: "Release Matrix",
            requiresAuth: true
        },
        props: true
    },
    {   
        path: "/ReleaseMatrix/Editor",
        name: "ReleaseMatrixEditor",
        component:  () => import("@/components/ReleaseMatrix/Editor.vue"),
        meta: {
            name: "ReleaseMatrixEditor",
            menuTitle: "Release Matrix",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/Uom/Index",
        name: "UomIndex",
        component: () => import("@/components/Uom/Index.vue"),
        meta: {
            name: "UomIndex",
            menuTitle: "UoM",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/Uom/Editor",
        name: "UomEditor",
        component: () => import("@/components/Uom/Editor.vue"),
        meta: {
            name: "UomEditor",
            menuTitle: "UoM",
            requiresAuth: true
        },
        props: true
    },    
    {
        path: "/CertificateType/Index",
        name: "CertificateTypeIndex",
        component: () => import("@/components/CertificateType/Index.vue"),
        meta: {
            name: "CertificateTypeIndex",
            menuTitle: "CertificateType",
            requiresAuth: true
        },
        props: true
    },
    {
        path: "/CertificateType/Editor",
        name: "CertificateTypeEditor",
        component: () => import("@/components/CertificateType/Editor.vue"),
        meta: {
            name: "CertificateTypeEditor",
            menuTitle: "CertificateType",
            requiresAuth: true
        },
        props: true
    },
]   