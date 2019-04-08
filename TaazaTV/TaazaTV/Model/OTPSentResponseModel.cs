using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class OTPSentResponseModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public otpsend_data data { get; set; }
    }
    public class otpsend_data
    {
        public string user_id { get; set; }
    }
   
    
}
