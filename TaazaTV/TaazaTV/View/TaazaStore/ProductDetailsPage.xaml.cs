using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductDetailsPage : ContentPage
	{
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        public ProductDetailsPage ()
		{
			InitializeComponent ();
            InitialLoading();
        }

        private async void InitialLoading()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                    new KeyValuePair<string, string>("product_slug", "pro-281"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ProductDetailsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<ProductDetailsModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }
    }
}