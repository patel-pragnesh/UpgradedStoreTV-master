using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.View.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaCash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayConfirmPage : ContentPage
    {
        int TicketNumbers { get; set; } = 1;
        int TicketAmount { get; set; }
        int MaxTickets { get; set; }
        int TaazaCashPerTicket { get; set; }
        int TotalAmount { get; set; }
        int TaazaRedemed { get; set; } = 0;
        int WalletBalance, NetAmount;
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        string EventID { get; set; }
        int EventPriceID { get; set; }
        int TaazaCashLimit { get; set; }
        string RestID { get; set; }
        DateTime SelecetedDate;
        string SelcetedTime = "";
        bool IsEvent;

        public PayConfirmPage(string RestaurantId, string BannerSrc, string BannerHeading)
        {
            InitializeComponent();
#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif
            SeatsOrTickets.Text = "No. of Seats";
            IsEvent = false;
            TaazaCashAmount.Text = AppData.TaazaCash;
            RestaurantDirected.IsVisible = true;
            RestBookDate.Date = DateTime.Now;
            SelecetedDate = DateTime.Now;
            RestBookDate.MaximumDate = DateTime.Now.AddDays(30);
            RestBookDate.MinimumDate = DateTime.Now;
            RestID = RestaurantId;
            TopImage.Source = BannerSrc;
            TopText.Text = BannerHeading;
            TopSubText.IsVisible = false;
            NoOfTickets.Text = TicketNumbers.ToString();
            LoadRestPaymentDetails();
        }

        private async void LoadRestPaymentDetails()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("company_code", Constant.CompanyID),
                     new KeyValuePair<string, string>("restaurant_id", RestID),
                    new KeyValuePair<string, string>("user_id", AppData.UserId)
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.BookRestaurant], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    // NoInternet.IsVisible = true;
                    // Loader.IsVisible = false;
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<RestaurantDetailModel>(jsonstr);
                    RestBookTime.ItemsSource = Items.data.restaurant_details.time_schedule;
                    string[] token = Items.data.restaurant_details.seat_price.Split('.');
                    AmountPayable.Text = token[0];
                    NetPayable.Text = token[0];
                    TicketAmount = Convert.ToInt32(token[0]);
                    TotalAmount = TicketAmount;
                    NetAmount = TotalAmount;
                    MaxTickets = Convert.ToInt32(Items.data.restaurant_details.available_seat);
                    string[] balance = Items.data.restaurant_details.current_wallet_balance.Split('.');
                    WalletBalance = Convert.ToInt32(balance[0]);
                    TaazaCashLimit = Convert.ToInt32(Items.data.restaurant_details.maximum_taazacash_can_use);


                }
            }

            catch (Exception ex)
            {

            }
        }

        public PayConfirmPage(string EventId, string BannerSrc, string BannerHeading, string SubHeading, int PriceID, int MaxTaazaCash)
        {
            InitializeComponent();
#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif
            IsEvent = true;
            SeatsOrTickets.Text = "No. of Tickets";
            TaazaCashAmount.Text = AppData.TaazaCash;
            EventID = EventId;
            EventPriceID = PriceID;
            TopImage.Source = BannerSrc;
            TopText.Text = BannerHeading;
            TopSubText.Text = SubHeading;
            TaazaCashPerTicket = MaxTaazaCash;
            LoadEventPaymentDetails(EventPriceID);
        }

        private async void LoadEventPaymentDetails(int EventPriceId)
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("company_code", Constant.CompanyID),
                     new KeyValuePair<string, string>("event_id", EventID),
                    new KeyValuePair<string, string>("user_id", AppData.UserId),
                    new KeyValuePair<string, string>("price_id", EventPriceId.ToString())
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.BookEvent], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    // NoInternet.IsVisible = true;
                    // Loader.IsVisible = false;
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<EventDetailsResponseModel>(jsonstr);
                    string[] price = Items.data.event_details.event_price.ticket_price.Split('.');
                    AmountPayable.Text = price[0];
                    NetPayable.Text = price[0];
                    TicketAmount = Convert.ToInt32(price[0]);
                    TotalAmount = TicketAmount;
                    NetAmount = TotalAmount;
                    MaxTickets = Convert.ToInt32(Items.data.event_details.event_price.available_seat);
                    string[] balance = Items.data.event_details.current_wallet_balance.Split('.');
                    WalletBalance = Convert.ToInt32(balance[0]);
                    TaazaCashLimit = Convert.ToInt32(Items.data.event_details.maximum_taazacash_can_use);
                    TaazaRedemed = TaazaCashPerTicket;
                    TaazaPayable.Text = TaazaCashPerTicket.ToString();
                    NoOfTickets.Text = TicketNumbers.ToString();
                }
            }

            catch (Exception ex)
            {

            }
            // You will get AmountPayable here
            //Net Payable is Same as Amount Payable
            // Get Wallet Balance Here
        }

        private void DecreaseTicketNo(object sender, EventArgs e)
        {
            if (TicketNumbers > 1)
            {
                TicketNumbers--;
                NoOfTickets.Text = TicketNumbers.ToString();
                TotalAmount = (TicketAmount * TicketNumbers);
                AmountPayable.Text = TotalAmount.ToString();
                TaazaRedemed = TicketNumbers * TaazaCashPerTicket;

                if (TaazaRedemed <= WalletBalance && TaazaRedemed <= TaazaCashLimit)
                {
                    TaazaPayable.Text = TaazaRedemed.ToString();
                }
                else
                {
                    TaazaRedemed = (WalletBalance < TaazaCashLimit) ? WalletBalance : TaazaCashLimit;
                    TaazaPayable.Text = TaazaRedemed.ToString();
                }

                if ((TotalAmount - TaazaRedemed) >= 0)
                    NetPayable.Text = (TotalAmount - TaazaRedemed).ToString();
            }
        }

        private void IncreaseTicketNo(object sender, EventArgs e)
        {
            if (TicketNumbers < 100 && TicketNumbers < MaxTickets)
            {
                TicketNumbers++;
                NoOfTickets.Text = TicketNumbers.ToString();
                TotalAmount = (TicketAmount * TicketNumbers);
                TaazaRedemed = TicketNumbers * TaazaCashPerTicket;

                if (TaazaRedemed <= WalletBalance && TaazaRedemed <= TaazaCashLimit)
                {
                    TaazaPayable.Text = TaazaRedemed.ToString();
                }
                else
                {
                    TaazaRedemed = (WalletBalance < TaazaCashLimit) ?  WalletBalance : TaazaCashLimit;
                    TaazaPayable.Text = TaazaRedemed.ToString();
                }

                AmountPayable.Text = TotalAmount.ToString();
                NetPayable.Text = (TotalAmount - TaazaRedemed).ToString();
            }
        }

        private void TaazaPayableChanged(object sender, TextChangedEventArgs e)
        {
            int limit = 3;
            ErrorMessage.IsVisible = false;
            try
            {
                if ((TaazaPayable.Text).Length == 0)
                {
                    TaazaRedemed = 0;
                    NetAmount = TotalAmount;
                    NetPayable.Text = NetAmount.ToString();
                }

                else
                {
                    if ((TaazaPayable.Text).Length > limit)
                    {
                        TaazaPayable.Text = TaazaPayable.Text.Substring(0, (TaazaPayable.Text).Length - 1);  // Remove Last character
                        TaazaRedemed = Convert.ToInt32(TaazaPayable.Text);
                    }

                    else
                    {
                        TaazaRedemed = Convert.ToInt32(TaazaPayable.Text);
                    }

                    if (TaazaRedemed <= WalletBalance && TaazaRedemed <= TaazaCashLimit && TaazaRedemed <= TotalAmount)
                    {
                        NetAmount = TotalAmount - TaazaRedemed;
                        NetPayable.Text = NetAmount.ToString();
                    }
                    else
                    {
                        if (TaazaRedemed <= TotalAmount)
                        {
                            if (WalletBalance < TaazaCashLimit)
                            {
                                TaazaPayable.Text = WalletBalance.ToString();
                                TaazaRedemed = Convert.ToInt32(TaazaPayable.Text);
                            }
                            else
                            {
                                TaazaPayable.Text = TaazaCashLimit.ToString();
                                TaazaRedemed = Convert.ToInt32(TaazaPayable.Text);
                            }
                        }

                        else
                        {
                            int value = 0;
                            TaazaPayable.Text = value.ToString();
                            TaazaRedemed = Convert.ToInt32(TaazaPayable.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TaazaPayable.Text = string.Empty;
                TaazaRedemed = 0;
                ErrorMessage.IsVisible = true;
            }
        }

        private void RestBookDateChanged(object sender, DateChangedEventArgs e)
        {
            var picker = (DatePicker)sender;
            SelecetedDate = picker.Date;
        }

        private async void NavToTransactionDetails(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }

        private async void BackBtn_Tapped(object sender, EventArgs e)
        {
            AppData.PickerProblem = false;
            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            AppData.PickerProblem = false;

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }

        private void RestBookTimeChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            SelcetedTime = picker.SelectedItem.ToString();
        }

        private async void DoSomething(object sender, EventArgs e)
        {
            AppData.PickerProblem = false;
            await Navigation.PopAsync();
        }

        private async void CallPaymentGateway(object sender, EventArgs e)
        {
            if (IsEvent)
            {
                if (TaazaRedemed <= WalletBalance && TaazaRedemed <= TaazaCashLimit && NetAmount >= 1)
                {
                    string src = Constant.APIs[(int)Constant.APIName.PaymentEventGateway] + EventID + "&price_id=" + EventPriceID + "&no_of_seat=" + TicketNumbers + "&user_id=" + AppData.UserId + "&deduct_wallet_amount=" + TaazaRedemed;
                    await Navigation.PushAsync(new PaymentGatewayNav(src));
                }
                else
                {
                    ErrorMessage.IsVisible = true;
                }
            }

            else
            {
                if (SelcetedTime == "")
                    await DisplayAlert("Error", "Please Select Time Slot", "OK");
                else
                {
                    if (TaazaRedemed <= WalletBalance && TaazaRedemed <= TaazaCashLimit && NetAmount >= 1)
                    {

                        string src = Constant.APIs[(int)Constant.APIName.PaymentRestGateway] + RestID + "&no_of_seat=" + TicketNumbers + "&user_id=" + AppData.UserId + "&date_of_booking=" + SelecetedDate + "&time_of_booking=" + SelcetedTime + "&deduct_wallet_amount=" + TaazaRedemed;
                        await Navigation.PushAsync(new PaymentGatewayNav(src));
                    }
                    else
                    {
                        ErrorMessage.IsVisible = true;
                    }
                }
            }


        }

        //private async void CallPaymentGateway(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
        //        {
        //             new KeyValuePair<string, string>("event_id", EventID),
        //            new KeyValuePair<string, string>("user_id", AppData.UserId),
        //             new KeyValuePair<string, string>("no_of_seat", TicketNumbers.ToString()),
        //              new KeyValuePair<string, string>("wallet_amount", WalletBalance.ToString())
        //        };

        //        var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.PaymentGatewayCall], parameters);
        //        if (jsonstr.ToString() == "NoInternet")
        //        {
        //            // NoInternet.IsVisible = true;
        //            // Loader.IsVisible = false;
        //        }
        //        else
        //        {
        //            object dec = JsonConvert.DeserializeObject(jsonstr); // deserializing Json string (it will deserialize Json string)
        //            JObject obj = JObject.Parse(dec.ToString()); // it will parse deserialize Json object
        //            if (obj["responseText"].ToString() == "Success")
        //            {
        //                JObject obj2 = JObject.Parse(obj["data"].ToString());  // now after parsing deserialize Json object you can get individual values by key i.e.
        //                var x = obj2["payurl"].ToString();
        //                await Navigation.PushAsync(new PaymentGatewayNav(x));
        //            }
        //        }
        //    }

        //    catch
        //    {

        //    }
        //}
    }
}