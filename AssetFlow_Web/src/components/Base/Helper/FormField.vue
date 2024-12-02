<template>
    <div class="field grid">
        <label :for="fieldName" :class="labelClass">{{labelName}} <span v-if="required" style="color:red">*</span></label>
        <div :class="fieldClass">
            <slot>

            </slot>
            <slot name="error">
                
            </slot>
            <!-- <InputText :name="fieldName" v-model="header.UoM" type="text" :placeholder="$t('uom')" v-if="header" :disabled="true" /> -->
            <Skeleton v-if="!modelValue && skeleton" />
        </div>
    </div>

</template>

<script>
    export default{
        props:{
            modelValue: null,
            labelClass:{
                type:String,
                default: "col-2"
            },
            fieldClass:{
                type:String,
                default: "col-4 md-4"
            },
            fieldName:{
                type:String,
                default: ""
            },
            labelName:{
                type:String,
                default:""
            },
            skeleton: {
                type:Boolean,
                default:true
            },
            required:{
                type: Boolean,
                default: false
            }
        },
        data(){
            return {
                dataModel: this.modelValue
            }
        }
    }

</script>