using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class SellerListModel
    {
        public int responseCode { get; set; }

        public string responseText { get; set; }

        public object[] resourceList { get; set; }

        public Seller_List_Data data { get; set; }
    }
}

public class Seller_List_Data
{
    public int total_result { get; set; }

    public int current_page { get; set; }

    public int total_pages { get; set; }

    public Seller_Data[] sellers { get; set; }
}

public class Seller_Data
{
    public int seller_id { get; set; }

    public string full_name { get; set; }

    public string phone_no { get; set; }

    public string email_id { get; set; }

    public string seller_details { get; set; }

    public long seller_type { get; set; }

    public string[] avater { get; set; }

    public string avatar
    {
        get
        {
            try
            {
                if (string.IsNullOrEmpty(avater[0]))
                    return "";
                else
                    return avater[0];
            }
           catch(Exception ex)
            {
                return "";
            }
        }
        set { }
    }

    public string address { get; set; }

    public string state { get; set; }

    public string city { get; set; }

    public string StateCity
    {
        get
        {
            return city + ", " + state;
        }
    }
}
