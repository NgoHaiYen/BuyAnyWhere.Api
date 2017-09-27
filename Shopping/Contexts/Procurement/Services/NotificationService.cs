using Newtonsoft.Json;
using Shopping.Contexts.Procurement.Applications.Interfaces;
using System;
using System.Net;

namespace Shopping.Contexts.Procurement.Services
{
    public class NotificationService : INotificationService
    {
        public class Notification
        {
            public string title { get; set; }
            public string body { get; set; }
        }

        public class RootObject
        {
            public Notification notification { get; set; }
            public string to { get; set; }
        }


        public void Notify(string title, string body, string cloudToken)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.Headers.Add("Authorization", "key=AAAATI0Ktp4:APA91bGXZpx7TGL7S1HFRkjulP7P7lppwzkWEUciBEjyNaP8w9dqurqSC53ooDZ0l8rxJjwfuLYL3G4ODrq7bS4nnE61eX_udAjIA-Dw4w-2t-V1SQj1nTSC3Uop7er1swWZSx6Hke2o");
                    webClient.Headers.Add("Content-Type", "application/json");
                    var uri = "https://fcm.googleapis.com/fcm/send";

                    var rootObject = new RootObject();
                    rootObject.notification = new Notification();
                    rootObject.notification.title = title;
                    rootObject.notification.body = body;
                    rootObject.to = cloudToken;

                    webClient.UploadString(uri, "POST", JsonConvert.SerializeObject(rootObject));
                }
                catch (Exception e)
                {
                    throw new BadRequestException(e.InnerException.ToString());
                }
            }
        }
    }
}