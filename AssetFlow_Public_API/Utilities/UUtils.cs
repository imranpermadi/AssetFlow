using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using FAMS_Models;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Options;
using System.Configuration;
using FAMS_Models.Utilities;
using RestSharp;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FAMS_Public_API.Utilities
{
    

    public class UUtils
    {

        public static long ToUnixTimestamp(DateTime thisDateTime)
        {
            TimeSpan _UnixTimeSpan = (thisDateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)_UnixTimeSpan.TotalSeconds;
        }
        public static string ToMomentDate(DateTime thisDateTime)
        {
            return thisDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static DateTime FromMomentDate(string momentDateTime)
        {
            return DateTime.ParseExact(momentDateTime, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// URL ends with /, returns ResultData
        /// </summary>
        /// <param name="middlewareUrl"></param>
        /// <param name="headers"></param>
        /// <param name="parameters"></param>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ResultData CallMiddlewareAPI(string url, Dictionary<string, string> headers = null, string jsonBody = "", string method = "POST", string contentType = "application/json")
        {
            var result = new ResultData();

            string middlewareUrl = Configuration.AppSettings[Constants.AppSettings.API_URL] + url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient();
            var request = new RestRequest(middlewareUrl, method == "POST" ? Method.POST : (method == "PUT" ? Method.PUT : Method.GET));
            //request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", contentType);
            request.AddHeader("cache-conrtol", "no-cache");

            var apiAuth = Configuration.AppSettings[Constants.AppSettings.API_AUTH].ToString();
            request.AddHeader("Authorization", "Bearer " + apiAuth);

            if(contentType == "application/json")
            {
                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                if (jsonBody != null && !string.IsNullOrEmpty(jsonBody))
                {
                    request.RequestFormat = DataFormat.Json;
                    request.AddBody(jsonBody);
                }
            }


            //HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(middlewareUrl);
            //myRequest.Method = method;
            //myRequest.ContentType = contentType;
            //myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            //myRequest.Headers.Add("cache-control", "no-cache");
            ////myRequest.Timeout = 3000;

            //if(headers != null && headers.Count > 0)
            //{
            //    foreach(var header in headers)
            //    {
            //        myRequest.Headers.Add(header.Key, header.Value);
            //    }
            //}

           
            try
            {
                var response = client.Execute(request);
                string s = response.Content;//It is always XML format.

                result = JsonConvert.DeserializeObject<ResultData>(s);

            }
            catch (WebException ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        public static ResultByteArrayData CallMiddlewareAPIByteArray(string url, Dictionary<string, string> headers = null, string jsonBody = "", string method = "POST", string contentType = "application/json")
        {
            var result = new ResultByteArrayData();

            string middlewareUrl = Configuration.AppSettings[Constants.AppSettings.API_URL] + url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient();
            var request = new RestRequest(middlewareUrl, method == "POST" ? Method.POST : (method == "PUT" ? Method.PUT : Method.GET));
            //request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", contentType);
            request.AddHeader("cache-conrtol", "no-cache");

            var apiAuth = Configuration.AppSettings[Constants.AppSettings.API_AUTH].ToString();
            request.AddHeader("Authorization", "Bearer " + apiAuth);

            if (contentType == "application/json")
            {
                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                if (jsonBody != null && !string.IsNullOrEmpty(jsonBody))
                {
                    request.RequestFormat = DataFormat.Json;
                    request.AddBody(jsonBody);
                }
            }


            //HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(middlewareUrl);
            //myRequest.Method = method;
            //myRequest.ContentType = contentType;
            //myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            //myRequest.Headers.Add("cache-control", "no-cache");
            ////myRequest.Timeout = 3000;

            //if(headers != null && headers.Count > 0)
            //{
            //    foreach(var header in headers)
            //    {
            //        myRequest.Headers.Add(header.Key, header.Value);
            //    }
            //}


            try
            {
                var response = client.Execute(request);
                //string s = response.Content;//It is always XML format.

                //result = JsonConvert.DeserializeObject<ResultByteArrayData>(s);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Read bytes
                    byte[] fileBytes = response.RawBytes;
                    var headervalue = response.Headers.FirstOrDefault(x => x.Name == "Content-Disposition")?.Value;
                    string contentDispositionString = Convert.ToString(headervalue);
                    ContentDisposition contentDisposition = new ContentDisposition(contentDispositionString);
                    string fileName = contentDisposition.FileName;

                    result.data_byte = fileBytes;
                    result.message = fileName;

                }

            }
            catch (WebException ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        public static ResultData CallMiddlewareAPIFormData(string url, Dictionary<string, string> headers = null, Dictionary<string, string> parameters = null, IFormFileCollection formFiles = null, string method = "POST", string contentType = "multipart/form-data")
        {
            var result = new ResultData();

            string middlewareUrl = Configuration.AppSettings[Constants.AppSettings.API_URL] + url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient();
            var request = new RestRequest(middlewareUrl, method == "POST" ? Method.POST : (method == "PUT" ? Method.PUT : Method.GET));
            //request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", contentType);
            request.AddHeader("cache-control", "no-cache");

            var apiAuth = Configuration.AppSettings[Constants.AppSettings.API_AUTH].ToString();
            request.AddHeader("Authorization", "Bearer " + apiAuth);

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            if (parameters != null && parameters.Count > 0)
            {
                foreach(var parameter in parameters)
                {
                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }

            if(formFiles != null && formFiles.Count > 0)
            {
                foreach(var file in formFiles)
                {
                    MemoryStream stream = new MemoryStream();
                    file.CopyTo(stream);
                    request.AddFile("file", stream.GetBuffer(),file.FileName);
                }
                
            }
            else
            {
                MemoryStream stream = new MemoryStream();
                request.AddFile("file", stream.GetBuffer(), "NONE");
            }


            //HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(middlewareUrl);
            //myRequest.Method = method;
            //myRequest.ContentType = contentType;
            //myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            //myRequest.Headers.Add("cache-control", "no-cache");
            ////myRequest.Timeout = 3000;

            //if(headers != null && headers.Count > 0)
            //{
            //    foreach(var header in headers)
            //    {
            //        myRequest.Headers.Add(header.Key, header.Value);
            //    }
            //}


            try
            {
                var response = client.Execute(request);
                string s = response.Content;//It is always XML format.

                result = JsonConvert.DeserializeObject<ResultData>(s);

            }
            catch (WebException ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        public static Byte[] CallMiddlewareAPIFile(string url, Dictionary<string, string> headers = null, string jsonBody = "", string method = "POST", string contentType = "application/json")
        {

            string middlewareUrl = Configuration.AppSettings[Constants.AppSettings.API_URL] + url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient();
            var request = new RestRequest(middlewareUrl, method == "POST" ? Method.POST : (method == "PUT" ? Method.PUT : Method.GET));
            //request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", contentType);
            request.AddHeader("cache-conrtol", "no-cache");

            var apiAuth = Configuration.AppSettings[Constants.AppSettings.API_AUTH].ToString();
            request.AddHeader("Authorization", "Bearer " + apiAuth);

            if (contentType == "application/json")
            {
                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                if (jsonBody != null && !string.IsNullOrEmpty(jsonBody))
                {
                    request.RequestFormat = DataFormat.Json;
                    request.AddBody(jsonBody);
                }
            }


            //HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(middlewareUrl);
            //myRequest.Method = method;
            //myRequest.ContentType = contentType;
            //myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            //myRequest.Headers.Add("cache-control", "no-cache");
            ////myRequest.Timeout = 3000;

            //if(headers != null && headers.Count > 0)
            //{
            //    foreach(var header in headers)
            //    {
            //        myRequest.Headers.Add(header.Key, header.Value);
            //    }
            //}


            try
            {
                //var response = client.Execute(request);
                //string s = response.Content;//It is always XML format.

                //string tempFile = Path.GetTempFileName();
                //var writer = File.OpenWrite(tempFile);
                //request.ResponseWriter = (responseStream) =>
                //{
                //    responseStream.CopyTo(writer);
                //};

                var response = client.Execute(request);

                var a = response.RawBytes;

                return null;
            }
            catch (WebException ex)
            {
                return null;
            }
        }

        public static ResultData CallMiddlewareAPI_(string url, Dictionary<string, string> headers = null, string jsonBody = "", string method = "POST", string contentType = "application/json")
        {
            var result = new ResultData();

            string middlewareUrl = Configuration.AppSettings[Constants.AppSettings.API_URL] + url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(middlewareUrl);
            myRequest.Method = method;
            myRequest.ContentType = contentType;
            myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            myRequest.Headers.Add("cache-control", "no-cache");
            //myRequest.Timeout = 3000;

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    myRequest.Headers.Add(header.Key, header.Value);
                }
            }

            if (jsonBody != null && !string.IsNullOrEmpty(jsonBody))
            {
                //string jsonRequest = JsonConvert.SerializeObject(parameters);

                using (var dataStream = myRequest.GetRequestStream())
                {
                    dataStream.Write(Encoding.UTF8.GetBytes(jsonBody.ToCharArray()), 0, jsonBody.ToCharArray().Length);
                }
            }

            try
            {
                using (StreamReader streamReader = new StreamReader(myRequest.GetResponse().GetResponseStream()))
                {
                    var d = streamReader.ReadToEnd();

                    result = JsonConvert.DeserializeObject<ResultData>(d);
                }
            }
            catch (WebException ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }


        public static Dictionary<string, string> CreateMiddlewareAuth(UserData user, string mode = "", string language = "")
        {
            var headers = new Dictionary<string, string>();

            headers.Add("language", language);
            headers.Add("username", user.Username);
            headers.Add("fullname", user.Fullname);

            return headers;
        }


        /// <summary>
        /// URL ends with /, returns object
        /// </summary>
        /// <param name="middlewareUrl"></param>
        /// <param name="headers"></param>
        /// <param name="parameters"></param>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static object CallMiddlewareListAPI(string url, Dictionary<string, string> headers = null, Dictionary<string, object> parameters = null, string jsonBody = "", string method = "POST", string contentType = "application/json")
        {
            var result = new Object();

            string middlewareUrl = Configuration.AppSettings[Constants.AppSettings.API_URL] + url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient();
            var request = new RestRequest(middlewareUrl, method == "POST" ? Method.POST : (method == "PUT" ? Method.PUT : Method.GET));
            //request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", contentType);
            request.AddHeader("cache-conrtol", "no-cache");

            var apiAuth = Configuration.AppSettings[Constants.AppSettings.API_AUTH].ToString();
            request.AddHeader("Authorization", "Bearer " + apiAuth);


            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }

            if (contentType == "application/json")
            {
                if (jsonBody != null && !string.IsNullOrEmpty(jsonBody))
                {
                    request.RequestFormat = DataFormat.Json;
                    request.AddBody(jsonBody);
                }
            }

            try
            {
                var response = client.Execute(request);
                string s = response.Content;//It is always XML format.

                result = JsonConvert.DeserializeObject<object>(s);

            }
            catch (WebException ex)
            {

            }


            return result;
        }

        /// <summary>
        /// URL ends with /, returns object
        /// </summary>
        /// <param name="middlewareUrl"></param>
        /// <param name="headers"></param>
        /// <param name="parameters"></param>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static object CallMiddlewareListAPI_(string url, Dictionary<string, string> headers = null, string jsonBody = "", string method = "POST", string contentType = "application/json")
        {
            var result = new Object();

            string middlewareUrl = Configuration.AppSettings[Constants.AppSettings.API_URL] + url;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(middlewareUrl);
            myRequest.Method = method;
            myRequest.ContentType = contentType;
            myRequest.Headers.Add("cache-control", "no-cache");
            //myRequest.Timeout = 3000;

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    myRequest.Headers.Add(header.Key, header.Value);
                }
            }

            if (jsonBody != null && !string.IsNullOrEmpty(jsonBody))
            {
                //string jsonRequest = JsonConvert.SerializeObject(parameters);

                using (var dataStream = myRequest.GetRequestStream())
                {
                    dataStream.Write(Encoding.UTF8.GetBytes(jsonBody.ToCharArray()), 0, jsonBody.ToCharArray().Length);
                }
            }

            try
            {
                using (StreamReader streamReader = new StreamReader(myRequest.GetResponse().GetResponseStream()))
                {
                    var d = streamReader.ReadToEnd();

                    result = JsonConvert.DeserializeObject<object>(d);
                }
            }
            catch (WebException ex)
            {

            }

            return result;
        }


        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        //public static string GetAppLink()
        //{
        //    var link = ConfigurationManager.AppSettings["AppLink"];

        //    return link;
        //}
    }
}
