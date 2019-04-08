using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.View.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.Model;
using Newtonsoft.Json;

namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnNewUserTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegistrationPage());
        }

        private async Task OnforgetLabelTappedAsync(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPasswordPage());
        }

        private void OnforgetLabelTapped(object sender, EventArgs e)
        {
            OnforgetLabelTappedAsync(sender, e);
        }

        public Boolean ChkNullAll()
        {
            bool c = true;
            if (usernameEntry.Text == null || userpasswordEntry.Text == null)
            {
                DisplayAlert("Error", "Required field not be empty", "OK");
                c = false;
            }
            else if (usernameEntry.Text == "" || userpasswordEntry.Text == "")
            {
                DisplayAlert("Error", "Required field not be  empty", "OK");
                c = false;
            }
            else if (usernameEntry.Text == " " || userpasswordEntry.Text == " ")
            {
                DisplayAlert("Error", "Required field not be  empty", "OK");
                c = false;
            }
            return c;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;

            await Task.Delay(1000);


            HttpRequestWrapper wrapper = new HttpRequestWrapper();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            logInModel LogInModel = new logInModel();

            parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));

            parameters.Add(new KeyValuePair<string, string>("phone_no", usernameEntry.Text));
            parameters.Add(new KeyValuePair<string, string>("password", userpasswordEntry.Text));

            parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
            parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
            parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));

            if (ChkNullAll() == true)
            {
                try
                {
                    string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Login], parameters);

                    if (data.ToString() == "NoInternet")
                    {
                        NoInternet.IsVisible = true;
                        MainContainer.IsVisible = false;
                        
                        //await DisplayAlert("No Connection", "Please check your internet connection", "OK");
                        //await Navigation.PushModalAsync(new NoInternetPage());
                    }

                    else
                    {
                        var des = JsonConvert.DeserializeObject<LoginModel>(data);
                        if (des.responseText == "Success")
                        {
                            AppData.IsLogin = true;
                            AppData.UserName = des.data.user_data.name;
                            AppData.UserId = des.data.user_data.user_id;
                            AppData.UserCityId = des.data.user_data.city_id;
                            AppData.UserCity = des.data.user_data.city_name;
                            AppData.Avatar = des.data.user_data.avatar;

                            App.Current.MainPage = new MasterDetailsPage();
                            // Navigation.PushModalAsync(new NewsSubmitPage());
                            usernameEntry.Text = userpasswordEntry.Text = "";
                        }
                        else
                        {
                            DisplayAlert("Error", "Invalid phone Number or Password", "OK");
                        }
                    }
                }

                catch (Exception ex)
                {

                }

            }
            Loader.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            INativeHelper nativeHelper = null;
            nativeHelper = DependencyService.Get<INativeHelper>();
            if (nativeHelper != null)
            {
                nativeHelper.CloseApp();
            }
            nativeHelper.CloseApp();
            base.OnBackButtonPressed();
            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            OnLoginButtonClicked(sender, e);
        }

        private async void Facebook_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FacebookProfilePage());
        }
    }
}