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
		public PopUpTaskView ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        private async void Get_OTP_Clicked()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("seller_id", "ANDROID"),
                    new KeyValuePair<string, string>("user_id", "2.0"),
                    new KeyValuePair<string, string>("seller_bill_no", "43"),
                    new KeyValuePair<string, string>("seller_bill_amount", "43"),
                    new KeyValuePair<string, string>("reedem_taaza_cash_amount", "43"),
                    new KeyValuePair<string, string>("type", "43"),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetOfflinePayOTPAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    // Toast to show that the otp is sent successfully
                    // Close the pop up automatically. Save the data ihn cache
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }
    }
}