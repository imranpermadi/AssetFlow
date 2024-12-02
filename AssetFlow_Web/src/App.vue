<template>
  <div v-if="!authenticated">
    <!-- <Login /> -->
    <router-view />
  </div>
  <div v-else-if="this.$route.name == 'NotFound' && authenticated">
    <router-view />
</div>
  <div
    v-else-if="authenticated"
    :class="containerClass"
    @click="onWrapperClick"
  >
    <Toast position="top-right" />

    <BlockUI :blocked="blocked">
      <AppTopBar @menu-toggle="onMenuToggle" @toggle-changelog="onToggleChangelog"/>
      <div class="layout-sidebar" @click="onSidebarClick">
        <AppMenu :model="menuItems" @menuitem-click="onMenuItemClick" />
      </div>

      <div class="layout-main-container">
        <BlockUI :blocked="blockedBody" :baseZIndex="1150">
          <div class="layout-main">
            <router-view @block-ui="blockUI" @unblock-ui="unblockUI" @block-body="blockBody" @unblock-body="unblockBody" v-if="!toggleChangelog" :menuItem="menuItem"/>
               <Changelog v-else @close="onToggleChangelog"/>
              
           
          </div>
          <AppFooter />
        </BlockUI>
      </div>

      <!-- <AppConfig :layoutMode="layoutMode" :layoutColorMode="layoutColorMode" @layout-change="onLayoutChange" @layout-color-change="onLayoutColorChange" /> -->
      <transition name="layout-mask">
        <div
          class="layout-mask p-component-overlay"
          v-if="mobileMenuActive"
        ></div>
      </transition>
    </BlockUI>
  </div>
</template>

<script>
import AppTopBar from "./AppTopbar.vue";
import AppMenu from "./AppMenu.vue";
// import AppConfig from './AppConfig.vue';
import AppFooter from "./AppFooter.vue";
import Login from "./components/Base/Auth/Login.vue";
import Changelog from "./Changelog.vue";

export default {
  data() {
    return {
      layoutMode: "static",
      layoutColorMode: "light",
      staticMenuInactive: false,
      overlayMenuActive: false,
      mobileMenuActive: false,
      blocked: false,
      blockedBody:false,
      menu: this.menuItems,
      toggleChangelog: false,
      changeLogData: [],
      menuItem: null
    };
  },
  mounted() {
    document.documentElement.style.fontSize = "13px";
    if(this.$store.getters.isExpired){
      this.$router.push({ name: "Logout", query: {sessionExpired: true} });
    }

    this.setLanguage();
  },
  watch: {
    $route() {
      document.title = this.$route.meta.menuTitle + " - " + this.$APP_SHORT_NAME;
      this.menuActive = false;
      //this.$toast.removeAllGroups();

    },
  },
  methods: {
    onWrapperClick() {
      if (!this.menuClick) {
        this.overlayMenuActive = false;
        this.mobileMenuActive = false;
      }

      this.menuClick = false;
    },
    onMenuToggle() {
      this.menuClick = true;

      if (this.isDesktop()) {
        if (this.layoutMode === "overlay") {
          if (this.mobileMenuActive === true) {
            this.overlayMenuActive = true;
          }

          this.overlayMenuActive = !this.overlayMenuActive;
          this.mobileMenuActive = false;
        } else if (this.layoutMode === "static") {
          this.staticMenuInactive = !this.staticMenuInactive;
        }
      } else {
        this.mobileMenuActive = !this.mobileMenuActive;
      }

      event.preventDefault();
    },
    onSidebarClick() {
      this.menuClick = true;
    },
    onMenuItemClick(event) {
      this.toggleChangelog = false;
      if (event.item && !event.item.items) {
        this.overlayMenuActive = false;
        this.mobileMenuActive = false;

        this.menuItem = event.item;
        console.log(this.menuItem);
      }
    },
    onLayoutChange(layoutMode) {
      this.layoutMode = layoutMode;
    },
    onLayoutColorChange(layoutColorMode) {
      this.layoutColorMode = layoutColorMode;
    },
    addClass(element, className) {
      if (element.classList) element.classList.add(className);
      else element.className += " " + className;
    },
    removeClass(element, className) {
      if (element.classList) element.classList.remove(className);
      else
        element.className = element.className.replace(
          new RegExp(
            "(^|\\b)" + className.split(" ").join("|") + "(\\b|$)",
            "gi"
          ),
          " "
        );
    },
    isDesktop() {
      return window.innerWidth >= 992;
    },
    isSidebarVisible() {
      if (this.isDesktop()) {
        if (this.layoutMode === "static") return !this.staticMenuInactive;
        else if (this.layoutMode === "overlay") return this.overlayMenuActive;
      }

      return true;
    },
    blockUI() {
      this.blocked = true;
    },
    unblockUI() {
      this.blocked = false;
    },
    blockBody(){
      this.blockedBody=true;
    },
    unblockBody(){
      this.blockedBody=false;
    },
    onToggleChangelog(){
      this.toggleChangelog = !this.toggleChangelog;
    },
    setLanguage(){
      var self = this;

      var language = this.$store.getters.getLanguage;
      if(language){
        self.$root.$i18n.locale = language;
				self.$axios.defaults.headers.common["language"] = language;
      }

    }

  },
  computed: {
    authenticated() {
      return this.$store ? this.$store.getters.isAuthenticated : false;
    },
    // menu() {
    //     return this.$store ? this.$store.getters.authorizedMenu : false;
    // },
    containerClass() {
      return [
        "layout-wrapper",
        {
          "layout-overlay": this.layoutMode === "overlay",
          "layout-static": this.layoutMode === "static",
          "layout-static-sidebar-inactive":
            this.staticMenuInactive && this.layoutMode === "static",
          "layout-overlay-sidebar-active":
            this.overlayMenuActive && this.layoutMode === "overlay",
          "layout-mobile-sidebar-active": this.mobileMenuActive,
          "p-input-filled": this.$primevue.config.inputStyle === "filled",
          "p-ripple-disabled": this.$primevue.config.ripple === false,
          "layout-theme-light": this.$appState.theme.startsWith("saga"),
        },
      ];
    },
    logo() {
      return this.layoutColorMode === "dark"
        ? "images/logo-white.svg"
        : "images/logo.svg";
    },
    menuItems(){
      var menu = this.$store.getters.getMenu;

      // menu.unshift(
			// 		{
			// 			label: 'Dashboard',
			// 			icon: 'pi pi-fw pi-chart-bar',
			// 			to: '/'
			// 		}
			// 	)

      return [
        {
          label: "Main Menu",
          items: menu
        },
      ];
    },
    languages() {
      return this.$i18n;
    }
  },
  beforeUpdate() {
    if (this.mobileMenuActive)
      this.addClass(document.body, "body-overflow-hidden");
    else this.removeClass(document.body, "body-overflow-hidden");
  },
  components: {
    AppTopBar: AppTopBar,
    AppMenu: AppMenu,
    // 'AppConfig': AppConfig,
    AppFooter: AppFooter,
    Login: Login,
    Changelog: Changelog
  },
  created(){
   

  }
};
</script>

<style lang="scss">
@import "./App.scss";

.p-disabled, .p-component:disabled{
  background:#eee;
  color: black;
}

.p-disabled, .p-component:disabled:hover{
  cursor: not-allowed;
}

.fade-enter-active,
.fade-leave-active{
  transition: opacity 0.1s ease;
}

.fade-enter-from,
.fade-leave-to{
  opacity: 0;
}
</style>
