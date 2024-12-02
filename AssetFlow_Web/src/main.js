import 'primevue/resources/primevue.min.css';
import 'primeflex/primeflex.css';
import 'primeicons/primeicons.css';
import 'prismjs/themes/prism-coy.css';
import './assets/styles/layout.scss';
// import './assets/demo/flags/flags.css';

import { createApp, reactive } from 'vue';
import router from './router';
import store from './store';

import moment from 'moment';
import axios from 'axios';
import constants from './constants.js';
import {Loader} from "@googlemaps/js-api-loader"
import plotly from 'plotly.js-dist'

import App from './App.vue';
import PrimeVue from 'primevue/config';
import AutoComplete from 'primevue/autocomplete';
import Accordion from 'primevue/accordion';
import AccordionTab from 'primevue/accordiontab';
import Avatar from 'primevue/avatar';
import AvatarGroup from 'primevue/avatargroup';
import Badge from 'primevue/badge';
import BadgeDirective from 'primevue/badgedirective';
import Button from 'primevue/button';
import Breadcrumb from 'primevue/breadcrumb';
import Calendar from 'primevue/calendar';
import Card from 'primevue/card';
import Carousel from 'primevue/carousel';
import Chart from 'primevue/chart';
import Checkbox from 'primevue/checkbox';
import Chip from 'primevue/chip';
import Chips from 'primevue/chips';
import ColorPicker from 'primevue/colorpicker';
import Column from 'primevue/column';
import ConfirmDialog from 'primevue/confirmdialog';
import ConfirmPopup from 'primevue/confirmpopup';
import ConfirmationService from 'primevue/confirmationservice';
import ContextMenu from 'primevue/contextmenu';
//import DataTable from 'primevue/datatable';
import DataView from 'primevue/dataview';
import DataViewLayoutOptions from 'primevue/dataviewlayoutoptions';
import Dialog from 'primevue/dialog';
import Divider from 'primevue/divider';
//import Dropdown from 'primevue/dropdown';
import Fieldset from 'primevue/fieldset';
import FileUpload from 'primevue/fileupload';
import InlineMessage from 'primevue/inlinemessage';
import Inplace from 'primevue/inplace';
import InputMask from 'primevue/inputmask';
//import InputNumber from 'primevue/inputnumber';
import InputSwitch from 'primevue/inputswitch';
// import InputText from 'primevue/inputtext';
import Knob from 'primevue/knob';
import Galleria from 'primevue/galleria';
import Listbox from 'primevue/listbox';
import MegaMenu from 'primevue/megamenu';
import Menu from 'primevue/menu';
import Menubar from 'primevue/menubar';
import Message from 'primevue/message';
import MultiSelect from 'primevue/multiselect';
import OrderList from 'primevue/orderlist';
import OrganizationChart from 'primevue/organizationchart';
import OverlayPanel from 'primevue/overlaypanel';
import Paginator from 'primevue/paginator';
import Panel from 'primevue/panel';
import PanelMenu from 'primevue/panelmenu';
import Password from 'primevue/password';
import PickList from 'primevue/picklist';
import ProgressSpinner from 'primevue/progressspinner';
import ProgressBar from 'primevue/progressbar';
import Rating from 'primevue/rating';
import RadioButton from 'primevue/radiobutton';
import Ripple from 'primevue/ripple';
import SelectButton from 'primevue/selectbutton';
import ScrollPanel from 'primevue/scrollpanel'
import ScrollTop from 'primevue/scrolltop';
import Slider from 'primevue/slider';
import Sidebar from 'primevue/sidebar';
import Skeleton from 'primevue/skeleton';
import SplitButton from 'primevue/splitbutton';
import Splitter from 'primevue/splitter';
import SplitterPanel from 'primevue/splitterpanel';
import Steps from 'primevue/steps';
import StyleClass from 'primevue/styleclass';
import TabMenu from 'primevue/tabmenu';
import Tag from 'primevue/tag';
import TieredMenu from 'primevue/tieredmenu';
// import Textarea from 'primevue/textarea';
import Timeline from 'primevue/timeline';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import Toolbar from 'primevue/toolbar';
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import Tooltip from 'primevue/tooltip';
import ToggleButton from 'primevue/togglebutton';
import Tree from 'primevue/tree';
import TreeTable from 'primevue/treetable';
import TriStateCheckbox from 'primevue/tristatecheckbox';

// import HtmlToPaper from './components/Base/Helper/VueHtmlToPaper';
import HtmlToPaper from 'vue-html-to-paper';
import ArrowNavigate from './components/Base/Helper/ArrowNavigate';
import CodeHighlight from './AppCodeHighlight';

const app = createApp(App);

app.config.globalProperties.$ArrowNavigate = ArrowNavigate;
app.config.globalProperties.$appState = reactive({ theme: 'saga-blue' });
app.config.globalProperties.$moment = moment;

app.use(PrimeVue, { ripple: true, inputStyle: 'outlined' });
app.use(ConfirmationService);
app.use(ToastService);


const options = {
    name: '_blank',
    specs: [
        'fullscreen=yes',
        'titlebar=yes',
        'scrollbars=yes'
    ],
    styles: [
        './assets/styles/_print.scss'
    ]
}
app.use(HtmlToPaper, options);

app.use(router);
app.use(store);



app.directive('tooltip', Tooltip);
app.directive('ripple', Ripple);
app.directive('code', CodeHighlight);
app.directive('badge', BadgeDirective);
app.directive('styleclass', StyleClass);

app.component('Accordion', Accordion);
app.component('AccordionTab', AccordionTab);
app.component('AutoComplete', AutoComplete);
app.component('Avatar', Avatar);
app.component('AvatarGroup', AvatarGroup);
app.component('Badge', Badge);
app.component('Breadcrumb', Breadcrumb);
app.component('Button', Button);
app.component('BlockUI', BlockUI);
app.component('Calendar', Calendar);
app.component('Card', Card);
app.component('Carousel', Carousel);
app.component('Chart', Chart);
app.component('Checkbox', Checkbox);
app.component('Chip', Chip);
app.component('Chips', Chips);
app.component('ColorPicker', ColorPicker);
app.component('Column', Column);
app.component('ConfirmDialog', ConfirmDialog);
app.component('ConfirmPopup', ConfirmPopup);
app.component('ContextMenu', ContextMenu);
// app.component('DataTable', DataTable);
app.component('DataView', DataView);
app.component('DataViewLayoutOptions', DataViewLayoutOptions);
app.component('Dialog', Dialog);
app.component('Divider', Divider);
app.component('Dropdown', Dropdown);
app.component('Fieldset', Fieldset);
app.component('FileUpload', FileUpload);
app.component('InlineMessage', InlineMessage);
app.component('Inplace', Inplace);
app.component('InputMask', InputMask);
//app.component('InputNumber', InputNumber);
app.component('InputSwitch', InputSwitch);
// app.component('InputText', InputText);
app.component('Galleria', Galleria);
app.component('Knob', Knob);
app.component('Listbox', Listbox);
app.component('MegaMenu', MegaMenu);
app.component('Menu', Menu);
app.component('Menubar', Menubar);
app.component('Message', Message);
app.component('MultiSelect', MultiSelect);
app.component('OrderList', OrderList);
app.component('OrganizationChart', OrganizationChart);
app.component('OverlayPanel', OverlayPanel);
app.component('Paginator', Paginator);
app.component('Panel', Panel);
app.component('PanelMenu', PanelMenu);
app.component('Password', Password);
app.component('PickList', PickList);
app.component('ProgressSpinner', ProgressSpinner);
app.component('ProgressBar', ProgressBar);
app.component('RadioButton', RadioButton);
app.component('Rating', Rating);
app.component('SelectButton', SelectButton);
app.component('ScrollPanel', ScrollPanel);
app.component('ScrollTop', ScrollTop);
app.component('Slider', Slider);
app.component('Sidebar', Sidebar);
app.component('Skeleton', Skeleton);
app.component('SplitButton', SplitButton);
app.component('Splitter', Splitter);
app.component('SplitterPanel', SplitterPanel);
app.component('Steps', Steps);
app.component('TabMenu', TabMenu);
app.component('TabView', TabView);
app.component('TabPanel', TabPanel);
app.component('Tag', Tag);
// app.component('Textarea', Textarea);
app.component('TieredMenu', TieredMenu);
app.component('Timeline', Timeline);
app.component('Toast', Toast);
app.component('Toolbar', Toolbar);
app.component('ToggleButton', ToggleButton);
app.component('Tree', Tree);
app.component('TreeTable', TreeTable);
app.component('TriStateCheckbox', TriStateCheckbox);


router.beforeEach(function (to, from, next) {
    window.scrollTo(0, 0);
    if (to.matched.some(record => record.meta.requiresAuth)) {
        // this route requires auth, check if logged in
        // if not, redirect to login page.
        if (!store.getters.isAuthenticated) {
            if (store.getters.isExpired) {
                store.state.token = null;
                next({ name: 'Logout', params: { sessionExpired: "Y" } });
            }
            else {
                store.state.token = null;
                next({ name: 'Login' });
            }            
        }
        else next(); // go to wherever I'm going
    } else {
        next(); // does not require auth, make sure to always call next()!
    }
});

if (store.state.token != null) {
    axios.defaults.headers.common["Authorization"] = store.state.token.Token;
    axios.defaults.headers.common["language"] = store.state.token.Language;
}

axios.interceptors.response.use(function (response) {
    if(response && response.data && response.data.success == false  && response.data.message == "Unauthorized")
    {
        router.push({ name: "Logout", params: { sessionExpired: "Y"} });
    }

    return response;
}, function (error) {
    if (error.response.status === 401) {
        // store.dispatch("logout");
        // axios.defaults.headers.common["Authorization"] = null;
        router.push({ name: "Logout", params: { sessionExpired: "Y"} });
    }
});


axios.interceptors.request.use(function (config) {
        // Do something before request is sent
        var auth = router.currentRoute._value.meta.requiresAuth;

        if(auth){
            
            if (!store.getters.isAuthenticated) {
                // store.dispatch("logout");
                // axios.defaults.headers.common["Authorization"] = null;
                router.push({ name: "Logout" });
            }
        }
        return config;
    }, function (error) {
    // Do something with request error
        return Promise.reject(error);
    });

app.config.globalProperties.$axios = axios;
constants.setGlobalProps(app, import.meta.env);
store.dispatch("role", app.config.globalProperties.$ROLES);

// app.config.globalProperties.$FORM_MODE_CREATE = "Create";
// app.config.globalProperties.$FORM_MODE_EDIT = "Edit";
// app.config.globalProperties.$FORM_MODE_DELETE = "Delete";
// app.config.globalProperties.$FORM_MODE_UNCHANGED = "Unchanged";
// app.config.globalProperties.$API_URL = process.env.VUE_APP_API_URL;

const loader = new Loader({
    apiKey:app.config.globalProperties.$maps_api_key,
    version:"weekly"
})

app.config.globalProperties.$maps_loader = loader
app.config.globalProperties.$formatDate = function (value) {
    return value ? moment(value).format("DD/MM/YYYY") : "";
};

app.config.globalProperties.$formatDateName = function (value) {
    return value ? moment(value).format("DD MMM YYYY") : "";
};

app.config.globalProperties.$formatDateTime = function (value) {
    return value ? moment(value).format("DD/MM/YYYY HH:mm") : "";
};
app.config.globalProperties.$formatTime = function (value) {
    return value ? moment(value).format("HH:mm") : "";
};
app.config.globalProperties.$formatNumber = function(value){
    return value ? value.toLocaleString("en-GB") : "0";
}
app.config.globalProperties.$formatBoolean = function(value){
    return value ? "Y" : "N";
}
app.config.globalProperties.$compareDateIsLater = function(dateFrom, dateTo) {
    return moment(dateFrom).isAfter(dateTo)
}


app.config.globalProperties.$showToast = function () {
    var params = this.$route.params;
    if (params.showToast) {
        var severity = params.severity;
        var summary = params.summary;
        var detail = params.detail;
        var life = params.life;

        this.$toast.add({ severity: severity, summary: summary, detail: detail, life: life});
    }
};

app.config.globalProperties.$showToastV2 = function (event) {
    this.$toast.add(event);
};

app.config.globalProperties.$plotly = plotly

import CustomSelect from '/src/components/Base/Helper/CustomSelect.vue';
import DatePicker from '/src/components/Base/Helper/DatePicker.vue';
import InputText from '/src/components/Base/Helper/InputText.vue';
import Textarea from '/src/components/Base/Helper/Textarea.vue';
import DataTable from '/src/components/Base/Helper/DataTable/DataTable.vue';
import ChangePassword from '/src/components/Base/Helper/ChangePassword.vue';
import CustomFileUpload from '/src/components/Base/Helper/CustomFileUpload.vue';
import CustomSidebar from '/src/components/Base/Helper/CustomSidebar.vue';
import CustomChips from '/src/components/Base/Helper/CustomChips.vue';
import BlockUI from '/src/components/Base/Helper/BlockUI.vue'; 
import FormField from '/src/components/Base/Helper/FormField.vue';
import YNSelect from '/src/components/Base/Helper/YNSelect.vue';
import YNCheckbox from '/src/components/Base/Helper/YNCheckbox.vue';
import CustomMultiSelect from '/src/components/Base/Helper/CustomMultiSelect.vue';
import CustomSelectButton from '/src/components/Base/Helper/CustomSelectButton.vue';
import InputNumber from '/src/components/Base/Helper/CustomInputNumber.vue';
import Dropdown from '/src/components/Base/Helper/Dropdown.vue';
import CustomDropdown from '/src/components/Base/Helper/CustomDropdown.vue';
import CustomProgressSpinner from '/src/components/Base/Helper/CustomProgressSpinner.vue';
import TreeMap from '/src/components/Base/Helper/TreeMap.vue';
import ChangeHistory from '/src/components/Base/Helper/ChangeHistory.vue';
import IndexBase from '/src/components/Base/Helper/IndexBase.vue';

app.component("CustomSelect", CustomSelect);
app.component("DatePicker", DatePicker);
app.component("InputText", InputText);
app.component("Textarea", Textarea);
app.component("CustomFileUpload", CustomFileUpload);
app.component("CustomSidebar", CustomSidebar);
app.component("DataTable", DataTable);
app.component("CustomChips", CustomChips);
app.component("BlockUI", BlockUI);
app.component("FormField", FormField);
app.component("YNSelect", YNSelect);
app.component("YNCheckbox", YNCheckbox);
app.component("CustomMultiSelect", CustomMultiSelect);
app.component("CustomSelectButton", CustomSelectButton);
app.component("ChangePassword",ChangePassword)
app.component("InputNumber", InputNumber);
app.component("Dropdown", Dropdown);
app.component("CustomDropdown",CustomDropdown);
app.component("CustomProgressSpinner",CustomProgressSpinner);
app.component("TreeMap",TreeMap);
app.component("ChangeHistory",ChangeHistory);
app.component("IndexBase",IndexBase);

//Create i18n dynamically

import i18n from './resources';
app.use(i18n);

axios.get(app.config.globalProperties.$UTIL_URL + "localization")
    .then((response) => {
        //console.log(response.data.data);
        store.dispatch("setLanguages", response.data.data.languages);
        //app.use(createI18n(JSON.parse(response.data.data.localizations)));

        for (const [key, value] of Object.entries(response.data.data.localizations)) {
           //console.log(key);
            //console.log({...value});
            i18n.global.setLocaleMessage(key, {...value});
            // console.log(self.$root.$i18n.availableLocales);
           
        }

        //console.log(Object.entries(response.data.data.localizations));

        var lang = store.getters.getLanguage;
        if(lang){
            i18n.locale = lang;
            i18n.global.locale = lang;
        }

        //this.$nextTick(() => {});
        //self.header.mode = self.mode;
    })
    .catch(function (error) {
        //alert(error);
    });




app.mount('#app');



