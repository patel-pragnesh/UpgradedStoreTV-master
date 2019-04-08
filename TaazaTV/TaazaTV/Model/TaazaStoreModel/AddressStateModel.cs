using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class AddressStateModel
    {
        public long responseCode { get; set; }

        public string responseText { get; set; }

        public object[] resourceList { get; set; }

        public User_State_List_Data Data { get; set; }
    }

    public class User_State_List_Data
    {
        public State[] states { get; set; }
    }

    public class State
    {
        public long state_id { get; set; }

        public string name { get; set; }

        public long country_id { get; set; }
    }
}