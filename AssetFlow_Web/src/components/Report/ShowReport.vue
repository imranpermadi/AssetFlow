<template>
    <CustomSidebar v-model:visible="this.visible" exitLabel="Close" position="full"
        @hide="$emit('close')" v-if="reportTbl">
        <template #actions>
            <Button label="PDF" icon="pi pi-file-pdf pi-fw" class="p-button-danger mr-2" @click="exportToPDF" />
            <Button label="Excel" icon="pi pi-file-excel pi-fw" class="p-button-success mr-1" @click="tableToExcel()" />
        </template>
         <div id="app" ref="document">
            <table v-if="reportTbl.header">
                <tbody v-html="reportTbl.header"></tbody>
            </table>
        <div v-html="reportTbl.content" class="tableFixHead"/>
        </div>
    </CustomSidebar>
</template>
<script>
import html2pdf from 'html2pdf.js'

export default {
    emits: ["close"],
    props: {
        reportTbl: {
            type: Object,
            default: null,
        },
        isVisible: {
            type: Boolean,
            default: true,
        },
        configHtml2Pdf :{
            type: Object,
            default : null
        }
    },
    data() {
        return {
            visible: false
        };
    },
    watch: {
        isVisible(newVal) {
            this.visible = newVal;
        }
    },
    methods: {
        tableToExcel() {
            var uri = "data:application/vnd.ms-excel;base64,",
                template =
                    '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>',
                base64 = function (s) {
                    return window.btoa(unescape(encodeURIComponent(s)));
                },
                format = function (s, c) {
                    return s.replace(/{(\w+)}/g, function (m, p) {
                        return c[p];
                    });
                };
            var ctx = {
                worksheet: "Default" || "Worksheet",
                table: this.reportTbl.header + this.reportTbl.content,
            };
            var currentDate = this.$moment().format("DD-MM-yyyy_hh-mm");
            var fileName = this.reportTbl.title + "_" + currentDate;
            const link = document.createElement("a");
            link.href = uri + base64(format(template, ctx));
            link.download = fileName;
            link.click();
        },
        exportToPDF() {
           html2pdf(this.$refs.document, this.configHtml2Pdf)
        },
        showError() {
        },
    },
    mounted() {
        this.visible = this.isVisible
    },
};
</script>

<style type="text/css" >
.dt-body-right {
    text-align: right;
}

.bgDone {
    background-color: rgba(130, 205, 55, 0.39);
}

.bgSigned {
    background-color: rgba(205, 205, 55, 0.39);
}

.bgPending {
    background-color: rgba(205, 55, 55, 0.39);
}

.bgPendingImport {
    background-color: rgba(55, 130, 205, 0.39);
}

.bgDelete {
    background-color: #808080;
}

.text {
    mso-number-format: "'";
    /*force text*/
}

.noLabel {
    display: none;
}

.tableFixHead {
    overflow-y: auto;
   height: 70vh;
    /* top: -1px !important; */
}

.tableFixHead thead th {
    background: #ddd;
    position: sticky;
    top: 0;
}

.table-container {
    width: 100%;
    overflow-x: scroll;
    overflow-y: scroll;
}

/* Just common table stuff. */
table {
    /*border-collapse: collapse;*/
    width: 100%;
}

table td {
    border: 10px solid white;
    border-right: 0;
    border-left: 0;
}

.table-scroll {
    width: 100%;
    z-index: 1;
    margin: auto;
    overflow: auto;
}

.table-scroll table {
    width: 98%;
    min-width: 1280px;
    margin: auto;
    border-collapse: separate;
    border-spacing: 0;
}

.table-wrap {
    position: relative;
}

.table-scroll tfoot th {
    padding: 5px 10px;
    border: 1px solid #000;
    background: #fff;
    vertical-align: top;
}

.table-scroll tfoot,
.table-scroll tfoot th {
    position: -webkit-sticky;
    position: sticky;
    bottom: 0;
    background-color: white;
    color: #000;
    z-index: 4;
}
</style>
