using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
   public class PreniumAdModel
    {
        public string ResponseText{ get; set; }
        
        public PreniumData data { get; set; }
    }

  public  class PreniumData
    {
        public int Time_Interval { get; set; }
        public PreniumAdList[] Ad_list { get; set; }
    }
    public class PreniumAdList
    {
        public int Ad_id { get; set; }
        public string Ad_title { get; set; }
        public string Ad_url { get; set;}
        public string Add_image { get; set; }
    }
}
