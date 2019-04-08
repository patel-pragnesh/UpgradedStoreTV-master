using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.View.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using TaazaTV.Model;
namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OTPValidationPage : ContentPage
    {
        LoginModel des = new LoginModel();
        string user_Id;
        public OTPValidationPage(string uid)
        {
            InitializeComponent();
            user_Id = uid;
        }

        protected override void OnDisappearing()
        {
            Loader.IsVisible = false;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public Boolean ChkNullAll()
        {
            bool c = true;
            if (string.IsNullOrEmpty(OTP.Text))
            {
                DisplayAlert("Error", "Required field not be empty", "OK");
                c = false;
            }
            else if (string.IsNullOrWhiteSpace(OTP.Text))
            {
                DisplayAlert("Error", "Required field not be  empty", "OK");
                c = false;
            }
            return c;
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);

            if (user_Id != null)
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();

                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

                OTPModel OModel = new OTPModel();

                if (ChkNullAll() == true)
                {
                    parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));

                    parameters.Add(new KeyValuePair<string, string>("otp", OTP.Text.Trim()));
                    parameters.Add(new KeyValuePair<string, string>("user_id", user_Id));

                    parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
                    parameters.Add(new KeyValuePair<string, string>("device_token", Constant.DeviceToken));
                    parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));

                    try
                    {
                        string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.OTP], parameters);
                        if (data.ToString() == "NoInternet")
                        {
                            NoInternet.IsVisible = true;
                            MainContainer.IsVisible = false;
                            logoimg.IsVisible = false;
                        }
                        else
                        {
                            try
                            {
                                des = JsonConvert.DeserializeObject<LoginModel>(data);
                            }
                            catch
                            {
                                await DisplayAlert("Internal server error", "Please try again later", "Cancel");
                            }

                            if (des.responseText == "Success")
                            {
                                AppData.IsLogin = true;
                                AppData.UserName = des.data.user_data.name;
                                AppData.UserId = des.data.user_data.user_id;
                                AppData.UserCityId = des.data.user_data.city_id;
                                AppData.UserCity = des.data.user_data.city_name;
                                AppData.Avatar = des.data.user_data.avatar;

                                App.Current.MainPage = new MasterDetailsPage();
                            }
                            else
                            {
                                //Invalid
                                await DisplayAlert("Error", des.responseText, "Cancel");
                            }
                        }
                    }

                    catch (Exception ex)
                    {

                    }
                }

            }
            Loader.IsVisible = false;
            MainContainer.IsVisible = true;
        }

        private async void Resend_Tapped(object sender, EventArgs e)
        {
            Loader.IsVisible = true;
            await Task.Delay(1000);

            HttpRequestWrapper wrapper = new HttpRequestWrapper();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
            parameters.Add(new KeyValuePair<string, string>("user_id", user_Id));

            try
            {
                string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ResendOTP], parameters);
                if (data.ToString() == "NoInternet")
                {
                    await DisplayAlert("No Internet", "Please check your internet connection", "Cancel");
                }
                else
                {

                }
            }
            catch { }

            await DisplayAlert("Resend OTP", "OTP has been sent on your mobile no", "Cancel");
            Loader.IsVisible = false;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            SubmitButton_Clicked(sender, e);
        }
    }
}