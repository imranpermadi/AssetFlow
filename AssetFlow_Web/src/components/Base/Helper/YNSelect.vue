<template>
    <CDropDown v-model="dataModel" :filter="true" :useDefaultFilter="false" :options="optionsData" optionLabel="name" :placeholder="placeholder" :showClear="true" 
              @show="onOpen" @hide="onClose" @filter="onFilter" @change="onChange" :hasNext="hasNextPage"  >
        <template #value="slotProps">
            <div v-if="slotProps.value">
                <div>{{ slotProps.value.text }}</div>
            </div>
            <span v-else>
                {{slotProps.placeholder}}
            </span>
        </template>
        <template #option="slotProps">
            <div>{{ show(slotProps) }}</div>
        </template>
        <template #loadMore>
            <li v-show="hasNextPage" class="p-dropdown-empty-message" ref="load">
                <span name="more">Load more...</span>
            </li>
        </template>
    </CDropDown>
</template>
<script>
    import DropDown from './Dropdown.vue'

    export default {
        name: "CustomSelect",
        emits: [
            "change", "init"
        ],
        props: {
            modelValue: null,
            minLength: {
                type: Number,
                default: 3
            },
            url: String,
            params: Object,
            placeholder: {
                type: String,
                default: "Please select"
            },
            useBinary:{
                type: Boolean,
                default: false
            }
        },
        data() {
            return {
                dataModel:this.modelValue,
                optionsData:  [
                    {text: 'Y', id: this.useBinary ? '1' : 'Y'},
                    {text: 'N', id: this.useBinary ? '0' : 'N'}
                ],
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
        },
        mounted() {
            this.observer = new IntersectionObserver(this.infiniteScroll);
        },
        methods: {
            async onOpen() {
                this.page = 0;
                this.hasNextPage = false;
                this.searchFilter = "";

                if (this.hasNextPage) {
                    await this.$nextTick();
                    this.observer.observe(this.$refs.load);
                }

                if (this.minLength == 0) {
                    this.filterData("");
                }
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
            show(data) {
                return data.option.text;
            },
            onChange(event) {
                var value = event.value == null ? null : event.value.id;
                console.log(event);
                console.log(value);
                this.$emit("update:modelValue", value);
                this.$emit("change", value);
                console.log(this.dataModel);
            }

        },
        components: {
            'CDropDown': DropDown
        }
    }
</script>



