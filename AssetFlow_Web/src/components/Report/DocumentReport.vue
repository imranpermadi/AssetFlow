<template>
    <form id="mainForm" action="#" method="post">
        <Card>
            <template #title>
                <Toolbar>
                    <template #left>
                        <div class="p-card-title">Document Report</div>
                    </template>
                </Toolbar>
            </template>
            <template #content>
                <div class="p-inputtext-sm p-fluid">
                    <div class="grid">
                       <div class="col-12">
                        <FormField fieldName="Report_Type" labelName="Report Type" v-model="header"
                        fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="Report_Type" :filter="true"  
                            v-model="header.Report_Type" required="true" 
                            placeholder="Please Select"
                            :url="$API_URL + 'list/lookup'"
                            :minLength="0"
                            :params="{ group: $LOOKUP_GROUP.REPORT_TYPE }" />
                            <small v-if="header && !header.Report_Type && submitted && mode != this.$FORM_MODE_DELETE"
                                class="p-error">{{ $t('is_required').replace('Value', $t('report_type')) }}
                            </small>
                        </FormField>
                        
                        <FormField fieldName="Company_Id" labelName="Company" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                                <CustomMultiSelect class="p-inputtext-sm"
                                    v-model="header.Company_Id"
                                    placeholder="Please Select"
                                    :filter="true"
                                    :url="this.$API_URL + 'list/company_Id'" 
                                    :minLength="0" 
                                    :params="{}"
                                    @change="changeCompany($event)"/>
                        </FormField>

                        <FormField fieldName="Location_Id" labelName="Location" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                                <CustomMultiSelect class="p-inputtext-sm" 
                                    v-model="header.Location_Id"
                                    placeholder="Please Select"
                                    :url="this.$API_URL + 'list/location_Id'" 
                                    :minLength="0" 
                                    :params="{}"
                                    :filter="true" 
                                    @change="changeLocation($event)"/>
                        </FormField>
                        
                        <FormField fieldName="Storage_Location_Id" labelName="Storage Location" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="Storage_Location_Id" :filter="true"  
                            v-model="header.Storage_Location_Id" required="true" 
                            placeholder="Please Select"
                            :url="this.$API_URL + 'list/storage_location'" :minLength="0" :params="{}" />
                        </FormField>

                        <FormField fieldName="Department_Id" labelName="Department" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="Department_Id" :filter="true"  
                            v-model="header.Department_Id" required="true" 
                            placeholder="Please Select"
                            :url="this.$API_URL + 'list/department_Id'" :minLength="0" :params="{}" />
                        </FormField>

                        <FormField fieldName="Packing_No" labelName="Packing No" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="Packing_No" :filter="true"  
                            v-model="header.Packing_No" required="true" 
                            placeholder="Please Select"
                            :url="this.$API_URL + 'list/packing_detail_Id'" :minLength="0" :params="{}" />
                        </FormField>

                        <FormField fieldName="Document_Category" labelName="Document Category" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="Document_Category" :filter="true"  
                            v-model="header.Document_Category" required="true" 
                            placeholder="Please Select"
                            :url="this.$API_URL + 'list/doc_category_code'" :minLength="0" :params="{}" />
                        </FormField>

                        <div class="field grid">
                            <label for="Period From" class="col-2">{{$t('period_from')}}</label>
                                    <div class="col-3 md-3">
                                        <DatePicker name="Period_From" v-model="header.Period_From" :placeholder="$t('period_from')" v-if="header"  
                                            dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY"/>
                                            <small v-if="header && !header.Period_From && header.Period_To && submitted"
                                                class="p-error">{{ $t('is_required').replace('Value', $t('report_type')) }}
                                            </small>
                                    </div>
                            <label for="Period To">{{$t('to')}}</label>
                                <div class="col-3 md-3">
                                     <DatePicker name="Period_To" v-model="header.Period_To" :placeholder="$t('period_to')" v-if="header"  
                                        dateFormat="dd/mm/yyyy" inputMask="99/99/9999" momentFormat="DD/MM/YYYY"/>
                                </div>
                        </div>

                        <FormField fieldName="Status" labelName="Status" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="Status" :filter="true"  
                            v-model="header.Status" required="true" 
                            placeholder="Please Select"
                            :url="$API_URL + 'list/lookup'"
                            :minLength="0"
                            :params="{ group: $LOOKUP_GROUP.DOCUMENT_STATUS }" />
                        </FormField>
                        
                        </div>
                    </div>
                </div>
                <Button type="button" label="Generate" @click="getReport()" />
                &nbsp;
                <Button type="button" class="p-button-success" label="Reopen" @click="reopenTab()"
                    v-show="this.reportTbl" />
            </template>
        </Card>
    </form>
    <ShowReport :isVisible="showReport" :reportTbl="this.reportTbl" :configHtml2Pdf="this.configHtml2Pdf" @close="resetBtn()" />
</template>

<script type="text/javascript">
import ShowReport from "../Report/ShowReport.vue";

export default {
    components: {
        ShowReport
    },
    data() {
        return {
            test: 0,
            mode: "",
            dataUrl: "Report/generate_document_report",
            reportTbl: null,
            showReport: false,
            loadingReport: false,
            mergedOptions: [
                { label: "Y", value: 1 },
                { label: "N", value: 0 },
            ],
            header: {
                Report_Type: null,
                Company_Id: [],
                Location_Id: [],
                Storage_Location_Id: null,
                Department_Id: null,
                Period_From: null,
                Period_To: null,
                Packing_No: null,
                Document_Category : null,
                Status:null,
            },
             submitted :false,
             configHtml2Pdf : null,
        };
    },
      methods: {
        changeCompany(e){
           this.header.Company_Code = e == null ? []:e;
        },
         changeLocation(e){
           this.header.Location_Code = e == null ? []:e;
        },
        reopenTab() {
            this.showReport = true;
        },
        resetBtn() {
            this.showReport = false;
        },
        tableToExcel() {
            this.loadingReport = !this.loadingReport;
        },
        getReport() {
           var self = this;
           this.$emit('block-ui');
           this.submitted = true;

            if(!this.header.Report_Type || (!this.header.Period_From && this.header.Period_To))
            {
                this.$emit('unblock-ui');
                return false;
            }
            
            this.configHtml2Pdf = {
                margin: 0.50,
                filename: 'DocuementReport.pdf',
                image: { type: 'jpeg', quality: 0.98 },
                html2canvas: {scale: 3, dpi: 192},
                jsPDF: { unit: 'in', format: [25.4,25.4], orientation: 'portrait' }
            }
            this.$axios.post(this.$API_URL + this.dataUrl, this.header)
                .then((response) => {
                    this.reportTbl = {
                        header: response.data.data.header,
                        content: response.data.data.content,
                        title: response.data.data.title,
                    };
                    this.showReport = true;
                    this.$emit('unblock-ui');
                })
                .catch(function (error) {
                    self.showError("Error get data", error);
            });
           
        },
        // showError() {
        //     this.loadingReport = !this.loadingReport;
        // },
        showError(summary, message) {
            this.$emit("unblock-ui");
            this.$toast.add({
                severity: "error",
                summary: summary,
                detail: message,
                life: 3000,
            });
        },
        closeReport() {
            this.showReport = false;
        },
    },
    mounted() {},
     computed : {
           years() {
            let years = [];
            for (var i = 2022; i <= new Date().getFullYear(); i++) {
                years.push({ value: i.toString() })
            }
            return years;
        },
    }
};
</script>
