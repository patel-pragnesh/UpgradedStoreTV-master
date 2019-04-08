using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace TaazaTV.Model
{
    public class jsonResponseClass
    {  
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public reg_data data { get; set; }
    }
    public class reg_data
    {
        public string user_id { get; set; }
        public string phone_no { get; set; } 
    }
    //public class UserData
    //{
       
    //    public string user_type { get; set; }
    //    public string phone_no { get; set; }
    //}

}