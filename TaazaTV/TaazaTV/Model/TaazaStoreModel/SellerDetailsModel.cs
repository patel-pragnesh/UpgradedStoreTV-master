using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class SellerDetailsModel
    {
        public int responseCode { get; set; }

        public string responseText { get; set; }

        public Seller_Details data { get; set; }

        public string resourceList { get; set; }
    }

    public class Seller_Details
    {
        public int seller_id { get; set; }

        public string full_name { get; set; }

        public string phone_no { get; set; }

        public string email_id { get; set; }

        public string avatar { get; set; }

        public string address { get; set; }

        public string state { get; set; }

        public string seller_details { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }
    }
}