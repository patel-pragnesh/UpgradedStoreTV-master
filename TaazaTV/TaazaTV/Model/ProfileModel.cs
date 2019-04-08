using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class ProfileResponse
    {

        public string responseCode { get; set; }
        public string responseText { get; set; }

        public string[] resourceList { get; set; }

        public prof_data data { get; set; }
    }
    public class prof_data
    {

        public profuser_data user_data { get; set; }

    }
    public class profuser_data
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
        public Interested_news_categorys[] interested_news_categorys { get; set; }
    }

    public class UpdateProfile
    {

       
    }

    public class Interested_news_categorys
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
    }
}
