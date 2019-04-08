using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class DashboardUnreadCounterModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public Dashboard_UnreadData data { get; set; }

    }
    public class Dashboard_UnreadData
    {
        public string opinion_poll { get; set; }
        public string contests { get; set; }
        public string events { get; set; }
    }

}
