<template>
    <IndexBase @auth="onAuth($event)" action="Index" controller="Localization"></IndexBase>

    <form id="mainForm" action="#" method="post">
        <Card>
            <template #title>
                <Toolbar>
                    <template #left>
                        <div class="p-card-title">
                            {{$t('localization')}}
                        </div>
                        <!-- <div style="margin-left: 10px" v-if="mode != this.$FORM_MODE_CREATE">
                            <Button type="button" icon="pi pi-info" @click="toggle" aria:haspopup="true" aria-controls="overlay_panel" class="p-button-rounded p-button-outlined p-button-sm" />

                            <OverlayPanel ref="op" appendTo="body" :showCloseIcon="true" id="overlay_panel" style="width: 550px" :breakpoints="{'960px': '80vw'}">
                                <div class="grid field">
                                    <div class="row">
                                        <i class="pi pi-plus-circle" /> {{this.$formatDateTime(header.CreatedDate)}}
                                    </div>
                                    <div class="row">
                                        &nbsp;by {{header.CreatedBy}}
                                    </div>
                                </div>
                                <div class="grid field" v-if="header.EditedDate">
                                    <div class="row">
                                        <i class="pi pi-pencil" /> {{this.$formatDateTime(header.EditedDate)}}
                                    </div>
                                    <div class="row">
                                        &nbsp;by {{header.EditedBy}}
                                    </div>
                                </div>
                            </OverlayPanel>
                        </div> -->
                    </template>

                    <template #right>
                        <Button icon="pi pi-download" :label="$t('export')" class="p-button-primary mr-2" @click="exportData()"/>
                        <Button icon="pi pi-upload" :label="$t('import')" class="p-button-help mr-2" @click="importData()"/>
                        <Button icon="pi pi-save" :label="$t('save')" class="p-button-success" @click="confirm($event)"/>
                        <ConfirmPopup></ConfirmPopup>
                    </template>
                </Toolbar>
            </template>
            <template #content>
                <div class="p-inputtext-sm p-fluid">
                                        
                    <div class="col-12">
                        <DataTable 
                            :loading="!header" 
                            :value="header ? header : null" v-if="header" 
                            responsiveLayout="scroll" 
                            class="p-datatable-sm editable-cells-table" 
                            :rowClass="hideRow">
                            <Column>
                                <template #header="">
                                    <Button label="" icon="pi pi-plus" class="p-button-primary p-mr-2 p-button-sm" @click="addDetail"  :disabled="!canEdit" />
                                </template>
                                <template #body="slotProps">
                                    <Button icon="pi pi-minus" class="p-button-danger p-mr-2 p-button-sm" @click="deleteDetail(slotProps)"  :disabled="!canEdit"/>
                                </template>
                            </Column>
                            <Column field="Code" :header="$t('code')" >
                                <template #body="slotProps">
                                    <InputText name="Code" type="text" v-model="slotProps.data.Code"  :disabled="slotProps.data.mode != $FORM_MODE_CREATE" :autoUppercase="false" /> 
                                </template>  
                            </Column>
                            <Column v-for="(language, index) of languages"  :key="language.Id" :header="language.LookupValue">
                                <template #body="slotProps">
                                    <InputText type="text" v-model="slotProps.data.Details[index].Label" :autoUppercase="false" @change="editLocale(slotProps.data.Details[index])" /> 
                                </template> 
                            </Column>

                            
                        </DataTable>
                    </div>

                </div>
            </template>
        </Card>

        <Dialog v-model:visible="showImportDialog" modal :header="$t('import')" class="w-5">
            <div class="grid">
                <div class="col-12">
                    <FormField :labelName="$t('choose_file')" fieldName="podFiles" v-model="showImportDialog" fieldClass="col-6" labelClass="col-4">
                        <CustomFileUpload 
                            accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
                            :multiple="false" 
                            :showUploadButton="false" 
                            :showCancelButton="false" 
                            :customUpload="false" 
                            @select="onFileSelect" 
                            @remove="onFileRemove" 
                            />
                    </FormField>

                    <FormField :labelName="''" fieldName="podFiles" v-model="showImportDialog" fieldClass="col-6" labelClass="col-4">
                        <Button icon="pi pi-upload" :label="$t('import')" class="p-button-help mr-2" @click="confirmImportData()"/>
                    </FormField>
                    
                </div>
                
            </div>
            
        </Dialog>


    </form>
</template>


<script>
    export default {
        name: "Index",
        data() {
            return {
                indexName: "LocalizationIndex",
                header: null,
                detailModel: null,
                headerModel:null,
                mode: "",
                id: 0,
                formEditable: true,
                dataModel: false,
                dataUrl: "localization/",
                listUrl: "localization/list",
                canCreate: false,
                canEdit: false,
                canDelete: false,
                canView: false,

                showImportDialog: false,
                importFile: null
            }
        },
        methods: {
            hideRow(data) {
                return data.mode == this.$FORM_MODE_DELETE ? 'row-hidden' : null;
            },
            onAuth(event) {
                this.canCreate = event.canCreate;
                this.canEdit = event.canEdit;
                this.canDelete = event.canDelete;
                this.canView = event.canView;
            },
            getData() {
                var self = this;
                this.$axios.get(this.$API_URL + this.listUrl)
                    .then((response) => {
                        self.header = response.data.data.header;
                        self.languages = response.data.data.languages;
                        self.detailModel = response.data.data.detail_model;
                        self.headerModel = response.data.data.header_model;
                        //self.header.mode = self.mode;
                    })
                    .catch(function (error) {
                        //alert(error);
                    });
            },
            saveData() {
                var self =this;
                self.$emit('block-ui');
                this.$axios.post(this.$API_URL + this.dataUrl, this.header)
                    .then((response) => {
                        if (response.data.success) {
                            //self.$emit('unblock-ui');
                            self.$toast.add({ severity: 'success', summary: "Data Saved", detail: response.data.message, life: 120000});
                            //self.getData();
                            window.location.reload();
                            //this.$router.push({ name: this.indexName, params: { showToast: true, severity: 'success', summary: 'Data Saved', detail: response.data.message, life: 120000} } );
                        }
                        else {
                            self.showError('Error Saving Data', response.data.message);
                        }
                    })
                    .catch(function (error) {
                        this.showError('Error Saving Data', error);
                    });
            },
            showError(summary, message) {
                this.$emit('unblock-ui');
                this.$toast.add({ severity: 'error', summary: summary, detail: message, life: 120000});
            },
            toggle(event) {
                this.$refs.op.toggle(event);
            },
            confirm(event) {
                this.$confirm.require({
                    target: event.currentTarget,
                    message: 'Are you sure?',
                    icon: 'pi pi-exclamation-triangle',
                    accept: () => {
                        this.saveData();
                    },
                    reject: () => {
                        //callback to execute when user rejects the action
                    }
                });
            },
            addDetail() {
                var self = this;
                
                var newDetail = this.headerModel;
                newDetail.mode = this.$FORM_MODE_CREATE;
                var det = { ...newDetail };

                det.Details = [];

                if(newDetail.Details.length > 0){
                    for(var i = 0; i< newDetail.Details.length;i++){
                        var nD = newDetail.Details[i];

                        var newD = { ...nD };

                        det.Details[i] = newD;
                    }
                }

                

                this.header.push(det);
            },
            deleteDetail(event) {
                if (event) {
                    var detail = this.header[event.index];

                    if (detail.mode == this.$FORM_MODE_CREATE) this.header.splice(this.header.indexOf(detail), 1);
                    else detail.mode = this.$FORM_MODE_DELETE;
                }
            },
            editDetail(index) {
                let m = this.header[index].mode;

                if (m == this.$FORM_MODE_UNCHANGED) {
                    this.header[index].mode = this.$FORM_MODE_EDIT;
                }
            },
            editLocale(data){
                let m = data.mode;

                if (m == this.$FORM_MODE_UNCHANGED) {
                    data.mode = this.$FORM_MODE_EDIT;
                }
            },
            exportData(){
                var self = this;
                this.$axios.get(this.$API_URL + this.dataUrl + "export", {
                    responseType: 'blob'
                    })
                    .then((response) => {
                        const url = window.URL.createObjectURL(new Blob([response.data]));
                        const link = document.createElement('a');
                        link.href = url;
                        link.setAttribute('download', "Localization.xlsx");
                        document.body.appendChild(link);
                        link.click();
                        
                    })
                    .catch(function (err) {
                        console.log(err);
                        self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: "File not found", life: 120000});
                        self.blocked = false;
                        //self.showAssignModal = false;
                    });
                
            },
            importData(){
                this.showImportDialog = true;
            },
            onFileSelect(event){
                this.importFile = event.files;
            },
            onFileRemove(files){
                this.importFile = files;
            },
            confirmImportData(){
                var self = this;

                var data = new FormData();

                data.append("file", this.importFile[0], this.importFile[0].name);

                self.blocked = true;

                this.$axios.put(this.$API_URL + this.dataUrl + "import", data,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then((response) =>{
                        if (response.data.success) {
                            self.$parent.$toast.add({ severity: 'success', summary: "Success", detail: response.data.message, life: 120000});
                            self.showImportDialog = false;
                            window.location.reload();
                        }
                        else {
                            self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: response.data.message, life: 120000});
                            self.blocked = false;
                        }
                        
                       
                    }).catch((err) => {
                        console.log(err);
                        self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: err, life: 120000});
                        self.blocked = false;
                        self.showImportDialog = false;
                    });
            }
        },
        created() {
            // this.mode = this.$route.query.mode;
            // this.id = this.$route.query.id ?? 0;

            this.getData();
        },
    }
</script>