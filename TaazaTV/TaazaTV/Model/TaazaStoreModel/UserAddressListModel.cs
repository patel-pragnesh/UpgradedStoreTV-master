using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    public class UserAddressListModel
    {
        public long responseCode { get; set; }

        public string responseText { get; set; }

        public object[] resourceList { get; set; }

        public Address_List_Data data { get; set; }
    }

    public class Address_List_Data
    {
        public Address_List[] address { get; set; }
    }

    public class Address_List
    {
        public int customer_address_id { get; set; }

        public int user_id { get; set; }

        public string full_name { get; set; }

        public string phone { get; set; }

        public string address_1 { get; set; }

        public string address_2 { get; set; }

        public string address_3 { get; set; }

        public long pincode { get; set; }

        public string city_name { get; set; }

        public string state_name { get; set; }

        public int type { get; set; }

        public int is_default { get; set; }
    }

}
