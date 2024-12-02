<template>
    <form id="mainForm" action="#" method="post">
        <Card>
            <template #title>
                <Toolbar>
                    <template #left>
                        <div class="p-card-title">
                            [{{mode}}] {{title}}
                        </div>
                        <div style="margin-left: 10px" v-if="mode != this.$FORM_MODE_CREATE">
                            <Button type="button" icon="pi pi-info" @click="toggle" aria:haspopup="true" aria-controls="overlay_panel" class="p-button-rounded p-button-outlined p-button-sm" />

                            <OverlayPanel ref="op" appendTo="body" :showCloseIcon="true" id="overlay_panel" style="width: 550px" :breakpoints="{'960px': '80vw'}">
                                <div class="grid field">
                                    <div class="row">
                                        <i class="pi pi-plus-circle" /> {{this.$formatDateTime(header.Created_Date)}}
                                    </div>
                                    <div class="row">
                                        &nbsp;by {{header.Created_By}}
                                    </div>
                                </div>
                                <div class="grid field" v-if="header.Edited_Date">
                                    <div class="row">
                                        <i class="pi pi-pencil" /> {{this.$formatDateTime(header.Edited_Date)}}
                                    </div>
                                    <div class="row">
                                        &nbsp;by {{header.Edited_By}}
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
                    <FormField :labelName="$t('code')" fieldName="Code" v-model="header">
                        <InputText name="Code" v-model="header.Code" type="text" :placeholder="$t('code')" 
                        :class="{ 'p-invalid': submitted && !header.Code }"
                        v-if="header" :disabled="mode != this.$FORM_MODE_CREATE" :readonly="mode != this.$FORM_MODE_CREATE" />
                    </FormField>

                    <div v-if="isDetail">
                        <FormField :labelName="$t('all_company')" fieldName="All_Company" v-model="header">
                            <YNCheckbox v-model="header.All_Company" name="All_Company" :options="this.$TYPE"
                                v-if="header" :showClear="true" :binary="true"
                                :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                                :readonly="mode != this.$FORM_MODE_CREATE"
                                :class="{ 'p-invalid': submitted && !header.All_Company }"/>

                            <!-- <Dropdown v-model="header.All_Company" name="All_Company" :options="this.$TYPE"
                                 v-if="header" :showClear="true"
                                 :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                                :readonly="mode != this.$FORM_MODE_CREATE"
                                :class="{ 'p-invalid': submitted && !header.All_Company }"/> -->
                            <Skeleton v-else-if="!header" />

                           
                        </FormField>

                        <FormField :labelName="$t('company')" fieldName="Code" v-model="header" v-if="header && header.All_Company == 'N'">
                            <CustomSelect class="p-inputtext-sm" name="Company_Id"
                            v-model="header.Company_Id" type="text" :placeholder="'Company'"
                            v-if="header" :class="{ 'p-invalid': submitted && !header.Company_Id }"
                            :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                            :readonly="mode != this.$FORM_MODE_CREATE" :filter="true"
                            :url="this.$API_URL + 'list/company_id'" :minLength="0" :params="{}" />
                            <Skeleton v-else-if="!header" />
                        </FormField>
                    </div>

                    <FormField :labelName="$t('location')" fieldName="Location_Id" v-model="header"> 
                        <CustomSelect class="p-inputtext-sm" name="Location_Code"
                        v-model="header.Location_Id" type="text" :placeholder="'Location'"
                        v-if="header" :class="{ 'p-invalid': submitted && !header.Location_Id }"
                        :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                        :readonly="mode != this.$FORM_MODE_CREATE" :filter="true"
                        :url="this.$API_URL + 'list/location_id'" :minLength="0" :params="{}" />
                    <Skeleton v-else-if="!header" />
                    </FormField>
                    
                    <div v-if="isDetail">
                        <FormField :labelName="$t('department')" fieldName="Department" v-model="header">
                             <CustomSelect 
                                v-model="header.Department_Id" 
                                :url="$API_URL + 'list/department_id'"
                                :minLength="0" 
                                :filter="true"
                                :disabled="!canEdit" 
                                v-if="header"
                                />
                        </FormField>
                    </div>

                    <FormField :labelName="$t('parent')" fieldName="Parent_Id" v-model="header">
                        <CustomSelect class="p-inputtext-sm" name="Parent_Id"
                        v-model="header.Parent_Id" type="text" :placeholder="'Parent'"
                        v-if="header"
                        :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                        :readonly="mode != this.$FORM_MODE_CREATE"
                        :url="this.$API_URL + 'list/parentmaintid'" :minLength="0" :params="{isMain: this.isMain, id : header.Id, location_id : header.Location_Id}" />
                    <Skeleton v-else-if="!header" />
                    </FormField>

                    <FormField :labelName="$t('description')" fieldName="Description" v-model="header">
                        <InputText name="Description" v-model="header.Description" type="text" :placeholder="$t('description')" 
                        :class="{ 'p-invalid': submitted && !header.Description }"
                        v-if="header" 
                        :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                        :readonly="mode != this.$FORM_MODE_CREATE"  />
                    </FormField>

                    <FormField :labelName="$t('type')" fieldName="Type" v-model="header">
                        <CustomSelect 
                            v-model="header.Type" 
                            :url="$API_URL + 'list/lookup'"
                            :params="{ group: $LOOKUP_GROUP.STORAGE_LOCATION_TYPE }"
                            :minLength="0" 
                            :filter="true"
                            :class="{ 'p-invalid': submitted && !header.Type }"
                            :disabled="!canEdit" 
                            v-if="header"
                            />
                    </FormField>


                    <div v-if="isDetail">
                        <FormField :labelName="$t('is_storage')" fieldName="Is_Storage" v-model="header">
                            <Checkbox v-model="header.Is_Storage" :binary="true" v-if="header"
                            :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                            :readonly="mode != this.$FORM_MODE_CREATE"/>
                            <Skeleton v-else-if="!header" />
                        </FormField>
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
                indexName: "StorageLocationIndex",
                url: this.$API_URL + "storagelocation/",
                header: null,
                detail: null,
                mode: "",
                id: 0,
                formEditable: true,
                type: "",
                isMain: false,
                isDetail: false,
                submitted : false
            }
        },
        methods: {
            getData() {
                var self = this;
                this.$axios.get(this.url + this.id + "/" + this.mode)
                    .then((response) => {
                        self.header = response.data.data.header;
                        self.header.mode = self.mode;
                        self.header.Is_Storage = self.header.Is_Storage == null ? false : ( self.header.Is_Storage == "Y" ? true : false);
                    })
                    .catch(function (error) {
                        console.log(error);
                       
                    });
            },
            saveData() {
                this.$emit('block-ui');

                this.header.Category = this.isMain ? this.$CATEGORY_STORAGE.MAIN : this.$CATEGORY_STORAGE.DETAIL;

                this.$axios.post(this.url, this.header)
                    .then((response) => {
                        if (response.data.success) {
                            if(this.$route.query.type == this.$STORAGE_LOCATION_CATEGORY.MAIN){
                                this.$router.push({ name: "StorageLocationIndex", query: { type: this.type}, params: { showToast: true, severity: 'success', summary: 'Data ' + this.mode, detail: response.data.message, life: 120000} } );
                            }else if(this.$route.query.type == this.$STORAGE_LOCATION_CATEGORY.DETAIL){
                                this.$router.push({ name: "StorageLocationIndex", query: { type: this.type}, params: { showToast: true, severity: 'success', summary: 'Data ' + this.mode, detail: response.data.message, life: 120000} } )
                            }
                        }
                        else {
                            this.showError('Error Saving Data', response.data.message);
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
            backToIndex() {
                this.$router.push({ path: "/StorageLocation/Index", query: { type: this.type} });
            },
            toggle(event) {
                this.$refs.op.toggle(event);
            },
            confirm(event) {
                this.submitted= true;
               
                if(!this.header.Description || !this.header.Code || !this.header.Type || !this.header.Location_Id || (this.isDetail && !this.header.All_Company)
                || (this.isDetail && !this.header.Company_Id && this.header.All_Company == 'N') || (this.isDetail && !this.header.Department_Id)){
                    return;
                }
                else
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
        },
        created() {
            this.mode = this.$route.query.mode;
            this.id = this.$route.query.id ?? 0;
            this.type = this.$route.query.type;
            this.isMain = this.$route.query.type == this.$STORAGE_LOCATION_CATEGORY.MAIN;
            this.isDetail = this.isMain ? false : true;
           
            this.getData();
        },
        computed: {
            title(){
                return (this.isMain ? "Main" : "Detail") + " " + this.$t('storage_location');
            },
            canEdit(){
                return this.mode == this.$FORM_MODE_CREATE || this.mode == this.$FORM_MODE_EDIT;
            }
        },
    }
</script>