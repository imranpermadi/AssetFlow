<template>
    <form id="mainForm" action="#" method="post">
      <Card>
        <template #title>
          <Toolbar>
            <template #left>
              <div class="p-card-title">[{{ mode }}] Company</div>
              <div
                style="margin-left: 10px"
                v-if="mode != this.$FORM_MODE_CREATE"
              >
                <Button
                  type="button"
                  icon="pi pi-info"
                  @click="toggle"
                  aria:haspopup="true"
                  aria-controls="overlay_panel"
                  class="p-button-rounded p-button-outlined p-button-sm"
                />
  
                <OverlayPanel
                  ref="op"
                  appendTo="body"
                  :showCloseIcon="true"
                  id="overlay_panel"
                  style="width: 550px"
                  :breakpoints="{ '960px': '80vw' }"
                >
                  <div class="grid field">
                    <div class="row">
                      <i class="pi pi-plus-circle" />
                      {{ this.$formatDateTime(header.Created_Date) }}
                    </div>
                    <div class="row">&nbsp;by {{ header.Created_By }}</div>
                  </div>
                  <div class="grid field" v-if="header.Edited_Date">
                    <div class="row">
                      <i class="pi pi-pencil" />
                      {{ this.$formatDateTime(header.Edited_Date) }}
                    </div>
                    <div class="row">&nbsp;by {{ header.Edited_By }}</div>
                  </div>
                  <div class="grid field" v-if="header.Deleted_Date">
                    <div class="row">
                      <i class="pi pi-trash" />
                      {{ this.$formatDateTime(header.Deleted_Date) }}
                    </div>
                    <div class="row">&nbsp;by {{ header.Deleted_By }}</div>
                  </div>
                </OverlayPanel>
              </div>
            </template>
  
            <template #right>
              <Button
                icon="pi pi-chevron-left"
                label="Back"
                class="p-button mr-2"
                @click="backToIndex"
              />
              <Button
                icon="pi pi-save"
                label="Save"
                class="p-button-success"
                @click="confirm($event)"
                v-if="
                  mode != this.$FORM_MODE_DELETE && mode != this.$FORM_MODE_VIEW
                "
              />
              <Button
                icon="pi pi-trash"
                label="Delete"
                class="p-button-danger"
                @click="confirm($event)"
                v-if="mode == this.$FORM_MODE_DELETE"
              />
              <ConfirmPopup></ConfirmPopup>
            </template>
          </Toolbar>
        </template>
        <template #content>
          <div class="p-inputtext-sm p-fluid">
            <div class="grid">
              <div class="col-6">
                <div class="field grid">
                  <label for="Code" class="col-4">{{ $t("comp_code") }}</label>
                  <div class="col-8 md-8">
                    <InputText
                      name="Code"
                      v-model="header.Code"
                      type="text"
                      :placeholder="$t('comp_code')"
                      v-if="header"
                      :class="{ 'p-invalid': submitted && !header.Code }"
                      :disabled="
                        mode != this.$FORM_MODE_CREATE"
                      :readonly="mode != this.$FORM_MODE_CREATE"
                    />
                    <small
                      v-if="
                        header &&
                        !header.Code &&
                        submitted &&
                        mode != this.$FORM_MODE_DELETE
                      "
                      class="p-error"
                      >{{ $t("is_required").replace("Value", $t("comp_code")) }}
                    </small>
                    <Skeleton v-else-if="!header" />
                  </div>
                </div>
                <div class="field grid">
                  <label for="Code_SAP" class="col-4">{{ $t("sap_cod") }}</label>
                  <div class="col-8 md-8">
                    <InputText
                      name="Code_SAP"
                      v-model="header.Code_SAP"
                      required="true"
                      type="text"
                      :placeholder="$t('sap_cod')"
                      v-if="header"
                      :class="{ 'p-invalid': submitted && !header.Code_SAP }"
                      :readonly="mode != this.$FORM_MODE_CREATE"
                      :disabled="
                        mode != this.$FORM_MODE_CREATE"
                    />
                     <small
                      v-if="
                        header &&
                        !header.Code_SAP &&
                        submitted &&
                        mode != this.$FORM_MODE_DELETE
                      "
                      class="p-error"
                      >{{ $t("is_required").replace("Value", $t("sap_cod")) }}
                    </small>
                    <Skeleton v-else-if="!header" />
                  </div>
                </div>
                <div class="field grid">
                  <label for="Description" class="col-4">{{ $t("desc") }}</label>
                  <div class="col-8 md-8">
                    <InputText
                      name="Description"
                      v-model="header.Description"
                      required="true"
                      type="text"
                      :placeholder="$t('desc')"
                      v-if="header"
                      :class="{ 'p-invalid': submitted && !header.Description }"
                      :disabled="
                        mode != this.$FORM_MODE_CREATE &&
                        mode != this.$FORM_MODE_EDIT
                      "
                    />
                       <small
                      v-if="
                        header &&
                        !header.Description &&
                        submitted &&
                        mode != this.$FORM_MODE_DELETE
                      "
                      class="p-error"
                      >{{ $t("is_required").replace("Value", $t("desc")) }}
                    </small>
                    <Skeleton v-else-if="!header" />
                  </div>
                </div>
              </div>
  
                <div class="col-12">
                              <DataTable v-if="header" :loading="!header" :value="header.Locations ? header.Locations : null"
                                  responsiveLayout="scroll" class="p-datatable-sm editable-cells-table">
                                  <Column :exportable="false" style="width:30px">
                                      <template #header>
                                          <Button label="" icon="pi pi-plus" class="p-button-primary p-mr-2 p-button-sm"
                                              @click="addDetail()"
                                              :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT" />
                                      </template>
                                      <template #body="slotProps">
                                          <Button icon="pi pi-minus" class="p-button-danger p-mr-2 p-button-sm"
                                              @click="deleteDetail(slotProps)"
                                              :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                                              v-show="slotProps.data.mode != 'Delete'" />
                                      </template>
                                  </Column>
                                    <Column field="Location_Id" :header="$t('location')" Key="Location_Id">
                                      <template #body="slotProps">
                                          <div class="p-fluid">
                                              <CustomSelect name="Location_Id" :filter="true"
                                                  v-model="slotProps.data[slotProps.column.props.field]" required="true"
                                                  placeholder="- Please select -" @change="onCellEditComplete(slotProps)"
                                                  :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                                                  v-show="slotProps.data.mode != 'Delete'"
                                                  :url="this.$API_URL + 'list/location_id'" :minLength="0" :params="{}" />
                                          </div>
                                      </template>
                                  </Column>
                                  <Column field="Address" :header="$t('address')" Key="Release_As">
                                      <template #body="slotProps">
                                          <div class="p-fluid">
                                              <InputText 
                                                  v-model="slotProps.data[slotProps.column.props.field]" required="true"
                                                  :placeholder="$t('address')" @change="onCellEditComplete(slotProps)"
                                                  :disabled="mode != this.$FORM_MODE_CREATE && mode != this.$FORM_MODE_EDIT"
                                                  v-show="slotProps.data.mode != 'Delete'" />
                                          </div>
                                      </template>
                                  </Column>
                              </DataTable>
                          </div>
            </div>
          </div>
        </template>
      </Card>
    </form>
  </template>
  
  
  <script>
  export default {
    name: "Editor",
    data() {
      return {
        indexName: "CompanyIndex",
        url: this.$API_URL + "company/",
        header: null,
        detail: null,
        mode: "",
        submitted: false,
        id: 0,
        formEditable: true,
      };
    },
    methods: {
      getData() {
        var self = this;
        this.$axios
          .get(this.url + this.id + "/" + this.mode)
          .then((response) => {
            self.header = response.data.data.header;
            self.header.mode = self.mode;
            self.detail = response.data.data.location;       
          })
          .catch(function (error) {
            alert(error);
          });
      },
      saveData() {
        this.$emit("block-ui");
        var self = this;
        this.$axios
          .post(this.url, this.header)
          .then((response) => {
            if (response.data.success) {
              if (this.mode == this.$FORM_MODE_DELETE) {
                self.$router.push({
                  name: self.indexName,
                  params: {
                    showToast: true,
                    severity: "success",
                    summary: "Data Deleted",
                    detail:
                      "Company " +
                      self.header.Code +
                      " has been deleted successfully",
                    life: 3000,
                  },
                });
              } else {
                self.$router.push({
                  name: self.indexName,
                  params: {
                    showToast: true,
                    severity: "success",
                    summary: "Data Saved",
                    detail:
                      "Company " +
                      self.header.Code +
                      " has been saved successfully",
                    life: 3000,
                  },
                });
              }
            } else {
              self.showError("Error Saving Data", response.data.message);
            }
          })
          .catch(function (error) {
            self.showError("Error Saving Data", error);
          });
      },
      showError(summary, message) {
        this.$emit("unblock-ui");
        this.$toast.add({
          severity: "error",
          summary: summary,
          detail: message,
          life: 3000,
        });
      },
      backToIndex() {
        this.$router.push({ path: "/Company/Index" });
      },
      toggle(event) {
        this.$refs.op.toggle(event);
      },
      confirm(event) {
        this.submitted = true;
        if (
          !this.header.Code_SAP ||
          !this.header.Code ||
          !this.header.Description
        ) {
          return;
        } 
        else if(this.header.Details == 0){
           this.showError("Error Saving Data", "Must have at least one detail");
        }
        else {
          this.$confirm.require({
            target: event.currentTarget,
            message: "Are you sure?",
            icon: "pi pi-exclamation-triangle",
            accept: () => {
              this.saveData();
            },
            reject: () => {},
          });
        }
      },
      hideRow(data) {
        return data.mode == this.$FORM_MODE_DELETE ? "row-hidden" : null;
      },
      addDetail() {
        var newDetail = this.detail;
        newDetail.mode = this.$FORM_MODE_CREATE;
        var det = { ...newDetail };
  
        this.header.Locations.push(det);
      },
      deleteDetail(event) {
        if (event) {
          var detail = this.header.Locations[event.index];
          if (detail.mode == this.$FORM_MODE_CREATE)
            this.header.Locations.splice(this.header.Locations.indexOf(detail), 1);
          else detail.mode = this.$FORM_MODE_DELETE;
        }
      },
      onCellEditComplete(event) {
        this.editDetail(event.index);
      },
      editDetail(index) {
        if (this.header && this.header.Details) {
          let mode = this.header.Details[index].mode;
          if (mode == this.$FORM_MODE_UNCHANGED) {
            this.header.Details[index].mode = this.$FORM_MODE_EDIT;
          }
        }
      },
    },
    created() {
      this.mode = this.$route.query.mode;
      this.id = this.$route.query.id ?? 0;
  
      this.getData();
    },
  };
  </script>