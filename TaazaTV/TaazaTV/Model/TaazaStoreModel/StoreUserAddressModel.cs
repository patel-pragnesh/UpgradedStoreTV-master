using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class StoreUserAddressModel
    {
        public int responseCode { get; set; }

        public string responseText { get; set; }

        public object[] resourceList { get; set; }

        public User_Address_Data data { get; set; }
    }

    public class User_Address_Data
    {
        public Address address { get; set; }
    }

    public class Address
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

        public int index_sel
        {
            get
            {
                return type--;
            }
            set { }
        }
    }
}