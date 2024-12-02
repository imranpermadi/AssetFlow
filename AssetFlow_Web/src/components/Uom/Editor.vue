<template>
    <form id="mainForm" action="#" method="post">
        <Card>
            <template #title>
                <Toolbar>
                    <template #left>
                        <div class="p-card-title">
                            [{{mode}}] Uom
                        </div>
                        <div style="margin-left: 10px" v-if="mode != this.$FORM_MODE_CREATE">
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
                                <div class="grid field" v-if="header.Deleted_Date">
                                    <div class="row">
                                        <i class="pi pi-trash" /> {{this.$formatDateTime(header.DeletedDate)}}
                                    </div>
                                    <div class="row">
                                        &nbsp;by {{header.DeletedBy}}
                                    </div>
                                </div>
                            </OverlayPanel>
                        </div>
                    </template>

                    <template #right>
                        <Button icon="pi pi-chevron-left" label="Back" class="p-button mr-2" @click="backToIndex" />
                        <Button icon="pi pi-save" label="Save" class="p-button-success" @click="confirm($event)" v-if="mode != this.$FORM_MODE_DELETE && mode != this.$FORM_MODE_VIEW"/>
                        <Button icon="pi pi-trash" label="Delete" class="p-button-danger" @click="confirm($event)" v-if="mode == this.$FORM_MODE_DELETE"/>
                        <ConfirmPopup></ConfirmPopup>
                    </template>
                </Toolbar>
            </template>
            <template #content>
                <div class="p-inputtext-sm p-fluid">
                    <div class="grid">
                        <div class="col-6">
                             <div class="field grid">
                                 <label for="Code" class="col-4">{{$t('code')}}</label>
                                <div class="col-8 md-8">
                                    <InputText name="Code" v-model="header.Code" type="text" :placeholder="$t('code')" v-if="header" :class="{'p-invalid': submitted && !header.Code}" :disabled="mode != this.$FORM_MODE_CREATE" :readonly="mode != this.$FORM_MODE_CREATE"/>
                                    <Skeleton v-else-if="!header" />
                                     <small
                                        v-if="header && !header.Code && submitted && mode != this.$FORM_MODE_DELETE"
                                        class="p-error">{{ $t('is_required').replace('Value', $t('code')) }}
                                    </small>
                                </div>
                            </div>
                            <div class="field grid">
                                <label for="Description" class="col-4">{{$t('description')}}</label>
                                <div class="col-8 md-8">
                                    <InputText name="Description" v-model="header.Description" required="true" type="text" :placeholder="$t('description')" v-if="header" :class="{'p-invalid': submitted && !header.Description}" :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"/>
                                    <Skeleton v-else-if="!header" />
                                    <small
                                        v-if="header && !header.Code && submitted && mode != this.$FORM_MODE_DELETE"
                                        class="p-error">{{ $t('is_required').replace('Value', $t('description')) }}
                                    </small>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
        </Card>
    </form>
</template>


<script>
    export default {
        name: "Editor",
        data() {
            return {
                indexName: "UomIndex",
                url: this.$API_URL + "uom/",
                header: null,
                detail: null,
                mode: "",
                submitted: false,
                id: 0,
                formEditable: true,
            }
        },
        methods: {
            getData() {
                var self = this;
                this.$axios.get(this.url + this.id + "/" + this.mode)
                    .then((response) => {
                        self.header = response.data.data.header;
                        self.header.mode = self.mode;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            saveData() {
                this.$emit('block-ui');
                var self = this;
                this.$axios.post(this.url, this.header)
                    .then((response) => {
                        if (response.data.success) {
                            if(this.mode == this.$FORM_MODE_DELETE){
                                 self.$router.push({ name: self.indexName, params: { showToast: true, severity: 'success', summary: 'Data Deleted', detail: "Uom " + self.header.Code + " has been deleted successfully", life: 3000 } } );        
                            }else{
                                self.$router.push({ name: self.indexName, params: { showToast: true, severity: 'success', summary: 'Data Saved', detail: "Uom " + self.header.Code + " has been saved successfully", life: 3000 } } );  
                            }
                           
                        }
                        else {
                            self.showError('Error Saving Data', response.data.message);
                        }
                    })
                    .catch(function (error) {
                        self.showError('Error Saving Data', error);
                    });
            },
            showError(summary, message) {
                this.$emit('unblock-ui');
                this.$toast.add({ severity: 'error', summary: summary, detail: message, life: 3000 });
            },
            backToIndex() {
                this.$router.push({ path: "/Uom/Index" });
            },
            toggle(event) {
                this.$refs.op.toggle(event);
            },
            confirm(event) {
                this.submitted= true;
                if(!this.header.Description || !this.header.Code){
                    return;
                }else{
                    this.$confirm.require({
                    target: event.currentTarget,
                    message: 'Are you sure?',
                    icon: 'pi pi-exclamation-triangle',
                    accept: () => {
                        this.saveData();
                    },
                    reject: () => {
                      
                    }
                });
                }
                
            },
            hideRow(data) {
                return data.mode == this.$FORM_MODE_DELETE ? 'row-hidden' : null;
            },
            addDetail() {
                var newDetail = this.detail;
                newDetail.mode = this.$FORM_MODE_CREATE;
                var det = { ...newDetail };

                this.header.Details.push(det);
            },
            deleteDetail(event) {
                if (event) {
                    var detail = this.header.Details[event.index];

                    if (detail.mode == this.$FORM_MODE_CREATE) this.header.Details.splice(this.header.Details.indexOf(detail), 1);
                    else detail.mode = this.$FORM_MODE_DELETE;
                }
            },
            onCellEditComplete(event) {
                this.editDetail(event.index);
            },
            editDetail(index) {
                if (this.header && this.header.Details) {
                    let mode = this.header.Details[index].mode;

                    if (mode == this.$FORM_MODE_UNCHANGED) {
                        this.header.Details[index].mode = this.$FORM_MODE_EDIT;
                    }
                }
            },
        },
        created() {
            this.mode = this.$route.query.mode;
            this.id = this.$route.query.id ?? 0;

            this.getData();
        }
    }
</script>