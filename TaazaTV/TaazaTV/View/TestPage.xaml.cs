using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //activityIndicator.IsVisible = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //activityIndicator.IsVisible = true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsVisible = true;
            await Task.Delay(1000);

            HttpRequestWrapper wrapper = new HttpRequestWrapper();

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("company_code", "COM1000"));
            parameters.Add(new KeyValuePair<string, string>("phone_no", "8481807716"));
            parameters.Add(new KeyValuePair<string, string>("password", "123456"));
            parameters.Add(new KeyValuePair<string, string>("device_type", "Android"));
            parameters.Add(new KeyValuePair<string, string>("device_token", "1234567890"));
            parameters.Add(new KeyValuePair<string, string>("app_version", "1.1"));

            string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Login], parameters);
            try
            {
                LabelText.Text = data.ToString();
                activityIndicator.IsVisible = false;
                DisplayAlert("Success", "Data Successfully Fetched", "OK", "Cancel");
            }
            catch (Exception ex)
            {

            }

        }
    }
}