using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{

    public class SocialDetailsModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public SocialDetails_Data data { get; set; }
    }

    public class SocialDetails_Data
    {
        public Group_Details group_details { get; set; }
        public Group_Feed group_feed { get; set; }
    }

    public class Group_Details
    {
        public int group_id { get; set; }
        public string group_name { get; set; }
        public string group_organisation_head { get; set; }
        public string membership_category_id { get; set; }
        public string membership_category_text { get; set; }
        public string group_organisation_phone { get; set; }
        public string group_organisation_email { get; set; }
        public string group_details { get; set; }
        public string group_logo { get; set; }
        public string group_banner { get; set; }
    }

    public class Group_Feed
    {
        public Feed_Data[] feed_data { get; set; }
        public int total_feed { get; set; }
        public int total_pages { get; set; }
        public int current_page { get; set; }
    }

    public class Feed_Data
    {
        public string title { get; set; }
        public string content { get; set; }
        public string feed_image { get; set; }
        public User_Details user_details { get; set; }
    }

    public class User_Details
    {
        public int user_id { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
    }
}