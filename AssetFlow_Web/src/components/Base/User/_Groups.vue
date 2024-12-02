<template>
    <DataTable :loading="!header" :value="dataArray" responsiveLayout="scroll" class="p-datatable-sm editable-cells-table" :rowClass="hideRow" editMode="cell" @cell-edit-complete="editDetail">
        <Column >
            <template #header="">
                <Button label="" icon="pi pi-plus" class="p-button-primary p-mr-2 p-button-sm" @click="addDetail" :disabled="!canEdit" />
            </template>
            <template #body="slotProps">
                <Button icon="pi pi-minus" class="p-button-danger p-mr-2 p-button-sm" @click="deleteDetail(slotProps)" :disabled="!canEdit" />
            </template>
        </Column>
        <Column field="Group_Id" :header="$t('group')" key="Group_Id" class="w-12">
            <template #body="slotProps">
                 <CustomSelect 
                    v-model="slotProps.data[slotProps.column.props.field]" 
                    :url="this.$API_URL + 'list/group'" 
                    :minLength="0" 
                    :filter="true"
                    @change="onCellEditComplete(slotProps)"
                    :disabled="!canEdit" 
                    />
            </template>
        </Column>
    </DataTable>
</template>
<script>
    export default {
        props:{
            dataArray:{
                type:Array,
                default: []
            },
            dataModel:{
                type:Object,
                default:[]
            },
            mode:{
                type:String,
                default: ""
            },
            header:{
                type:Object,
                default:null
            }
        },
        data(){
            return {

            }
        },
        methods:{
            hideRow(data) {
                return data.mode == this.$FORM_MODE_DELETE ? 'row-hidden' : null;
            },
            addDetail() {
                var newDetail = this.dataModel;
                newDetail.mode = this.$FORM_MODE_CREATE;
                var det = { ...newDetail };

                this.dataArray.push(det);
            },
            deleteDetail(event) {
                if (event) {
                    var detail = this.dataArray[event.index];

                    if (detail.mode == this.$FORM_MODE_CREATE) this.dataArray.splice(this.dataArray.indexOf(detail), 1);
                    else detail.mode = this.$FORM_MODE_DELETE;
                }
            },
            onCellEditComplete(event) {
                this.editDetail(event.index);
            },
            editDetail(index) {
                if (this.dataArray && this.dataArray.length > 0) {
                    let m = this.dataArray[index].mode;

                    if (m == this.$FORM_MODE_UNCHANGED) {
                        this.dataArray[index].mode = this.$FORM_MODE_EDIT;
                    }
                }
            }
        },
        computed:{
            canEdit(){
                return this.mode == this.$FORM_MODE_CREATE || this.mode == this.$FORM_MODE_EDIT;
            }
        }
    }
</script>