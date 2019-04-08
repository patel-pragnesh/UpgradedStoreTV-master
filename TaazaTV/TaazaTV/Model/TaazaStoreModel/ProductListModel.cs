using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class ProductListModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public Product_List_Data data { get; set; }
    }

    public class Product_List_Data
    {
        public Store_Product_List[] product_list { get; set; }
        public Category_Details_Data category_details { get; set; }
        public int current_page { get; set; }
        public bool hasMoreData { get; set; }
    }

    public class Category_Details_Data
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_slug { get; set; }
    }

    public class Store_Product_List
    {
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string product_slug { get; set; }
        public string product_description { get; set; }
        public string total_skus { get; set; }
        public string price_range { get; set; }
        public Store_Product_Images image { get; set; }
        public Product_Sku_Details_Data sku_details { get; set; }
    }

    public class Product_Sku_Details_Data
    {
        public string product_id { get; set; }
        public string sku_id { get; set; }
        public string sku { get; set; }
        public string regular_price { get; set; }
        public string sale_price { get; set; }
    }

    public class Store_Product_Images
    {
        public Product_Image normal { get; set; }
        public Product_Image thumb { get; set; }
    }

    public class Product_Image
    {
        public string image_path { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
