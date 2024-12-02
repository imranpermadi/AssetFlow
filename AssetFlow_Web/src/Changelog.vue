<template>
    <Card>
        <template #title>
            <Toolbar>
                <template #left>
                    <div class="p-card-title">
                        {{$t('changelog')}}
                    </div>
                </template>

                <template #right>
                    <Button icon="pi pi-times" class="p-button-rounded p-button-danger p-button-outlined" @click="close()"/>
                </template>
            </Toolbar>

        </template>
        <template #content>
            <div style="min-height:30vh">
                <BlockUI :blocked="loading">
                    <Timeline :value="changeLogData">
                        <template #content="slotProps">
                            <Card class="mb-2">
                                <template #title>
                                    <span class="text-teal-700">v.{{slotProps.item.Version}}</span> - {{this.$formatDateName(slotProps.item.VersionDate)}}
                                </template>
                                <template #content>
                                    <div v-for="(item) in slotProps.item.Details" v-bind:key="item.Id">
                                        <i class="pi pi-check-circle mr-2 text-pink-700"></i> {{item.Description}}
                                    </div>
                                </template>
                            </Card>
                        </template>
                    </Timeline>
                </BlockUI>
            </div>
            

        </template>
    </Card>
    
    
</template>
<script>
    export default{
        data(){
            return{
                changeLogData: null,
                loading: false
            }
        },
        emits: ["close"],
        mounted(){
            this.getChangelog();
        },
        methods:{
            getChangelog(){
                var self = this;
                this.loading = true;
                this.$axios.get(this.$UTIL_URL + "changelog")
                    .then((response) => {
                        if(response && response.data.success){
                            self.changeLogData = response.data.data;
                        }
                        self.loading = false;
                    })
                    .catch((error) => {
                        self.loading = true;
                        //self.showError('Error Getting Order Type Data', error);
                    });
            },
            close(){
                this.$emit('close');
            }
        }
    }

</script>

<style scoped lange="scss">
    ::v-deep(.p-timeline-event-opposite){
        flex:0 !important;
    }

    .p-timeline-event-opposite{
        flex: 0!important;
    }
</style>
