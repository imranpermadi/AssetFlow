<template>
    <form id="mainForm" action="#" method="post">
        <Card>
            <template #title>
                <Toolbar>
                    <template #left>
                        <div class="p-card-title">Document Movement Report</div>
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
                            :params="{ group: $LOOKUP_GROUP.MOVEMENT_REPORT_TYPE }" />
                            <small v-if="header && !header.Report_Type && submitted && mode != this.$FORM_MODE_DELETE"
                                class="p-error">{{ $t('is_required').replace('Value', $t('report_type')) }}
                            </small>
                        </FormField>
                        
                        <FormField fieldName="Company_Code" labelName="Company" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                                <CustomMultiSelect class="p-inputtext-sm" v-model="header.Company_Id"
                                    placeholder="Please Select"
                                    :url="this.$API_URL + 'list/company_id'" :minLength="0" :params="{}"
                                    :filter="true" @change="changeCompany($event)"/>
                        </FormField>

                        <FormField fieldName="Location_Code" labelName="Location" v-model="header"
                        fieldClass="col-3" labelClass="col-2">
                            <CustomMultiSelect class="p-inputtext-sm" v-model="header.Location_Id"
                                placeholder="Please Select"
                                :url="this.$API_URL + 'list/location_id'" :minLength="0" :params="{}"
                                :filter="true" @change="changeLocation($event)"/>
                        </FormField>
                    
                        <FormField fieldName="Relocation No" labelName="Relocation No" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="Packing_No" :filter="true"  
                            v-model="header.Relocation_No" required="true" 
                            placeholder="Please Select"
                            :url="this.$API_URL + 'list/relocation_no'" :minLength="0" :params="{}" />
                        </FormField>

                        <FormField fieldName="From_Packing_No" labelName="From Packing No" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="From_Packing_No" :filter="true"  
                            v-model="header.From_Packing_No" required="true" 
                            placeholder="Please Select"
                            :url="this.$API_URL + 'list/packing_detail_id'" :minLength="0" :params="{}" />
                        </FormField>

                        <FormField fieldName="To_Packing_No" labelName="Relocate to Packing No" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="To_Packing_No" :filter="true"  
                            v-model="header.To_Packing_No" required="true" 
                            placeholder="Please Select"
                            :url="this.$API_URL + 'list/packing_detail_id'" :minLength="0" :params="{}" />
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

                        <FormField fieldName="Remarks" labelName="Remarks" v-model="header"
                            fieldClass="col-3" labelClass="col-2">
                            <CustomSelect name="Remarks" :filter="true"  
                            v-model="header.Remarks" required="true" 
                            placeholder="Please Select"
                            :disabled="header.Report_Type == null"
                            :url="this.$API_URL + 'list/remarks_document_movement'" :minLength="0" :params="{report_type:header.Report_Type}" />
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
        ShowReport,
        FormField,
    },
    data() {
        return {
            test: 0,
            mode: "",
            dataUrl: "Report/generate_movement_report",
            reportTbl: null,
            showReport: false,
            loadingReport: false,
            mergedOptions: [
                { label: "Y", value: 1 },
                { label: "N", value: 0 },
            ],
            header: {
                Report_Type: null,
                Company_Code: [],
                Location_Code: [],
                From_Packing_No: null,
                To_Packing_No: null,
                Relocation_No: null,
                Period_From: null,
                Period_To: null,
                Document_Category : null,
                Remarks: null,
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
