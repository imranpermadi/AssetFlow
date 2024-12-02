<template>
    <div class="login-body"
        style="background-image:url(assets/layout/images/bg.jpg);font-size:14px;background-repeat: no-repeat;background-size: cover;background-position: center;">
        <div class=" login-panel ui-fluid">
            <div class="login-panel-header">
                <img src="/assets/layout/images/wilmar-logo.png" style="">
            </div>
            <div class="login-panel-content">
                <div class="flex flex-row flex-wrap grid p-fluid">
                    <div class="col-12" style="padding-top:0.2em!important">
                        <h1>{{ $APP_NAME }}</h1>
                        <h2>Welcome, login to start your session</h2>
                    </div>
                    <form @submit.prevent="handleSubmit(!v$.$invalid)" class="col-12" style="padding:0.5em!important">
                        <div class="flex flex-row flex-wrap">
                            <div class="col-12" style="padding-top:0!important">
                                <div v-show="submitted && errormessage">
                                    <Message severity="error" :closable="false">{{errormessage}}</Message>
                                </div>
                                <div v-show="changePasswordSuccess">
                                    <Message severity="success" :closable="false">{{changePasswordSuccess}}. Please
                                        login using new password</Message>
                                </div>

                                <div v-if="isExpired">
                                    <Message severity="error" :closable="true">Session Expired</Message>
                                </div>
                            </div>
                            <div class="col-12 ml-2 mr-2">
                                <span class="p-float-label">
                                    <!--<input class="p-inputtext p-component" id="Username" v-model="Username" type="text" style="width: 100%;">-->
                                    <InputText :autoUppercase="false" v-model="username" type="text" id="username"
                                        style="width:100%" :class="{'p-invalid':v$.username.$invalid && submitted}" />
                                    <label for="username">Username</label>
                                </span>
                                <small v-if="(v$.username.$invalid && submitted) || v$.username.$pending.$response"
                                    class="p-error">{{v$.username.required.$message.replace('Value',
                                    'Username')}}</small>
                            </div>
                            <div class="col-12 ml-2 mr-2 mt-1">
                                <span class="p-float-label">
                                    <!--<input class="p-inputtext p-component" id="Username" v-model="Username" type="text" style="width: 100%;">-->
                                    <Password :autoUppercase="false" v-model="password" id="password" style="width:100%"
                                        inputStyle="width:100%" :feedback="false"
                                        :class="{'p-invalid':v$.password.$invalid && submitted}" toggleMask />
                                    <label for="password">Use your AD (HRSystem) password</label>
                                </span>
                                <small v-if="(v$.password.$invalid && submitted) || v$.password.$pending.$response"
                                    class="p-error">{{v$.password.required.$message.replace('Value',
                                    'Password')}}</small>
                            </div>
                            <div class="col-12 ml-2 mr-2"
                                style="padding-top:0.3em!important;padding-bottom:0.3em!important" v-if="useCaptcha">
                                <div class="grid" style="vertical-align:middle!important;align-items:center">
                                    <div class="col-6" style="padding:0!important">
                                        <Captcha :value="captcha" @isValid="checkValidCaptcha" />
                                    </div>
                                    <div class="col-6" style="padding:0!important">
                                        <span class="p-float-label">
                                            <!--<input class="p-inputtext p-component" id="Username" v-model="Username" type="text" style="width: 100%;">-->
                                            <InputText :autoUppercase="false" v-model="captcha" type="text" id="captcha"
                                                style="width:100%"
                                                :class="{'p-invalid':v$.captcha.$invalid && submitted}" />
                                            <label for="captcha">Captcha</label>
                                        </span>
                                        <small
                                            v-if="(v$.captcha.$invalid && submitted) || v$.captcha.$pending.$response"
                                            class="p-error">{{v$.captcha.required.$message.replace('Value',
                                            'Captcha')}}</small>
                                    </div>
                                </div>
                            </div>

                            <div :class="'col-12 ml-2 mr-2 ' + (useCaptcha ? '' : 'mt-6')">
                                <div class="grid">
                                    <div class="col-5">
                                        <!-- <a href="assets/mobile/FAMS.apk" download="FAMS.apk" class="p-button p-button-success p-button-lg" style="text-align:center">
                                                <span class="p-button-label">Android App</span>
                                            </a> -->
                                    </div>
                                    <div class="col-2">

                                    </div>
                                    <div class="col-5" style="text-align: right;">
                                        <Button class="p-button p-component p-button-lg" type="submit"
                                            :disabled="processing">
                                            <span class="p-button-label" v-if="!processing"
                                                style="font-family:Arial">Login</span>
                                            <i class="pi pi-spin pi-spinner" style="margin:auto;font-size:1.5rem"
                                                v-else-if="processing"></i>
                                        </Button>
                                    </div>

                                </div>
                            </div>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    </div>

    <ChangePassword v-if="changePassword" :isVisible="changePassword" :userId="username" :requireCurrentPassword="true"
        :initialErrorMessage="errormessage" @hide="hideChangePasswordDialog" />

</template>

<script>
    import { ref } from "vue";
    import { required, requiredIf } from "@vuelidate/validators";
    import { useVuelidate } from "@vuelidate/core";
    import VueClientRecaptcha from 'vue-client-recaptcha';
    //import 'vue-client-recaptcha/dist/style.css';
    
    export default {
        components:{
            "Captcha": VueClientRecaptcha
        },
        name: "Login",
        setup: () => ({ v$: useVuelidate() }),
        props: {
            sessionExpired: {
                type: String,
                default: ""
            }
        },
        data() {
            return {
                username: "",
                password: "",
                submitted: false,
                processing: false,
                errormessage: "",
                changePassword: false,
                changePasswordSuccess: "",
                captcha: null,
                captchaValid: false,
                isExpired: this.sessionExpired == "Y"
            }
        },
        validations() {
            return {
                username: {
                    required
                },
                password: {
                    required
                },
                captcha:{
                    required: requiredIf(() => {
                        return this.useCaptcha
                    })
                }
            }
        },
        methods: {
            checkValidCaptcha(success){
                this.captchaValid = success;
            },
            hideChangePasswordDialog(event){
                this.changePassword = false;
                this.errormessage = "";

                if(event && event.success){
                    this.changePasswordSuccess = event.message;
                }
            },
            handleSubmit(isFormVaild) {
                this.submitted = true;
                if (!isFormVaild) {
                    return;
                }
                this.errormessage = "";

                if(this.useCaptcha){
                    if(!this.captchaValid){
                        this.errormessage = "Invalid/Incorrect Captcha";
                        return;
                    }
                }

                this.processing = true;
                this.messages = [];
                this.changePasswordSuccess = "";

                var self = this;

                this.$axios.post(this.$API_URL + "auth/login", { Username: this.username, Password: this.password }).then((response) => {
                    if(response){
                        if (response.data.success) {
                            console.log(response.data.data);
                            self.$axios.defaults.headers.common["Authorization"] = response.data.data.user_data.Token;
                            self.$store.dispatch("login", response.data.data);
                            self.$root.$i18n.locale = response.data.data.user_data.Language;
                            self.$router.push("/");
                        }
                        else{
                            if(response.data.message_code == "FIRST_LOGIN" || response.data.message_code == "CHANGE_PASS"){
                                self.changePassword = true;
                            }
                            else if(response.data.message_code == "CAPTCHA"){
                                self.$store.dispatch("setCaptchaCount");
                            }
                            
                            self.errormessage = response.data.message;
                            self.processing = false;
                        }
                        
                    }
                    else{
                        self.errormessage = response.message;
                        self.processing = false;
                    }
                }).catch((error) => {
                    self.errormessage = error;
                    self.processing = false;
                });
            },
            onDownloadMobile(file){
                var self = this;
                this.$axios.get(this.$API_URL + "auth/download_mobile/")
                    .then((response) => {
                        const url = window.URL.createObjectURL(new Blob([response.data]));
                        const link = document.createElement('a');
                        link.href = url;
                        link.setAttribute('download', file.name);
                        document.body.appendChild(link);
                        link.click();
                        
                    })
                    .catch(function (err) {
                        self.$parent.$toast.add({ severity: 'error', summary: "Error", detail: "File not found", life: 120000});
                        self.blocked = false;
                        //self.showAssignModal = false;
                    });
            },
        },
        mounted() {
            if(this.$store.getters.isAuthenticated)
                this.$router.push("/");

            this.useCaptcaCount = 0;
        },
        computed:{
            useCaptcha(){
                return this.$store.getters.getCaptchaCount >= 3;
            }
        }
    }
</script>

<style scoped>
    .p-progress-spinner-circle {
        stroke:white;
    }

</style>