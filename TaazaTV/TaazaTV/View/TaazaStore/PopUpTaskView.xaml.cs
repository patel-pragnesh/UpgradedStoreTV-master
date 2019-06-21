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
    public partial class PopUpTaskView
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        string RestSellId, RestSellType;

        public PopUpTaskView(string ID, string Type)
        {
            InitializeComponent();
            RestSellId = ID;
            RestSellType = Type;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            AppData.PopUpProblem = true;
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            AppData.PopUpProblem = false;
        }

        private async void GetOtpClicked(object sender, EventArgs e)
        {
            try
            {
                if(Invoice.Text != null && Billing.Text != null && TaazaRedeemed.Text != null)
                {
                    Loader.IsVisible = true;
                    string[] token = AppData.TaazaCash.Split('.');
                    if (Convert.ToInt32(token[0]) >= Convert.ToInt32(TaazaRedeemed.Text))
                    {
                        List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                        {
                          new KeyValuePair<string, string>("seller_id", RestSellId),
                          new KeyValuePair<string, string>("user_id", AppData.UserId),
                          new KeyValuePair<string, string>("seller_bill_no", Invoice.Text),
                          new KeyValuePair<string, string>("seller_bill_amount", Billing.Text),
                          new KeyValuePair<string, string>("reedem_taaza_cash_amount", TaazaRedeemed.Text),
                          new KeyValuePair<string, string>("type", RestSellType),
                        };

                        var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetOfflinePayOTPAPI], parameters);
                        if (jsonstr.ToString() == "NoInternet")
                        {
                            Loader.IsVisible = false;
                        }
                        else
                        {
                            var Model = JsonConvert.DeserializeObject<OtpResponseModel>(jsonstr);
                            if (Model.responseText == "Success")
                            {
                                OtpLabel.Text = Model.otp;
                                ReedemStack.IsVisible = false;
                                OtpStack.IsVisible = true;
                                GetOtpButton.IsVisible = false;
                               
                            }
                            Loader.IsVisible = false;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Alert", "You don't have sufficient Taaza Cash", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Alert", "Please fill the necessary fields!!", "OK");
                }
                
            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                var x = ex.Message;
            }
        }

        private async void CancelClicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        private async void ChangeNetPay(object sender, FocusEventArgs e)
        {
            try
            {
                if(Convert.ToInt32(Billing.Text) > Convert.ToInt32(TaazaRedeemed.Text))
                {
                    NetPay.Text = (Convert.ToInt32(Billing.Text) - Convert.ToInt32(TaazaRedeemed.Text)).ToString();
                }

                else
                {
                    await DisplayAlert("Alert", "Taaza Cash amount cannot be more than Billing Amount!!", "OK");
                }
            }

            catch
            {
                await DisplayAlert("Alert", "Please enter a valid amount", "OK");
            }
        }

    }
}
