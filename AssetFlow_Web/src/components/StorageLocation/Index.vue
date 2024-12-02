<template>
    <IndexBase @auth="onAuth($event)" action="Index?type=MAIN" controller="StorageLocation"  v-if="this.$route.query.type == this.$STORAGE_LOCATION_CATEGORY.MAIN"/>
    <IndexBase @auth="onAuth($event)" action="Index?type=DETAIL" controller="StorageLocation"  v-if="this.$route.query.type == this.$STORAGE_LOCATION_CATEGORY.DETAIL"/>
    <Card>
        <template #title>
            <Toolbar>
                <template #left>
                    <div class="p-card-title">
                        {{title}}
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
                    <template #header>
                        <Button v-show="canCreate" :label="$t('create')" icon="pi pi-plus" class="p-button-primary p-button-sm" @click="editor(this.$FORM_MODE_CREATE, 0)" />
                    </template>
                    <template #filter>
                        <Button :label="$t('clear')" icon="pi pi-filter-slash" class="p-button-outlined p-button-primary p-button-sm" @click="clearFilter" :badge="filterBadge" badgeClass="p-badge-danger" />
                    </template>
                    <template #body="dataModel">
                        <Button icon="pi pi-search" v-show="canView" class="p-button-rounded p-button-info mr-2 p-button-sm" @click="editor(this.$FORM_MODE_VIEW, dataModel.data.Id)" />
                        <Button icon="pi pi-pencil" v-show="canEdit" class="p-button-rounded p-button-success mr-2 p-button-sm" @click="editor(this.$FORM_MODE_EDIT, dataModel.data.Id)" />
                        <Button icon="pi pi-trash" v-show="canDelete" class="p-button-rounded p-button-danger mr-2 p-button-sm" @click="editor(this.$FORM_MODE_DELETE, dataModel.data.Id)" />
                        <Button icon="pi pi-clone" v-show="true" class="p-button-rounded p-button-secondary mr-2 p-button-sm" @click="visualize(dataModel.data.Id)" />
                    </template>
                </Column>
                 <Column field="Company_Name" :header="$t('company')" :sortable="true"  :class="{ 'row-hidden':  !isDetail }">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" :placeholder="$t('company')" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Location_Name" :header="$t('location')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" :placeholder="$t('location')" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Parent_Name" :header="$t('parent')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" :placeholder="$t('parent')" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Description" :header="$t('storage_desc')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" :placeholder="$t('storage_desc')" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Type" :header="$t('type')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" :placeholder="$t('type')" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Is_Storage" :header="$t('is_storage')" :sortable="true" :class="{ 'row-hidden':  !isDetail }">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" :placeholder="$t('is_storage')" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column field="Created_Date" :header="$t('created_date')" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker :placeholder="$t('created_date')" dateFormat="dd/mm/yyyy" @date-select="filterCallback()" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
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
                        <DatePicker dateFormat="dd/mm/yyyy" inputMask="99/99/9999" @date-select="filterCallback()" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" placeholder="Filter Edited Date" />
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
                        <DatePicker dateFormat="dd/mm/yyyy" inputMask="99/99/9999" @date-select="filterCallback()" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" placeholder="Filter Edited Date" />
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
    <TreeMap :storageId="this.storageId" v-model:visibility="this.visibility"></TreeMap>
    <!-- <Dialog v-model:visible="display" :breakpoints="{'960px': '75vw', '640px': '100vw'}" :style="{width: '70vw'}">
       <h1>Test</h1>
    </Dialog>  -->
</template>

<script>
    export default {
        name: "StorageLocation",
        data() {
            return {
                dataModel: null,
                loading: true,
                totalRecords: 0,
                start: 0,
                stateKey: this.keyState,
                isAdmin: this.$store.getters.getIsAdmin,
                sorts: null,
                lazyParams: {},
                filters: null,
                stateFilters: null,
                editorPath: "/StorageLocation/Editor",
                dataUrl: "storagelocation/list",
                canCreate: false,
                canEdit: false,
                canDelete: false,
                canView: false,
                isMain: false,
                isDetail: false,
                display:false,
                type: "",
                storageId:null,
                visibility:false
            }
        },
        methods: {
            visualize(id){
                this.storageId = id;
                this.visibility = true;
            },
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
                this.$router.push({ path: this.editorPath, query: { mode: mode, id: id, type: this.type} });
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

                this.$axios.post(this.$API_URL + this.dataUrl+`?type=${this.type}`, this.lazyParams)
                    .then((response) => {
                        if (response.data.success) {
                            this.dataModel = response.data.data;
                            this.totalRecords = response.data.totalRecords;
                        }
                        else {
                            self.$toast.add({ severity: 'error', summary: "Error", detail: response.data.message, life: this.$DEFAULT_TIMER});
                        }

                        this.loading = false;
                    })
                    .catch(function (err) {
                        self.$toast.add({ severity: 'error', summary: "Error", detail: err.message, life: this.$DEFAULT_TIMER});
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
        created() {
            this.isMain = this.$route.query.type == this.$STORAGE_LOCATION_CATEGORY.MAIN;
            this.isDetail = this.isMain ? false : true;
            this.type = this.$route.query.type;
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
            },
            keyState(){
                return this.$STATE_NAME +'storagelocation-' + (this.isMain ? "main" : "detail") + '-' + this.$store.getters.getUsername;
            },
            title(){
                this.isMain = this.$route.query.type == this.$STORAGE_LOCATION_CATEGORY.MAIN;
                this.isDetail = this.isMain ? false : true;
                this.type = this.$route.query.type;
                this.loadLazyData();
                return (this.$route.query.type == this.$STORAGE_LOCATION_CATEGORY.MAIN ? "Main" : "Detail") + " " + this.$t('storage_location');
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