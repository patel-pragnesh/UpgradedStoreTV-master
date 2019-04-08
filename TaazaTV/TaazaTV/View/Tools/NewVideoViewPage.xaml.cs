using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.View.News;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.Tools
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewVideoViewPage : ContentPage
	{
		public NewVideoViewPage ()
		{
			InitializeComponent ();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza Live TV";
            LiveTVWebView.Source = "http://www.zengatv.com/embed?v=5d9eebd0-313d-11e1-8f87-1231400424bd.html&t=live";

        }

        private void NoInternet_ClickedButton(object sender, EventArgs e)
        {

        }

        private void NoDataPage_BackClickedButton(object sender, EventArgs e)
        {

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new VideoViewPage("http://www.zengatv.com/embed?v=5d9eebd0-313d-11e1-8f87-1231400424bd.html&t=live"));
        }

        private void LiveTVWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            WebViewLoader.IsVisible = false;
        }
    }
}