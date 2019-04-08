using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#if __IOS__
using Plugin.Media;
#endif


namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsSubmitPage : ContentPage
    {
        jsonResponseClass des = new jsonResponseClass();
        public static string CategoryName = "";
        public static string CategoryId = "";
        ImageViewModel model = new ImageViewModel();
        List<ImageViewModel> allItems = new List<ImageViewModel>();
        string ImageName = "";
        byte[] ImageArray = null;
        object imageSource = null;
        Image img = new Image();
        //StackLayout child_stacklayout = new StackLayout();
        ObservableCollection<ImageListViewModel> _selectedImageList = new ObservableCollection<ImageListViewModel>();
        ObservableCollection<VideoListViewModel> _selectedVideoList = new ObservableCollection<VideoListViewModel>();
        public NewsSubmitPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Submit News";
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //newscategory.Text = CategoryName;

            //Loader.IsVisible = false;
        }

        protected async void LoadGallery(object sender, EventArgs e)
        {
#if __ANDROID__
            await model.SelectPicture();
            ImageName = model.Status;
            ImageArray = model.ImageArray;
            imageSource = model.ImageSource;
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

                        ImageName = Path.GetFileName(file.Path);
                        imageSource = file.GetStream();

                        try
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                file.GetStream().CopyTo(memoryStream);
                                file.Dispose();
                                ImageArray = memoryStream.ToArray();
                            }
                        }
                        catch { }

#endif

            try
            {
                if (ImageArray != null)
                {
                    if (model.Status != "A task was canceled.")
                    {
                        _selectedImageList.Add(new ImageListViewModel
                        {
                            // base64 = model.Status,
                            Name = ImageName,
                            Image = ImageArray,
#if __ANDROID__
                            ImgSource = (ImageSource)imageSource
#endif
#if __IOS__
                            ImgSource = ImageSource.FromStream(() =>
                            {
                                return (Stream)imageSource;

                            })
#endif
                        });
                        ImageListView.ItemsSource = _selectedImageList;
                        ImageListView.HeightRequest = (_selectedImageList.Count() * ImageListView.RowHeight);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected async void LoadCamera(object sender, EventArgs e)
        {
            ImageName = "";
            ImageArray = null;

#if __ANDROID__
            await model.TakePicture();

            ImageName = model.Status;
            ImageArray = model.ImageArray;
            imageSource = model.ImageSource;
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

                        ImageName = Path.GetFileName(file.Path);
                        imageSource = file.GetStream();
                        try
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                file.GetStream().CopyTo(memoryStream);
                                file.Dispose();
                                ImageArray = memoryStream.ToArray();
                            }
                        }
                        catch { }
#endif
            try
            {
                if (ImageArray != null)
                {
                    if (model.Status != "A task was canceled.")
                    {
                        _selectedImageList.Add(new ImageListViewModel
                        {
                            // base64 = model.Status,
                            Name = ImageName,
                            Image = ImageArray,
#if __ANDROID__
                            ImgSource = (ImageSource)imageSource
#endif
#if __IOS__
                            ImgSource = ImageSource.FromStream(() =>
                            {
                                return (Stream)imageSource;

                            })
#endif
                        });
                        ImageListView.ItemsSource = _selectedImageList;
                        ImageListView.HeightRequest = (_selectedImageList.Count() * ImageListView.RowHeight);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ImageListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ImageListViewModel model = (ImageListViewModel)e.Item;
            _selectedImageList.Remove(model);
            ImageListView.ItemsSource = _selectedImageList;

        }

        protected async void LoadVideoList(object sender, EventArgs e)
        {
            ImageName = "";
            ImageArray = null;
            //#if __ANDROID__
            //            var action = await DisplayActionSheet("Pick your Video", "Cancel", "Gallery");
            //#endif
            //#if __IOS__
            //            var action = await DisplayActionSheet("Pick your Video", "Camera", "Gallery");
            //#endif
            //            switch (action)
            //            {
            //                case "Camera":
            //                    {
            //#if __ANDROID__
            //                        await model.SelectVideo();

            //                        ImageName = model.Status;
            //                        ImageArray = model.ImageArray;
            //                        imageSource = "video.mp4";
            //#endif
            //#if __IOS__
            //                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            //                        {
            //                            DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
            //                            return;
            //                        }

            //                        var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            //                        {
            //                            Name = "video.mp4",
            //                            Directory = "DefaultVideos",
            //                        });

            //                        if (file == null)
            //                            return;

            //                        ImageName = Path.GetFileName(file.Path);
            //                        imageSource = file.GetStream();
            //                        try
            //                        {
            //                            using (var memoryStream = new MemoryStream())
            //                            {
            //                                file.GetStream().CopyTo(memoryStream);
            //                                file.Dispose();
            //                                ImageArray = memoryStream.ToArray();
            //                            }
            //                        }
            //                        catch { }
            //#endif
            //                        break;
            //                    }
            //                case "Gallery":
            //                    {
#if __ANDROID__
            await model.SelectVideo();
            ImageName = model.Status;
            ImageArray = model.ImageArray;
            imageSource = model.ImageSource;
#endif
#if __IOS__

                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                            return;
                        }
                        var file = await CrossMedia.Current.PickVideoAsync();

                        if (file == null)
                            return;

                        ImageName = Path.GetFileName(file.Path);
                        imageSource = file.GetStream();

                        try
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                file.GetStream().CopyTo(memoryStream);
                                file.Dispose();
                                ImageArray = memoryStream.ToArray();
                            }
                        }
                        catch { }

#endif

            //            break;
            //        }
            //    default:
            //        break;
            ////}

            try
            {
                if (ImageArray != null)
                {
                    if (model.Status != "A task was canceled.")
                    {
                        _selectedVideoList.Add(new VideoListViewModel
                        {
                            // base64 = model.Status,
                            Name = ImageName,
                            Video = ImageArray,
                            ImgSource = "noshows.png"
                        });
                        VideoListView.FlowItemsSource = _selectedVideoList;
                        //VideoListView.HeightRequest = (_selectedVideoList.Count() * VideoListView.RowHeight);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void VideoListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            VideoListViewModel model = (VideoListViewModel)e.Item;
            _selectedVideoList.Remove(model);
            VideoListView.FlowItemsSource = _selectedVideoList;

        }

        private async void Category_Tapped(object sender, ItemTappedEventArgs e)
        {

            await Navigation.PushModalAsync(new CategoryListPage());
            //ImageListViewModel model = (ImageListViewModel)e.Item;
            //await Navigation.PushAsync(new NewsSubmitPage(model.Image));
        }

        public Boolean ChkNullAll()
        {
            bool c = true;
            if (string.IsNullOrEmpty(Headlines.Text))
            {
                DisplayAlert("Error", "Required field not be empty", "OK");
                c = false;
            }
            else if (string.IsNullOrWhiteSpace(Headlines.Text))
            {
                DisplayAlert("Error", "Required field not be  empty", "OK");
                c = false;
            }
            return c;
        }

        private async void SubmitNewsButton_Clicked(object sender, EventArgs e)
        {
            Loader.IsVisible = true;
            NoInternet.IsVisible = false;
            await Task.Delay(1000);


            HttpRequestWrapper wrapper = new HttpRequestWrapper();

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            if (ChkNullAll() == true)
            {
                MultipartFormDataContent formdata = new MultipartFormDataContent();
                //formdata.Add(new StringContent(CategoryId), "category_id");
                formdata.Add(new StringContent(Headlines.Text.Trim()), "news_title");
                formdata.Add(new StringContent(string.IsNullOrEmpty(StoryDescription.Text) ? "" : StoryDescription.Text.Trim()), "sort_description");
                formdata.Add(new StringContent(_selectedImageList.Count().ToString()), "image_count");
                formdata.Add(new StringContent(AppData.UserId), "user_id");
                formdata.Add(new StringContent(Constant.CompanyID), "company_code");
                formdata.Add(new StringContent(Constant.DeviceType), "device_type");
                formdata.Add(new StringContent(AppData.DeviceToken), "device_token");
                formdata.Add(new StringContent(Constant.AppVersion), "app_version");
                formdata.Add(new StringContent(_selectedVideoList.Count().ToString()), "video_count");
                int VideoCount = 0;
                foreach (VideoListViewModel list in _selectedVideoList)
                {
                    VideoCount++;
                    formdata.Add(new StreamContent(new MemoryStream(list.Video)), "video_" + VideoCount, list.Name);
                }
                int ImageCount = 0;
                foreach (ImageListViewModel list in _selectedImageList)
                {
                    ImageCount++;
                    formdata.Add(new StreamContent(new MemoryStream(list.Image)), "image_" + ImageCount, list.Name);
                }

                try
                {
                    string items = await wrapper.PostFormDataAsync(Constant.APIs[(int)Constant.APIName.NewsPost], formdata);

                    if (items.ToString() == "NoInternet")
                    {
                        NoInternet.IsVisible = true;
                        submitform.IsVisible = false;
                        Headline.IsVisible = false;
                        shortdes.IsVisible = false;
                        LongDes.IsVisible = false;
                        pickimg.IsVisible = false;
                        pickvideo.IsVisible = false;
                        //NewscategoryList.IsVisible = false;
                        button.IsVisible = false;
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
                            await DisplayAlert("Success", "News submitted successfully!!", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Error", des.responseText, "OK", "Cancel");
                        }
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK", "Cancel");
                }
            }
            Loader.IsVisible = false;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            //LoadImageList();
            //LoadVideoList();
            SubmitNewsButton_Clicked(sender, e);
        }
    }
}