<template>

</template>
<script>
    export default{
         props: {
            sessionExpired: {
                type: Boolean,
                default: String
            }
        },
        methods:{
            deleteLocalStorage() {
                var keys = Object.keys(localStorage);
                var i = keys.length;
                var username = this.$store.getters.getUsername;

                while ( i-- ) {
                    if(keys[i].includes(this.$STATE_NAME) && keys[i].includes(username)){
                        localStorage.removeItem(keys[i]);
                    }
                }
            }
        },
        mounted(){
            if(this.$axios.defaults.headers.common["Authorization"] != null){
                 this.$axios.defaults.headers.common["Authorization"] = null;
                this.deleteLocalStorage();
                this.$store.dispatch("logout");
                
            }
            
            this.$router.push({ name: "Login",  params: { sessionExpired: this.sessionExpired }  });
           
        }
    }
</script>