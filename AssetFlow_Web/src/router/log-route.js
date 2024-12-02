export default [
    {
      path: "/SAPFilter/Index",
      name: "SAPFilterIndex",
      component: () => import("@/components/SAPFilter/Index.vue"),
      meta: {
        title:'SAPFilterIndex',
        menuTitle: "SAP Filter",
        name: "SAPFilterIndex",
        requiresAuth: true,
      },
      props: true,
    },
    {
        path: "/APILog/Index",
        name: "APILogIndex",
        component: () => import("@/components/APILog/Index.vue"),
        meta: {
          title:'APILogIndex',
          menuTitle: "API Log",
          name: "APILogIndex",
          requiresAuth: true,
        },
        props: true,
      },
      {
        path: "/ApprovalLog/Index",
        name: "ApprovalLogIndex",
        component: () => import("@/components/ApprovalLog/Index.vue"),
        meta: {
          title:'ApprovalLogIndex',
          menuTitle: "Approval Log",
          name: "ApprovalLogIndex",
          requiresAuth: true,
        },
        props: true,
      },
      {
        path: "/Changelog/Index",
        name: "ChangelogIndex",
        component: () => import("@/components/Changelog/Index.vue"),
        meta: {
          title:'ChangelogIndex',
          menuTitle: "Changelog",
          name: "ChangelogIndex",
          requiresAuth: true,
        },
        props: true,
      },
      {
        path: "/Changelog/Editor",
        name: "ChangelogEditor",
        component: () => import("@/components/Changelog/Editor.vue"),
        meta: {
          title:'ChangelogEditor',
          menuTitle: "Changelog",
          name: "ChangelogEditor",
          requiresAuth: true,
        },
        props: true,
      },
  ];
  