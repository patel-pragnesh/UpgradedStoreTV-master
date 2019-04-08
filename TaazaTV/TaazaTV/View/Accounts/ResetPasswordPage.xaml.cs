using Newtonsoft.Json;
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

namespace TaazaTV.View.Accounts
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResetPasswordPage : ContentPage
	{
		public ResetPasswordPage (string userid=null)
        {
            InitializeComponent();
            if(!string.IsNullOrEmpty(userid))
            {
                lblusrid.Text = userid;
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
           
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            //App.Current.MainPage = new MasterDetailsPage();
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);

            if (AppData.UserId != null)
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();

                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

                if (ChkNullAll() == true)
                {
                    if (!IsPasswordsEqual(Password.Text, RePassword.Text))
                    {
                        await DisplayAlert("Error", "Re-password does not matched!!", "Cancel");
                    }
                    else
                    {
                        parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                        parameters.Add(new KeyValuePair<string, string>("user_id", lblusrid.Text.Trim()));
                        parameters.Add(new KeyValuePair<string, string>("password", Password.Text));
                        //App.Current.MainPage = new MasterDetailsPage();
                        try
                        {
                            string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ForgotPasswordUpdatePassword], parameters);
                            if (data.ToString() == "NoInternet")
                            {
                                NoInternet.IsVisible = true;
                                MainContainer.IsVisible = false;
                            }
                            else
                            {


                                var des = JsonConvert.DeserializeObject<jsonResponseClass>(data);

                                if (des.responseText == "Success")
                                {
                                    await DisplayAlert("Success", "Password has been changed successfully.", "OK");
                                    App.Current.MainPage = new LoginPage();
                                }
                                else
                                {
                                    await DisplayAlert("Error", des.responseText, "Cancel");
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
            }
            Loader.IsVisible = false;
        }


        public Boolean ChkNullAll()
        {
            bool c = true;
            if ( string.IsNullOrEmpty(Password.Text) || string.IsNullOrEmpty(RePassword.Text))
            {
                DisplayAlert("Error", "Required field not be empty", "OK");
                c = false;
            }
            else if (string.IsNullOrEmpty(Password.Text) || string.IsNullOrEmpty(RePassword.Text))
            {
                DisplayAlert("Error", "Required field not be  empty", "OK");
                c = false;
            }
            return c;
        }

        public bool IsPasswordsEqual(string password1, string password2)
        {
            if (password1.Equals(password2))
            {
                return true;
            }

            return false;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            SubmitButton_Clicked(sender, e);
        }
    }
}