using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class OTPCheckResponseModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public otpcheck_data data { get; set; }
    }
    public class otpcheck_data
    {
        public string user_id { get; set; }
    }
}
