using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{

    class EventRestCityListModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public ListOfCities data { get; set; }
    }

    public class ListOfCities
    {
       public city_list_array[] city_list { get; set; }
    }

    public class city_list_array
    {
        public string city { get; set; }
    }
}