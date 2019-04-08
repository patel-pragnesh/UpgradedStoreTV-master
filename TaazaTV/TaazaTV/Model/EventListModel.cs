using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class EventListModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public _EventData data { get; set; }
    }

    public class _EventData
    {
        public _Event_Data[] featured_event { get; set; }
        public _Events events { get; set; }
        public EventCatoryList[] event_category_list { get; set; }
    }

    public class EventCatoryList
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string image_url { get; set; }
    }

    public class _Events
    {
        public _Event_Data[] event_data { get; set; }
        public int total_events { get; set; }
        public int total_pages { get; set; }
        public int current_page { get; set; }
    }

    public class _Event_Data
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
        public string show_data { get; set; }
        public string banner_image { get; set; }
        public _Event_Vanue event_vanue { get; set; }
    }

    public class _Event_Vanue
    {
        public string location { get; set; }
        public string picklocation { get; set; }
        public string city { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string address { get; set; }
    }
}
