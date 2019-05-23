using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SellerListPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();

        public SellerListPage()
        {
            InitializeComponent();
            InitialLoading("");
        }

        private async void InitialLoading(string seachText)
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                    new KeyValuePair<string, string>("search", seachText),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetSellerListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {

                }

                else
                {
                    var Items = JsonConvert.DeserializeObject<SellerListModel>(jsonstr);
                    SellersListView.ItemsSource = Items.data.sellers;
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void SellerListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SellersListView.SelectedItem = null;
            var model = e.Item as Seller_Data;
            await Navigation.PushAsync(new SellerDetailsPage(model.seller_id.ToString()));
        }

        private void Seller_Search_Clicked(object sender, EventArgs e)
        {
            InitialLoading((sender as SearchBar).Text);
        }

        //private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    scroller.ScrollToAsync(182, 0, true);
        //    for (int i = 0; i < 10; i++)
        //    {
        //        StackLayout a = new StackLayout();
        //        var image = new Image { Source = "http://pngimg.com/uploads/cat/cat_PNG50550.png", HeightRequest = 40, WidthRequest = 40 };
        //        Label xyz = new Label();
        //        xyz.Text = "SomeThing";
        //        a.Children.Add(image);
        //        a.Children.Add(xyz);
        //        scrollerstack.Children.Add(a);
        //    }
        //}
    }
}
