using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class OTPModel
    {
        public string company_code { get; set; }
        public string user_id { get; set; }
        public string OTP { get; set; } 
        public string device_type { get; set; }
        public string device_token { get; set; }
        public string app_version { get; set; }
    }
}
