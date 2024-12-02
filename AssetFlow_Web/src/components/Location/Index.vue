<template>
    <IndexBase @auth="onAuth($event)" action="Index" controller="Location"></IndexBase>

    <Card>
        <template #title>
            <Toolbar>
                <template #left>
                    <div class="p-card-title">
                        {{$t('location')}}
                    </div>
                </template>
            </Toolbar>
        </template>
        <template #content>            
            <DataTable ref="dt" responsiveLayout="scroll" class="p-datatable-sm" filterDisplay="row" sortMode="multiple" removableSort
                       stateStorage="local"
                       :stateKey="stateKey"
                       :value="dataModel" 
                       :loading="!dataModel || loading"
                       :paginator="true"
                       :rows="10"
                       :lazy="true"
                       v-model:first="this.start"
                       @state-restore="onStateRestore($event)"
                       @page="onPage($event)"
                       @sort="onSort($event)"
                       @filter="onFilter($event)"
                       v-model:filters="filters"
                       :totalRecords="totalRecords"
                       dataKey="Id" 
                    >
                <Column :exportable="false" :sortable="false" headerStyle="white-space:nowrap" >
                    <template #header="">
                        <Button v-show="canCreate" :label="$t('create')" icon="pi pi-plus" class="p-button-primary p-button-sm" @click="editor(this.$FORM_MODE_CREATE, 0)" />
                    </template>
                    <template #filter>
                        <Button :label="$t('clear')" icon="pi pi-filter-slash" class="p-button-outlined p-button-primary p-button-sm" @click="clearFilter" :badge="filterBadge" badgeClass="p-badge-danger" />
                    </template>
                    <template #body="dataModel">
                        <Button icon="pi pi-search" v-show="canView" class="p-button-rounded p-button-info mr-2 p-button-sm" @click="editor(this.$FORM_MODE_VIEW, dataModel.data.Id)" />
                        <Button icon="pi pi-pencil" v-show="canEdit" class="p-button-rounded p-button-success mr-2 p-button-sm" @click="editor(this.$FORM_MODE_EDIT, dataModel.data.Id)" />
                        <Button icon="pi pi-trash" v-show="canDelete" class="p-button-rounded p-button-danger mr-2 p-button-sm" @click="editor(this.$FORM_MODE_DELETE, dataModel.data.Id)" />
                    </template>
                </Column>
                <Column field="Code" :header="$t('code')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" :placeholder="$t('code')" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Description" :header="$t('description')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" :placeholder="$t('description')" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Created_Date" :header="$t('created_date')" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker :placeholder="$t('created_date')" @date-select="filterCallback()" dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDateTime(data.Created_Date)}}
                    </template>
                </Column>
                <Column field="Created_By" :header="$t('created_by')"  :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Created By" />
                    </template>
                </Column>
                <Column field="Edited_Date" :header="$t('edited_date')" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker dateFormat="dd/mm/yyyy" @date-select="filterCallback()" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" placeholder="Filter Edited Date" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDateTime(data.Edited_Date)}}
                    </template>
                </Column>
                <Column field="Edited_By" :header="$t('edited_by')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Edited By" />
                    </template>
                </Column>
                <Column field="Is_Deleted" :header="$t('is_deleted')" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <YNSelect :placeholder="$t('is_deleted')" :useBinary="true" v-if="filterModel" v-model="filterModel.value" @change="filterCallback()"/>
                    </template>
                </Column>
                <Column field="Deleted_Date" :header="$t('deleted_date')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker dateFormat="dd/mm/yyyy" @date-select="filterCallback()" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" placeholder="Filter Edited Date" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDateTime(data.Deleted_Date)}}
                    </template>
                </Column>
                <Column field="Deleted_By" :header="$t('deleted_by')" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" placeholder="Filter Edited By" />
                    </template>
                </Column>
            </DataTable>
        </template>
    </Card>

   
</template>

<script>
    export default {
        name: "Location",
        data() {
            return {
                dataModel: null,
                loading: true,
                totalRecords: 0,
                start: 0,
                stateKey: this.$STATE_NAME +'location-' + this.$store.getters.getUsername,
                isAdmin: this.$store.getters.getIsAdmin,
                sorts: null,
                lazyParams: {},
                filters: null,
                stateFilters: null,
                editorPath: "/Location/Editor",
                dataUrl: "location/list",
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
            onSelect(event){
                this.selectedRow = event.data;
            },
             onRowExpand(event){
                this.expandedRows = [];
                this.expandedRows.push(event.data);
            },
            editor(mode, id) {
                this.$router.push({ path: this.editorPath, query: { mode: mode, id: id } });
            },
            filter(mode) {
                this.$router.push({ path: this.filterPath, query: { mode: mode} });
            },
            sidebarMessage(event){
                this.$toast.add(event);
            },
            loadLazyData() {
                this.loading = true;
                this.selectedRow = null;

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
                        self.loading = false;
                    });
            },
            setFilters() {
                this.filters = {};
                var exportable = this.$refs.dt.columns.filter(c => c.props.field);
                for (var i = 0; i < exportable.length; i++) {
                    var filterValue = null;
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
            editOrder(){
                this.$router.push({ path: this.editorPath, query: { mode: this.$FORM_MODE_EDIT, id: this.selectedRow[0].Id } });
            },
             onModalClose(){
                this.loadLazyData();
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
                            field: key ? key : null,
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
            },
            filterBadge(){
                var badgeNumber = 0;

                this.dataFilter.filter(function(item) {                    
                    if(item.value && item.field != "Status"){
                        badgeNumber++;
                    }
                });

                return badgeNumber;
            }
        },
    }
</script>

<style #scoped>
    .p-card .p-card-content{
        margin:0!important;
        padding: 0 !important;
    }
</style>