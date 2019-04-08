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
    public partial class ChangePasswordPage : ContentPage
    {
        jsonResponseClass des = new jsonResponseClass();
        public ChangePasswordPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Change Password";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //activityIndicator.IsVisible = false;
            //this.Navigation.RemovePage(new ForgotPasswordPage());
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            //App.Current.MainPage = new MasterDetailsPage();
            Loader.IsVisible = true;
            MainFrame.IsVisible = false;
            LogoImg.IsVisible = false;
            await Task.Delay(1000);

            if (AppData.UserId != null)
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

                if (!IsPasswordsEqual(Password.Text, RePassword.Text))
                {
                    await DisplayAlert("Error", "Re-password does not matched!!", "Cancel");
                }
                else
                {
                    if (ChkNullAll() == true)
                    {
                        parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                        parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                        parameters.Add(new KeyValuePair<string, string>("current_password", CurrentPassword.Text));
                        parameters.Add(new KeyValuePair<string, string>("new_password", Password.Text));

                        try
                        {
                            string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ChangePassword], parameters);
                            if (data.ToString() == "NoInternet")
                            {
                                NoInternet.IsVisible = true;
                                MainFrame.IsVisible = false;
                                LogoImg.IsVisible = false;
                            }
                            else
                            {
                                try
                                {
                                    des = JsonConvert.DeserializeObject<jsonResponseClass>(data);
                                }
                                catch
                                {
                                    await DisplayAlert("Internal server error", "Please try again later", "Cancel");
                                }
                                if (des.responseText == "Success")
                                {
                                    await DisplayAlert("Success", "Password has been changed successfully", "OK");
                                    App.Current.MainPage = new MasterDetailsPage();
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
            MainFrame.IsVisible = true;
            LogoImg.IsVisible = true;
        }

        public Boolean ChkNullAll()
        {
            bool c = true;
            if (string.IsNullOrEmpty(CurrentPassword.Text) || string.IsNullOrEmpty(Password.Text) || string.IsNullOrEmpty(RePassword.Text))
            {
                DisplayAlert("Error", "Required field not be empty", "OK");
                c = false;
            }
            else if (string.IsNullOrEmpty(CurrentPassword.Text) || string.IsNullOrEmpty(Password.Text) || string.IsNullOrEmpty(RePassword.Text))
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