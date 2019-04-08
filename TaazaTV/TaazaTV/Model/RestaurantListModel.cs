using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class RestaurantListModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public _RestaurantData data { get; set; }
    }

    public class _RestaurantData
    {
        public _Restaurants restaurants { get; set; }
        public RestaurantCatoryList[] restaurant_category_list { get; set; }
        public RestaurantCatoryCuisine[] restaurant_category_cuisine { get; set; }

    }

    public class _Restaurants
    {
        public _Restaurant_Data[] restaurant_data { get; set; }
        public int total_restaurant { get; set; }
        public int total_pages { get; set; }
        public int current_page { get; set; }
    }

    public class _Restaurant_Data
    {
        public int restaurant_id { get; set; }
        public string restaurant_name { get; set; }
        public string company_id { get; set; }
        public string created_by { get; set; }
        public string category_id { get; set; }
        public Cuisine_List[] cuisine_list { get; set; }
        public string opening_time { get; set; }
        public string closing_time { get; set; }
        public string seat_price { get; set; }
        public string cash_back { get; set; }
        public string available_seat { get; set; }
        public string description { get; set; }
        public string offers { get; set; }
        public string contact_details { get; set; }
        public string status { get; set; }
        public Restaurant_Images[] restaurant_images { get; set; }
        public _Restaurant_Vanue restaurant_vanue { get; set; }
        public string RestBannerImg { get; set; }
    }

    public class Cuisine_List
    {
        public int cuisine_id { get; set; }
        public string cuisine_name { get; set; }
    }

    public class Restaurant_Images
    {
        public string imageURL { get; set; }
    }

    public class _Restaurant_Vanue
    {
        public string location { get; set; }
        public string picklocation { get; set; }
        public string city { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string address { get; set; }
    }

    public class RestaurantCatoryList
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string image_url { get; set; }
    }

    public class RestaurantCatoryCuisine
    {
        public int cuisine_id { get; set; }
        public string cuisine_name { get; set; }
        public string image_url { get; set; }
    }
}
