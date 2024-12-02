<template>
    <form id="mainForm" action="#" method="post">
        <Card>
            <template #title>
                <Toolbar>
                    <template #left>
                        <div class="p-card-title">
                            [{{mode}}] Running Number
                        </div>
                        <div style="margin-left: 10px" v-if="mode != this.$FORM_MODE_CREATE">
                            <Button type="button" icon="pi pi-info" @click="toggle" aria:haspopup="true"
                                aria-controls="overlay_panel" class="p-button-rounded p-button-outlined p-button-sm" />

                            <OverlayPanel ref="op" appendTo="body" :showCloseIcon="true" id="overlay_panel"
                                style="width: 550px" :breakpoints="{'960px': '80vw'}">
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
                        </div>
                    </template>

                    <template #right>
                        <Button icon="pi pi-chevron-left" label="Back" class="p-button mr-2" @click="backToIndex" />
                        <Button icon="pi pi-save" label="Save" class="p-button-success" @click="confirm($event)"
                            v-if="mode != this.$FORM_MODE_VIEW" />
                        <ConfirmPopup></ConfirmPopup>
                    </template>
                </Toolbar>
            </template>
            <template #content>
                <div class="p-inputtext-sm">

                    <div class="p-fluid">
                        <div class="field grid">
                            <label for="Company_Name" class="col-2">{{$t('company')}}</label>
                            <div class="col-6 md-6">
                                <CustomSelect v-model="header.Company_Id" class="p-inputtext-sm" v-if="header"
                                    :url="this.$API_URL + 'list/company'" :minLength="0" :params="{}" :disabled="mode == this.$FORM_MODE_VIEW || mode == this.$FORM_MODE_EDIT" />
                                <Skeleton v-else-if="!header" />
                            </div>
                        </div>

                        <!-- <div class="field grid">
                            <label for="Trans_Type" class="col-2">Trans Type</label>
                            <div class="col-6 md-6">
                                <CustomSelect v-model="header.Trans_Type" class="p-inputtext-sm" v-if="header" @change="TransTypeChange($event)"
                                    :url="this.$API_URL + 'list/trans_type'" :minLength="0" :params="{}"  />
                                <Skeleton v-else-if="!header" />
                            </div>
                        </div>

                        <div class="field grid">
                            <label for="RangeFromNumber" class="col-2">{{$t('range_from_number')}}</label>
                            <div class="col-6 md-6">
                                <InputText name="RangeFromNumber" v-model="header.RangeFromNumber" type="number"
                                    placeholder="Range From Number" v-if="header"  />
                                <Skeleton v-else-if="!header" />
                            </div>
                        </div>

                        <div class="field grid">
                            <label for="RangeToNumber" class="col-2">{{$t('range_to_number')}}</label>
                            <div class="col-6 md-6">
                                <InputText name="RangeToNumber" v-model="header.RangeToNumber" type="number"
                                    placeholder="Range To Number" v-if="header"  />
                                <Skeleton v-else-if="!header" />
                            </div>
                        </div>

                        <div class="field grid">
                            <label for="Format" class="col-2">Format</label>
                            <div class="col-6 md-6">
                                <InputText name="Format" v-model="header.Format" type="text" placeholder="Format"
                                    v-if="header"  />
                                <Skeleton v-else-if="!header" />
                            </div>
                        </div> -->
                    </div>
                    <div class="grid">
                        <div class="col-12">
                            <Fieldset legend="Detail" :toggleable="true">
                                <DataTable :loading="!header" v-if="header"
                                    :value="header.Detail ? header.Detail : null" responsiveLayout="scroll"
                                    class="p-datatable-sm editable-cells-table" :rowClass="hideRow">
                                    <Column style="width:30px">
                                        <template #header="">
                                            <Button label="" icon="pi pi-plus"
                                                class="p-button-primary p-mr-2 p-button-sm" @click="addDetail"
                                                :disabled="isDisable"    />
                                        </template>
                                        <template #body="slotProps">
                                            <Button icon="pi pi-minus" class="p-button-danger p-mr-2 p-button-sm"
                                                @click="deleteDetail(slotProps)" :disabled="isDisable" />
                                        </template>
                                    </Column>

                                    <Column field="Trans_Type" :header="'Trans Type'">
                                        <template #body="slotProps">
                                            <CustomSelect v-model="slotProps.data[slotProps.column.props.field]" @change="TransTypeChange(slotProps)" style="width:100%"
                                                :url="this.$API_URL + 'list/trans_type'" :minLength="0" :params="{}" :disabled="isDisable"  />
                                        </template>
                                    </Column>

                                    <Column field="RangeFromNumber" :header="$t('range_from_number')">
                                        <template #body="slotProps">
                                            <InputText name="RangeFromNumber" style="width:100%" @change="editDetail(slotProps)"
                                                v-model="slotProps.data[slotProps.column.props.field]" type="number" :disabled="isDisable"
                                                :placeholder="$t('range_from_number')"  />
                                        </template>
                                    </Column>

                                    <Column field="RangeToNumber" :header="$t('range_to_number')">
                                        <template #body="slotProps">
                                            <InputText name="RangeToNumber" style="width:100%" @change="editDetail(slotProps)"
                                                v-model="slotProps.data[slotProps.column.props.field]" type="number" :disabled="isDisable"
                                                :placeholder="$t('range_to_number')"  />
                                        </template>
                                    </Column>

                                    <Column field="Format" :header="'Format'">
                                        <template #body="slotProps">
                                            <InputText name="Format" style="width:100%" @change="editDetail(slotProps)"
                                                v-model="slotProps.data[slotProps.column.props.field]" type="text" :disabled="isDisable"
                                                :placeholder="'Format'"  />
                                        </template>
                                    </Column>

                                </DataTable>
                            </Fieldset>
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
                indexName: "RunningNumberIndex",
                url: this.$API_URL + "RunningNumber/",
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
                        self.detail = response.data.data.detail;
                        self.header.mode = self.mode;
                    })
                    .catch(function (error) {
                        console.log("DATA");
                        alert(error);

                    });
            },
            saveData() {
                // console.log(this.header)
                // return;

                if(this.header.Company_Id == 0){
                    this.showError('Error Saving Data', 'Company is required.');
                    return;
                }

                this.$emit('block-ui');
                this.$axios.post(this.url, this.header)
                    .then((response) => {
                        if (response.data.success) {
                            this.$router.push({
                                name: this.indexName,
                                params: {
                                    showToast: true,
                                    severity: 'success',
                                    summary: 'Data Saved',
                                    detail: "Running Number has been saved successfully",
                                    life: 120000
                                }
                            });
                        } else {
                            this.showError('Error Saving Data', response.data.message);
                        }
                    })
                    .catch(function (error) {
                        this.showError('Error Saving Data', error);
                    });
            },
            showError(summary, message) {
                this.$emit('unblock-ui');
                this.$toast.add({
                    severity: 'error',
                    summary: summary,
                    detail: message,
                    life: 120000
                });
            },
            backToIndex() {
                this.$router.push({
                    path: "/RunningNumber/Index"
                });
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
            editDetail(event) {
                var index = event.index;
                if (this.header.Detail && this.header.Detail.length > 0) {
                    let m = this.header.Detail[index].mode;

                    if (m == this.$FORM_MODE_UNCHANGED) {
                        this.header.Detail[index].mode = this.$FORM_MODE_EDIT;
                    }
                }
            },
            deleteDetail(event) {
                if (event) {
                    var detail = this.header.Detail[event.index];

                    if (detail.mode == this.$FORM_MODE_CREATE) this.header.Detail.splice(this.header.Detail.indexOf(detail), 1);
                    else detail.mode = this.$FORM_MODE_DELETE;  
                }
            },
            addDetail() {
                var newDetail = this.detail;
                newDetail.mode = this.$FORM_MODE_CREATE;
                var det = { ...newDetail };
                this.header.Detail.push(det);
                
            },
            TransTypeChange(event){
                var detail = this.header.Detail[event.index];
                if(detail.Trans_Type != null){
                    var TransType = detail.Trans_Type;
                    if(TransType == "S"){
                        detail.Format = "@AT@TT@LOC@COY@MONTH@Y2@LN";
                    }
                    else{
                        detail.Format = "@TT@LOC@COY@MONTH@Y2@LN";
                    }
                }
                else{
                    detail.Format = null;
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
        computed: {
            isDisable() {
                return this.mode == this.$FORM_MODE_VIEW || this.mode == this.$FORM_MODE_DELETE ? true : false;
            }
        }
    }
</script>