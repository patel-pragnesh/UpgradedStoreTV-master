using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace TaazaTV.Model
{
    class CategoryListModel
    {
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
    }
    public class CategoryAPIResponse
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }

        public string[] resourceList { get; set; }

        public category_datajson data { get; set; }
    }
    public class category_datajson
    {
        public List<category_data> category { get; set; }

    }
    public class category_data
    {
        public string category_id { get; set; }
        public string category_name { get; set; }
        public string category_image { get; set; }
    }

    public class NewsCategoryListModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public CategoryList_Data data { get; set; }
    }

    public class CategoryList_Data
    {
        public CategoryList[] category { get; set; }
        public Top_News[] top_news { get; set; }
    }

    public class CategoryList: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isSelected;
        public bool IsSelected
        {
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                            new PropertyChangedEventArgs("IsSelected"));
                    }
                }
            }
            get
            {
                return isSelected;
            }
        }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_image { get; set; }
    }

    public class Top_News
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

    public class Images_Videos
    {
        public string[] images { get; set; }
        public VideoData[] videos { get; set; }
    }
    public class VideoData
    {
        public string video_url { get; set; }
        public string[] video_thumb { get; set; }
    }


}
