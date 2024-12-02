import { createStore, createLogger } from 'vuex'; 
import createPersistedState from 'vuex-persistedstate';

export default createStore({
    plugins: [createPersistedState(), createLogger()],
    state: {
        token: null,
        expiration: null,
        role: null,
        language: "EN",
        captchaCount: 0,
        languages: []
    },
    mutations: {
        setToken: (state, model) => {
            state.captchaCount = 0;
            state.token = model.user_data;
            state.expiration = model.token_expiry;
            // if(state.token.Language != null){
            //     state.language = state.token.Language;
            // }
            //state.expiration = new Date()
        },
        clearToken: (state) => {
            state.token = null;
            state.expiration = null;
            //state.expiration = Date.now();
        },
        setRole: (state, model) =>{
            state.role = model;
        },
        setLanguage: (state, model)=>{
            state.language = model;
        },
        setCaptchaCount: (state) => {
            state.captchaCount++;
        },
        setMenu: (state, model) =>{
            model.count = 12;
            var menuItem = model.item;
            if(state.token){
                if(state.token.Menus){
                    var menu = recursiveSearch(state.token.Menus, menuItem);
                    menu.badge = model.count;
                }
            }
        },
        setLanguages: (state, model) => {
            state.languages = model;
        }
    },
    getters: {
        isAuthenticated: (state) => {
            var currentToken = state.token;
            if (currentToken) {
                return true;
            }
            else return false;

            //return state.token != "" && state.expiration > Date.now();
        },
        authorizedMenu: (state) => {
            if (state.token) {
                return state.token.Menus;
            }
            else return null;
        },
        isExpired: (state) => {
            var currentToken = state.token;

            if (currentToken) {
                if (new Date(state.expiration) <= new Date()) {
                    return true;
                }
                else return false;
            } else return false;
        },
        getUsername: (state) => {
            var currentToken = state.token;

            if (currentToken) {
                return currentToken.Username;
            }
            else return "";
        },
         getFullname: (state) => {
            var currentToken = state.token;

            if (currentToken) {
                return currentToken.Fullname;
            }
            else return "";
        },
        getIsAdmin :(state) => {
            var currentToken = state.token;

            if (currentToken) {
                return currentToken.IsAdmin == "Y";
            }
            else return false;
        },
        getUseAD: (state) => {
            var currentToken = state.token;

            if(currentToken){
                return currentToken.UseAD == "Y";
            }
            else return false;
        },
        getLanguage: (state) =>{
            return state.language;
        },
        getMenu: (state) => {
            return state.token ? state.token.Menus : [];
        },
        getCaptchaCount: (state) => {
            return state.captchaCount;
        },
        getLocationCode: (state) => {
            var currentToken = state.token;
            if (currentToken) {
                return currentToken.Location_Code;
            }
            else return "";
        },
        getLanguages: (state) =>{
            return state.languages;
        },
        getServer: (state) => {
            var currentToken = state.token;

            if (currentToken) {
                return currentToken.SERVER;
            }
            else return "";
        },        
    },
    actions: {
        login: async ({ commit }, data) => {
            commit("setToken", data);
        },
        logout: async ({ commit }) => {
            commit("clearToken");
        },
        role: async ({ commit }, data) => {
            commit("setRole", data);
        },
        language: async({commit}, data) =>{
            commit("setLanguage", data);
        },
        menu: async({commit}, data) => {
            commit("setMenu", data);
        },
        setCaptchaCount: async({commit}) => {
            commit("setCaptchaCount");
        },
        setMenu: async({commit}, data) => {
            commit("setMenu", data);
        },
        setLanguages: async({commit}, data) =>{
            commit("setLanguages", data);
        },
    }
});

function recursiveSearch(arr, target) {
    for (let i = 0; i < arr.length; i++) {
        if (arr[i].id === target.id) {
            return arr[i];
        }
        if (arr[i].items) {
            const result = recursiveSearch(arr[i].children, target);
            if (result) {
                return result;
            }
        }
    }
    return null;
}

// function checkRole(state, role){
//     var currentToken = state.token;

//     if (currentToken) {
//         if(currentToken.Is_Admin) return true;
//         else return currentToken.Role == role;
//     }

//     else return false;
// }