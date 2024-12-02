<template>
	<div class="layout-topbar" :style="{'background-color': $BAR_BG_COLOR + ' !important'}">
		<router-link to="/" class="layout-topbar-logo">
			<!-- <img alt="Logo" :src="darkTheme ? require(`@/assets/layout/images/wilmar-logo.png@`) : '@/assets/layout/images/wilmar-logo.png'" /> -->
			<img alt="Logo" :src="'/assets/layout/images/wilmar-logo.png'" style="mix-blend-mode: multiply;" />
			<span>{{ $APP_SHORT_NAME }}</span>
			<sup style="font-size: 10pt; font-weight: bold;color: orange;">&nbsp;&nbsp;[{{ this.$store.getters.getServer }}]</sup>
		</router-link>
		<button class="p-link layout-menu-button layout-topbar-button" @click="onMenuToggle"
			:style="{'color': $BAR_TEXT_COLOR + ' !important'}">
			<i class="pi pi-bars"></i>
		</button>

		<button class="p-link layout-topbar-menu-button layout-topbar-button" v-styleclass="{ selector: '@next', enterClass: 'hidden', enterActiveClass: 'scalein', 
			leaveToClass: 'hidden', leaveActiveClass: 'fadeout', hideOnOutsideClick: true}"
			:style="{'color': $BAR_TEXT_COLOR + ' !important'}">
			<i class="pi pi-ellipsis-v"></i>
		</button>
		<ul class="layout-topbar-menu hidden lg:flex origin-top">
			<!-- <li>
				<button class="p-link layout-topbar-button" @click="readNotif">
					<i class="pi pi-bell p-text-secondary" v-badge="12"></i>
					<span>Notifications</span>
				</button>
			</li> -->
			<li>
				<div class="layout-topbar-button topbar-display-name"
					:style="{'color': $BAR_TEXT_COLOR + ' !important'}">
					{{this.username}}
					<br />
					{{this.displayName}}
					<Divider layout="vertical" />
				</div>
			</li>
			<!-- <li>
				<button class="p-link layout-topbar-button" @click="toggleChangelog">
					<i class="pi pi-list"></i>
					<span style="display:none">Changelog</span>
				</button>
			</li> -->
			<li>
				<button class="p-link layout-topbar-button" @click="toggleLanguage">
					<i class="pi pi-globe p-text-secondary" v-badge="badgeLang"></i>
					<span>Language</span>

					<Menu ref="languageMenu" :model="languageItems" :popup="true" />
				</button>
			</li>
			<li>
				<button class="p-link layout-topbar-button" @click="toggle"
					:style="{'color': $BAR_TEXT_COLOR + ' !important'}">
					<i class="pi pi-cog"></i>
					<span style="display:none">Settings</span>
					<Menu ref="menu" :model="items" :popup="true" />
				</button>
			</li>
		</ul>
	</div>

	<ChangePassword v-if="changePassword" :isVisible="changePassword" :userId="username" :requireCurrentPassword="true"
		@hide="hideChangePasswordDialog" @block-ui="blockUI" @unblock-ui="unblockUI" />


</template>

<script>
export default {
	data() {
		return {
			username: this.$store.getters.getUsername,
			changePassword: false,
		}
	},	
    methods: {
		hideChangePasswordDialog(event){
			this.changePassword = false;
			this.errormessage = "";

			if(event && event.success){
				this.$toast.add({ severity: 'success', summary: "Success", detail: event.message, life: 120000});
			}
		},
		blockUI() {
			this.$emit("block-ui");
		},
		unblockUI() {
			this.$emit("unblock-ui");
		},
        onMenuToggle(event) {
			this.$emit('menu-toggle', event);
		},
		toggle(event) {
			this.$refs.menu.toggle(event);
		},
		toggleLanguage(event) {
			this.$refs.languageMenu.toggle(event);
		},
		logout() {
			//this.$emit('block-ui');
			
			// this.$axios.defaults.headers.common["Authorization"] = null;
			// this.$store.dispatch("logout");

			this.$router.push({ name: "Logout", params: { sessionExpired: "N"} });
		},
		changeLanguage(event){
			var lang = event.item.id;
			var self = this;
			self.$emit("block-ui");
			self.blockUI();
			this.$axios.post(this.$API_URL + "util/change_language", {language: lang})
			.then((response) => {
				
				self.$root.$i18n.locale = lang;
				self.$axios.defaults.headers.common["language"] = lang;
				self.$store.dispatch("language", lang);
				self.$emit("unblock-ui");
				// console.log(response);
			})
			.catch(function(err){
				//console.log(err);
				self.$emit("unblock-ui");
			})

		},
		readNotif(){
			//Send API to Flag all Read
		},
		toggleChangelog(){
			this.$emit('toggle-changelog');
		}
    },
	computed: {
		darkTheme() {
			return this.$appState.theme.startsWith('saga');
		},
		lang(){
			return this.$store.getters.getLanguage;
		},
		badgeLang(){
			return (this.$store.getters.getLanguage).toUpperCase();
		},
		username(){
			return this.$store.getters.getUsername;
		},
		displayName(){
			return this.$store.getters.getFullname;
		},
		languageItems(){
			var languages = this.$store.getters.getLanguages;

			var h = [];

			for(var i=0; i< languages.length;i++){
				var language = languages[i];
				h.push({
					label: language.text,
					icon: this.lang == language.id ? 'pi pi-check' : "",
					command: (event)=>{
						this.changeLanguage(event);
					},
					id: language.id
				})
			}

			return h;
		},
		items(){
			var dataItems = [
				{
					label: 'Logout',
					icon: 'pi pi-fw pi-power-off',
					command: () => {
						this.logout();
					}
				}
			];

			if(!this.$store.getters.getUseAD){
				dataItems.unshift(
					{
						label: 'Change Password',
						icon: 'pi pi-fw pi-lock',
						command: () =>{
							this.changePassword = true;
						}
					}
				)
			}

			return dataItems;
		}
	}
}
</script>
