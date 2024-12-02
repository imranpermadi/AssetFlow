<template>
    <IndexBase @auth="onAuth($event)" action="Index" controller="Approval"></IndexBase>

    <OverlayPanel ref="op" :showCloseIcon="true" :dismissable="true" :breakpoints="{'960px': '75vw', '640px': '100vw'}" :style="{width: '450px'}">
        <InputText v-model="overlayText" @input="onEditRemarks" style="width:100%"/> 
    </OverlayPanel>

    <Dialog :header="$t('approval_confirmation')" v-model:visible="showApprovalConfirmationModal" :closeOnEscape="false" :closable="false" :modal="true" :breakpoints="{'960px': '75vw', '640px': '100vw'}" :style="{width: '50vw'}">
        <div>
            {{this.$t('message.approval_confirmation') + confirmationStatus}}
<!-- 
            <ul v-for="(doc) in selectedDocuments" v-bind:key="doc.id">
               <li>{{doc.ref_no}}</li> 
            </ul> -->

            <div v-if="!processed">
                <div v-for="doc in selectedDocuments" v-bind:key="doc.ref_no">
                    <div class="grid">
                        <div class="col-5">
                            {{doc.ref_no}}
                        </div>
                        <div class="col-6">
                            
                            <div class="p-fluid">
                                <InputText type="text" class="p-inputtext-sm" v-model="doc.Remarks"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div v-else>
                <div v-for="item in resultData" v-bind:key="item.ref_no">
                    <div class="grid">
                        <div class="col-5">
                            {{item.ref_no}}
                        </div>
                        <div class="col-6">
                            <div class="p-fluid">
                                <InputText type="text" class="p-inputtext-sm" v-model="item.remarks" :disabled="true" />
                            </div>
                        </div>
                        <div class="col-1">
                            <div v-if="item.success">
                                <Tag class="mr-2" :severity="confirmationStatus == $APPROVAL_STATUS.APPROVED ? 'success' : 'danger'" :value="confirmationStatus == $APPROVAL_STATUS.APPROVED ? 'Approved' : 'Rejected'"></Tag>
                            </div>
                            <div v-else>
                                <Tag class="mr-2" severity="danger" :value="item.message"></Tag>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <template #footer>
            <div v-if="processed">
                <Button label="Close" @click="dismissConfirmation" />
            </div>
            <div v-else>
                <div v-if="processing" style="float:right!important">
                    <ProgressSpinner style="width:50px;height:50px"/>
                </div>
                <div v-else>
                    <div v-if="!processed">
                        <Button label="No" @click="dismissConfirmation" />
                        <Button label="Yes" autofocus @click="confirmApproval"/>
                    </div>
                    <div v-else>
                        <Button label="Close" icon="pi pi-times" class="p-button-text" @click="onClose"/>
                    </div>
                    
                </div>
            </div>
        </template>
    </Dialog>


    <Dialog header="Approval Details" v-model:visible="showApprovalDetails" :closeOnEscape="true" :closable="true" :modal="true" :breakpoints="{'960px': '75vw', '640px': '100vw'}" :style="{width: '60vw'}" >
        <DataTable :loading="!approvalDetails" :value="approvalDetails ? approvalDetails : null" responsiveLayout="scroll" class="p-datatable-sm editable-cells-table" :rowClass="setCurrentRow">
            <Column field="Username" :header="$t('username')" key="Username" style="width:10%">  
            </Column>
            <Column field="fullname" :header="$t('full_name')" key="Fullname" style="width:10%">  
            </Column>
            <Column field="Level" :header="$t('level')" key="Level" style="width:10%">  
            </Column>
            <Column field="Status" :header="$t('status')" key="Status">
            </Column>
            <Column field="Approval_By" :header="$t('approval_by')" key="Approval_By" style="width:15%">
            </Column>
            <Column field="Approval_Date" :header="$t('approval_date')" key="Approval_Date" style="width:15%">
                <template #body={data}>
                    {{$formatDateTime(data.Approval_Date)}}
                </template>
            </Column>
            <Column field="Info1" :header="$t('matrix')" key="Info1" style="width:10%">
            </Column>
        </DataTable>
    </Dialog>

    <Card>
        <template #title>
            <Toolbar>
                <template #left>
                    <div class="p-card-title">
                        {{$t('approval')}}
                        <Button icon="pi pi-refresh" class="p-button-rounded p-button-info p-button-outlined ml-2" @click="loadLazyData"/>
                    </div>
                </template>
                <template #right>
                    <Button class="pi-button p-button-success" :label="$t('approve')" v-if="selectedRow != null && selectedRow.length > 0" @click="processSelectedData($APPROVAL_STATUS.APPROVED)" />
                    <Button class="pi-button p-button-danger ml-2" :label="$t('reject')" v-if="selectedRow != null && selectedRow.length > 0" @click="processSelectedData($APPROVAL_STATUS.REJECTED)" />
                </template>
            </Toolbar>
        </template>
        <template #content>
            <Button text='Test' @click="refreshBadge"></Button>

            <div class="p-fluid mb-3">
                <CustomSelectButton v-model="selectedStatus" :options="statusList" optionLabel="option" class="p-text-center" @change="onStatusChange" :setDefault="true"> 
                    <template #option="slotProps">
                        <div class="p-text-center" style="width:100%">{{slotProps.option}}</div>
                    </template>
                </CustomSelectButton>
            </div>

            <DataTable ref="dt" responsiveLayout="scroll" class="p-datatable-sm" filterDisplay="row" sortMode="multiple" removableSort
                       v-model:expandedRows="expandedRows"
                       stateStorage="local"
                       :stateKey="stateKey"
                       :value="dataModel" :loading="!dataModel || loading"
                       :paginator="true"
                       :rows="10"
                       :lazy="true"
                       v-model:first="this.start"
                       @rowExpand="onRowExpand"
                       @state-restore="onStateRestore($event)"
                       @state-save="onStateSave($event)"
                       @page="onPage($event)"
                       @sort="onSort($event)"
                       @filter="onFilter($event)"
                       @update:selection="onSelect($event)"
                       @row-dblclick="onRowDoubleClick"
                       v-model:filters="filters"
                       :totalRecords="totalRecords"
                       v-model:selection="selectedRow" selectionMode="multiple" dataKey="Id" :metaKeySelection="false" 
                    >
                <Column :exportable="false" :sortable="false" :expander="true" headerStyle="width: 1rem">
                    <template #header>
                        <!-- <Button v-if="canCreate" label="Create" icon="pi pi-plus" class="p-button-primary p-button-sm" @click="editor(this.$FORM_MODE_CREATE, 0)" /> -->
                    </template>
                    <template #filter>
                        <Button :label="$t('clear')" icon="pi pi-filter-slash" class="p-button-outlined p-button-primary p-button-sm" @click="clearFilter" :badge="filterBadge" badgeClass="p-badge-danger" />
                    </template>
                    <!-- <template #body="{data}">
                        <button class="p-row-toggler p-link ml-2" type="button" v-ripple @click="toggleRemarksPopup($event,data)" v-tooltip.right="{value: data.Remarks, disabled: !data.Remarks}">
                            <i class="pi pi-comment p-text-secondary" v-badge="Y" v-if="data.Remarks"></i>
                            <i class="pi pi-comment p-text-secondary" v-else></i>
                        </button>                        
                    </template> -->
                </Column>
                <Column field="Reference_No" :header="$t('ref_no')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('ref_no')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm"/>
                    </template>
                    <template #body="slotProps">
                        <Button :label="slotProps.data.Reference_No" class="p-button-text" @click="goToDetail(slotProps)" />
                    </template>
                </Column>
                <Column field="Reference_No1" :header="$t('sap_no')" :sortable="true" v-if="deactivationDocument || returnDocument">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('sap_no')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm"/>
                    </template>
                </Column>
                <Column field="Reference_No2" :header="$t('order_no')" :sortable="true" v-if="deactivationDocument">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('order_no')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm"/>
                    </template>
                </Column>
                 <Column v-if="transferDocument" field="Status" :header="$t('status')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('status')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm"/>
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Packing_No" :header="$t('packing_no')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('packing_no')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Document_Desc" :header="$t('document_desc')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('document_desc')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Transfer_Type" :header="$t('transfer_type')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('transfer_type')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Storage_From" :header="$t('storage_from')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('storage_from')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Destination" :header="$t('destination')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('destination')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Remarks" :header="$t('remark')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('remark')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Created_By" :header="$t('created_by')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('created_by')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Created_Date" :header="$t('created_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" :placeholder="$t('created_date')" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Created_Date)}}
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Last_Approval_By" :header="$t('last_approval_by')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('last_approval_by')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="transferDocument" field="Last_Approval_Date" :header="$t('last_approval_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" :placeholder="$t('last_approval_date')" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Last_Approval_Date)}}
                    </template>
                </Column>

                 <Column v-if="returnDocument" field="Info1" :header="$t('approval_matrix')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('approval_matrix')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Transporter_Name" :header="$t('transporter')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('transporter')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Loading_Destination_From_Name" :header="$t('loading')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('loading')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Loading_Destination_To_Name" :header="$t('destination')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('destination')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Cost_Suggested" :header="$t('freight_master')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_master')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Cost" :header="$t('freight_cost')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_cost')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Cost_Per" :header="$t('freight_per')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_per')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="System_NoT" :header="$t('system_num_trip')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('system_not')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Total_Cost" :header="$t('freight_total_cost')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('frieght_total_cost')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="User_NoT" :header="$t('user_num_trip')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('user_not')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Total_Cost_User_NoT" :header="$t('freight_total_cost_user_not')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_total_cost_user_not')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Best_Price_UOM" :header="$t('freight_best_price_uom')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_best_price_uom')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Best_Price" :header="$t('freight_best_price')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_best_price')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Different_Per_UOM" :header="$t('freight_different_per_uom')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_different_per_uom')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Different_Percentage" :header="$t('freight_different_percentage')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_different_percentage')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}} %
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Freight_Total_Different" :header="$t('freight_total_different')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('freight_total_different')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="slotProps">
                        {{this.$formatNumber(slotProps.data[slotProps.column.props.field])}}
                    </template>
                </Column>
               
                <Column v-if="returnDocument" field="Assign_Reason" :header="$t('assign_reason')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('assign_reason')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Truck_Type_Name" :header="$t('truck_type')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('truck_type')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Stuffing_Date_From" :header="$t('stuffing_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" :placeholder="$t('stuffing_date')" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Stuffing_Date_From)}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Stuffing_Date_To" :header="$t('stuffing_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" :placeholder="$t('stuffing_date')" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Stuffing_Date_To)}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Shipment_Type_Name" :header="$t('shipment_type')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('shipment_type')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Category" :header="$t('category')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('category')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Created_By" :header="$t('created_by')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('created_by')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Created_Date" :header="$t('created_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" :placeholder="$t('created_date')" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Created_Date)}}
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Last_Approval_By" :header="$t('last_approval_by')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('last_approval_by')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="returnDocument" field="Last_Approval_Date" :header="$t('last_approval_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" :placeholder="$t('last_approval_date')" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Last_Approval_Date)}}
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Appointment_Date" :header="$t('appointment_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('appointment_date')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Appointment_Date)}}
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Appointment_Time" :header="$t('appointment_time')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('appointment_time')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Appointment_Transporter_Name" :header="$t('transporter')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('transporter')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Appointment_Loading" :header="$t('loading')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('loading')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Appointment_Destination" :header="$t('destination')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('destination')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Vehicle_Name" :header="$t('vehicle')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('vehicle')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Driver_Name" :header="$t('driver')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('driver')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Appointment_Warehouse" :header="$t('factory')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('factory')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Appointment_Business_Segment" :header="$t('product_category')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('product_category')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Category" :header="$t('category')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('category')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Created_By" :header="$t('created_by')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('created_by')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Created_Date" :header="$t('created_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" :placeholder="$t('created_date')" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Created_Date)}}
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Status" :header="$t('status')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('status')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Last_Approval_By" :header="$t('last_approval_by')" :sortable="true">
                    <template #filter="{filterModel,filterCallback}">
                        <InputText :placeholder="$t('last_approval_by')" type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" />
                    </template>
                </Column>
                <Column v-if="deactivationDocument" field="Last_Approval_Date" :header="$t('last_approval_date')" :sortable="true">
                    <!--<Column field="Code" header="Code" :sortable="true" style="flex-grow:1; flex-basis:100px">-->
                    <template #filter="{filterModel,filterCallback}">
                        <InputText type="text" v-if="filterModel" v-model="filterModel.value" @keydown.enter="filterCallback()" class="p-column-filter p-inputtext-sm" :placeholder="$t('last_approval_date')" />
                    </template>
                    <template #body="{data}">
                        {{ this.$formatDate(data.Last_Approval_Date)}}
                    </template>
                </Column>
                <template #expansion="slotProps">
                    <div class="orders-subtable">
                        <DataTable :value="slotProps.data.Details" class="w-screen-71">
                            <Column field="Doc_No" :header="$t('doc_no')"></Column>
                            <Column field="Doc_Description" :header="$t('doc_desc')"></Column>
                            <Column field="Doc_Category" :header="$t('doc_cat')"></Column>
                            <Column field="Company" :header="$t('company')"></Column>
                            <Column field="Period_From" :header="$t('period_from')">
                                <template #body="{data}">
                                    {{ $formatDate(data.Period_From)}}
                                </template>
                            </Column>
                            <Column field="Period_To" :header="$t('period_to')">
                                <template #body="{data}">
                                    {{ $formatDate(data.Period_To)}}
                                </template>
                            </Column>
                        </DataTable>
                    </div>
                </template>
            </DataTable>
        </template>
    </Card>

   
</template>

<script>

    export default {
        name: "Approval",
        props:{
            menuItem:{
                type:Object,
                default:null
            }
        },
        data() {
            return {
                dataModel: null,
                loading: false,
                totalRecords: 0,
                start: 0,
                stateKey: this.$STATE_NAME + 'approval-' + this.$store.getters.getUsername,
                role: this.$store.getters.getRole,
                isAdmin: this.$store.getters.getIsAdmin,
                isUser: this.$store.getters.getIsUser,
                sorts: null,
                lazyParams: {},
                filters: null,
                stateFilters: null,
                editorPath: "/Approval/Editor",
                approvalDetailsUrl: "approval/approval_details/",
                dataUrl: "approval/list",
                statusUrl: "approval/status",
                approveUrl: "approval/process",
                canCreate: false,
                canEdit: false,
                canDelete: false,
                canView: false,
                selectedRow: null,
                statusList: null,
                selectedStatus: null,
                expandedRows: [],
                createOrder: false,
                assignTransporter:false,
                createAppointment: false,
                category: "",
                selectedCategoryIndex:0,
                overlayData: null,
                overlayText:"",
                showApprovalConfirmationModal: false,
                confirmationStatus: "",
                processed: false,
                processing: false,
                resultData: [],
                showApprovalDetails: false,
                approvalDetails: []
            }
        },
        methods: {
            refreshBadge(){
                var self = this;

                var data = {
                    item: this.menuItem,
                    count: 123
                };

                self.$store.dispatch("setMenu", data);
            },
            onEditRemarks(){
                this.overlayData.Remarks = this.overlayText;
            },
            toggleRemarksPopup(event, data){
                this.overlayData = data;
                this.overlayText = data.Remarks;
                this.$refs.op.toggle(event);
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
                this.$router.push({ path: this.editorPath, query: { mode: mode, id: id } });
            },
            loadLazyData() {
                
                this.selectedRow = null;

                this.lazyParams = {
                    start: this.start,
                    sorts: this.dataSort,
                    filters: this.dataFilter,
                    category: this.selectedStatus
                };

                var self = this;

                if(!self.loading){
                    this.loading = true;

                    this.$axios.post(this.$API_URL + this.dataUrl, this.lazyParams)
                        .then((response) => {
                            if (response.data.success) {
                                self.dataModel = response.data.data;
                                self.totalRecords = response.data.totalRecords;
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
                }

               
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

                this.selectedCategoryIndex = event.selectedCategoryIndex ?? 0;
            },
            onStateSave(event){
                event.selectedCategoryIndex = this.selectedCategoryIndex;

            },
            onModalClose(){
                this.loadLazyData();
            },
            clearFilter(){
                this.filters = {};
                var exportable = this.$refs.dt.columns.filter(c => c.props.field);
                for (var i = 0; i < exportable.length; i++) {
                    this.filters[exportable[i].props.field] = { value: "", matchMode: "contains" };
                }
                this.loadLazyData();
            },
            onStatusChange(event){
                this.selectedStatus = event.value;
                this.selectedCategoryIndex = event.index;
                this.$refs.dt.triggerSaveState();
                this.setFilters();
                this.$toast.removeAllGroups();
                this.loadLazyData();
            },
            processSelectedData(status){
                this.showApprovalConfirmationModal = true;
                this.confirmationStatus = status;
            },
            confirmApproval(){
                var self = this;

                var data = {
                    data: this.selectedDocuments,
                    status: this.confirmationStatus
                };
                self.processing = true;
                
                this.$axios.put(this.$API_URL + this.approveUrl, data)
                    .then((response) => {
                        self.processed = true;
                        self.processing = false;

                        if (response.data.success) {
                            self.resultData = response.data.data.data;
                            self.refreshBadge();
                            self.$router.push({ name: this.indexName, params: { showToast: true, severity: 'success', summary: 'Data Saved', detail: response.data.message, life: 120000} } );
                        }
                        else {
                            self.$toast.add({ severity: 'error', summary: "Error", detail: response.data.message, life: 120000});
                        }
                    })
                    .catch(function (error) {
                        self.processed = false;
                        self.processing = false;

                        self.$toast.add({ severity: 'error', summary: "Error", detail: error, life: 120000});
                    });
            },
            dismissConfirmation(){
                this.processed = false;
                this.showApprovalConfirmationModal = false;
                this.loadLazyData();
            },
            onRowDoubleClick(event){
                var self = this;

                self.approvalDetails = [];
                var approvalDataId = event.data.Approval_Id;
                var dataId = event.data.Id;

                self.showApprovalDetails = true;

                this.$axios.get(this.$API_URL + this.approvalDetailsUrl + dataId + "/" + approvalDataId)
                    .then((response) => {
                        if (response.data.success) {
                            self.approvalDetails = response.data.data;
                            self.$router.push({ name: this.indexName, params: { showToast: true, severity: 'success', summary: 'Data Saved', detail: response.data.message, life: 120000} } );
                        }
                        else {
                            self.showApprovalDetails = false;
                            self.$toast.add({ severity: 'error', summary: "Error", detail: response.data.message, life: 120000});
                        }
                    })
                    .catch(function (error) {
                        self.showApprovalDetails = false;

                        self.$toast.add({ severity: 'error', summary: "Error", detail: error, life: 120000});
                    });
            },
            setCurrentRow(data){
                console.log(data);
                return data.is_current ? "current-approval" : "";
            },
            goToDetail(event){
                var index = event.index;
                var data = event.data;

                if(this.transferDocument){
                    let routeData = this.$router.resolve({name: 'DocumentTransferEditor', query: {mode: this.$FORM_MODE_VIEW, id: data.Document_Transfer_Id}});
                    window.open(routeData.href, '_blank');
                }
                // else if(this.returnDocument){
                //     let routeData = this.$router.resolve({name: 'OrderEditor', query: {mode: this.$FORM_MODE_VIEW, id: data.Order_Id, view_type: "APPR"}});
                //     window.open(routeData.href, '_blank');
                // }
            }
        },
        mounted() {
            this.$emit('unblock-ui');
            this.setFilters();

            console.log(this.menuItem);

            var self = this;

            this.$axios.get(this.$API_URL + this.statusUrl)
                .then((response) => {
                    if (response.data.success) {
                        self.statusList = response.data.data;
                        var selectedStatusValue = Object.values(this.statusList)[this.selectedCategoryIndex];
                        if(selectedStatusValue == undefined){
                            self.selectedStatus = Object.values(this.statusList)[0];
                        }
                        else{
                            self.selectedStatus = selectedStatusValue;
                        }

                        self.loadLazyData();
                    }
                    else {
                        self.$toast.add({ severity: 'error', summary: "Error", detail: response.data.message, life: 120000});
                    }

                    self.loading = false;
                })
                .catch(function (err) {
                    console.log(self);
                    self.$toast.add({ severity: 'error', summary: "Error", detail: err, life: 120000});
                    self.loading = false;
                });            
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
            },
            transferDocument(){
                return this.selectedCategoryIndex == 0;
            },
            returnDocument(){
                return this.selectedCategoryIndex == 1;
            },
            deactivationDocument(){
                return this.selectedCategoryIndex == 2;
            },
            selectedDocuments(){
                var data = [];

                if(this.selectedRow == null) return data;

                for(var x = 0; x< this.selectedRow.length; x++){
                    var row = this.selectedRow[x];

                    var d = {
                        id: row.Id,
                        remarks: row.Remarks,
                        ref_no: row.Reference_No
                    };

                    data.push(d);
                }

                return data;
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