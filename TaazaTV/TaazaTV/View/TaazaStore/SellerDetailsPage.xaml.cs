using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellerDetailsPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        string SellerID;
        public SellerDetailsPage(string ID)
        {
            InitializeComponent();
            SellerID = ID;
            InitialLoading();
        }

        //protected async override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    await PopupNavigation.PopAsync(true);
        //}

        private async void InitialLoading()
        {
            try
            {
                Loader.IsVisible = true;
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                    new KeyValuePair<string, string>("seller_id", SellerID),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetSellerDetailsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                    NoDataPage.IsVisible = true;
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<SellerDetailsModel>(jsonstr);
                    this.BindingContext = Items.data;

                    var html = new HtmlWebViewSource
                    {
                        Html = "<iframe width=\"100%\" height=\"232\" frameborder=\"0\" scrolling=\"no\" marginheight=\"0\" marginwidth=\"0\" src = \"https://maps.google.com/maps?q=" + Items.data.latitude + "," + Items.data.latitude + "&hl=es;z=14&amp;output=embed\" ></ iframe > "
                    };
                    WebMap.Source = html;
                    Loader.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                NoDataPage.IsVisible = true;
                var x = ex.Message;
            }
        }

        private async void OpenPopUpPay(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new PopUpTaskView(SellerID, "1"), true);
        }

        private async void ViewSellerProducts(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductListPage("", "", SellerID));
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            InitialLoading();
        }
    }
}