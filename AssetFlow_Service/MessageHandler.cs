using FAMS_Service;
using FAMS_Models;
using FAMS_Models.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
using FAMS_ViewModels.Utilities;

namespace RabbitReceiver
{
    class MessageHandler
    {
        public static ResultData HandleMessage(string routingKey, string jsonData)
        {            
            var result = new ResultData();
            var message = "";

            try
            {
                var routingArray = routingKey.Split('.');
                string module = routingArray[0];
                string action = routingArray.Length > 1 ? routingArray[1] : "";

                BackendModel model = JsonConvert.DeserializeObject<BackendModel>(jsonData);

                if(model.user != null)
                {
                    var culture = UUtils.GetCulture(model.user.Language);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                }

                if(module == Constants.BackendModule.DOCUMENT)
                {
                    if(action == Constants.BackendAction.CREATE)
                    {

                    }
                    //else if(action == Constants.BackendAction.DEACTIVATION)
                    //{
                    //    result.success = DocumentHandler.Deactivation(jsonData, model.user, out message);
                    //    result.message = message;
                    //}
                    //else if(action == Constants.BackendAction.ENTRY)
                    //{
                    //    result.success = DocumentHandler.Entry(model.data, model.user, out message);
                    //    result.message = message;
                    //}
                    //else if (action == Constants.BackendAction.PACKAGE)
                    //{
                    //    result.success = DocumentHandler.Package(model.data, model.user, out message);
                    //    result.message = message;
                    //}
                    //else if (action == Constants.BackendAction.RELOCATION)
                    //{
                    //    result.success = DocumentHandler.Relocation(model.data, model.user, out message);
                    //    result.message = message;
                    //}
                    //else if(action == Constants.BackendAction.TRANSFER)
                    //{
                    //    result.success = DocumentHandler.Transfer(model.data, model.user, out message);
                    //    result.message = message;
                    //}
                }

                //if (module == Constants.BackendModule.SAP_ORDER)
                //{
                //    if(action == Constants.BackendAction.CREATE)
                //    {
                //        result.success = SAPOrderHandler.Create(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    if (action == Constants.BackendAction.CREATE_EDIT)
                //    {
                //        result.success = SAPOrderHandler.CreateOrEdit(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if (action == Constants.BackendAction.ASSIGN_TRANSPORTER)
                //    {
                //        result.success = SAPOrderHandler.AssignTransporter(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if(action == Constants.BackendAction.ASSIGN_CUSTOMER_VENDOR)
                //    {
                //        result.success = SAPOrderHandler.AssignCustomerVendor(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if(action == Constants.BackendAction.CANCEL)
                //    {
                //        result.success = SAPOrderHandler.CancelSAPOrder(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //}
                //else if (module == Constants.BackendModule.ORDER)
                //{
                //    if (action == Constants.BackendAction.ASSIGN_DRIVER)
                //    {
                //        result.success = OrderHandler.AssignDriver(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if(action == Constants.BackendAction.MERGE)
                //    {
                //        result.success = OrderHandler.Merge(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if (action == Constants.BackendAction.CANCEL)
                //    {
                //        result.success = OrderHandler.Cancel(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if(action == Constants.BackendAction.REASSIGN_DO)
                //    {
                //        result.success = OrderHandler.ReassignDO(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if(action == Constants.BackendAction.ADD_TO_SHIPMENT)
                //    {
                //        result.success = OrderHandler.AddToShipment(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if(action == Constants.BackendAction.CREATE_EDIT) //Only Edit
                //    {
                //        result.success = OrderHandler.Edit(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //}
                //else if (module == Constants.BackendModule.SHIPMENT)
                //{
                //    if(action == Constants.BackendAction.APPOINTMENT)
                //    {
                //        result.success = ShipmentHandler.Appointment(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if(action == Constants.BackendAction.CANCEL)
                //    {
                //        result.success = ShipmentHandler.Cancel(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //    else if(action == Constants.BackendAction.CREATE_EDIT)
                //    {
                //        result.success = ShipmentHandler.EditShipment(model.data, model.user, out message);
                //        result.message = message;
                //    }
                //}
                if(module == Constants.BackendModule.APPROVAL)
                {
                    ApprovalHelperModel resultModel = new ApprovalHelperModel();  

                    result.success = ApprovalHandler.Process(model.data, model.user, out message, out resultModel);

                    result.data = resultModel;
                    result.message = message;
                }
                else if(module == "util")
                {
                    if(action == "ping")
                    {
                        result.success = true;
                        result.message = "PINGED";
                    }
                }
            }
            catch(Exception ex)
            {
                result.message = ex.Message;
            }

            return result;


        }


    }
}
