﻿using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DotNetAppSqlDb.Controllers
{
    public class NotificationsController : ApiController
    {
        [Authorize]
        public async Task<HttpResponseMessage> Post(string pns, [FromBody]string message, string to_tag)
        {
            var user = User.Identity.Name;
            string[] userTag = new string[2];
            userTag[0] = to_tag;
            userTag[1] = "from:" + user;

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            switch (pns.ToLower())
            {
                case "wns":
                    // Windows 8.1 / Windows Phone 8.1 / Windows 10
                    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                "From " + user + ": " + message + "</text></binding></visual></toast>";
                    outcome = await Notifications.Instance.Hub.SendWindowsNativeNotificationAsync(toast, userTag[0]);
                    break;
                case "wns/raw":
                    Notification notification = new WindowsNotification(message);
                    notification.Headers.Add("X-WNS-Type", "wns/raw");
                    await Notifications.Instance.Hub2.SendNotificationAsync(notification, userTag[0]);
                    break;
                case "apns":
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"" + "From " + user + ": " + message + "\"}}";
                    outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(alert, userTag);
                    break;
                case "gcm":
                    // Android
                    var notif = "{ \"data\" : {\"message\":\"" + "From " + user + ": " + message + "\"}}";
                    outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, userTag);
                    break;
            }

            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }

            return Request.CreateResponse(ret);
        }
    }
}