using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.View.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.View;
using Plugin.Share;
using TaazaTV.View.TaazaCash;

namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProfilePage : ContentPage
    {
        ProfileResponse Items;
        bool IsLoad = true;
        public MyProfilePage()
        {
            InitializeComponent();
            IsLoad = true;
            TaazaCashAmount.Text = AppData.TaazaCash;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (IsLoad)
            {
                loadprofiledata();
                IsLoad = false;
            }

        }

        private async void loadprofiledata()
        {
            Loader.IsVisible = true;
            NoInternet.IsVisible = false;
            await Task.Delay(1000);

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Profile], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    //Name.IsVisible = false;
                    //Email.IsVisible = false;
                    //phone.IsVisible = false;
                    //city.IsVisible = false;
                    //Alertcategory.IsVisible = false;
                    //button.IsVisible = false;
                    //avatar.IsVisible = false;
                    // Logoimg.IsVisible = true;
                    MainContainer.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<ProfileResponse>(jsonstr);
                    }
                    catch
                    {
                        await DisplayAlert("Internal server error", "Please try again later", "Cancel");
                    }
                    AppData.TaazaCash = Items.data.user_data.current_wallet_balance;
                    AppData.UserReferralCode = Items.data.user_data.referral_code;
                    lblReferral.Text = Items.data.user_data.referral_code;
                    lblName.Text = Items.data.user_data.name;
                    lblEmailId.Text = Items.data.user_data.email_id;
                    lblPhoneNo.Text = Items.data.user_data.phone_no;
                    cityEntry.Text = Items.data.user_data.city_name;
                    avatar.Source = Items.data.user_data.avatar;
                    MainContainer.IsVisible = true;
                    NoInternet.IsVisible = false;

                    alert.Text = string.Join(", ", Items.data.user_data.interested_news_categorys.Select(x => x.category_name));
                }
            }
            catch (Exception ex)
            {
            }

            Loader.IsVisible = false;
        }

        private async void UpdateButtonClicked(object sender, EventArgs e)
        {
            IsLoad = true;
            await Navigation.PushAsync(new ProfilePage(Items));
        }

        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                App.Current.MainPage = new MasterDetailsPage();
            });

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }

        private async void ChangePasswordButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }
        private void DoSomething(object sender, EventArgs e)
        {
            loadprofiledata();
            IsLoad = false;
        }

        //private void HomeButton_Clicked(object sender, EventArgs e)
        //{
        //    App.Current.MainPage = new MasterDetailsPage();
        //}

        private void InviteButtonClicked(object sender, EventArgs e)
        {
            CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage
            {
                  Title = "I have 100 Taaza Cash for you",
                  Text = "Your friend " + AppData.UserName + " has invited you to download taaza tv app for earning taaza cash.Use promo code " + AppData.UserReferralCode+ " on sign up" + System.Environment.NewLine + System.Environment.NewLine,
#if __ANDROID__
                Url = "https://play.google.com/store/apps/details?id=com.maxmobility.taazatv"
#endif
#if __IOS__
                Url = "https://itunes.apple.com/us/app/taaza-tv/id1289638107" + AppData.UserReferralCode
#endif
            });
        }

        private void BackBtn_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new MasterDetailsPage();
        }

        private async void NavToTransactionsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }
    }
}