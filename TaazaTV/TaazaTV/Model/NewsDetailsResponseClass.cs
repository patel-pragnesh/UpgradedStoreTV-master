using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    //public class NewsDetailsResponseClass
    //{
    //    public string responseCode { get; set; }
    //    public string responseText { get; set; }
    //    public object[] resourceList { get; set; }
    //    public _Data data { get; set; }
    //}

    //public class _Data
    //{
    //    public News_Details news_details { get; set; }
    //}

    //public class News_Details
    //{
    //    public string news_title { get; set; }
    //    public string sort_description { get; set; }
    //    public string description { get; set; }
    //    public string date { get; set; }
    //    public string time { get; set; }
    //    public mediaValue media_value { get; set; }
    //    //public string banner_image { get; set; }
    //    public int approve_status { get; set; }
       
    //    public News_Categorys[] news_categorys { get; set; }
    //    public city_data[] citys { get; set; }
    //}
    //public class mediaValue
    //{
    //    public bannerimg banner_image { get; set; }
    //    public Images_Videos images_videos { get; set; }
    //}
    //public class Images_Videos
    //{
    //    public ImageData[] images { get; set; }
    //    public VideoData[] videos { get; set; }
    //}
    //public class ImageData
    //{
    //    public string[] normal { get; set; }
    //    public string[] thumb { get; set; }
    //}
    //public class bannerimg
    //{
    //    public string normal { get; set; }
    //    public string thumb { get; set; }
    //}
    //public class News_Categorys
    //{
    //    public int category_id { get; set; }
    //    public string category_name { get; set; }
    //}

    //public class ImageList
    //{
    //    public string images { get; set; }
    //}

    //public class VideoList
    //{
    //    public string VideoThumb { get; set; }
    //    public string VideoURL { get; set; }
    //}


    public class NewsDetailsResponseClass
    {
        public int responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public _Data data { get; set; }
    }

    public class _Data
    {
        public News_Details news_details { get; set; }
    }

    public class News_Details
    {
        public string news_title { get; set; }
        public string sort_description { get; set; }
        public string description { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public int approve_status { get; set; }
        public Media_Value media_value { get; set; }
        public News_Categorys[] news_categorys { get; set; }
        public City[] citys { get; set; }
    }

    public class Media_Value
    {
        public Banner_Image banner_image { get; set; }
        public Images_Videoss images_videos { get; set; }
    }

    public class Banner_Image
    {
        public string normal { get; set; }
        public string thumb { get; set; }
    }

    public class Images_Videoss
    {
        public image[] images { get; set; }
        public Video[] videos { get; set; }
    } 

    public class image
    {
        public string normal { get; set; }
        public string thumb { get; set; }
    }

    public class Video
    {
        public string video_url { get; set; }
        public string[] video_thumb { get; set; }
    }

    public class News_Categorys
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
    }

    public class City
    {
        public int city_id { get; set; }
        public string city_name { get; set; }
    }

    public class DisplayVideo
    {
        public string video_url { get; set; }
        public string video_thumb { get; set; }
    }
}