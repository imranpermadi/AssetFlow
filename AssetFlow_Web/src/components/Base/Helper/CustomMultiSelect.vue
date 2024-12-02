<template>
    <CMultiSelect 
        v-model="dataModel" 
        :options="optionsData" 
        optionLabel="text" 
        optionValue="id"
        display="chip" 
        :filter="true"
        :useDefaultFilter="false"
        @show="onOpen" @hide="onClose" @filter="onFilter" @change="onChange" :hasNext="hasNextPage">
            <template #value="slotProps">
                <div v-for="option of slotProps.text" class="p-multiselect-token" :key="option.id">
                    <span class="p-multiselect-token-label">{{option}}</span>
                    <span v-if="!disabled" class="p-multiselect-token-icon pi pi-times-circle" @click="removeValue(option)"></span>
                </div>
            </template>
            <template #option="slotProps">
                <div>
                    <div>{{slotProps.option.text}}</div>
                </div>
            </template>
            <template #loadMore>
                <li v-show="hasNextPage" class="p-dropdown-empty-message" ref="load">
                    <span name="more">Load more...</span>
                </li>
            </template>
    </CMultiSelect>
</template>
<script>
    import MultiSelect from './MultiSelect.vue'

    export default {
        name: "CustomMultiSelect",
        emits: [
            "change", "init"
        ],
        props: {
            modelValue: [],
            url: String,
            params: Object,
        },
        components: {
            'CMultiSelect': MultiSelect
        },
        data() {
            return {
                dataModel: this.modelValue,
                optionsData: [],
                observer: null,
                page: 0,
                searchFilter: "",
                hasNextPage: false
            }
        },
        watch: {
            hasNextPage(value) {
                if (value) {
                    if (this.hasNextPage) {
                        this.$nextTick(function () {
                            this.observer.observe(this.$refs.load);
                        });

                    }
                }
            },
            modelValue(value){
                this.dataModel = value;               
            }
        },
        mounted() {
            this.observer = new IntersectionObserver(this.infiniteScroll);
        },
        methods:{
            async onOpen() {
                this.page = 0;
                this.hasNextPage = false;
                this.searchFilter = "";
                this.optionsData = [];
                    
                if (this.hasNextPage) {
                    await this.$nextTick();
                    this.observer.observe(this.$refs.load);
                }

                 this.filterData("");
            },
            onClose() {
                this.observer.disconnect();
            },
            async infiniteScroll([{ isIntersecting, target }]) {
                if (isIntersecting) {
                    const ul = target.offsetParent;
                    const scrollTop = target.offsetParent.scrollTop;
                    this.page += 1;
                    await this.$nextTick();
                    ul.scrollTop = scrollTop;

                    this.filterData(this.searchFilter, true);
                }
            },
            onFilter(event) {
                this.searchFilter = event.value;
                this.page = 0;
                this.optionsData = [];
                this.filterData(this.searchFilter,false);
            },
            filterData(filter, more) {
                var filterParams = {
                    q: filter,
                    page: more == true ? this.page : 0
                };

                var fParams = filterParams;
                if (this.params != null) {
                    Object.assign(fParams, this.params);
                }

                this.$axios.get(this.url, { params: fParams }).then((response) => {
                    if (more) {
                        var appended = this.optionsData.concat(response.data.items);
                        this.optionsData = appended;
                        //this.optionsData.push(response.data.items);
                    }
                    else {
                        this.optionsData = response.data.items;
                    }

                    this.hasNextPage = response.data.items && response.data.items.length >= 10;
                });
            },
            show(data) {
                return data.option.text;
            },
            onChange(event) {
                var value = event.value == null ? null : event.value;
                this.$emit("update:modelValue", value);
                this.$emit("change", event.value);
            },
            removeValue(option){
                this.dataModel.splice(this.dataModel.indexOf(option), 1);
            }
        }
    }
</script>
<style #scoped>
    .p-multiselect-label-container {
        margin: 0;
        padding: 0;
        list-style-type: none;
        cursor: text;
        overflow: hidden;
        display: -webkit-box;
        display: -ms-flexbox;
        display: flex;
        -webkit-box-align: center;
        -ms-flex-align: center;
        align-items: center;
        -ms-flex-wrap: wrap;
        flex-wrap: wrap;
    }

    .p-multiselect-label  {
        font-size:0.875rem!important;
        cursor: pointer;
        overflow: hidden;
        white-space:normal;
        text-overflow: unset !important;
    }

    .p-multiselect.p-multiselect-chip .p-multiselect-token{
        /* padding-top:0rem!important;
        padding-bottom:0rem!important; */
        
        margin-top:0.1rem!important;
        margin-bottom:0.1rem!important;
    }
</style>
