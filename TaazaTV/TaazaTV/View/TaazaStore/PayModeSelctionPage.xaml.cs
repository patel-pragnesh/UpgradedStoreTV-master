﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using TaazaTV.View.TaazaCash;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayModeSelctionPage : ContentPage
    {

        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        StoreCartModel Items = new StoreCartModel();
        string addrId;

        public PayModeSelctionPage(string addr_id, string full_name, string full_addr, string phone)
        {
            InitializeComponent();
            addrId = addr_id;
            DName.Text = full_name;
            DAddr.Text = full_addr;
            DPhone.Text = phone;
            LoadDataFromCartPage();
        }

        private async void LoadDataFromCartPage()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("user_id", AppData.UserId),
                    new KeyValuePair<string, string>("group_id", ""),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetCartHistoryAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    Items = JsonConvert.DeserializeObject<StoreCartModel>(jsonstr);
                    Amount.Text = Items.data.cart_data.total_price.ToString();
                    ItemCount.Text = Items.data.cart_data.history_data.Count().ToString();
                    TLabel.Text = Items.data.cart_data.total_taazacash_can_use.ToString();

                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void CallPaymentApI(object sender, EventArgs e)
        {
            if (AddressType.SelectedIndex == 0)
            {
                var x = Constant.APIs[(int)Constant.APIName.CallStorePaymentGateway] + "?user_id=" + AppData.UserId + "&cart_id=" + Items.data.cart_data.cart_id + "&address_id=" + addrId + "&mode=" + "OLP" + "&deduct_wallet_amount=" + TaazaPayable.Text;
                await Navigation.PushAsync(new PaymentGatewayNav(x));
            }
            if (AddressType.SelectedIndex == 1)
            {
                var x = Constant.APIs[(int)Constant.APIName.CallStorePaymentGateway] + "?user_id=" + AppData.UserId + "&cart_id=" + Items.data.cart_data.cart_id + "&address_id=" + addrId + "&mode=" + "COD" + "&deduct_wallet_amount=" + TaazaPayable.Text;
                await Navigation.PushAsync(new PaymentGatewayNav(x));
            }
            if (AddressType.SelectedIndex == -1)
            {
                await DisplayAlert("Alert","Please select mode of payment","OK");
            }
        }
           

        private void TaazaPaymentChanged(object sender, FocusEventArgs e)
        {
            try
            {
                string[] tcash = AppData.TaazaCash.Split('.');

                if (Convert.ToInt32((sender as Entry).Text) <= Items.data.cart_data.total_taazacash_can_use && Convert.ToInt32((sender as Entry).Text) <= Convert.ToInt32(tcash[0]))
                {
                    Amount.Text = (Items.data.cart_data.total_price - Convert.ToInt32((sender as Entry).Text)).ToString();
                }

                else
                {
                    TaazaPayable.Text = "0";
                    Amount.Text = Items.data.cart_data.total_price.ToString();
                }
            }
            catch
            {
                Amount.Text = Items.data.cart_data.total_price.ToString();
            }
        }
    }
}