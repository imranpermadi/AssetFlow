using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using FAMS_Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace FAMS_ViewModels.Utilities
{
    public class FirebaseUtils
    {
        static DataEntities db = new DataEntities();

        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }

        public static void initFirebase()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                /*string root = "";
                string rootP = Path.GetPathRoot("~/");
                string rootMP = HostingEnvironment.MapPath("~/");
                root = string.IsNullOrEmpty(rootMP) ? rootP : rootMP;
                if (string.IsNullOrEmpty(root)){
                    var solutionPath = TryGetSolutionDirectoryInfo();
                    root = solutionPath.Parent.FullName;
                }

                string a = Path.Combine(root, "key.json");
                //string a = Path.Combine(rootMP, "key.json");
                //string b = Path.Combine(root, "key.json");*/
                var firebaseKey = ConfigurationManager.AppSettings["firebaseKey"].ToString();
                var defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    //Credential = GoogleCredential.FromFile(a),
                    Credential = GoogleCredential.FromJson(firebaseKey),
                });

                System.Diagnostics.Debug.WriteLine("Successfully sent message TESING: " + defaultApp.Name);

            }
        }

        public static async System.Threading.Tasks.Task seAsync(List<String> tokens, String title, String description)
        {
            // This registration token comes from the client FCM SDKs.
            //var registrationToken = "fADY1ftFQ6WtYs8J-IqUx4:APA91bEyKRhhQSbt-o3DkWPQ7FIXeZu60gWamP2NRf9ZykhIXSKpEvNybqoiD70U7ruIyMWWnS5Sjl_aR17pz3wC3ANsT0erUtD1Rd5mcGtCMU2kd51D2meq6LO0G4YeZkb4fJGeSvbp";

            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Notification = new Notification
                {
                    Title = title,
                    Body = description
                },
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            Console.WriteLine($"{response.SuccessCount} messages were sent successfully");
        }
    }
}
