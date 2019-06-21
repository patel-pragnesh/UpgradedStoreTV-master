using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.Model.TaazaStoreModel;
using TaazaTV.View.Navigation;
using TaazaTV.View.TaazaStore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaCash
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentGatewayNav : ContentPage
	{
        bool navHelper;

        public PaymentGatewayNav (string urlsource, bool navhelper)
		{
			InitializeComponent ();
            navHelper = navhelper;
#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif
            TaazaCashAmount.Text = AppData.TaazaCash;
            PayWebPage.Source = urlsource;
        }

        private void BackBtn_Tapped(object sender, EventArgs e)
        {
            JustDoIT(navHelper);
        }

        // This code is to update taaza cash. payment done or not done both cases!!!!

        private async void JustDoIT(bool helper)
        {
            try
            {
                Loader.IsVisible = true;
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Profile], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Navigation.PopAsync();
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<ProfileResponse>(jsonstr);
                    AppData.TaazaCash = Items.data.user_data.current_wallet_balance;
                    Loader.IsVisible = false;
                    if (helper)
                    {
                        await Navigation.PushAsync(new OrdersPage());
                    }
                   
                    else
                    {
                        await Navigation.PushAsync(new TaazaTransactionPage());
                    }
                }
            }

            catch (Exception ex)
            {
                Loader.IsVisible = false;
                await Navigation.PushAsync(new TaazaTransactionPage());
            }

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetCartHistoryAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<StoreCartModel>(jsonstr);
                    if (Items.data.cart_data.history_data.Count() == 0)
                    {
                        AppData.CartCount = String.Empty;
                    }
                    else
                    {
                        AppData.CartCount = Items.data.cart_data.history_data.Count().ToString();
                    }
                       

                }
            }

            catch (Exception ex)
            {

            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                JustDoIT(navHelper);
            });
           
           
            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }
    }
}