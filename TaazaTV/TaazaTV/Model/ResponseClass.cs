using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class ResponseClass
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public _Data data { get; set; }
    }
}
