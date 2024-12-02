<template>
    <input :class="['p-inputtext p-component', {'p-filled': filled}]" :value="modelValue" @keydown="onKeyDown" @focus="onFocus" @input="onInput" />
</template>

<script>
export default {
    name: 'InputText',
    emits: ['update:modelValue'],
    props: {
        modelValue: null,
        autoUppercase:{
            type: Boolean,
            default: true
        }
    },
    methods: {
        onKeyDown(event){
            this.$ArrowNavigate.arrownavigate(event);
        },
        onFocus(event){
            event.target.select();
        },
        onInput(event) {
            var text = event.target.value;
            if(this.autoUppercase)
                text = event.target.value.toUpperCase();
            
            this.$emit('update:modelValue', text);
        }
    },
    computed: {
        filled() {
            return (this.modelValue != null && this.modelValue.toString().length > 0)
        }
    }
}
</script>
