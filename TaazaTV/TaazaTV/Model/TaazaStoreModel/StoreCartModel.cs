using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class StoreCartModel
    {
        public long responseCode { get; set; }

        public string responseText { get; set; }

        public object[] resourceList { get; set; }

        public Data data { get; set; }
    }

    public partial class Data
    {
        public CartData cart_data { get; set; }
    }

    public partial class CartData
    {
        public long cart_id { get; set; }

        public Cart_History_Data[] history_data { get; set; }

        public int total_price { get; set; }

        public int total_taazacash_can_use { get; set; }

        public long user_id { get; set; }

        public object group_id { get; set; }
    }

    public partial class Cart_History_Data
    {
        public long cart_history_id { get; set; }

        public long product_id { get; set; }

        public int max_taaza_cash_use_each { get; set; }

        public int max_taaza_cash_use_total { get; set; }

        public int max_cashback_each { get; set; }

        public int max_cashback_total { get; set; }

        public string product_name { get; set; }

        public long sku_id { get; set; }

        public string ProductSku { get; set; }

        public object product_sku_description { get; set; }

        public long quantity { get; set; }

        public string sale_price { get; set; }

        public long total_price { get; set; }

        public string product_image_url { get; set; }

        public string seller_name { get; set; }
    }
}

