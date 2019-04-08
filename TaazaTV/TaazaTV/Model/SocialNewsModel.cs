using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    
    public class SocialNewsModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public Social_Data data { get; set; }
    }

    public class Social_Data
    {
        public Group[] groups { get; set; }
    }

    public class Group
    {
        public int group_id { get; set; }
        public string group_name { get; set; }
        public string group_logo { get; set; }
    }

  

}
