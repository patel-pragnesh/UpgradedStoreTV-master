using System.Linq;
using TaazaTV.Model;
using TaazaTV.View.Navigation;
using TaazaTV.ViewModel;
using Xamarin.Forms;


namespace TaazaTV.View.Accounts
{
    public class FacebookProfileCsPage : ContentPage
    {
        /// <summary>
        /// Make sure to get a new ClientId from:
        /// https://developers.facebook.com/apps/
        /// </summary>
        private string ClientId = "165942640479284";

        public FacebookProfileCsPage()
        {

            BindingContext = new FacebookViewModel();
            
            Title = "Facebook Profile";
            BackgroundColor = Color.White;

            var apiRequest =
                "https://www.facebook.com/dialog/oauth?client_id="
                + ClientId
                + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html";

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            Content = webView;
        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                var vm = BindingContext as FacebookViewModel;

                await vm.SetFacebookUserProfileAsync(accessToken);

                App.Current.MainPage = new MasterDetailsPage();
                
            }
        }

        //private void SetPageContent(FacebookProfile facebookProfile)
        //{
        //    Content = new StackLayout
        //    {
        //        Orientation = StackOrientation.Vertical,
        //        Padding = new Thickness(8, 30),
        //        Children =
        //        {
        //            new Label
        //            {
        //                Text = facebookProfile.Name,
        //                TextColor = Color.Black,
        //                FontSize = 22,
        //            },
        //            new Label
        //            {
        //                Text = facebookProfile.Id,
        //                TextColor = Color.Black,
        //                FontSize = 22,
        //            },
        //            new Label
        //            {
        //                Text = facebookProfile.IsVerified.ToString(),
        //                TextColor = Color.Black,
        //                FontSize = 22,
        //            },
        //            //new Label
        //            //{
        //            //    Text = facebookProfile.Devices.FirstOrDefault().Os,
        //            //    TextColor = Color.Black,
        //            //    FontSize = 22,
        //            //},
        //            new Label
        //            {
        //                Text = facebookProfile.Gender,
        //                TextColor = Color.Black,
        //                FontSize = 22,
        //            },
        //            new Label
        //            {
        //                Text = facebookProfile.AgeRange.Min.ToString(),
        //                TextColor = Color.Black,
        //                FontSize = 22,
        //            },
        //            new Label
        //            {
        //                Text = facebookProfile.Picture.Data.Url,
        //                TextColor = Color.Black,
        //                FontSize = 22,
        //            },
        //            new Label
        //            {
        //                Text = facebookProfile.Cover.Source,
        //                TextColor = Color.Black,
        //                FontSize = 22,
        //            },
        //        }
        //    };
        //}

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
    }
}

