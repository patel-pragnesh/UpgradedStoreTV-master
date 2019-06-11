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
        StoreCartModel Items;
        string pid, sid, quan;

        public CartPage()
        {
            InitializeComponent();
            LoadInitialDetails();
        }

        private async void LoadInitialDetails()
        {
            try
            {
                CartListView.IsVisible = false;
                Items = new StoreCartModel();
                Loader.IsVisible = true;
                await Task.Delay(1000);
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("user_id", AppData.UserId),
                    new KeyValuePair<string, string>("group_id", ""),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetCartHistoryAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                    CartListView.IsVisible = false;
                    NoDataPage.IsVisible = true;
                }
                else
                {
                    Items = JsonConvert.DeserializeObject<StoreCartModel>(jsonstr);
                    CartListView.ItemsSource = Items.data.cart_data.history_data;
                    CartListView.IsVisible = true;
                    Loader.IsVisible = false;
                    AppData.CartCount = Items.data.cart_data.history_data.Count().ToString();
                }

            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                CartListView.IsVisible = false;
                NoDataPage.IsVisible = true;
                var x = ex.Message;
            }
        }

        private async void CheckOut_Clicked(object sender, EventArgs e)
        {
            if(Items.data.cart_data.history_data.Count() > 0)
            await Navigation.PushAsync(new AddressListPage());
        }

        private void Decrease_CountTapped(object sender, EventArgs e)
        {
            pid = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[0] as Label).Text;
            sid = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[1] as Label).Text;
            quan = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[2] as Label).Text;
            quan = (Convert.ToInt32(quan) - 1).ToString();
            ModifyItemCount(pid, sid, quan, "add");
        }

        private void IncreaseCountTapped(object sender, EventArgs e)
        {
            pid = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[0] as Label).Text;
            sid = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[1] as Label).Text;
            quan = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[2] as Label).Text;
            quan = (Convert.ToInt32(quan) + 1).ToString();
            ModifyItemCount(pid, sid, quan, "add");
        }

        private void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            LoadInitialDetails();
        }

        private async void RemoveItemClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Do you want to delete this item", "Yes", "No");
            if(action == "Yes")
            {
                pid = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[0] as Label).Text;
                sid = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[1] as Label).Text;
                quan = "0";
                ModifyItemCount(pid, sid, quan, "remove");
            }
        }

        private async void ModifyItemCount(string productId, string skuId, string quantity, string mode)
        {
            try
            {
                Loader.IsVisible = true;
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                     new KeyValuePair<string, string>("user_id", AppData.UserId),
                     new KeyValuePair<string, string>("mode", mode),
                     new KeyValuePair<string, string>("product_id", productId),
                     new KeyValuePair<string, string>("product_sku_id", skuId),
                     new KeyValuePair<string, string>("quantity", quantity),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.AddRemoveCartAPI], parameters);
            
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                }

                else
                {
                   
                    Loader.IsVisible = false;
                    LoadInitialDetails();
                }
            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                await DisplayAlert("Alert", "Some Error Occured!!", "OK");
            }
        }
    }
}
