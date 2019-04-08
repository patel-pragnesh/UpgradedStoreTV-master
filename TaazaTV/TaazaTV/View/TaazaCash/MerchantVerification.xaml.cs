using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaCash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantVerification : ContentPage
    {
        public MerchantVerification()
        {
            InitializeComponent();
        }


        protected override bool OnBackButtonPressed()
        {
            try
            {
                // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await DisplayAlert("Exit page?", "Are you sure you want to exit this page? You will not be able to continue it.", "Yes", "No"))
                    {
                        INativeHelper nativeHelper = null;
                        nativeHelper = DependencyService.Get<INativeHelper>();
                        if (nativeHelper != null)
                        {
                            nativeHelper.CloseApp();
                        }
                        base.OnBackButtonPressed();

                        //await Navigation.PopAsync();
                    }
                });
            }

            catch (Exception ex)
            {
                Logging.Write(ex, "OnBackButtonPressed");
            }
            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }

        private async void CheckUniqueKey(object sender, EventArgs e)
        {
            if (UniqueCode.Text == null)
            {
                await DisplayAlert("Failure", "Please Enter Booking ID", "OK");
            }
            else
            {
                Loader.IsVisible = true; ;
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("merchant_id", AppData.MerchantUserId));
                parameters.Add(new KeyValuePair<string, string>("booking_id", UniqueCode.Text));

                string jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.MerchantCodeVerification], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                }
                else
                {
                    try
                    {
                        object dec = JsonConvert.DeserializeObject(jsonstr); // deserializing Json string (it will deserialize Json string)
                        JObject obj = JObject.Parse(dec.ToString()); // it will parse deserialize Json object
                        if (obj["responseText"].ToString() == "Success")
                        {

                            await DisplayAlert("Success", "It is a Valid Ticket", "OK");
                            //JObject obj2 = JObject.Parse(obj["data"].ToString());  // now after parsing deserialize Json object you can get individual values by key i.e.
                            //var x = obj2["payurl"].ToString();
                            //await Navigation.PushAsync(new PaymentGatewayNav(x));
                        }

                        else
                        {
                            await DisplayAlert("Failure", "It is not a Valid Ticket", "OK");
                        }
                    }
                    catch
                    {
                        await DisplayAlert("Internal server error", "Please try again later", "Cancel");
                    }
                }
                Loader.IsVisible = false;
            }
        }

        private void DoSomething(object sender, EventArgs e)
        {
            InitializeComponent();
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }
    }
}