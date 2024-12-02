// eslint-disable-next-line no-unused-vars

function getConfigFile(env){
    var configLocation = (env.NODE_ENV == "production" ? "" : "public/");
    return require("../" + configLocation + "config.json"); //require('../public/config.json'); 
}

function invertColor(hex, bw) {
    if (hex.indexOf('#') === 0) {
        hex = hex.slice(1);
    }
    // convert 3-digit hex to 6-digits.
    if (hex.length === 3) {
        hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    if (hex.length !== 6) {
        throw new Error('Invalid HEX color.');
    }
    var r = parseInt(hex.slice(0, 2), 16),
        g = parseInt(hex.slice(2, 4), 16),
        b = parseInt(hex.slice(4, 6), 16);
    if (bw) {
        // https://stackoverflow.com/a/3943023/112731
        return (r * 0.299 + g * 0.587 + b * 0.114) > 186
            ? '#000000'
            : '#FFFFFF';
    }
    // invert color components
    r = (255 - r).toString(16);
    g = (255 - g).toString(16);
    b = (255 - b).toString(16);
    // pad each with zeros and return
    return "#" + padZero(r) + padZero(g) + padZero(b);
}

function padZero(str, len) {
    len = len || 2;
    var zeros = new Array(len).join('0');
    return (zeros + str).slice(-len);
}

var constants = {
    async setGlobalProps(app, env){
        var props = app.config.globalProperties;

        //var url = "http://172.16.62.131:5000/api/";
        //var url= "http://localhost:5000/api/";
        //var url = "https://itmpapi-beta.wilmar.co.id/api/";

        //var config = getConfigFile(env);
        //var url = config.connection;

        var url = env.VITE_APP_API_URL;

        props.$APP_NAME = "Fixed Asset Management System";
        props.$APP_SHORT_NAME = "FAMS";
        props.$STATE_NAME = "pbv-state-";
        props.$DEFAULT_TIMER = 6000;
        props.$BAR_BG_COLOR = '#a0d2fa';
        props.$BAR_TEXT_COLOR = '#000000'; //invertColor(props.$BAR_BG_COLOR, true);


        props.$OK = "OK"
        props.$FORM_MODE_VIEW = "View"
        props.$FORM_MODE_CREATE = "Create"
        props.$FORM_MODE_EDIT = "Edit"
        props.$FORM_MODE_DELETE = "Delete"
        props.$FORM_MODE_UNCHANGED = "Unchanged"
        props.$API_URL = url
        props.$AUTH_URL = url + "auth/menu" //env.VUE_APP_API_URL + "auth/menu"
        props.$UTIL_URL = url + "util/" //env.VUE_APP_API_URL + "util/"s

        props.$LOOKUP_GROUP = {
            DOC_CATEGORY_TYPE: "DOC_CATEGORY_TYPE",
            DOC_CATEGORY_EXPIRY: "DOC_CATEGORY_EXPIRY",
            STORAGE_LOCATION_TYPE : "STORAGE_LOCATION_TYPE",
            REPORT_TYPE : "REPORT_TYPE",
            DOCUMENT_STATUS : "DOCUMENT_STATUS" ,
            MOVEMENT_REPORT_TYPE : "MOVEMENT_REPORT_TYPE",
            APPROVAL_AS_MATRIX : "APPROVAL_AS_MATRIX",
            VALIDITY_MATRIX : "VALIDITY_MATRIX",
            DOCUMENT_TRANSFER_TYPE : "DOCUMENT_TRANSFER_TYPE",

            LANGUAGE: "LANGUAGE",
            LOCALIZATION_TYPE: "LOCALIZATON_TYPE"
        }

        props.$LOCALIZATION_TYPE = {
            LABEL: "LABEL",
            RESOURCES: "RESOURCES"
        }

        props.$CATEGORY_STORAGE = {
            MAIN: "MAIN",
            DETAIL: "DETAIL"          
        }
    
        props.$APPROVAL_STATUS = {
            WAITING_FOR_APPROVAL : "WAITING FOR APPROVAL",
            APPROVED : "APPROVED",
            REJECTED : "REJECTED",
        }

        props.$TRANSACTION_TYPE = {
            ORDER: "ORDER",
            SHIPMENT: "SHIPMENT",
            APPOINTMENT: "APPOINTMENT"
        }

        props.$STORAGE_LOCATION_CATEGORY = {
            MAIN: "MAIN",
            DETAIL: "DETAIL"
        }
        
        props.$TRANSFER_TYPE= {
            WITHIN_STORAGE_LOCATION : "WITHIN STORAGE LOCATION",
            ANOTHER_DEPT : "ANOTHER DEPT",
            EXTERNAL : "EXTERNAL"
        }

        props.$TYPE= [
            "Y","N"
        ]
    
    }
}

export default constants

