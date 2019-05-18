using Newtonsoft.Json;
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
    public partial class CartPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();

        public CartPage()
        {
            InitializeComponent();
            LoadInitialDetails();
        }

        private async void LoadInitialDetails()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("user_id", AppData.UserId),
                    new KeyValuePair<string, string>("group_id", "42"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetCartHistoryAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<StoreCartModel>(jsonstr);
                    CartListView.ItemsSource = Items.data.cart_data.history_data;
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddressListPage());
        }

        private void Decrease_CountTapped(object sender, EventArgs e)
        {

        }

        private void IncreaseCountTapped(object sender, EventArgs e)
        {

        }

        private void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }

        private void RemoveItemClicked(object sender, EventArgs e)
        {

        }
    }
}