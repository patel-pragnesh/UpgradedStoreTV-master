using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class MainCategoryListModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public Store_Category_Data data { get; set; }
    }

    public class Store_Category_Data
    {
        public Store_Category[] categorys { get; set; }
        public Store_Main_Category_Data main_category_data { get; set; }
    }

    public class Store_Main_Category_Data
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public Store_Banner_Image banner_image { get; set; }
    }

    public class Store_Category
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_slug { get; set; }
        public Store_Category_Image category_image { get; set; }
        public Store_Sub_Category[] subcategory { get; set; }
    }

    public class Store_Sub_Category
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_slug { get; set; }
        public Store_Category_Image category_image { get; set; }

    }

    public class Store_Category_Image
    {
        public string category_image_normal { get; set; }
        public string category_image_thumb { get; set; }
        public string category_image_crop { get; set; }
    }

    public class Store_Banner_Image
    {
        public string banner_image_normal { get; set; }
        public string banner_image_thumb { get; set; }
    }
}
