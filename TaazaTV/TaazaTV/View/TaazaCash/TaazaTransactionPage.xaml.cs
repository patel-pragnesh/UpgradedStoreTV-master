using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.Model;
using Newtonsoft.Json;
using TaazaTV.View.Navigation;

namespace TaazaTV.View.TaazaCash
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TaazaTransactionPage : ContentPage
	{
        TaazaTransactionsModel Items1 = new TaazaTransactionsModel();
        AllTransactions Items2 = new AllTransactions();
        bool IsNoDataVisible;

        public TaazaTransactionPage ()
		{
			InitializeComponent ();
#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif
            TaazaCashAmount.Text = AppData.TaazaCash;
            TranscationsList.IsVisible = true;
            BookingsList.IsVisible = false;
            GetTaazaTransactions();
            GetBookingTransactions();
        }

        private async void GetTaazaTransactions()
        {
            try
            {
               
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.TaazaTransactions], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                  
                }
                else
                {
                    try
                    {
                        Items1 = JsonConvert.DeserializeObject<TaazaTransactionsModel>(jsonstr);
                    }
                    catch (Exception ex)
                    {

                    }

                    TranscationsList.ItemsSource = Items1.data.transaction;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void GetBookingTransactions()
        {

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.AllTransactionsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    //NewsDetailslbl.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items2 = JsonConvert.DeserializeObject<AllTransactions>(jsonstr);
                    }
                    catch (Exception ex)
                    {

                    }
                    if (Items2.data.booking_data.Count() == 0)
                    {
                        IsNoDataVisible = true;
                    }
                        
                    else
                    {
                        BookingsList.ItemsSource = Items2.data.booking_data;
                        IsNoDataVisible = false;
                    }
                   
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Bookings_Clicked(object sender, EventArgs e)
        {

            TranscationsList.IsVisible = false;
            if (IsNoDataVisible)
                NoDataPage.IsVisible = true;
            BookingsList.IsVisible = true;
        }

        private void Transactions_Clicked(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            TranscationsList.IsVisible = true;
            BookingsList.IsVisible = false;
        }

        private async void BackBtn_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                App.Current.MainPage = new MasterDetailsPage();
            });
        }

        private async void NavToBookingDetails(object sender, ItemTappedEventArgs e)
        {
            var model = (AllBookings)e.Item;
            BookingsList.SelectedItem = null;
            await Navigation.PushAsync(new BookingDetailsPage(model));
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

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                App.Current.MainPage = new MasterDetailsPage();
            });
        }
    }
}
