using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.Model.TaazaStoreModel;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressListPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        public AddressListPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            LoadAddressList();
        }

        private async void LoadAddressList()
        {
           
            try
            {
                Loader.IsVisible = true;
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                    new KeyValuePair<string, string>("user_id", AppData.UserId),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UserAddressListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                    NoDataPage.IsVisible = true;
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<UserAddressListModel>(jsonstr);
                    AddressList.ItemsSource = Items.data.address;
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

        private async void AddressListTapped(object sender, ItemTappedEventArgs e)
        {
            var default_addr = ((sender as ListView).SelectedItem as Address_List);
            AddressList.SelectedItem = null;

            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("customer_address_id", default_addr.customer_address_id.ToString()),
                    new KeyValuePair<string, string>("user_id", AppData.UserId),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.SetDefaultAddressAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {

                }

                else

                {
                    await Navigation.PushAsync(new PayModeSelctionPage(default_addr.customer_address_id.ToString(), default_addr.full_name, default_addr.full_addr, default_addr.phone));
                }
            }

            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void DeleteButtonTapped(object sender, EventArgs e)
        {
            var id = ((((sender as Image).Parent as StackLayout).Parent as StackLayout).Children[0] as Label).Text;
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("customer_address_id", id),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.DeleteUserAddressAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    LoadAddressList();
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void AddressNewClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAddressPage(""));
        }

        private async void EditButtonTapped(object sender, EventArgs e)
        {
            Loader.IsVisible = true;
            var id = ((((sender as Image).Parent as StackLayout).Parent as StackLayout).Children[0] as Label).Text;

            await Navigation.PushAsync(new NewAddressPage(id));
            Loader.IsVisible = false;
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            LoadAddressList();
        }
    }
}