using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
namespace TaazaTV.Model
{

    public class BreakingNewsModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public Data_BNews data { get; set; }
    }

    public class Data_BNews
    {
        public object[] featured_news_list { get; set; }
        public News_List_Bnews[] news_list { get; set; }
        public int total_news { get; set; }
        public int current_page { get; set; }
        public int total_pages { get; set; }
    }

    public class News_List_Bnews
    {
        public int news_id { get; set; }
        public string news_title { get; set; }
        public string sort_description { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string time_elapsed_string { get; set; }
        public string banner_image { get; set; }
        public string is_breaking_news { get; set; }
        //public Images_Videos_BNews images_videos { get; set; }
    }

    //    public class Images_Videos_BNews
    //{
    //        public string[] images { get; set; }
    //        public string[] videos { get; set; }
    //    }


}

