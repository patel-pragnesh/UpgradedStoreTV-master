using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.ViewModel;
using TaazaTV.Model;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using TaazaTV.View.Navigation;

namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacebookProfilePage : ContentPage
    {
        /// <summary>
        /// Make sure to get a new ClientId from:
        /// https://developers.facebook.com/apps/
        /// </summary>
        private string ClientId = "1573474726045452";
        //private string ClientId = "142865696297893";

        public FacebookProfilePage()
        {
            InitializeComponent();
            BindingContext = new FacebookViewModel();
            //socialogin();            
        }

        public async void socialogin()
        {
            //ActivityIndicator.IsVisible = true;

            HttpRequestWrapper wrapper = new HttpRequestWrapper();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            FacebookProfile FacebookProfile = new FacebookProfile();

            parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
            parameters.Add(new KeyValuePair<string, string>("f_name", FacebookProfile.FirstName));
            parameters.Add(new KeyValuePair<string, string>("l_name", FacebookProfile.LastName));

            parameters.Add(new KeyValuePair<string, string>("phone_no", FacebookProfile.PhoneNo));
            parameters.Add(new KeyValuePair<string, string>("email_id", FacebookProfile.Emailid));
            parameters.Add(new KeyValuePair<string, string>("social_login_type", "1"));
            parameters.Add(new KeyValuePair<string, string>("social_user_id", FacebookProfile.Id));

            parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
            parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
            parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));

            try
            {
                string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.socialLogin], parameters);
                //if (data.ToString() == "NoInternet")
                //{
                //    NoInternet.IsVisible = true;
                //    MainContainer.IsVisible = false;

                //    //await DisplayAlert("No Connection", "Please check your internet connection", "OK");
                //    //await Navigation.PushModalAsync(new NoInternetPage());
                //}

                var des = JsonConvert.DeserializeObject<facebooklogin>(data);
                if (des.responseText == "Success")
                {
                    AppData.IsLogin = true;
                    AppData.UserName = des.data.user_data.name;
                    AppData.UserId = des.data.user_data.user_id;
                    AppData.UserCityId = des.data.user_data.city_id;
                    AppData.UserCity = des.data.user_data.city_name;
                    AppData.Avatar = FacebookProfile.Picture.ToString();

                    App.Current.MainPage = new MasterDetailPage();
                }
                else
                {
                    await DisplayAlert("Error", des.responseText, "Cancel");
                }
            }
            catch (Exception ex)
            {

            }
            //var apiRequest =
            //    "https://www.facebook.com/dialog/oauth?client_id="
            //    + ClientId
            //    + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html";

            //var webView = new WebView
            //{
            //    Source = apiRequest,
            //    HeightRequest = 1
            //};
            //    webView.Source = apiRequest;
            webView.HeightRequest = 1;

            webView.Navigated += WebViewOnNavigated;
            webView.Navigating += WebViewOnNavigating;

            //Content = webView;
            webView.IsVisible = true;
            activityIndicator.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            activityIndicator.IsVisible = true;

            var apiRequest =
                "https://www.facebook.com/dialog/oauth?client_id="
                + ClientId
                + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html&scope=email";

            webView.Source = apiRequest;
            webView.HeightRequest = 1;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            activityIndicator.IsVisible = true;
        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            webView.IsVisible = true;
            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                
                var vm = BindingContext as FacebookViewModel;

                await vm.SetFacebookUserProfileAsync(accessToken);

                //socialogin();

                App.Current.MainPage = new MasterDetailsPage();

            }
            activityIndicator.IsVisible = false;
        }

        private async void WebViewOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            activityIndicator.IsVisible = true;
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace("http://www.facebook.com/connect/login_success.html#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;
            }
            
            return string.Empty;
        }

        private async void BackButton(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
