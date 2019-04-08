using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Share;
using TaazaTV.Helper;
using Newtonsoft.Json;
using Xamarin.Forms.Internals;
using TaazaTV.View.Tools;
using System.Net.Http;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsDetailsPage : ContentPage
    {
        NewsDetailsResponseClass Items = new NewsDetailsResponseClass();
        List<image> imageList = new List<image>();
        List<DisplayVideo> videoList = new List<DisplayVideo>();
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        string NewsId = "";
        string Zoomimage = "";
        bool IsDetailsLoad = true;
        bool IsLoad = true;
        HtmlWebViewSource html;

        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;


        public NewsDetailsPage(string NewsId)
        {
            InitializeComponent();
            FooterAdFunc();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV News";
            this.NewsId = NewsId;
            IsDetailsLoad = true;
            IsLoad = true;
            // loadNewsdata(NewsId);
            int SlidePosition = 0;
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                try
                {
                    SlidePosition++;
                    if (Items != null)
                    {
                        if (SlidePosition == Items.data.news_details.media_value.images_videos.images.Count())
                            SlidePosition = 0;
                    }
                    else
                    {
                        SlidePosition = 0;
                    }

                    CarouselZoos.Position = SlidePosition;
                }
                catch { }
                return true;
            });
        }

        protected async override void OnAppearing()
        {
           // this.Animate("", s => Layout(new Rectangle(((-1 + s) * Width), Y, Width, Height)), 16, 250, Easing.Linear, null, null);
                      
            if (IsLoad)
            {
                var ScreenWidth = MainFrame.Width;
                CarouselZoos.HeightRequest = (ScreenWidth / 16) * 9;

                IsLoad = false;
                loadNewsdata(NewsId);
            }
            base.OnAppearing();
        }

        private async void loadNewsdata(string Id)
        {
            MainFrame.IsVisible = false;
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);

            try
            {
               
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                if (AppData.UserId != "")
                {
                    parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                }
                parameters.Add(new KeyValuePair<string, string>("news_id", Id));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewsDetails], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                    NoDataPage.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<NewsDetailsResponseClass>(jsonstr);
                    }
                    catch
                    {
                        NoInternet.IsVisible = false;
                        MainFrame.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    BindingContext = Items.data.news_details;

                    imageList.Clear();
                    foreach (image image in Items.data.news_details.media_value.images_videos.images)
                    {
                        imageList.Add(new image { thumb = image.thumb.ToString(), normal = image.normal.ToString() });
                    }


                    if (imageList.Count() == 0)
                    {
                        imageList.Add(new image { thumb = Items.data.news_details.media_value.banner_image.thumb.ToString(), normal = Items.data.news_details.media_value.banner_image.normal.ToString() });
                    }
                    CarouselZoos.ItemsSource = imageList;
                    if (imageList.Count() > 0)
                    {
                        Zoomimage = imageList.FirstOrDefault().normal;
                    }
                    videoList.Clear();
                    if (Items.data.news_details.media_value.images_videos.videos.Count() > 0)
                    {
                        foreach (Video video in Items.data.news_details.media_value.images_videos.videos)
                        {
                            try
                            {
                                videoList.Add(new DisplayVideo { video_thumb = video.video_thumb[0], video_url = video.video_url });
                            }
                            catch { }
                        }
                        VideoView.ItemsSource = videoList;
                        VideoView.IsVisible = true;
                        VideoView.HeightRequest = videoList.Count() * VideoView.RowHeight;
                    }
                    else
                    {
                        VideoView.IsVisible = false;
                    }

                    html = new HtmlWebViewSource
                    {
                        Html = "<html><head><style type=\"text/css\">" +
                        " body {" +
                        "font-family: Arial;" +
                        "font-size: 15;" +
                        //"text-align: justify;" +
                        "}" +
                        "</style></head><body>" +
                        Items.data.news_details.description+"</body></html>"
                        //"<html><body><iframe id=\"ytplayer\" type=\"text/html\" width=\"360\" height=\"200\" src = \"https://www.youtube.com/embed/M7lc1UVf-VE\" frameborder = \"0\" allowfullscreen ></body></html>"
                    };
                    NewsDetails.Source = html;
                    MainFrame.IsVisible = true;
                    ////HtmlLabel label = new HtmlLabel();
                    ////label.Text = Items.data.news_details.description;
                    ////NewsDetails.Children.Add(label);
                    //newsTitle.Text = Items.data.news_details.news_title;
                    //shortDesc.Text = Items.data.news_details.sort_description;
                    //date.Text = Items.data.news_details.date;
                    //time.Text = Items.data.news_details.time;              
                }
            }
            catch (Exception ex)
            {
            }
            Loader.IsVisible = false;

            if (!IsDetailsLoad)
            {
                MainFrame.IsVisible = true;
                NoInternet.IsVisible = false;
                Loader.IsVisible = false;
            }
        }

        private async void CarouselZoos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            image model = (image)e.SelectedItem;
            Zoomimage = model.normal;
            //   await Navigation.PushAsync(new ZoomImagePage(Zoomimage));

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            // Banner_Image model = (Banner_Image)e;
            // Zoomimage = model.normal;
            await Navigation.PushAsync(new ZoomImagePage(Zoomimage));
        }

        private void CarouselZoos_PositionSelected(object sender, SelectedPositionChangedEventArgs e)
        {

        }

        private void Share_Tapped(object sender, EventArgs e)
        {
            CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage
            {
                Title = Items.data.news_details.news_title,
                Text = Items.data.news_details.sort_description,
#if __ANDROID__
                Url = "https://play.google.com/store/apps/details?id=com.maxmobility.taazatv"
#endif
#if __IOS__
                Url = "https://itunes.apple.com/us/app/taaza-tv/id1289638107"
#endif
            });
        }

        private void DoSomething(object sender, EventArgs e)
        {
            loadNewsdata(NewsId);
        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void VideoView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            VideoView.SelectedItem = null;
            DisplayVideo model = (DisplayVideo)e.Item;
            await Navigation.PushModalAsync(new VideoViewPage(model.video_url));

        }

        string AdURL;
        private void CarouselAd_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                PreniumAdModel model = (PreniumAdModel)e.SelectedItem;
                AdURL = (model.data.Ad_list.Select(x => x.Add_image)).ToString();
            }
            catch (Exception ex)
            {
                Logging.Write(ex, "CarouselAd_ItemSelected");
            }
        }

        private void CarouselAd_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(AdURL))
                    Device.OpenUri(new Uri(AdURL));
            }
            catch (Exception ex)
            {
                Logging.Write(ex, "CarouselAd_Tapped");
            }
        }

        public async void FooterAdFunc()
        {
            try
            {
                MultipartFormDataContent formdata = new MultipartFormDataContent();
                formdata.Add(new StringContent("event"), "page_name");
                formdata.Add(new StringContent(AppData.UserId), "user_id");

                string items = await wrapper.PostFormDataAsync(Constant.APIs[(int)Constant.APIName.GeneralUrl], formdata);

                if (items.ToString() == "NoInternet")
                {
                    // NoInternet.IsVisible = true;
                }

                else
                {
                    var deobj = JsonConvert.DeserializeObject<PreniumAdModel>(items);
                    var footersrc = deobj.data.Ad_list.ToList();
                    if (footersrc != null)
                    {
                        if (footersrc.Count() > 0)
                        {
                            CarouselAd.ItemsSource = footersrc;
                            CarouselAd.IsVisible = true;
                        }
                        else
                        {
                            CarouselAd.IsVisible = false;
                        }
                    }
                    else
                    {
                        CarouselAd.IsVisible = false;
                    }
                }
            }

            catch
            {
                throw new NotImplementedException();
            }
        }

        //Zoom Image
        //public void  PinchToZoomContainer()
        //{
        //    var pinchGesture = new PinchGestureRecognizer();
        //    pinchGesture.PinchUpdated += OnPinchUpdated;
        ////    image.GestureRecognizers.Add(pinchGesture);
        //}

        //void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        //{
        //    if (e.Status == GestureStatus.Started)
        //    {
        //        // Store the current scale factor applied to the wrapped user interface element,
        //        // and zero the components for the center point of the translate transform.
        //        startScale = Content.Scale;
        //        Content.AnchorX = 0;
        //        Content.AnchorY = 0;
        //    }
        //    if (e.Status == GestureStatus.Running)
        //    {
        //        // Calculate the scale factor to be applied.
        //        currentScale += (e.Scale - 1) * startScale;
        //        currentScale = Math.Max(1, currentScale);

        //        // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
        //        // so get the X pixel coordinate.
        //        double renderedX = Content.X + xOffset;
        //        double deltaX = renderedX / Width;
        //        double deltaWidth = Width / (Content.Width * startScale);
        //        double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

        //        // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
        //        // so get the Y pixel coordinate.
        //        double renderedY = Content.Y + yOffset;
        //        double deltaY = renderedY / Height;
        //        double deltaHeight = Height / (Content.Height * startScale);
        //        double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

        //        // Calculate the transformed element pixel coordinates.
        //        double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
        //        double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

        //        // Apply translation based on the change in origin.
        //        Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
        //        Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

        //        // Apply scale factor.
        //        Content.Scale = currentScale;
        //    }
        //    if (e.Status == GestureStatus.Completed)
        //    {
        //        // Store the translation delta's of the wrapped user interface element.
        //        xOffset = Content.TranslationX;
        //        yOffset = Content.TranslationY;
        //    }
        //} 
        //public static class DoubleExtensions
        //{
        //    public static double Clamp(this double self, double min, double max)
        //    {
        //        return Math.Min(max, Math.Max(self, min));
        //    }
        //}
        //end Zoom Image
    }
}