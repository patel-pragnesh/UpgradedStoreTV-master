using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Share;
using TaazaTV.Helper;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoViewPage : ContentPage
    {
        public VideoViewPage(string Source)
        {
            InitializeComponent();

            Media.Source = Source;
        }

        private async void Media_Navigated(object sender, WebNavigatedEventArgs e)
        {
            Loader.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                Media.Source = "about:blank";
                await Navigation.PopModalAsync();
            });

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }

        private async void BackButton(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }        
    }
}