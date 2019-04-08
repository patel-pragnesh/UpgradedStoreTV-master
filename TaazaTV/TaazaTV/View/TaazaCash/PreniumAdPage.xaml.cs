using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaCash
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PreniumAdPage : ContentPage
	{
        public string PreniumAdWebSource { get; set; }

        public PreniumAdPage(string imgsource, string urlsource)
        {
            InitializeComponent();
          //  AppData.IsPreniumAdOpen = true;
            PreniumAdWebSource = urlsource;
            AdBannerWebView.Source = imgsource;
        }

        private async void CloseBannerTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
          //  AppData.IsPreniumAdOpen = false;
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    AppData.IsPreniumAdOpen = false;
        //    return true;
        //}

        private async void NavToAdContent(object sender, EventArgs e)
        {
            if (PreniumAdWebSource != "")
            {
                try
                {
                    Device.OpenUri(new Uri(PreniumAdWebSource));
                    await Navigation.PopAsync();
                }

                catch
                {
                    await Navigation.PopAsync();
                 //   AppData.IsPreniumAdOpen = false;
                }
               
            }
        }
    }
}