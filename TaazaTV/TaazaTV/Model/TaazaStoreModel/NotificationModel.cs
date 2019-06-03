using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class NotificationModel
    {
        public long responseCode { get; set; }

        public string responseText { get; set; }

        public object[] resourceList { get; set; }

        public NotificationData data { get; set; }
    }

    public partial class NotificationData
    {
        public NotificationList[] notification_list { get; set; }
    }

    public partial class NotificationList
    {
        public string notification_title { get; set; }

        public string notification_body { get; set; }

        public string date { get; set; }
    }
}

