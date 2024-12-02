<template>
    <IndexBase @auth="onAuth($event)" action="Index" controller="RunningNumber"></IndexBase>

    <Card>
        <template #title>
            Running Number
        </template>
        <template #content>
            <!--:scrollable="true"
            scrollDirection="both"-->
            <DataTable ref="dt" responsiveLayout="scroll" class="p-datatable-sm" filterDisplay="row" sortMode="multiple" removableSort
                       stateStorage="local"
                       :stateKey="stateKey"
                       :value="dataModel" :loading="!dataModel"
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
                <Column :exportable="false" :sortable="false">
                    <template #header="">
                        <Button v-show="canCreate" label="Create" icon="pi pi-plus" class="p-button-primary p-button-sm" @click="editor(this.$FORM_MODE_CREATE, 0)" />
                    </template>
                    <template #filter>
                        <Button :label="$t('clear')" icon="pi pi-filter-slash" class="p-button-outlined p-button-primary p-button-sm" @click="clearFilter" :badge="filterBadge" badgeClass="p-badge-danger"/>
                    </template>
                    <template #body="dataModel">
                        <Button icon="pi pi-search" v-show="canView" class="p-button-rounded p-button-info mr-2 p-button-sm" @click="editor(this.$FORM_MODE_VIEW, dataModel.data.Company_Id)" />
                        <Button icon="pi pi-pencil" v-show="canEdit" class="p-button-rounded p-button-success mr-2 p-button-sm" @click="editor(this.$FORM_MODE_EDIT, dataModel.data.Company_Id)" />
                        <!-- <Button icon="pi pi-trash" v-show="canDelete" class="p-button-rounded p-button-danger mr-2 p-button-sm" @click="editor(this.$FORM_MODE_DELETE, dataModel.data.Id)" /> -->
                    </template>
                </Column>
                <Column field="Company_Code" header="Company Code" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Company Code" />
                    </template>
                </Column>
                <Column field="Company_Name" header="Company Name" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Company Name" />
                    </template>
                </Column>
                <!-- <Column field="Trans_Type" header="Trans Type" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Trans Type" />
                    </template>
                </Column>
                <Column field="RangeFromNumber" header="Range From Number" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Range From Number" />
                    </template>
                </Column>
                <Column field="RangeToNumber" header="Range To Number" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Range To Number" />
                    </template>
                </Column>
                <Column field="Format" header="Format" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Format" />
                    </template>
                </Column>
                <Column field="CreatedDate" header="Create Date" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker placeholder="Create Date" dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
                    </template>
                    <template #body="{data}"> {{ $formatDate(data.CreatedDate)}} </template>
                </Column>
                <Column field="CreatedBy" header="Created By" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Created By" />
                    </template>
                </Column>
                <Column field="EditedDate" header="Edit Date" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker placeholder="Edit Date" dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
                    </template>
                    <template #body="{data}"> {{ $formatDate(data.EditedDate)}} </template>
                </Column>
                <Column field="EditedBy" header="Edited By" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Edited By" />
                    </template>
                </Column> -->

            </DataTable>
        </template>
    </Card>

</template>

<script>
    export default {
        name: "RunningNumber",
        data() {
            return {
                dataModel: null,
                loading: true,
                totalRecords: 0,
                start: 0,
                stateKey: 'fams-state-running-number-' + this.$store.getters.getUsername,
                sorts: null,
                lazyParams: {},
                filters: null,
                editorPath: "/RunningNumber/Editor",
                dataUrl: "RunningNumber/list",
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
                    this.filters[exportable[i].props.field] = { value: "", matchMode: "contains" };
                }
            },
            onStateRestore(event) {
                this.first = event.first;
                this.filters = event.filters;
                this.sorts = event.multiSortMeta;
            },
            clearFilter(){
                this.filters = {};
                var exportable = this.$refs.dt.columns.filter(c => c.props.field);
                for (var i = 0; i < exportable.length; i++) {
                    var fieldName = exportable[i].props.field;
                    var filterValue = "";
                    if(fieldName.includes("Date")){
                        filterValue = null;
                    }
                    this.filters[exportable[i].props.field] = { value: filterValue, matchMode: "contains" };
                }
                this.loadLazyData();
            }
        },
        mounted() {
            this.$emit('unblock-ui');
            if (this.filters == null) this.setFilters();
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