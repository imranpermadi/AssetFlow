<template>
    <IndexBase @auth="onAuth($event)" action="Index" controller="Group"></IndexBase>

    <Card>
        <template #title>
            <Toolbar>
                <template #left>
                    <div class="p-card-title">
                        {{$t('group')}}
                    </div>
                </template>
            </Toolbar>
        </template>
        <template #content>
            <DataTable ref="dt" responsiveLayout="scroll" class="p-datatable-sm" filterDisplay="row" sortMode="multiple" removableSort
                       v-model:expandedRows="expandedRows"
                       stateStorage="local"
                       :stateKey="stateKey"
                       :value="dataModel" :loading="!dataModel || loading"
                       :paginator="true"
                       :rows="15"
                       :lazy="true"
                       v-model:first="this.start"
                       scrollHeight="70vh"
                       wrapperClass="responsive-height"
                       @rowExpand="onRowExpand"
                       @state-restore="onStateRestore($event)"
                       @page="onPage($event)"
                       @sort="onSort($event)"
                       @filter="onFilter($event)"
                       @update:selection="onSelect($event)"
                       v-model:filters="filters"
                       :totalRecords="totalRecords"
                       v-model:selection="selectedRow" 
                       selectionMode="multiple" 
                       dataKey="Id" 
                       :metaKeySelection="false" 
                    >
                <template #empty>
                    No records found
                </template>
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
                <Column field="CreatedDate" :header="$t('created_date')" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker :placeholder="$t('created_date')" dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDateTime(data.CreatedDate)}}
                    </template>
                </Column>
                <Column field="CreatedBy" :header="$t('created_by')"  :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('created_by')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm"/>
                    </template>
                </Column>
                <Column field="EditedDate" :header="$t('edited_date')" dataType="date" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <DatePicker :placeholder="$t('edited_date')" dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY" class="p-column-filter p-inputtext-sm" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDateTime(data.EditedDate)}}
                    </template>
                </Column>
                <Column field="EditedBy" :header="$t('edited_by')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('edited_by')"  type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
            </DataTable>
        </template>
    </Card>

   
</template>

<script>
    export default {
        name: "Group",
        data() {
            return {
                dataModel: null,
                loading: true,
                totalRecords: 0,
                start: 0,
                stateKey: this.$STATE_NAME + 'group-' + this.$store.getters.getUsername,
                role: this.$store.getters.getRole,
                isAdmin: this.$store.getters.getIsAdmin,
                isUser: this.$store.getters.getIsUser,
                isTransporter: this.$store.getters.getIsTransporter,
                isDriver: this.$store.getters.getIsDriver,
                sorts: null,
                lazyParams: {},
                filters: null,
                stateFilters: null,
                editorPath: "/Group/Editor",
                dataUrl: "group/list",
                canCreate: false,
                canEdit: false,
                canDelete: false,
                canView: false,
                selectedRow: null,
                selectedStatus: null,
                expandedRows: [],

                transporterStatusList: [],
                statusList: [],
                shipmentTypeList: []
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

                        self.loading = false;
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
                    var filterValue = "";
                    var fieldName = exportable[i].props.field;
                    if(this.stateFilters){
                        var field = this.stateFilters[fieldName];
                        if(field) filterValue = field.value ? field.value : null;
                        else filterValue = null;
                    }
                    this.filters[fieldName] = { value: filterValue, matchMode: "contains" };

                    if(fieldName == "Status"){
                        if(filterValue)                            
                            this.selectedStatus = filterValue;
                        else this.selectedStatus = "All";
                    }

                }
            },
            onStateRestore(event) {
                this.start = event.first;
                this.stateFilters = event.filters;
                this.sorts = event.multiSortMeta;
            },
            onModalClose(){
                this.loadLazyData();
            },
            clearFilter(){
                this.filters = {};
                var exportable = this.$refs.dt.columns.filter(c => c.props.field);
                for (var i = 0; i < exportable.length; i++) {
                    var fieldName = exportable[i].props.field;
                    //console.log(exportable[i].props);

                    var filterValue = "";

                    if(fieldName == "Status") {
                        filterValue = this.selectedStatus;
                    }
                    else if(fieldName.includes("Date")){
                        filterValue = null;
                    }

                    this.filters[exportable[i].props.field] = { value: filterValue, matchMode: "contains" };
                }
                this.sorts = [];
                this.loadLazyData();
            },
            onChangeStatus(event){
                var status = event.value;

                this.filters.Status.value = status;

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
            filterBadge(){
                var badgeNumber = 0;

                this.dataFilter.filter(function(item) {                    
                    if(item.value && item.field != "Status"){
                        badgeNumber++;
                    }
                });

                return badgeNumber;
            },
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
            needTransporterAction(){
                var data = this.selectedRow;
                if(data != null && data.length >= 1){
                    for(var i=0; i< data.length;i++){
                        if(!data[i].need_transporter_action){
                            return false;
                        }
                    }
                    return true;
                }
            },
            canAssignDriver(){
                var data = this.selectedRow;
                if(data != null && data.length == 1){
                    if(data[0].can_assign_driver){
                        return true;
                    }
                }
                return false;
            },
            canViewShipments(){
                var data = this.selectedRow;
                if(data != null && data.length == 1){
                    if(data[0].can_view_shipments){
                        return true;
                    }
                }
                return false;
            },
            canViewHistory(){
                 var data = this.selectedRow;
                if(data != null && data.length == 1){
                    return true;
                }
                return false;
            },
            canCancelOrder(){
                 var data = this.selectedRow;
                if(data != null && data.length == 1){
                    if(data[0].can_cancel_order){
                        return true;
                    }
                }
                return false;
            },
            canMerge(){
                var data = this.selectedRow;
                var canMerge = false;
                if(data != null && data.length > 1){
                    var transporterCode = "";
                    var truckTypeId = 0;
                    var loadingFrom = 0;
                    var loadingTo = 0;
                    var orderTypeId = 0;
                    var shipmentTypeCategory = "";

                    for(var i=0; i< data.length;i++){
                        var d = data[i];
                        var oTruckTypeId = d.Truck_Type_Id;
                        var oTransporterId = d.Transporter_Code;
                        var oShipmentType = d.shipment_type_category;
                        var oLoadingFrom = d.Loading_Destination_From_Id;
                        var oLoadingTo = d.Loading_Destination_To_Id;
                        var oOrderType = d.Order_Type_Id;
                        //|| dispatchStatus != this.$OrderStatus.ACCEPTED || d.Status != this.$OrderStatus.DISPATCHED || oShipmentType == this.$ShipmentTypeCategory.COLLECTIVE
                        if(!d.can_assign_driver){
                            canMerge = false;
                            break;
                        }

                        if (orderTypeId == 0) orderTypeId = oOrderType;
                        else if (orderTypeId != oOrderType) {
                            canMerge = false;
                            break;
                        }

                        if (truckTypeId == 0) truckTypeId = oTruckTypeId;
                        else if (truckTypeId != oTruckTypeId) {
                            canMerge = false;
                            break;
                        }

                        if (transporterCode == "") transporterCode = oTransporterId;
                        else if (transporterCode != oTransporterId) {
                            canMerge = false;
                            break;
                        }

                        if (shipmentTypeCategory == "") shipmentTypeCategory = oShipmentType;
                        else if (shipmentTypeCategory != oShipmentType) {
                            canMerge = false;
                            break;
                        }

                        if (loadingFrom == 0) loadingFrom = oLoadingFrom;
                        else if (loadingFrom != oLoadingFrom) {
                            canMerge = false;
                            break;
                        }

                        if (loadingTo == 0) loadingTo = oLoadingTo;
                        else if (loadingTo != oLoadingTo) {
                            canMerge = false;
                            break;
                        }

                        canMerge = true;
                    }
                }
                return canMerge;
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