using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class OrdersModel
    {
        public int responseCode { get; set; }

        public string responseText { get; set; }

        public object[] resourceList { get; set; }

        public OrdersData data { get; set; }
    }


    public class OrdersData
    {
        public OrderDatum[] order_data { get; set; }
    }

    public class OrderDatum
    {
        public int order_history_id { get; set; }

        public string order_string { get; set; }

        public int order_status_int { get; set; }

        public string order_status_string { get; set; }

        public string product_id { get; set; }

        public string product_name { get; set; }

        public string product_description { get; set; }

        public string product_sku_id { get; set; }

        public string quantity { get; set; }

        public string product_price { get; set; }

        public string total_product_price { get; set; }

        public string product_image_url { get; set; }

        public string order_date { get; set; }

        public string dispatched_date { get; set; }

        public string delivered_date { get; set; }

        public string cancelled_date { get; set; }

        public string imgSource
        {
            get
            {
                switch(order_status_int)
                {
                    case 1:
                        return "orderedIcon.png";
                    case 2:
                        return "dispatchIcon.png";
                    case 3:
                        return "deliveredIcon.png";
                    case 4:
                        return "";
                    default:
                        return "";
                }
            }
            set { }
        }


    }
}