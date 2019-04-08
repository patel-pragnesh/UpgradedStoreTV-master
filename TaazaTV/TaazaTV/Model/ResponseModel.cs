using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
   public  class ResponseModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
    }

    public class LoginModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public Login user_data { get; set; }
    }
    public class Login
    {
        public string user_id { get; set; }
        public string user_type { get; set; }
        public string name { get; set; }
        public string phone_no { get; set; }
        public string email_id { get; set; }
        public string city_id { get; set; }
        public string city_name { get; set; }
        public string avatar { get; set; }
    }
}
