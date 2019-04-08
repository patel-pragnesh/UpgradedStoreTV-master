using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TaazaTV.View.Navigation;

namespace TaazaTV.View.Tools
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivacyPolicyPage : ContentPage
    {

        
        public PrivacyPolicyPage()
        {
            InitializeComponent();
            LoadStatic();

        }

        private async void LoadStatic()
        {
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            MainFrame.IsVisible = false;
            await Task.Delay(1000);
            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                var jsonstr = await wrapper.GetResponseAsync("http://api.taazatv.com/public/api/V2/static-content/privacy-policy?company_code=COM1000", "GET");
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                    header.IsVisible = false;
                }
                else
                {
                    var html = new HtmlWebViewSource
                    {
                        Html = @"<html>
                <head>
                      <meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/>
                </head>
                <body height='100%'>"
                           + jsonstr
                           + @"</body>
           </html>"
                    };
                    PrivacyDetails.Source = html;
                    MainFrame.IsVisible = true;
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Internal server error", "Please try again later", "Cancel");
            }

            MainFrame.IsVisible = true;
            header.IsVisible = true;
            bar.IsVisible = true;
            Loader.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                App.Current.MainPage = new MasterDetailsPage();
            });

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }
        private void DoSomething(object sender, EventArgs e)
        {
            LoadStatic();
        }

        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MasterDetailsPage();
        }
    }
}