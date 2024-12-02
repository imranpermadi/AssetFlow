<template>
    <form id="mainForm" action="#" method="post">
        <Card>
            <template #title>
                <Toolbar>
                    <template #left>
                        <div class="p-card-title">
                            [{{mode}}] {{$t('approval_matrix')}}
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
                        <Button icon="pi pi-chevron-left" :label="$t('back')" class="p-button mr-2" @click="backToIndex" />
                        <Button icon="pi pi-save" :label="$t('save')" class="p-button-success" @click="confirm($event)" v-if="mode != this.$FORM_MODE_VIEW"/>
                        <ConfirmPopup></ConfirmPopup>
                    </template>
                </Toolbar>
            </template>
            <template #content>
                <div class="p-inputtext-sm p-fluid">
                    <div class="p-fluid grid">
                        <div class="col-12">
                            <Message severity="info" :closable="false">
                                <table>
                                    <tr>
                                        <td>Assign to Transporter</td>
                                        <td>| Mode = ASSIGN TO TRANSPORTER</td>
                                        <td>| Type = ORDER</td>
                                    </tr>
                                    <tr>
                                        <td>Create Order</td>
                                        <td>| Mode = CREATE</td>
                                        <td>| Type = ORDER</td>
                                    </tr>
                                    <tr>
                                        <td>Create Appointment</td>
                                        <td>| Mode = CREATE</td>
                                        <td>| Type = APPOINTMENT</td>
                                    </tr>
                                </table>
                            </Message>
                        </div>
                        <div class="col-6">
                            
                            

                            <FormField :labelName="$t('mode')" fieldName="Mode" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Mode" v-if="header" :placeholder="$t('mode')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>


                            <FormField :labelName="$t('type')" fieldName="Type" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Type" v-if="header" :placeholder="$t('type')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('validity')" fieldName="Validity" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Validity" v-if="header" :placeholder="$t('validity')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('company')" fieldName="Company" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <CustomSelect 
                                    class="p-inputtext-sm" 
                                    v-model="header.Company_Code" 
                                    :url="this.$API_URL + 'list/company_code'" 
                                    :minLength="0" 
                                    :params="{ }" 
                                    v-if="header" 
                                    :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE"
                                    :filter="true"
                                    />
                            </FormField>

                            <FormField :labelName="$t('product_category')" fieldName="Business_Segment" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Business_Segment_Code" v-if="header" :placeholder="$t('product_category')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('pur_sales_org')" fieldName="Purchase_Organization_Code" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Purchase_Organization_Code" v-if="header" :placeholder="$t('pur_sales_org')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('order_type')" fieldName="Order_Type" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Order_Type" v-if="header" :placeholder="$t('order_type')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                        </div>
                        <div class="col-6">
                            <FormField :labelName="$t('location')" fieldName="Location" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Location" v-if="header" :placeholder="$t('location')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('doc_type')" fieldName="Document_Type" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Document_Type" v-if="header" :placeholder="$t('doc_type')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('distribution')" fieldName="Distribution" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Distribution" v-if="header" :placeholder="$t('distribution')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('info1')" fieldName="Info1" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Info1" v-if="header" :placeholder="$t('info1')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('info2')" fieldName="Info2" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Info2" v-if="header" :placeholder="$t('info2')"  :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>

                            <FormField :labelName="$t('description')" fieldName="Description" v-model="header" labelClass="col-4" fieldClass="col-6">
                                <InputText v-model="header.Description" v-if="header" :placeholder="$t('description')" :disabled="mode == $FORM_MODE_VIEW || mode == $FORM_MODE_DELETE" />
                            </FormField>
                        </div>

                        <div class="col-12">
                            <DataTable :loading="!header" v-if="header" :value="header.Details" responsiveLayout="scroll" class="p-datatable-sm editable-cells-table" :rowClass="hideRow">
                                <Column >
                                    <template #header="">
                                        <Button label="" icon="pi pi-plus" class="p-button-primary p-mr-2 p-button-sm" @click="addDetail" :disabled="!canEdit" />
                                    </template>
                                    <template #body="slotProps">
                                        <Button icon="pi pi-minus" class="p-button-danger p-mr-2 p-button-sm" @click="deleteDetail(slotProps)" :disabled="!canEdit" />
                                    </template>
                                </Column>
                                <Column field="Username" :header="$t('username')" key="Username">
                                    <template #body="slotProps">
                                        <CustomSelect 
                                            v-model="slotProps.data[slotProps.column.props.field]" 
                                            :url="$API_URL + 'list/user_code'"
                                            :params="{ }" 
                                            :minLength="0" 
                                            :filter="true"
                                            @change="onCellEditComplete(slotProps)"
                                            :disabled="!canEdit"
                                            />
                                    </template>  
                                </Column>
                                <Column field="Level" :header="$t('level')" key="Level">
                                    <template #body="slotProps">
                                        <InputNumber :maxFractionDigits="0" v-if="header" v-model="slotProps.data[slotProps.column.props.field]" mode="decimal" @blur="onCellEditComplete(slotProps)" :disabled="!canEdit" />
                                    </template>  
                                </Column>
                                <Column field="Value" :header="$t('amount')" key="Amount">
                                    <template #body="slotProps">
                                        <InputNumber :maxFractionDigits="0" v-if="header" v-model="slotProps.data[slotProps.column.props.field]" mode="decimal" @blur="onCellEditComplete(slotProps)" :disabled="!canEdit" />
                                    </template>  
                                </Column>
                            </DataTable>
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
                indexName: "ApprovalMatrixIndex",
                url: this.$API_URL + "approvalmatrix/",
                header: null,
                detail: null,
                mode: "",
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
                        self.header.nmode = self.mode;
                        self.detail = response.data.data.detail;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            saveData() {
                this.$emit('block-ui');
                this.$axios.post(this.url, this.header)
                    .then((response) => {
                        if (response.data.success) {
                            this.$router.push({ name: this.indexName, params: { showToast: true, severity: 'success', summary: 'Data Saved', detail: response.data.message, life: 120000} } );
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
                this.$router.push({ path: "/ApprovalMatrix/Index" });
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
                if (this.header.Details && this.header.Details.Length > 0) {
                    let m = this.header.Details[index].mode;

                    if (m == this.$FORM_MODE_UNCHANGED) {
                        this.header.Details[index].mode = this.$FORM_MODE_EDIT;
                    }
                }
            },
            hideRow(data) {
                return data.mode == this.$FORM_MODE_DELETE ? 'row-hidden' : null;
            },
        },
        created() {
            this.mode = this.$route.query.mode;
            this.id = this.$route.query.id ?? 0;

            this.getData();
        },
        computed:{
            noEdit(){
                return 
            },
            canEdit(){
                return this.mode == this.$FORM_MODE_CREATE || this.mode == this.$FORM_MODE_EDIT;
            }
        }
    }
</script>