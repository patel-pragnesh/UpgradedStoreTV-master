using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.View.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewLoginPage : ContentPage
    {
        NewLoginCheckResponseModel result = new NewLoginCheckResponseModel();
        public NewLoginPage()
        {
            InitializeComponent();

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
            await Task.Delay(1000);

            if (ChkNullAll() == true)
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("phone_no", Mobileno.Text.Trim()));

                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
                parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
                parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));

                string jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewLogincheck], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                }
                else
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<NewLoginCheckResponseModel>(jsonstr);
                    }
                    catch
                    {
                        await DisplayAlert("Internal server error", "Please try again later", "Cancel");
                    }
                    if (result.responseText == "Success")
                    {
                        AppData.IsLogin = true;
                        AppData.UserName = result.data.user_data.name;
                        AppData.UserId = result.data.user_data.user_id;
                        AppData.UserCityId = result.data.user_data.city_id;
                        AppData.UserCity = result.data.user_data.city_name;
                        AppData.Avatar = result.data.user_data.avatar;
                        AppData.TaazaCash = result.data.user_data.current_wallet_balance;
                        App.Current.MainPage = new MasterDetailsPage();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Phone Number does not match please register first ", "OK");
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