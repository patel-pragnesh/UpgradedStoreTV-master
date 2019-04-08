using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.Model.TaazaStoreModel;

namespace TaazaTV.View.TaazaStore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddressListPage : ContentPage
	{
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        public AddressListPage ()
		{
			InitializeComponent ();
            LoadAddressList();
        }

        private async void LoadAddressList()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                    new KeyValuePair<string, string>("user_id", "100"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UserAddressListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<UserAddressListModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void SetDefaultAddress()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("customer_address_id", "ANDROID"),
                    new KeyValuePair<string, string>("user_id", "100"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.SetDefaultAddressAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                   // Change the color of selcected item in the list
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void DeleteUserAddress()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("customer_address_id", "1"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.DeleteUserAddressAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    LoadAddressList();
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }
    }
}