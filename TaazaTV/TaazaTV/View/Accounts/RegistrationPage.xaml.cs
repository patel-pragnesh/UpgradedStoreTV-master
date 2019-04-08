using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.Model;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using XLabs.Forms;
using XLabs.Platform.Services.Media;
using System.IO;
using System.Net.Http;
using TaazaTV.View.Navigation;
using System.Text.RegularExpressions;
using TaazaTV.View.TaazaCash;
#if __IOS__
using Plugin.Media;
#endif

namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        jsonResponseClass des = new jsonResponseClass();
        public static string CityName;
        public static string CityId;
        ImageViewModel model = new ImageViewModel();
        byte[] ProfileImage;
        //bool IsAllCity = false;
        public RegistrationPage(string data = null)
        {

            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Padding = new Thickness(0, 20, 0, 0);
                this.Title = "Taaza TV Registration Page";
            }
                
            CityName = "Kolkata";
            CityId = "5583";
            //if (data != null)
            //{
            //    dateLabel.Text = data;
            //}

            //this.BindingContext = new CameraViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cityEntry.Text = CityName;

            //Loader.IsVisible = false;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void City_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CityListPage(false));
        }

        public Boolean ChkNullAll()
        {
            bool c = true;
            //  Regex r = new Regex(@"[@\\*+]+");

            //if (r.IsMatch(Name.Text))
            //{
            //    DisplayAlert("Error", "Special character not allow in name field", "OK");
            //    c = false;
            //}
            Regex email = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (string.IsNullOrEmpty(Name.Text) || string.IsNullOrEmpty(PhoneNo.Text))
            {
                DisplayAlert("Error", "Required field not be empty", "OK");
                c = false;
            }
            else if (string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(PhoneNo.Text))
            {
                DisplayAlert("Error", "Required field not be empty", "OK");
                c = false;
            }
            else if (string.IsNullOrEmpty(EmailId.Text) || string.IsNullOrEmpty(EmailId.Text))
            {
                c = true;
            }
            else if (!email.IsMatch(EmailId.Text))
            {
                DisplayAlert("Error", "The email id must be a valid email address", "OK");
                c = false;
            }
            else if (PhoneNo.Text.Length != 10)
            {
                DisplayAlert("Error", "Enter a valid phone number", "OK");
                c = false;
            }

            return c;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {

            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);


            HttpRequestWrapper wrapper = new HttpRequestWrapper();

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

            CameraViewModel RegModel = new CameraViewModel();

            if (ChkNullAll() == true)
            {
                MultipartFormDataContent formdata = new MultipartFormDataContent();
                formdata.Add(new StringContent(Name.Text.Trim()), "user_name");
                formdata.Add(new StringContent(PhoneNo.Text), "phone_no");
                formdata.Add(new StringContent(string.IsNullOrEmpty(EmailId.Text) ? "" : EmailId.Text), "email_id");
                //formdata.Add(new StringContent(password.Text.Trim()), "password");
                formdata.Add(new StringContent(CityId), "city_id");
                formdata.Add(new StringContent(string.IsNullOrEmpty(ReferCode.Text) ? "" : ReferCode.Text), "referral_code");
                
                formdata.Add(new StringContent(Constant.CompanyID), "company_code");
                formdata.Add(new StringContent(Constant.DeviceType), "device_type");
                formdata.Add(new StringContent(AppData.DeviceToken), "device_token");
                formdata.Add(new StringContent(Constant.AppVersion), "app_version");

                //formdata.Add(new StringContent(Data, Encoding.UTF8, "application/json"));
                if (ProfileImage != null)
                {
                    formdata.Add(new StreamContent(new MemoryStream(ProfileImage)), "profile_image", ImageSourceName.Text);
                }

                try
                {

                    string items = await wrapper.PostFormDataAsync(Constant.APIs[(int)Constant.APIName.Registration], formdata);
                    if (items.ToString() == "NoInternet")
                    {
                        NoInternet.IsVisible = true;
                        MainFrame.IsVisible = false;
                        logoimg.IsVisible = false;
                        name.IsVisible = false;
                        Phone.IsVisible = false;
                        photo.IsVisible = false;
                        email.IsVisible = false;
                        cityList.IsVisible = false;
                        button.IsVisible = false;
                        SkipLogin.IsVisible = false;
                        social.IsVisible = false;
                    }
                    else
                    {
                        try
                        {
                            des = JsonConvert.DeserializeObject<jsonResponseClass>(items);
                        }
                        catch
                        {
                            await DisplayAlert("Internal server error", "Please try again later", "Cancel");
                        }
                        if (des.responseText == "Success")
                        {
                            var userid = des.data.user_id.ToString();
                            await Navigation.PushModalAsync(new OTPValidationPage(userid));
                        }
                        else
                        {
                            await DisplayAlert("Error", "Please check input data you provided", "OK", "Cancel");
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            MainFrame.IsVisible = true;

            Loader.IsVisible = false;

        }

        private async void Photo_Tapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Pick your Photo", "Camera", "Gallery");
            Image image = new Image();
            switch (action)
            {
                case "Camera":
                    {
#if __ANDROID__
                        await model.TakePicture();
#endif
#if __IOS__
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                            return;
                        }

                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                            Directory = "Sample",
                            Name = "default.png"
                        });

                        if (file == null)
                            return;

                        ImageSourceName.Text = Path.GetFileName(file.Path);
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });

                        try
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                file.GetStream().CopyTo(memoryStream);
                                file.Dispose();
                                ProfileImage = memoryStream.ToArray();
                            }
                        }
                        catch { }
#endif
                        break;
                    }
                case "Gallery":
                    {
#if __ANDROID__
                        await model.SelectPicture();
#endif
#if __IOS__

                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                            return;
                        }
                        var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                        {
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                        });

                        if (file == null)
                            return;

                        ImageSourceName.Text = Path.GetFileName(file.Path);
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            file.Dispose();
                            return stream;
                        });

                        try
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                file.GetStream().CopyTo(memoryStream);
                                file.Dispose();
                                ProfileImage = memoryStream.ToArray();
                            }
                        }
                        catch { }
#endif

                        break;
                    }
            }
#if __ANDROID__
            if (model.ImageArray != null)
            {
                ProfileImage = model.ImageArray;
                ImageSourceName.Text = model.Status;
            }
#endif
        }
        private void DoSomething(object sender, EventArgs e)
        {
           //
        }

        private async void Facebook_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FacebookProfilePage());
        }

        private void skipButton_Clicked(object sender, EventArgs e)
        {
            AppData.IsSkip = true;
            App.Current.MainPage = new MasterDetailsPage();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NewLoginPage());
        }

        private async void MerchantLoginButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MerchantLogin());
        }
        
    }
}