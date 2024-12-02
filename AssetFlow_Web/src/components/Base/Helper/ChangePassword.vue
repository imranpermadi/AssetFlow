<template>
    <Dialog
        v-model:visible="isVisible"
        header="Change Password"
        @hide="hide()"
		@show="show()"
        :dismissable="false"
		:draggable="false"
		:modal="true"
		:style="{width: '30vw'}"
    >
        <span v-show="errMessage" class="p-error">{{ errMessage }}</span>

        <div class="mt-3">
            <FormField
            v-model="resetPassword"
            labelName="Current Passsword"
            fieldClass="col-6"
			labelClass="col-5"
			v-if="requireCurrentPassword"
        >
            <InputText
                type="password"
                v-model="this.resetPassword.currentPass"
                placeholder="Enter password"
            />
            <template #error>
                <small
                    v-if="v$.resetPassword.currentPass.$error"
                    class="p-error"
                    style="display: block"
                >
                    {{
                        v$.resetPassword.currentPass.$errors[0].$message.replace(
                            "Value",
                            "Password"
                        )
                    }}
                </small>
            </template>
        </FormField>
        <FormField
            v-model="resetPassword"
            labelName="New Password"
            fieldClass="col-6"
			labelClass="col-5"
        >
            <InputText
                type="password"
                v-model="this.resetPassword.pass"
                placeholder="Enter password"
            />
            <template #error>
                <small
                    v-if="v$.resetPassword.pass.$error"
                    class="p-error"
                    style="display: block"
                >
                    {{
                        v$.resetPassword.pass.$errors[0].$message.replace(
                            "Value",
                            "Password"
                        )
                    }}
                </small>
            </template>
        </FormField>
        <FormField
            v-model="resetPassword"
            labelName="Confirm New Password"
            fieldClass="col-6"
			labelClass="col-5"
        >
            <InputText
                type="password"
                v-model="this.resetPassword.confirmPass"
                placeholder="Confirm password"
            />
            <template #error>
                <small
                    v-if="v$.resetPassword.confirmPass.$error"
                    class="p-error"
                    style="display: block"
                >
                    {{
                        v$.resetPassword.confirmPass.$errors[0].$message.replace(
                            "other value",
                            "Password"
                        )
                    }}
                </small>
            </template>
        </FormField>
        </div>
		

		<template #footer>
			 <Button
                type="button"
                icon="pi pi-save"
                class="p-button p-button-success"
                @click="checkPass()"
                label="Save"
            />
		</template>
    </Dialog>
</template>
<script>
import useVuelidate from "@vuelidate/core";
import { required, minLength, sameAs, requiredIf } from "@vuelidate/validators";

export default {
    data() {
        return {
            v$: useVuelidate(),
            errMessage: null,
            url: this.$API_URL + "auth/change_password",
            modelPassword: null,
            resetPassword: {
				currentPass: null,
                pass: null,
                confirmPass: null,
            },
        };
    },
    validations() {
        return {
            resetPassword: {
                pass: { required, minLength: minLength(8) },
                confirmPass: {
                    required,
                    sameAs: sameAs(this.resetPassword.pass),
                },
				currentPass: { required: requiredIf(this.requireCurrentPassword), minLength: minLength(8)}
            },
        };
    },
    props: {
        model: {
            type: Object,
            default: null,
        },
        userId: {
            type: String,
            default: null,
        },
        isVisible: {
            type: Boolean,
            default: false,
        },
		requireCurrentPassword:{
			type: Boolean,
			default:false
		},
		initialErrorMessage:{
			type: String,
			default: ""
		},
        changeFirstLogin:{
            type:Boolean,
            default: false
        }
    },
    methods: {
		show(){
			if(this.initialErrorMessage){
				this.errMessage = this.initialErrorMessage;
			}
		},
        hide(changeSuccess, message) {
            this.resetPassword = {
                pass: null,
                confirmPass: null,
            };
            this.v$.$reset();
            this.errMessage = null;
            this.$emit("update:isVisible", false);
			this.$emit("hide", {
                success: changeSuccess,
                message: message
            });
        },
        passwordClass(data) {
            if (data.showPassword == undefined) {
                data.showPassword = "pi pi-eye";
            }
            data.showPassword =
                data.showPassword == "pi pi-eye-slash" ||
                data.showPassword == undefined
                    ? "pi pi-eye"
                    : "pi pi-eye-slash";
            return data.showPassword;
        },
        passwordType(data) {
            return data == "pi pi-eye" ? "pi pi-eye-slash" : "pi pi-eye";
        },
        checkPass() {
            this.v$.$validate();

			var self = this;

            if (!this.v$.$error) {
                this.$emit("block-ui");
                this.v$.$reset();

				if(this.model){
					this.model.Password = this.resetPassword.pass;
				}

                this.$axios
                    .post(this.url, {
                        username: this.userId,
                        currentPass: this.requireCurrentPassword ? this.resetPassword.currentPass : "",
                        newPass: this.resetPassword.pass,
                        changeFirstLogin: this.changeFirstLogin
                    })
                    .then((response) => {
                        if (response.data.success) {
                            self.$emit("update:model", this.model);
                            self.hide(true, response.data.message);
                        } else {
                            this.errMessage = "Failed: " + response.data.message;
                        }
                        this.$emit("unblock-ui");
                    })
                    .catch((err) => {
                        this.$emit("unblock-ui");
                        self.$toast.add({
                            severity: "error",
                            summary: "Error",
                            detail: err.message,
                            life: 120000,
                        });
                    });
            }
        },
    },
    emits: ["hide", "block-ui", "unblock-ui"],
};
</script>
