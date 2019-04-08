using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TaazaTV.Model
{
    public class facebooklogin
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public _FBdata data { get; set; }
    }
    public class _FBdata
    {
        public _FBUserdata user_data { get; set; }
    }
    public class _FBUserdata
    {
        public string user_id { get; set; }
        public string user_type { get; set; }
        public string name { get; set; }
        public string phone_no { get; set; }
        public string email_id { get; set; }
        public string city_id { get; set; }
        public string city_name { get; set; }
        public string avatar { get; set; }
        public FBinterestedCategory[] interested_news_categorys { get; set; }
    }
    public class FBinterestedCategory
    {
        public string category_id { get; set; }
        public string category_name { get; set; }
    }


    public class FacebookProfile
    {

        public string Name { get; set; }
        public string PhoneNo { get; set; }
        [JsonProperty("email")]
        public string  Emailid { get; set; }
        public Picture Picture { get; set; }
        public string Locale { get; set; }
        public string Link { get; set; }
        public Cover Cover { get; set; }
        [JsonProperty("age_range")]
        public AgeRange AgeRange { get; set; }
        //  public Device[] Devices { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public bool IsVerified { get; set; }
        public string Id { get; set; }
    }

    public class Picture
    {
        public Data_ Data { get; set; }
    }

    public class Data_
    {
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
    }

    public class Cover
    {
        public string Id { get; set; }
        public int OffsetY { get; set; }
        public string Source { get; set; }
    }

    public class AgeRange
    {
        public int Min { get; set; }
    }

    //public class Device
    //{
    //    public string Os { get; set; }
    //}

}
