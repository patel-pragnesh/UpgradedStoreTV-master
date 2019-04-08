using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class UserWiseSubmitNewsModel
    {

        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public UserWiseSubmitNewsModel_Data data { get; set; }
    }

    public class UserWiseSubmitNewsModel_Data
    {
        public UserwiseNews_List[] news_list { get; set; }

        public int totla_news { get; set; }
        public int current_page { get; set; }
        public int totla_pages { get; set; }
    }

    public class UserwiseNews_List
    {
        public int news_id { get; set; }
        public string news_title { get; set; }
        public string sort_description { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string time_elapsed_string { get; set; }
        public string banner_image { get; set; }
        public Images_Videos images_videos { get; set; }
        public string is_approve_by_admin { get; set; }
        public bool is_approve
        {
            get
            {
                return is_approve_by_admin == "1" ? true : false;
            }
            set { }
        }
    }
}

