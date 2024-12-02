using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_ViewModels.Utilities
{
    public class CloudStorage
    {
        string bucketName = ConfigurationManager.AppSettings["BucketName"].ToString();
        string accessKey = ConfigurationManager.AppSettings["BucketAccessKey"].ToString();
        string appName = ConfigurationManager.AppSettings["BucketAppName"].ToString(); //For DEV
        string bucketEnvironment = ConfigurationManager.AppSettings["BucketEnvironment"].ToString();

        StorageClient client;
        Bucket bucket;

        public CloudStorage()
        {
            var credential = GoogleCredential.FromJson(accessKey);
            client = StorageClient.Create(credential);
            bucket = client.GetBucket(bucketName);
        }

        /// <summary>
        /// fileName includes folder name
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Upload(Stream stream, string fileName, string contentType, out string message, out string fileFullName, out string fileLink)
        {
            var result = false;
            message = "";
            fileFullName = "";
            fileLink = "";

            try
            {
                var finalFileName = fileName;

                if(bucketEnvironment != "PRD")
                {
                    finalFileName = appName + bucketEnvironment + "/" + fileName;
                }

                fileFullName = finalFileName;

                var obj1 = client.UploadObject(bucketName, finalFileName, contentType, stream);
                if(obj1 != null)
                {
                    fileLink = obj1.MediaLink;
                }

                result = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = false;
            }

            // Upload some files

            //// List objects
            //foreach (var obj in client.ListObjects(bucketName, ""))
            //{
            //    Console.WriteLine(obj.Name);
            //}

            return result;
        }

        public Stream Download(string fileName)
        {
            var stream = new MemoryStream();

            var finalFileName = fileName;

            var data = client.DownloadObject(bucketName, finalFileName, stream);


            return stream;
        }


    }
}
