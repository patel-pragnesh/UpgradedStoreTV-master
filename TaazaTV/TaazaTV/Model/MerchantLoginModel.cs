using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class MerchantLoginModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public MerchantData data { get; set; }
    }

    public class MerchantData 
    {
        public MerchantDetails user_data { get; set; }
    }

    public class MerchantDetails
    {
        public string user_id { get; set; }
        public string user_type { get; set; }
        public string name { get; set; }
        public string phone_no { get; set; }
        public string email_id { get; set; }
        public string city_id { get; set; }
        public string city_name { get; set; }
        public string referral_code { get; set; }
        public string current_wallet_balance { get; set; }
        public string avatar { get; set; }
        public string[] interested_news_categorys { get; set; }
    }
}
