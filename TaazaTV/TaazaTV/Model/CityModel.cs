using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TaazaTV.Model
{
    public class CityList
    {
        public static ObservableCollection<city_data> _cityList = new ObservableCollection<city_data>();
    }
    //public class CityModel
    //{
    //    public string DisplayName { get; set; }
    //    public string DisplayId { get; set; }
    //}
    public class CityModel
    {

        public string responseCode { get; set; }
        public string responseText { get; set; }

        public string[] resourceList { get; set; }

        public data_json data { get; set; }
    }
    public class data_json
    {

        public List<city_data> city_data { get; set; }

    }
    public class city_data
    {
        public string city_id { get; set; }
        public string city_name { get; set; }

    }


}
