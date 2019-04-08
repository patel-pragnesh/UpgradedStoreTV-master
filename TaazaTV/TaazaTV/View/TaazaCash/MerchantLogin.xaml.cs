using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaCash
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MerchantLogin : ContentPage
	{

        MerchantLoginModel result = new MerchantLoginModel();

        public MerchantLogin ()
		{
			InitializeComponent ();
		}

        public Boolean ChkNullAll()
        {
            bool c = true;
            if (string.IsNullOrEmpty(Mobileno.Text) || Mobileno.Text.Length != 10)
            {
                DisplayAlert("Error", "Please enter a valid phone number", "OK");
                c = false;
            }
            return c;
        }
        private async void Validate_Tapped(object sender, EventArgs e)
        {
            Loader.IsVisible = true;

           // await Task.Delay(500);

            if (ChkNullAll() == true)
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("phone_no", Mobileno.Text.Trim()));
                parameters.Add(new KeyValuePair<string, string>("password", Password.Text.Trim()));
                parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
                parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
                parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));

                string jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.MerchantLoginCheck], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                }
                else
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<MerchantLoginModel>(jsonstr);
                    }
                    catch
                    {
                        await DisplayAlert("Internal server error", "Please try again later", "Cancel");
                    }
                    if (result.responseText == "Success")
                    {
                        AppData.MerchantUserName = result.data.user_data.name;
                        AppData.MerchantUserId = result.data.user_data.user_id;
                        AppData.IsMerchantLogin = true;
                      await  Navigation.PushModalAsync(new MerchantVerification());
                    }
                    else
                    {
                        await DisplayAlert("Error", "Phone Number/Password does not match please register first ", "OK");
                        await Navigation.PopModalAsync();
                    }
                }
            }
            Loader.IsVisible = false;
        }
        private void DoSomething(object sender, EventArgs e)
        {
            Validate_Tapped(sender, e);
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}