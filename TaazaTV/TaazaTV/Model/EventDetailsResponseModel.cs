using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    
    public class EventDetailsResponseModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public EventDetailsData data { get; set; }
    }

    public class EventDetailsData
    {
        public Event_Details event_details { get; set; }
    }

    public class Event_Details
    {
        public int event_id { get; set; }
        public string category_id { get; set; }
        public string event_name { get; set; }
        public string event_organiser_name { get; set; }
        public string event_duration_type { get; set; }
        public string event_duration_type_text { get; set; }
        public string event_duration_value { get; set; }
        public string event_ticket_option { get; set; }
        public string event_ticket_option_text { get; set; }
        public string event_tag_line { get; set; }
        public string season { get; set; }
        public string overview { get; set; }
        public string event_description { get; set; }
        public string show_data { get; set; }
        public string banner_image { get; set; }
        public string banner_normal_image { get; set; }
        public string current_wallet_balance { get; set; }
        public string maximum_taazacash_can_use { get; set; }
        public Event_Vanue event_vanue { get; set; }
        public Event_Prices[] event_prices { get; set; }
        public Event_Prices event_price { get; set; }
    }

    public class Event_Prices
    {
        public int price_id { get; set; }
        public string ticket_type { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string ticket_price { get; set; }
        public string cash_back { get; set; }
        public string max_taazacash_can_use { get; set; }
        public string available_seat { get; set; }
    }

    public class Event_Vanue
    {
        public string location { get; set; }
        public string picklocation { get; set; }
        public string city { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string address { get; set; }
    }
}
