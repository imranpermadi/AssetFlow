<template>
    <DataTable v-model="dataModel" :value="dataModel" showGridlines responsiveLayout="scroll"
    :loading="!dataModel && mode != this.$FORM_MODE_CREATE"
    >
    <template #empty>
     No records found
     </template>
     <Column v-for="col of dataRequestLog" :field="col.field" :header="col.header" :key="col.field">
        {{ col.field.Period }}
         <template #body="{data}" v-if="col.field.includes('Date')" >
         {{this.$formatDateTime(data.Action_Date)}}
         </template>
     </Column>
    </DataTable>
 </template>
 
 <script>
 
 export default {
     name: "RequestLogDataTable",
     props:{
         modelValue: {
             type:Object,
             default:null
         },
         dataRequestLog:{
             type: Array,
             default:null
         },
         mode:{
             type: String,
             default:""
         }
     },
     data(){
         return{
             dataModel : this.modelValue
         }
     },
     mounted(){
       
     },
     methods:{
       
     }
 }
 
 </script>
 