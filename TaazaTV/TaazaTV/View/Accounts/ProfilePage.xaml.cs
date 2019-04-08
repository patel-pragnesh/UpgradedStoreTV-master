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
using System.Net.Http;
using System.IO;
using System.Text.RegularExpressions;
#if __IOS__
using Foundation;
using Plugin.Media;
#endif

namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        LoginModel des = new LoginModel();
        public static string CityName = AppData.UserCity;
        public static string CityId = AppData.UserCityId;
        //public static string CityName = AppData.UserCity;
        //public static string CityId = AppData.UserCityId;
        ImageViewModel model = new ImageViewModel();
        byte[] ProfileImage;
        string ProfileImageName;
        public static string modelcityname = "";
        
        ProfileResponse Items = new ProfileResponse();
        public ProfilePage(ProfileResponse Items)
        {
            InitializeComponent();
            modelcityname = "";
            BindingContext = Items.data.user_data;
            cityEntry.Text = CityName = modelcityname = Items.data.user_data.city_name;
            //modelcityname = Items.data.user_data.city_name;
            CityId = Items.data.user_data.city_id;

            this.Items = Items;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrEmpty(CityName))
            {
                cityEntry.Text = modelcityname;
                modelcityname = "";
            }
            else
            {
                //CityName = AppData.UserCity;
                //CityId = AppData.UserCityId;
                cityEntry.Text = CityName;
            }
            //if(string.IsNullOrEmpty(cityEntry.Text))
            //{
            //      CityName = AppData.UserCity;
            //      CityId = AppData.UserCityId;
            //}
            SetData();
            //Loader.IsVisible = false;
        }
        private async void City_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CityListPage(false));
        }

        public Boolean ChkNullAll()
        {
            bool c = true;
            Regex r = new Regex(@"[@\\*+]+");
           // Regex n = new Regex(@"^[0 - 9] *$");
            if (r.IsMatch(Name.Text))
            {
                DisplayAlert("Error", "Special character not allow in name field", "OK");
                c = false;
            }
            //else if (n.IsMatch(Name.Text))
            //{
            //    DisplayAlert("Error", "Number not allow in name field", "OK");
            //    c = false;
            //}
            else if (string.IsNullOrEmpty(Name.Text))
            {
                DisplayAlert("Error", "Required field not be empty", "OK");
                c = false;
            }
            else if (string.IsNullOrWhiteSpace(Name.Text))
            {
                DisplayAlert("Error", "Required field not be  empty", "OK");
                c = false;
            }
            return c;
        }

        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);

            HttpRequestWrapper wrapper = new HttpRequestWrapper();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

            if (ChkNullAll() == true)
            {
                var SelectedCategory = ((List<CategoryList>)AlertListView.ItemsSource).Where(x => x.IsSelected);
                string Categories = string.Join(",", SelectedCategory.Select(x => x.category_id));


                MultipartFormDataContent formdata = new MultipartFormDataContent();
                formdata.Add(new StringContent(Name.Text.Trim()), "user_name");
                //  formdata.Add(new StringContent(AppData.MobileNo), "phone_no");
                formdata.Add(new StringContent(EmailId.Text.Trim()), "email_id");
                formdata.Add(new StringContent(CityId), "city_id");

                formdata.Add(new StringContent(AppData.UserId), "user_id");
                formdata.Add(new StringContent(Constant.CompanyID), "company_code");
                formdata.Add(new StringContent(Constant.DeviceType), "device_type");
                formdata.Add(new StringContent(AppData.DeviceToken), "device_token");
                formdata.Add(new StringContent(Constant.AppVersion), "app_version");
                formdata.Add(new StringContent(Categories), "interest_news_categorys");

                //formdata.Add(new StringContent(Data, Encoding.UTF8, "application/json"));
                if (ProfileImage != null)
                {
                    formdata.Add(new StreamContent(new MemoryStream(ProfileImage)), "profile_image", ProfileImageName);
                }

                try
                {
                    string items = await wrapper.PostFormDataAsync(Constant.APIs[(int)Constant.APIName.ProfileUpdate], formdata);
                    if (items.ToString() == "NoInternet")
                    {
                        NoInternet.IsVisible = true;
                        MainContainer.IsVisible = false;
                    }
                    else
                    {
                        try
                        {
                            des = JsonConvert.DeserializeObject<LoginModel>(items);
                        }
                        catch
                        {
                            await DisplayAlert("Internal server error", "Please try again later", "Cancel");
                        }
                       
                        if (des.responseText == "Success")
                        {
                            AppData.UserName = des.data.user_data.name;
                            AppData.UserId = des.data.user_data.user_id;
                            AppData.UserCityId = des.data.user_data.city_id;
                            AppData.UserCity = des.data.user_data.city_name;
                            AppData.Avatar = des.data.user_data.avatar;

                            MainContainer.IsVisible = true;
                            Loader.IsVisible = false;
                            await DisplayAlert("Success", "Profile updated successfully!!", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            MainContainer.IsVisible = true;
                            Loader.IsVisible = false;
                            await DisplayAlert("Error", des.responseText, "OK", "Cancel");
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            Loader.IsVisible = false;
            MainContainer.IsVisible = true;
        }

        private async void Photo_Tapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Pick your Photo", "Camera", "Gallery");
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

                        ProfileImageName = Path.GetFileName(file.Path);                        
                        Avatar.Source = ImageSource.FromStream(() =>
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

                        ProfileImageName = Path.GetFileName(file.Path);
                        Avatar.Source = ImageSource.FromStream(() =>
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
                Avatar.Source = model.ImageSource;
                ProfileImageName = model.Status;
            }
#endif
        }

        private void DoSomething(object sender, EventArgs e)
        {
            SaveButtonClicked(sender, e);
        }

        private async void SetData()
        {
            HttpRequestWrapper wrapper = new HttpRequestWrapper();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
            var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewsCategory], parameters);
            if (jsonstr.ToString() == "NoInternet")
            {
                //lstView.IsVisible = false;
                NoInternet.IsVisible = true;
                //MainFrame.IsVisible = false;
            }
            else
            {
                NewsCategoryListModel NewsCatergory = JsonConvert.DeserializeObject<NewsCategoryListModel>(jsonstr);

                foreach (var item in Items.data.user_data.interested_news_categorys)
                {
                    foreach (var category in NewsCatergory.data.category.Where(x => x.category_id == item.category_id))
                    {
                        category.IsSelected = true;
                    }
                }

                AlertListView.ItemsSource = NewsCatergory.data.category.ToList();
            }
        }
    }
}