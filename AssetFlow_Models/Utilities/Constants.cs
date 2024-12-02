using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.CodeDom;

namespace FAMS_Models.Utilities
{
    public class Constants
    {
        public const string FORM_MODE_UNCHANGED = "Unchanged";
        public const string FORM_MODE_CREATE = "Create";
        public const string FORM_MODE_EDIT = "Edit";
        public const string FORM_MODE_VIEW = "View";
        public const string FORM_MODE_DELETE = "Delete";
        public const string FORM_MODE_CANCEL = "Cancel";
        public const string FORM_MODE_UNDELETE = "Undelete";
        public const string FORM_MODE_UTIL = "Util";

        public const string CLAIM_USERNAME = "CLAIM_USERNAME";
        public const string CLAIM_ID = "CLAIM_ID";
        public const string CLAIM_NAME = "CLAIM_NAME";
        public const string CLAIM_FULLNAME = "CLAIM_FULLNAME";
        public const string CLAIM_EMAIL = "CLAIM_EMAIL";
        public const string CLAIM_IS_ADMIN = "CLAIM_IS_ADMIN";
        public const string CLAIM_USE_AD = "CLAIM_USE_AD";
        public const string CLAIM_MENU = "CLAIM_MENU";
        public const string CLAIM_LOCATION_CODE = "CLAIM_LOCATION_CODE";
        public const string CLAIM_LANGUAGE = "CLAIM_LANGUAGE";
        public const string CLAIM_SERVER = "CLAIM_SERVER";

        public const string AD_DOMAIN = "wil.local";
        public const string AD_SA_USER = "id_ad_integrator";
        public const string AD_SA_PASSWORD = "askl1290!@#";

        public const string OK = "OK";

        public static string UPLOAD_POD_PATH = "~/UploadedFiles/POD/{0}/";
        public static string UPLOAD_POD_PATH_DB = "UploadedFiles/POD/{0}/";

        public static string UPLOAD_DOC_TRANSFER_PATH = "~/UploadedFiles/DocumentTransfer/{0}/";
        public static string UPLOAD_DOC_TRANSFER_PATH_DB = "DocumentTransfer/";

        public static string WEB_PATH = "";
        public static string CONTENT_PATH = "";

        public static string[] UPLOAD_FILE_EXT = new string[]
        {
            ".jpeg",
            ".jpg",
            ".png",
            ".txt"
        };

        public class LocalizationType
        {
            public const string LABEL = "LABEL";
            public const string RESOURCES = "RESOURCES";
        }


        public class ApprovalStatus
        {
            public const string WAITING_FOR_APPROVAL = "WAITING FOR APPROVAL";
            public const string APPROVED = "APPROVED";
            public const string REJECTED = "REJECTED";
        }

        public class TransactionType
        {
            public const string DOCUMENT_ENTRY = "DOCUMENT ENTRY";
            public const string DOCUMENT_PACKAGE = "DOCUMENT PACKAGE";
            public const string PACKING_RELOCATION = "PACKING RELOCATION";
            public const string DOCUMENT_DEACTIVATION = "DOCUMENT DEACTIVATION";
            public const string DOCUMENT_TRANSFER = "DOCUMENT TRANSFER";
            public const string TRANSFER = "TRANSFER";
            public const string DEACTIVATION = "DEACTIVATION";
            public const string RETURN = "RETURN";
        }

        public class RunningNumberType
        {
            public const string ORDER = "OR";
            public const string APPOINTMENT = "A";
            public const string SHIPMENT = "S";
            public const string MANUAL_ORDER = "MO";
        }

        public class LookupGroup
        {
            public const string LANGUAGE = "LANGUAGE";

            public const string CHANGE_PASS_DURATION = "CHANGE_PASS_DURATION";

            public const string DOC_CATEGORY_TYPE = "DOC_CATEGORY_TYPE";
            public const string DOC_CATEGORY_EXPIRY = "DOC_CATEGORY_EXPIRY";
            public const string RUNNING_NUMBER_FORMAT = "RUNNING_NUMBER_FORMAT";
            public const string DOCUMENT_TRANSFER_TYPE = "DOCUMENT_TRANSFER_TYPE";

        }

        public class ApprovalValidity
        {
            public const string ALL = "ALL";
            public const string EITHER = "EITHER";
        }

        public class AppSettings
        {
            public const string TOKEN_SECRET = "TOKEN_SECRET";
            public const string API_URL = "API_URL";
            public const string API_AUTH = "API_AUTH";
            public const string DRIVE_ATTACHMENT = "DRIVE_ATTACHMENT";
            public const string URL_ATTACHMEN = "URL_ATTACHMEN";
        }

        public class BackendModule
        {
            public const string DOCUMENT = "document";
            public const string APPROVAL = "approval";
            public const string DEACTIVATION = "deactivation";
        }

        public class BackendAction
        {
            public const string EDIT = "edit";
            public const string CREATE = "create";
            public const string DEACTIVATION = "deactivation";
            public const string ENTRY = "entry";
            public const string PACKAGE = "package";
            public const string RELOCATION = "relocation";
            public const string TRANSFER = "transfer";
        }

        public class ReleaseStatus
        {
            public const string FULL = "FULL";
            public const string PARTIAL = "PARTIAL";
            public const string UNRELEASED = "UNRELEASED";
        }

        public class SettingsCode
        {
            public const string DEVICE_VERSION = "DEVICE_VERSION";
            public const string WEB_VERSION = "SETTING_VERSION";
        }



        public class CategoryStorage
        {
            public const string MAIN = "MAIN";
            public const string DETAIL = "DETAIL";
        }

        public class DocumentStatus
        {
            public const string ACTIVE = "ACTIVE";
            public const string INACTIVE = "INACTIVE";
            public const string WAITING_FOR_APPROVAL = "WAITING FOR APPROVAL";
            public const string IN_TRANSFER = "IN TRANSFER";
            public const string IN_DEACTIVATE = "IN DEACTIVATE";

        }


        public static class Modul
        {
            public static string DOCUMENT = "DOC";
        }

        public static class Modul_Detail
        {
            public static string DOC_ENTRY = "DE";
            public static string DOC_PACKAGE = "DP";
            
        }

        public static class ReleaseMatrixType
        {
            public const string DEACTIVATION = "DEACTIVATION";
            public const string TRANSFER = "TRANSFER";
        }

        public static class ReleaseMatrixCategory
        {
            public const string DOCUMENT = "DOCUMENT";
        }


        public static class Report_Type
        {
            public static string DOCUMENT_PER_LOCATION = "DOCUMENT PER LOCATION";
            public static string PACKING_RELOCATION = "PACKING RELOCATION";
        }

        public static class Transfer_Type
        {
            public static string WITHIN_STORAGE_LOCATION = "WITHIN STORAGE LOCATION";
            public static string ANOTHER_DEPT = "ANOTHER DEPT";
            public static string EXTERNAL = "EXTERNAL";
        }

        public static class Menu
        {
            public static string DOCUMENTPACKAGE = "DocumentPackage";
            public static string DOCUMENTENTRY = "DocumentEntry";
            public static string PACKINGRELOCATION = "PackingRelocation";
        }

        public static class Storage_Type
        {
            public static string EXTERNAL = "EXTERNAL";
            public static string DEACTIVE = "DEACTIVE";
            public static string INTERNAL = "INTERNAL";
        }

        public static class ApprovalAs
        {
            public static string REQUESTER = "REQUESTER";
            public static string HEAD_OF_REQUESTER = "HEAD OF REQUESTER";
            public static string HEAD_OF_ACCOUNTING = "HEAD OF ACCOUNTING";
            public static string DOCUMENT_KEEPER = "DOCUMENT KEEPER";
            public static string ADMIN = "ADMIN";
        }

        public static Dictionary<string, string> MuleURLOrder = new Dictionary<string, string>()
        {
            { "DEV", "https://mule-api.testcompany-intl.com:8444/itm-order-api/order-request" }, //TEMPORARY
            { "QA",  "https://mule-api.testcompany-intl.com:8443/itm-order-api/order-request" }, //TEMPORARY

            { "PRD", "https://api.companyapps.com/itm-order-api/order-request" },
            { "LOCAL", "https://api.testcompany-intl.com/dev/itm-order-api/order-request" },
            { "PRD_CLOUD", "https://api.companyapps.com/itm-order-api/order-request" },
        };

        public static Dictionary<string, string> ClientIdString = new Dictionary<string, string>()
        {
            { "DEV", "c7be94904d20452aaedfc46a692b73ac" },
            { "QA", "df0adf2d71fe45da84a627b4df484c7d" },
            { "PRD", "5309059a491a4ac693eaa6dc64b5459d" },
            { "LOCAL", "81082c4979c446539418496907539557" },
            { "PRD_CLOUD", "bb9ede143c7b4bffbe988b8e3face1af" },
        };

        public static Dictionary<string, string> ClientSecretString = new Dictionary<string, string>()
        {
            { "DEV", "912f0eAcdb4C494d9Ac82257Cb3cB528" },
            { "QA", "76201D15529A46c5927c3B79AF40Cb0F" },
            { "PRD", "845aA2a276874C5DB65350eC98d8E663" },
            { "LOCAL", "0124809caF1942f889aFB3B27e9dA87a" },
            { "PRD_CLOUD", "3A411CFa64ad4F199064cEa387E22EEF" },
        };


        public static string DEFAULT_CLIENT_ID = ConfigurationManager.AppSettings["ClientIdEnvironment"].ToString();
        public static string DEFAULT_CLIENT_SECRET = ConfigurationManager.AppSettings["ClientSecretEnvironment"].ToString();
        public static string DEFAULT_MULE_URL = ConfigurationManager.AppSettings["MuleURLEnvironment"].ToString();

    }
}
