using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.Model;
using TaazaTV.View;
namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        bool isMobileValidate = false;
        public static string userid = "";

        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private void SetPassword_Tapped(object sender, EventArgs e)
        {

        }

        private async void Validate_Tapped(object sender, EventArgs e)
        {
            Loader.IsVisible = true;
            await Task.Delay(1000);

            if (isMobileValidate)
            {
                //OTP Validate 
                if (!string.IsNullOrEmpty(OTP.Text))
                {
                    HttpRequestWrapper wrapper = new HttpRequestWrapper();
                    List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                    parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                    parameters.Add(new KeyValuePair<string, string>("user_id", userid.ToString()));
                    parameters.Add(new KeyValuePair<string, string>("otp", OTP.Text.Trim()));

                    string jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ForgotPasswordCheckOTP], parameters);
                    if (jsonstr.ToString() == "NoInternet")
                    {

                        NoInternet.IsVisible = true;
                        MainFrame.IsVisible = false;
                       
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<OTPCheckResponseModel>(jsonstr);

                        if (result.responseText == "Success")
                        {

                            await Navigation.PushModalAsync(new ResetPasswordPage(result.data.user_id.ToString()));

                        }
                        else
                        {
                            await DisplayAlert("Error", result.responseText, "Cancel");

                        }
                    }
                }
                else
                {
                    await DisplayAlert("Mandatory", "Please enter an OTP!!", "Cancel");
                }
            }
            else
            {

                if (string.IsNullOrEmpty(Mobile.Text))
                {
                    await DisplayAlert("Error", "Please enter Phone Number", "OK", "Cancel");
                }
                else if (Mobile.Text.ToString().Length == 10)
                {

                    HttpRequestWrapper wrapper = new HttpRequestWrapper();
                    List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                    parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                    parameters.Add(new KeyValuePair<string, string>("phone_no", Mobile.Text.ToString()));

                    string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ForgotPasswordSentOTP], parameters);
                    var des = JsonConvert.DeserializeObject<OTPSentResponseModel>(data);
                    userid = des.data.user_id.ToString();
                    if (des.responseText == "Success")
                    {
                        await DisplayAlert("Success", "OTP has been send successfully.", "OK");
                        OTPContainer.IsVisible = true;
                        MobileContainer.IsEnabled = false;
                        isMobileValidate = true;
                        SubmitButton.Text = "Validate";
                    }
                    else
                    {
                        await DisplayAlert("Error", des.responseText, "Cancel");
                    }
                    // var des = JsonConvert.DeserializeObject<ForgotPasswordModel>(data);

                }


                else
                {
                    await DisplayAlert("Error", "Phone Number does not exist", "OK", "Cancel");
                }
            }
            Loader.IsVisible = false;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Resend_Tapped(object sender, EventArgs e)
        {
            Loader.IsVisible = true;
            await Task.Delay(1000);

            await DisplayAlert("Resend OTP", "OTP has been sent on your mobile no", "Cancel");
            Loader.IsVisible = false;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            Validate_Tapped(sender, e);
           
        }
    }
}