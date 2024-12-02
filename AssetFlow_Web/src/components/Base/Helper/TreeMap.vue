<template>
  <CustomSidebar
    v-model:visible="isVisible"
    position="bottom"
    class="p-1"
    exitLabel="Close"
    title="Storage Visual"
    @hide="onHide()"
  >
    <BlockUI :blocked="isLoading">
      <div style="min-height: 500px">
        <div id="tree-container"></div>
      </div>
    </BlockUI>
  </CustomSidebar>
</template>
<script>
export default {
  name: "TreeMap",
  props: {
    storageId: {
      type: Number,
      default: 0,
    },
    visibility: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    visibility(newValue, oldValue) {
      if (newValue) {
        this.getData();
      }
    },
  },
  data() {
    return {
      isLoading: true,
      isVisible: false,
      dataUrl: this.$API_URL + "storagelocation/visualization/",
      data: [
        {
          type: "treemap",
          // values:  [10, 14, 12, 10, 2, 6, 6, 1, 4],
          textinfo: "label23123",
          labels: [
            "Eve",
            "Cain",
            "Seth",
            "Enos",
            "Noam",
            "Abel",
            "Awan",
            "Enoch",
            "Azura",
          ],
          parents: [
            "",
            "Eve",
            "Eve",
            "Seth",
            "Seth",
            "Eve",
            "Eve",
            "Awan",
            "Eve",
          ],
        },
      ],
    };
  },
  methods: {
    getData() {
      var that = this;
      this.isLoading = true;
      this.isVisible = true;
      this.$axios
        .post(this.dataUrl + this.storageId)
        .then((response) => {
          var mData = response.data
          that.isLoading = false;
          if (response.data.success) {
            that.data = [JSON.parse(mData.data)];
            that.$plotly.newPlot("tree-container", that.data);
          } else {
            that.isVisible = false;
            that.$parent.$toast.add({
              severity: "error",
              summary: "Error",
              detail: mData.message,
              life: 120000,
            });
          }
        })
        .catch(function (error) {
          that.isLoading = false;
          that.isVisible = false;
          that.$parent.$toast.add({
            severity: "error",
            summary: "Error",
            detail: error,
            life: 120000,
          });
        });
    },
    onHide() {
      this.$emit("update:visibility", false);
    },
  },
};
</script>
