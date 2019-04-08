using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class NewsListModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public NewsListModel_Data data { get; set; }
    }

    public class NewsListModel_Data
    {
        public News_List[] news_list { get; set; }
        public News_List[] featured_news_list { get; set; }
        public int total_news { get; set; }
        public int current_page { get; set; }
        public int total_pages { get; set; }
    }

    public class News_List
    {
        public int news_id { get; set; }
        public string news_title { get; set; }
        public string sort_description { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string time_elapsed_string { get; set; }
        public string banner_image { get; set; }
        public Images_Videos images_videos { get; set; }
    }
}
