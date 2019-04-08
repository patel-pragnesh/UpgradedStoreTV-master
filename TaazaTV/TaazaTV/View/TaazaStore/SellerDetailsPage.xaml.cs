using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
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
	public partial class SellerDetailsPage : ContentPage
	{
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        public SellerDetailsPage ()
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
                    new KeyValuePair<string, string>("seller_id", "43"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetSellerDetailsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<SellerDetailsModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void ShowPopUpMenu()
        {
            await PopupNavigation.PushAsync(new PopUpTaskView(), true);
        }
        
    }
}