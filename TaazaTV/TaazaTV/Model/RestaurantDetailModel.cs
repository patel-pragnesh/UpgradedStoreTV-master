using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class RestaurantDetailModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public RestaurantDetailsData data { get; set; }
    }

    public class RestaurantDetailsData
    {
        public Restaurant_Details restaurant_details { get; set; }
    }

    public class Restaurant_Details
    {
        public int restaurant_id { get; set; }
        public string restaurant_name { get; set; }
        public string company_id { get; set; }
        public string created_by { get; set; }
        public string category_id { get; set; }
        public CuisineList[] cuisine_list { get; set; }
        public string opening_time { get; set; }
        public string closing_time { get; set; }
        public string[] time_schedule { get; set; }
        public string seat_price { get; set; }
        public string cash_back { get; set; }
        public string available_seat { get; set; }
        public string description { get; set; }
        public string offers { get; set; }
        public string contact_details { get; set; }
        public string status { get; set; }
        public RestaurantImages[] restaurant_images { get; set; }
        public string current_wallet_balance { get; set; }
        public string maximum_taazacash_can_use { get; set; }
        public RestaurantVanue restaurant_vanue { get; set; }
    }

    
    public class CuisineList
    {
        public int cuisine_id { get; set; }
        public string cuisine_name { get; set; }
    }

    public class RestaurantImages
    {
        public string imageURL { get; set; }
    }

    public class RestaurantVanue
    {
        public string location { get; set; }
        public string picklocation { get; set; }
        public string city { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string address { get; set; }
    }
}

