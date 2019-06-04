using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    public class StoreDashModel
    {
        public long responseCode { get; set; }

        public string responseText { get; set; }

        public object[] resourceList { get; set; }

        public StoreDashData data { get; set; }
    }

    public partial class StoreDashData
    {
        public Offer[] offers { get; set; }

        public Feature[] features { get; set; }

        public TopSeller[] top_sellers { get; set; }
    }

    public partial class Feature
    {
        public string image { get; set; }

        public string title { get; set; }

        public string product_slug { get; set; }
    }

    public partial class Offer
    {
        public string image { get; set; }

        public string title { get; set; }

        public string sub_title { get; set; }

        public string product_category_id { get; set; }

        public string product_category_slug { get; set; }
    }

    public partial class TopSeller
    {
        public string seller_id { get; set; }

        public string full_name { get; set; }

        public string avater { get; set; }
    }
}