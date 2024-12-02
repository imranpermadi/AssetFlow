<template>
    <div>

    </div>
</template>
<script>
    export default {
        props: {
            action: null,
            controller:null
        },
        emits: [
            "auth"
        ],
        beforeMount() {
            var self = this;
            var data = { menu_action: this.action, menu_controller: this.controller, menu_check: "Y" };
            this.$axios.post(this.$AUTH_URL, data)
               .then((response) => {
                   var event = {
                       canView: response.data.data.View,
                       canCreate: response.data.data.Create,
                       canEdit: response.data.data.Edit,
                       canDelete: response.data.data.Delete
                   };

                   this.$emit('auth', event);
               })
               .catch(function (err) {
                   self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: err.message, life: 120000});
                   self.$router.go(-1);
               });
        },

    }
</script>
