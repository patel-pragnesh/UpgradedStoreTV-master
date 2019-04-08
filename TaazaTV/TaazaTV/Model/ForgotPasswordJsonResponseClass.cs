using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class ForgotPasswordJsonResponseClass
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public forgotpass_data data  { get; set; }
    }
    public class forgotpass_data
    {
        public  forgot_data user_data { get; set; }
    }
    public class forgot_data
    {
        public string user_id { get; set; }
        public string phone_no { get; set; }
        public string name { get; set; }
        public string email_id { get; set; }
        public string city_id { get; set; }
    }

}
