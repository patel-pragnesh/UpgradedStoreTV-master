using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace TaazaTV.Model
{
  
    public  class RegistrationModel
    {
        public string company_code { get; set; }
        public string user_name { get; set; }
        public string phone_no { get; set; }
        public string Profile_Image { get; set; }
        public string email_id { get; set; }
        public string password { get; set; }
        public string city_id { get; set; }
        public string device_type { get; set; }
        public string device_token { get; set; }
        public string app_version { get; set; }
    }
}
