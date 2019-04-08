using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class AllTransactions
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public _Booking_Data data { get; set; }
    }

    public class _Booking_Data
    {
        public AllBookings[] booking_data { get; set; }
    }

    public class AllBookings
    {
        public int booking_id { get; set; }
        public string booking_unique_id { get; set; }
        public string booking_type { get; set; }
        public string booking_data_time { get; set; }
        public _Booking_Detail booking_details { get; set; }
        public All_Detail details { get; set; }
    }

    public class _Booking_Detail
    {
        public string price { get; set; }
        public string total_number_seat { get; set; }
        public string total_price { get; set; }
        public string paid_amount { get; set; }
        public string used_taazacash { get; set; }
        public string booking_date_for { get; set; }
        public string booking_time_for { get; set; }
    }

    public class All_Detail
    {
        public string id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public _Venue_Detail vanue_details { get; set; }
    }
   
    public class _Venue_Detail
    {
        public string location { get; set; }
        public string picklocation { get; set; }
        public string city { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string address { get; set; }
    }
}