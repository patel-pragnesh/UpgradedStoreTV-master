using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class SuccessResponseModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
    }

    class OtpResponseModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string resourceList { get; set; }
        public string otp { get; set; }
    }

    class AddCartResponseModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public AddCartData data { get; set; }

    }

    public class AddCartData
    {
        public string cart_count { get; set; }
    }
}
