using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class DashboardModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public Dashboard_Data data { get; set; }
    }

    public class Dashboard_Data
    {
        public Breaking_News[] breaking_news { get; set; }
        public Headline_News[] headline_news { get; set; }
        public Menu[] menus { get; set; }
        public string userCurrentWalletBalance { get; set; }
        public Ad[] ad_list { get; set; }
    }

    public class Breaking_News
    {
        public int news_id { get; set; }
        public string news_title { get; set; }
        public string banner_image { get; set; }
    }

    public class Headline_News
    {
        public int news_id { get; set; }
        public string news_title { get; set; }
    }

    public class Menu
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string menu_item_id { get; set; }
        public string category_image { get; set; }
        public string type { get; set; }
        public string for_type { get; set; }
        public string for_category_id { get; set; }
        public string alias { get; set; }
    }

    public class Ad
    {
        public int ad_id { get; set; }
        public string ad_title { get; set; }
        public string ad_url { get; set; }
        public string add_image { get; set; }
    }
}
