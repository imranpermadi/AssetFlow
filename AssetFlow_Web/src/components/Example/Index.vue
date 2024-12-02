<template>
    <IndexBase @auth="onAuth($event)" action="Index" controller="Example"></IndexBase>

    <Card>
        <template #title>
            <Toolbar>
                <template #left>
                    <div class="p-card-title">
                        {{$t('example')}}
                    </div>
                </template>
            </Toolbar>
        </template>
        <template #content>     
            <div class="grid">
                <div class="col-12">
                    <Fieldset legend="Cloud Storage" :toggleable="true">
                            <FormField :labelName="'Upload'" fieldName="podFiles" :modelValue="cloudStorageDummy" fieldClass="col-6" labelClass="col-4">
                                <CustomFileUpload
                                    ref="upload"
                                    :multiple="true" 
                                    :showUploadButton="false" 
                                    :showCancelButton="false" 
                                    :customUpload="false" 
                                    @select="onFileSelect" 
                                    @remove="onFileRemove" 
                                    
                                    />
                            </FormField>

                            <FormField :labelName="''" fieldName="podFiles" :modelValue="cloudStorageDummy" fieldClass="col-6" labelClass="col-4">
                                <Button icon="pi pi-save" :label="$t('upload')" class="p-button-success" @click="onCloudStorageUpload" />
                            </FormField>

                            <FormField :labelName="'Download'" fieldName="podFiles" :modelValue="cloudStorageDummy" fieldClass="col-6" labelClass="col-4">
                                <CustomFileUpload
                                     :fileModels="cloudStorageViews"
                                    :multiple="true" 
                                    :showUploadButton="false" 
                                    :showCancelButton="false" 
                                    :customUpload="false" 
                                    @download="onCloudStorageDownload"
                                    :viewOnly="true"
                                    />
                            </FormField>


                            <!-- <FormField :labelName="'Download'" fieldName="podFiles" v-model="cloudStorageDummy" fieldClass="col-6" labelClass="col-4">
                                <CustomFileUpload
                                    :fileModels="cloudStorageFiles"
                                    :multiple="true" 
                                    :showUploadButton="false" 
                                    :showCancelButton="false" 
                                    :customUpload="false" 
                                    @select="onFileSelect" 
                                    @remove="onFileRemove" 
                                    @download="onDownloadFile"
                                    />
                            </FormField> -->
                    </Fieldset>


                </div>
               
            </div>
            
        </template>
    </Card>

   
</template>

<script>
    export default {
        name: "Example",
        data() {
            return {
                cloudStorageFiles: [],
                cloudStorageViews: [],
                blocked: false,
                cloudStorageDummy: "yes",
                dataUrl: "example/",

            }
        },
        methods: {
            onAuth(event) {
                this.canCreate = event.canCreate;
                this.canEdit = event.canEdit;
                this.canDelete = event.canDelete;
                this.canView = event.canView;
            },
            onFileSelect(event){
                this.cloudStorageFiles = event.files;
            },
            onFileRemove(files){
                this.cloudStorageFiles = files;
            },
            onRefreshFiles(){
                var self = this;
                 this.$axios.get(this.$API_URL + this.dataUrl + "cloud_storage_list")
                    .then((response) => {
                        if(response.data.success){
                            self.cloudStorageViews = response.data.data;
                        }
                        self.blocked = false;
                    })
                    .catch(function (err) {
                        self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: "File not found", life: 120000});
                        self.blocked = false;
                        //self.showAssignModal = false;
                    });

            },
            onCloudStorageDownload(file){
                var self = this;
                this.$axios.get(this.$API_URL + this.dataUrl + "cloud_storage_download/" + file.Id, {
                    responseType: 'blob'
                    })
                    .then((response) => {
                        const url = window.URL.createObjectURL(new Blob([response.data]));
                        const link = document.createElement('a');
                        link.href = url;
                        link.setAttribute('download', file.name);
                        document.body.appendChild(link);
                        link.click();
                        
                    })
                    .catch(function (err) {
                        self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: "File not found", life: 120000});
                        self.blocked = false;
                        //self.showAssignModal = false;
                    });
                
                
            },
            onCloudStorageUpload(){
                var self = this;

                var data = new FormData();

                // data.append("shipment_id", this.data.Id);
                // data.append("pod_remarks", this.shipmentData.POD_Remarks);

                for(var i=0; i< this.cloudStorageFiles.length;i++){
                    data.append("file", this.cloudStorageFiles[i], this.cloudStorageFiles[i].name);
                }

                // data.append("shipment_details", JSON.stringify(this.shipmentData.Details));

                self.blocked = true;

                this.$axios.post(this.$API_URL + this.dataUrl + "cloud_storage_upload", data,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then((response) =>{
                        if (response.data.success) {
                            self.$parent.$toast.add({ severity: 'success', summary: "Success", detail: response.data.message, life: 120000});
                            //self.showModal = false;
                            //self.$emit("close");
                            self.$refs.upload.clear();
                             self.onRefreshFiles();
                        }
                        else {
                            self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: response.data.message, life: 120000});
                        }
                        self.blocked = false;
                       
                    }).catch((err) => {
                        console.log(err);
                        self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: err, life: 120000});
                        self.blocked = false;
                        self.showModal = false;
                    });
            },

        },
        mounted() {
            this.onRefreshFiles();
        },
        computed: {
           
        },
    }
</script>

<style #scoped>
    .p-card .p-card-content{
        margin:0!important;
        padding: 0 !important;
    }
</style>