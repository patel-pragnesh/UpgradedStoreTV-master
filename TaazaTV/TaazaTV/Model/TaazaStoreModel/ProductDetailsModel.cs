using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class ProductDetailsModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public Product_Details_Data data { get; set; }
    }

    public class Product_Details_Data
    {
        public Store_Product_Details product_details { get; set; }
        public Product_Sku_Options[] product_options { get; set; }
    }

    public class Store_Product_Details
    {
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string product_slug { get; set; }
        public string product_description { get; set; }
        public string total_skus { get; set; }
        public string price_range { get; set; }
        public List<Store_Product_Images> images { get; set; }
        public Product_Sku_Variants[] sku_variants { get; set; }
        
    }

    public class Product_Sku_Variants
    {
        public string product_id { get; set; }
        public string sku_id { get; set; }
        public string sku { get; set; }
        public string description { get; set; }
        public string regular_price { get; set; }
        public string sale_price { get; set; }
        public Product_Sku_Options[] options { get; set; }
        public int[] variant_option_ids { get; set; }
        public Store_Product_Images[] images { get; set; }
    }

    public class Product_Sku_Options
    {
        public int variant_id { get; set; }
        public string attribute_name { get; set; }
        public Product_Variant_Options[] variant_options { get; set; }
    }

    public class Product_Variant_Options
    {
        public string variant_id { get; set; }
        public int variant_option_id { get; set; }
        public string attribute_value_name { get; set; }
    }
}
