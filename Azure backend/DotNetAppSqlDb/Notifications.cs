using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetAppSqlDb
{
    public class Notifications
    {
        public static Notifications Instance = new Notifications();

        public NotificationHubClient Hub { get; set; }
        public NotificationHubClient Hub2 { get; set; }

        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(
                "Endpoint=sb://pivovarnotif.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=/g2TGjxNpj2cKZ6Z/p+BtwLfQQwMjORGFJNi+Bh9hWY=",
                "pivovarnotif");
            Hub2 = NotificationHubClient.CreateClientFromConnectionString(
                "Endpoint=sb://pivovarnotif.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=DdCNgTA41xZ/gnXCfsMl0p9cvbcNmg/BpcCU4HpG3U0=",
                "deviceSettingsHub");
        }
    }
}