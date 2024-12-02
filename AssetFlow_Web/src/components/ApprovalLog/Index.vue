<template>
    <IndexBase @auth="onAuth($event)" action="Index" controller="ApprovalLog"></IndexBase>

    <Card>
        <template #title>
            <Toolbar>
                <template #left>
                    <div class="p-card-title">
                        Approval Log
                    </div>
                </template>
            </Toolbar>
        </template>
        <template #content>
            <!--:scrollable="true"
            scrollDirection="both"-->
            <DataTable ref="dt" responsiveLayout="scroll" class="p-datatable-sm" filterDisplay="row" sortMode="multiple" removableSort
                       stateStorage="local"
                       :stateKey="stateKey"
                       :value="dataModel" 
                       :loading="!dataModel"
                       :paginator="true"
                       :rows="15"
                       :lazy="true"
                       v-model:first="this.start"
                       scrollHeight="70vh"
                       wrapperClass="responsive-height"
                       @state-restore="onStateRestore($event)"
                       @page="onPage($event)"
                       @sort="onSort($event)"
                       @filter="onFilter($event)"
                       v-model:filters="filters"
                       :totalRecords="totalRecords">
                <Column :exportable="false" :sortable="false" frozen class="h-3rem">
                    <template #header="">
                        <!-- <Button v-show="canCreate" label="Create" icon="pi pi-plus" class="p-button-primary p-button-sm" @click="editor(this.$FORM_MODE_CREATE, 0)" /> -->
                    </template>
                    <template #filter>
                        <Button label="Clear" icon="pi pi-trash" class="p-button-outlined p-button-primary p-button-sm" @click="clearFilter"/>
                    </template>
                    <!-- <template #body="dataModel">
                        <Button icon="pi pi-search" v-show="canView" class="p-button-rounded p-button-info mr-2 p-button-sm" @click="editor(this.$FORM_MODE_VIEW, dataModel.data.Id)" />
                        <Button icon="pi pi-pencil" v-show="canEdit" class="p-button-rounded p-button-success mr-2 p-button-sm" @click="editor(this.$FORM_MODE_EDIT, dataModel.data.Id)" />
                        <Button icon="pi pi-trash" v-show="canDelete" class="p-button-rounded p-button-danger mr-2 p-button-sm" @click="editor(this.$FORM_MODE_DELETE, dataModel.data.Id)" />
                    </template> -->
                </Column>
                <Column field="Reference_No" header="Reference No" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Reference No" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                
                <Column field="Category" header="Category" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Category" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Mode" header="Mode" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Mode" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Info1" header="Info1" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Info1" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Info2" header="Info2" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Info2" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Reference_No1" header="Reference No1" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Reference No1" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Reference_No2" header="Reference No2" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Reference No2" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Detail_Username" header="Username" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Username'" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Detail_Status" header="Detail Status" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Detail Status" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Detail_Info1" header="Detail Info1" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Detail Info1" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Detail_Info2" header="Detail Info2" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Detail Info2" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Detail_Level" header="Level" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Level" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Detail_Approval_Date" header="Approval Date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker placeholder="Appoval_Date" dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDateTime(data.Detail_Approval_Date)}}
                    </template>
                </Column>
                <Column field="Detail_Approval_By" header="Approval By" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Approval By" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Status" header="Status" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Status" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Last_Approval_By" header="Last Approval By" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="'Last Approval By'" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Last_Approval_Date" header="Last Approval Date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker placeholder="Last Approval Date" dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDateTime(data.Last_Approval_Date)}}
                    </template>
                </Column>
                <Column field="Active" header="Active" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Active" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Created_Date" header="Created Date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker placeholder="Created Date" dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDateTime(data.Created_Date)}}
                    </template>
                </Column>
                <Column field="Created_By" header="Created By" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Created By" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Remarks" header="Remarks" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" placeholder="Remarks" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                
            </DataTable>
        </template>
    </Card>

</template>

<script>
    export default {
        name: "ApprovalLog",
        data() {
            return {
                dataModel: null,
                loading: true,
                totalRecords: 0,
                start: 0,
                stateKey: 'fams-state-approvallog-' + this.$store.getters.getUsername,
                sorts: null,
                lazyParams: {},
                filters: null,
                editorPath: "/ApprovalLog/Editor",
                dataUrl: "approvallog/list",
                canCreate: false,
                canEdit: false,
                canDelete: false,
                canView: false,
            }
        },
        methods: {
            onAuth(event) {
                this.canCreate = event.canCreate;
                this.canEdit = event.canEdit;
                this.canDelete = event.canDelete;
                this.canView = event.canView;
            },
            onPage(event) {
                this.start = event.first;
                this.loadLazyData();
            },
            onSort(event) {
                this.sorts = event.multiSortMeta;
                this.loadLazyData();
            },
            onFilter() {
                this.loadLazyData();
            },
            editor(mode, id) {
                this.$router.push({ path: this.editorPath, query: { mode: mode, id: id } });
            },
            loadLazyData() {
                this.loading = true;

                this.lazyParams = {
                    start: this.start,
                    sorts: this.dataSort,
                    filters: this.dataFilter
                };

                var self = this;

                this.$axios.post(this.$API_URL + this.dataUrl, this.lazyParams)
                    .then((response) => {
                        if (response.data.success) {
                            this.dataModel = response.data.data;
                            this.totalRecords = response.data.totalRecords;
                        }
                        else {
                            self.$toast.add({ severity: 'error', summary: "Error", detail: response.data.message, life: 120000});
                        }

                        this.loading = false;
                    })
                    .catch(function (err) {
                        self.$toast.add({ severity: 'error', summary: "Error", detail: err.message, life: 120000});
                        this.loading = false;
                    });
            },
            setFilters() {
                this.filters = {};
                var exportable = this.$refs.dt.columns.filter(c => c.props.field);
                for (var i = 0; i < exportable.length; i++) {
                    var filterValue = "";
                    if(this.stateFilters){
                        var field = this.stateFilters[exportable[i].props.field];
                        if(field) filterValue = field.value;
                    }
                    this.filters[exportable[i].props.field] = { value: filterValue, matchMode: "contains" };
                }
            },
            onStateRestore(event) {
                this.start = event.first;
                this.stateFilters = event.filters;
                this.sorts = event.multiSortMeta;
            },
            clearFilter(){
                this.filters = {};
                var exportable = this.$refs.dt.columns.filter(c => c.props.field);
                for (var i = 0; i < exportable.length; i++) {
                    var filterValue = "";
                    this.filters[exportable[i].props.field] = { value: "", matchMode: "contains" };
                }
                this.loadLazyData();
            }
        },
        mounted() {
            this.$emit('unblock-ui');
            this.setFilters();
            this.loadLazyData();
            this.$showToast();
        },
        computed: {
            dataFilter() {
                var h = [];
                if (this.filters) {
                    for (const [key, value] of Object.entries(this.filters)) {
                        h.push({
                            field: key,
                            value: value.value,
                            matchMode: value.matchMode
                        });
                    }
                }
                return h;
            },
            dataSort() {
                var h = [];
                if (this.sorts) {
                    for (var x = 0; x < this.sorts.length; x++) {
                        h.push({
                            field: this.sorts[x].field,
                            order: this.sorts[x].order == 1 ? "ASC" : "DESC"
                        });
                    }
                }
                return h;
            }
        },
    }
</script>