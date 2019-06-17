using Newtonsoft.Json;
using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using TaazaTV.View.Navigation;
using TaazaTV.View.TaazaCash;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();

        public OrdersPage()
        {
            InitializeComponent();
            TaazaCashAmount.Text = AppData.TaazaCash;
            LoadOrdersHistory();
        }

        private async void LoadOrdersHistory()
        {
            try
            {
                Loader.IsVisible = true;
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.OrdersHistoryAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                    await DisplayAlert("Alert", "Server Error", "OK");

                }

                else
                {
                    var des = JsonConvert.DeserializeObject<OrdersModel>(jsonstr);
                    if (des.responseText == "Success")
                    {
                        Loader.IsVisible = false;
                        OrdersListView.ItemsSource = des.data.order_data;
                    }
                    else
                    {
                        Loader.IsVisible = false;
                        await DisplayAlert("Alert", des.responseText, "OK");
                    }
                }
            }

            catch (Exception ex)
            {
                Loader.IsVisible = false;
                await DisplayAlert("Alert", "Server Error", "OK");
            }
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            LoadOrdersHistory();
        }

        private void BackBtnTapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new MasterDetailsPage();
        }

        private async void TaazaCashTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }

        private async void CancelOrderClicked(object sender, EventArgs e)
        {
            if(((sender as Label).TextColor) == Color.Black)
            {
                var action = await DisplayActionSheet("Do you want to cancel this order!!", "Yes", "No");
                if (action == "Yes")
                {
                    try
                    {
                        Loader.IsVisible = true;
                        List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                        parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                        parameters.Add(new KeyValuePair<string, string>("order_history_id", (((sender as Label).Parent as StackLayout).Children[0] as Label).Text.ToString()));
                        var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.CancelOrderAPI], parameters);
                        if (jsonstr.ToString() == "NoInternet")
                        {
                            Loader.IsVisible = false;
                            await DisplayAlert("Alert", "Server Error", "OK");

                        }
                        else
                        {
                            var des = JsonConvert.DeserializeObject<SuccessResponseModel>(jsonstr);
                            if (des.responseText == "Success")
                            {
                                Loader.IsVisible = false;
                                LoadOrdersHistory();
                            }
                            else
                            {
                                Loader.IsVisible = false;
                                await DisplayAlert("Alert", des.responseText, "OK");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Loader.IsVisible = false;
                        await DisplayAlert("Alert", "Server Error", "OK");
                    }

                }
            }
           
        }

        private void OrderListItemTapped(object sender, ItemTappedEventArgs e)
        {
            OrdersListView.SelectedItem = null;
        }

        private void SupportOrderClicked(object sender, EventArgs e)
        {
            var EmailTask = CrossMessaging.Current.EmailMessenger;

            if (EmailTask.CanSendEmail)
                EmailTask.SendEmail((((sender as Label).Parent as StackLayout).Children[1] as Label).Text.ToString(), "Support Regarding Order No. - " + (((sender as Label).Parent as StackLayout).Children[0] as Label).Text.ToString(), "Messsage Body");

        }
    }
}