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
	public partial class NewAddressPage : ContentPage
	{
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
		public NewAddressPage ()
		{
			InitializeComponent ();
            AddNewAddressView();  
            UserAddressDetailsView(); //Set Something to distinguish as upadte
        }

        private void AddNewAddressView()
        {
           GetStatesList();
        }

        private async void UserAddressDetailsView()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("customer_address_id", "1"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UserAddressDetailsViewAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<StoreUserAddressModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void GetStatesList()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetStateListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<AddressStateModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void SubmitNewAddress()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("user_id", "100"),
                    new KeyValuePair<string, string>("full_name", "100"),
                    new KeyValuePair<string, string>("phone", "100"),
                    new KeyValuePair<string, string>("address_1", "100"),
                    new KeyValuePair<string, string>("address_2", "100"),
                    new KeyValuePair<string, string>("address_3", "100"),
                    new KeyValuePair<string, string>("pincode", "100"),
                    new KeyValuePair<string, string>("city_name", "100"),
                    new KeyValuePair<string, string>("state_name", "100"),
                    new KeyValuePair<string, string>("type", "100"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.AddUserNewAddressAPI], parameters);
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

        private async void SubmitUpdatedAddress()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("customer_address_id", "1"),
                    new KeyValuePair<string, string>("user_id", "100"),
                    new KeyValuePair<string, string>("full_name", "100"),
                    new KeyValuePair<string, string>("phone", "100"),
                    new KeyValuePair<string, string>("address_1", "100"),
                    new KeyValuePair<string, string>("address_2", "100"),
                    new KeyValuePair<string, string>("address_3", "100"),
                    new KeyValuePair<string, string>("pincode", "100"),
                    new KeyValuePair<string, string>("city_name", "100"),
                    new KeyValuePair<string, string>("state_name", "100"),
                    new KeyValuePair<string, string>("type", "100"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UpdateUserAddressAPI], parameters);
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

    }
}