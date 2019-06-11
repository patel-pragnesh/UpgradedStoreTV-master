using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.Services;
using TaazaTV.View.News;
using TaazaTV.View.TaazaCash;
using TaazaTV.View.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace TaazaTV.View.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailsPageDetail : ContentPage
    {
        MonkeysViewModel model = new MonkeysViewModel();
        static DashboardModel Items;
        static DashboardUnreadCounterModel CountItems;
        static bool IsLoad = true;
        string BreakingNewsId = "";
        string AdURL = "";

        //Prenium Ad Model Properties

        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        public int AdCount { get; set; }
        PreniumAdModel PreniumAdModelObj = new PreniumAdModel();
        int flag = 0;
        public string PreniumImageSrc { get; set; }
        public string PreniumImageWebSource { get; set; }
        public int SecondsInterval { get; set; }

        public MasterDetailsPageDetail()
        {
            InitializeComponent();
            AppData.PickerProblem = false;
            if (AppData.IsLogin == false)
            TaazaCashTab.IsVisible = false;
            TaazaCashAmount.Text = AppData.TaazaCash;
            IsLoad = true;
            Icon = "menu.png";
            //BindingContext = model;         
#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif

            int SlidePosition = 0;
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                try
                {
                    SlidePosition++;
                    if (Items != null)
                    {
                        if (SlidePosition == Items.data.breaking_news.Count())
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

            //int AdSlidePosition = 0;
            //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            //{
            //    try
            //    {
            //        AdSlidePosition++;
            //        if (Items != null)
            //        {
            //            if (AdSlidePosition == Items.data.ad_list.Count())
            //                AdSlidePosition = 0;
            //        }
            //        else
            //        {
            //            AdSlidePosition = 0;
            //        }

            //        CarouselAd.Position = AdSlidePosition;

            //    }
            //    catch { }
            //    return true;
            //});

        }

        protected override async void OnAppearing()
        {

            var x = await CommonFunc.FooterAdFunc("dashboard");
            if (x != null && x.Count() > 0)
            {
                CarouselAd.ItemsSource = x.ToList();
                CarouselAd.IsVisible = true;
            }
            else
            {
                CarouselAd.IsVisible = false;
            }

            if (IsLoad)
            {
                Loader.IsVisible = true;
                await Task.Delay(1000);

                SetCategory();

                IsLoad = false;
                var ScreenWidth = MainFrame.Width;
                CarouselZoos.HeightRequest = (ScreenWidth / 16) * 9;
                //  CarouselAd.HeightRequest = ScreenWidth / 6;
            }

            GetPreniumBannerDetails();
            base.OnAppearing();
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

        private async void Click_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new NewsListPage());
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                logInModel LogInModel = new logInModel();

                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));

                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));

                parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
                parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
                parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));

                string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UpdateDeviceToken], parameters);
                AppData.IsLogin = false;
            }
            catch (Exception ex)
            {

            }
        }

        private async void HeadlinesTicker_Navigating(object sender, WebNavigatingEventArgs e)
        {
            try
            {
                string[] data = e.Url.Split('/');

                if (data[0].ToString() != "file:")
                {
                    e.Cancel = true;
                    await Navigation.PushAsync(new NewsDetailsPage(data[3].ToString()));
                }
            }
            catch (Exception ex)
            {
                Logging.Write(ex, "HeadlinesTicker_Navigating");
            }
        }

        private async void SetCategory()
        {
            try
            {
                MainFrame.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);

                if (Items == null)
                {
                    HttpRequestWrapper wrapper = new HttpRequestWrapper();
                    List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                    parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                    if (AppData.UserId != "")
                    {
                        parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                    }
                    parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
                    parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
                    parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));
                    var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Dashboard], parameters);

                    if (jsonstr.ToString() == "NoInternet")
                    {
                        NoInternet.IsVisible = true;
                        NoDataPage.IsVisible = false;
                        MainFrame.IsVisible = false;
                        Loader.IsVisible = false;
                        return;
                    }
                    else if (string.IsNullOrEmpty(jsonstr))
                    {
                        NoDataPage.IsVisible = true;
                        NoInternet.IsVisible = false;
                        MainFrame.IsVisible = false;
                        Loader.IsVisible = false;
                        return;
                    }
                    else
                    {
                        try
                        {
                            Items = JsonConvert.DeserializeObject<DashboardModel>(jsonstr);
                            AppData.NotificationCount = Items.data.ecommerce_unread_notification_count;
                            AppData.TaazaCash = Items.data.userCurrentWalletBalance;
                            TaazaCashAmount.Text = AppData.TaazaCash;
                            NoInternet.IsVisible = false;
                            MainFrame.IsVisible = true;
                        }
                        catch (Exception ex)
                        {
                            NoDataPage.IsVisible = true;
                            NoInternet.IsVisible = false;
                            MainFrame.IsVisible = false;
                            Loader.IsVisible = false;
                            return;
                        }
                    }

                }

                //CategoryList.Children.Clear();
                foreach (Model.Menu menu in Items.data.menus)
                {
                    double Width = (MainFrame.Width - 20) / 3;
                    StackLayout layout = new StackLayout();
                    layout.HeightRequest = Width + 20;
                    layout.WidthRequest = Width;

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += async (s, e) =>
                    {
                        // handle the tap
                        StackLayout parent = (StackLayout)s;

                        if (((Xamarin.Forms.Label)parent.Children[3]).Text == "static")
                        {
                            //if (((Xamarin.Forms.Label)parent.Children[5]).Text == "live_tv")
                            //{
                            //    await Navigation.PushModalAsync(new NewVideoViewPage());
                            //}
                            //else
                            //{
                            await Navigation.PushAsync(PageNameToPage.GetPage(((Xamarin.Forms.Label)parent.Children[5]).Text, ((Xamarin.Forms.Label)parent.Children[2]).Text, ((Xamarin.Forms.Image)parent.Children[0]).Source));
                            //}
                        }
                        else
                        {
                            if (((Xamarin.Forms.Label)parent.Children[6]).Text == "news")
                            {
                                await Navigation.PushAsync(new NewsListPage(((Xamarin.Forms.Label)parent.Children[4]).Text, ((Xamarin.Forms.Label)parent.Children[2]).Text));
                            }
                            else
                            {
                                await Navigation.PushAsync(new EventListPage(((Xamarin.Forms.Label)parent.Children[1]).Text, ((Xamarin.Forms.Label)parent.Children[2]).Text, ((Xamarin.Forms.Image)parent.Children[0]).Source));
                            }
                        }


                    };

                    layout.GestureRecognizers.Add(tapGestureRecognizer);

                    Image image = new Image();
                    image.WidthRequest = Width - 20;
                    image.WidthRequest = Width - 20;
                    image.Source = menu.category_image;

                    Label CategoryId = new Label()
                    {
                        IsVisible = false,
                        Text = menu.category_id.ToString()
                    };
                    Label Alias = new Label()
                    {
                        IsVisible = false,
                        Text = menu.alias.ToString()
                    };
                    Label ForType = new Label()
                    {
                        IsVisible = false,
                        Text = menu.for_type.ToString()
                    };
                    Label Type = new Label()
                    {
                        IsVisible = false,
                        Text = menu.type.ToString()
                    };
                    Label ForCategoryId = new Label()
                    {
                        IsVisible = false,
                        Text = menu.for_category_id.ToString()
                    };
                    Label CategoryName = new Label()
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold,
                        Text = menu.category_name.ToString()
                    };

                    layout.Children.Add(image);
                    layout.Children.Add(CategoryId);
                    layout.Children.Add(CategoryName);
                    layout.Children.Add(ForType);
                    layout.Children.Add(ForCategoryId);
                    layout.Children.Add(Alias);
                    layout.Children.Add(Type);
                    CategoryList.Children.Add(layout);
                }

                ObservableCollection<BreaingNews> BreakingNews = new ObservableCollection<BreaingNews>();

                foreach (Breaking_News news in Items.data.breaking_news)
                {
                    BreakingNews.Add(new BreaingNews
                    {
                        Id = news.news_id.ToString(),
                        Name = news.news_title,
                        ImageUrl = news.banner_image
                    });
                }

                model.breaingNews = BreakingNews;
                BindingContext = model;

                string SourceHTML = "<style>body{background-color: black;} a{color:white !important; text-decoration:none;} marquee a{white-space:nowrap;}</style><p><marquee>";
                var html = new HtmlWebViewSource();

                foreach (Headline_News headlines in Items.data.headline_news)
                {
                    SourceHTML += "<a href='http://data.com/" + headlines.news_id + "'>" + headlines.news_title + "</a>&nbsp;&nbsp;<img src='" + Constant.ServerURL + "/app/image/devider.png" + "' alt='•' />&nbsp;&nbsp;";
                }
                SourceHTML += "</marquee></p>";
                html.Html = SourceHTML;
                HeadlinesTicker.Source = html;

                //if (Items.data.ad_list != null)
                //{
                //    if (Items.data.ad_list.Count() > 0)
                //    {
                //        CarouselAd.ItemsSource = Items.data.ad_list.ToList();
                //        CarouselAd.IsVisible = true;
                //    }
                //    else
                //    {
                //        CarouselAd.IsVisible = false;
                //    }
                //}
                //else
                //{
                //    CarouselAd.IsVisible = false;
                //}

                MainFrame.IsVisible = true;
                Loader.IsVisible = false;
                if (AppData.UserId != "")
                {
                    SetCounter();
                }

            }
            catch (Exception ex)
            {
                Logging.Write(ex, "SetCategory");
            }
        }

        private async void CarouselZoos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                BreaingNews model = (BreaingNews)e.SelectedItem;
                BreakingNewsId = model.Id;
                //await Navigation.PushAsync(new NewsDetailsPage(model.Id));                
            }
            catch (Exception ex)
            {
                Logging.Write(ex, "CarouselZoos_ItemSelected");
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsDetailsPage(BreakingNewsId));
        }

        private void DoSomething(object sender, EventArgs e)
        {
            SetCategory();
        }

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

        private async void SetCounter()
        {
            //FooterAdFunc();
            //MainFrame.IsVisible = false;
            //Loader.IsVisible = true;
            //await Task.Delay(1000);
            //Call Counter API
            HttpRequestWrapper wrapper = new HttpRequestWrapper();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
            parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
            parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
            parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
            parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));
            var jsonstr1 = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.DashboardUnreadDataCount], parameters);

            if (jsonstr1.ToString() == "NoInternet")
            {
                NoInternet.IsVisible = true;
                MainFrame.IsVisible = false;
                CarouselAd.IsVisible = false;
                Loader.IsVisible = false;
                return;
            }
            else
            {

                CountItems = JsonConvert.DeserializeObject<DashboardUnreadCounterModel>(jsonstr1);

                int ChildCount = CategoryList.Children.Count();

                for (int i = 0; i < ChildCount; i++)
                {
                    CategoryList.Children.RemoveAt(0);
                }
                //CategoryList.Children.Clear();
                foreach (Model.Menu menu in Items.data.menus)
                {
                    try
                    {


                        double Width = (MainFrame.Width - 30) / 3;
                        StackLayout layout = new StackLayout();
                        layout.HeightRequest = Width + 20;
                        layout.WidthRequest = Width;

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += async (s, e) =>
                        {
                            // handle the tap
                            StackLayout parent = (StackLayout)s;
                            try
                            {
                                ((Xamarin.Forms.Grid)parent.Parent).Children.RemoveAt(1);
                            }
                            catch { }

                            if (((Xamarin.Forms.Label)parent.Children[3]).Text == "static")
                            {
                                //if (((Xamarin.Forms.Label)parent.Children[5]).Text == "live_tv")
                                //{
                                //    await Navigation.PushModalAsync(new VideoViewPage("http://www.zengatv.com/embed?v=5d9eebd0-313d-11e1-8f87-1231400424bd.html&t=live"));
                                //}
                                //else
                                //{
                                await Navigation.PushAsync(PageNameToPage.GetPage(((Xamarin.Forms.Label)parent.Children[5]).Text, ((Xamarin.Forms.Label)parent.Children[2]).Text, ((Xamarin.Forms.Image)parent.Children[0]).Source));
                                //}
                            }
                            else
                            {
                                await Navigation.PushAsync(new NewsListPage(((Xamarin.Forms.Label)parent.Children[4]).Text, ((Xamarin.Forms.Label)parent.Children[2]).Text));
                            }


                        };

                        layout.GestureRecognizers.Add(tapGestureRecognizer);

                        Image image = new Image();
                        image.WidthRequest = Width - 20;
                        image.WidthRequest = Width - 20;
                        image.Source = menu.category_image;

                        Label CategoryId = new Label()
                        {
                            IsVisible = false,
                            Text = menu.category_id.ToString()


                        };
                        Label Alias = new Label()
                        {
                            IsVisible = false,
                            Text = menu.alias.ToString()
                        };
                        Label ForType = new Label()
                        {
                            IsVisible = false,
                            Text = menu.for_type.ToString()
                        };
                        Label ForCategoryId = new Label()
                        {
                            IsVisible = false,
                            Text = menu.for_category_id.ToString()
                        };
                        Label CategoryName = new Label()
                        {
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontAttributes = FontAttributes.Bold,
                            Text = menu.category_name.ToString()
                        };

                        layout.Children.Add(image);
                        layout.Children.Add(CategoryId);
                        layout.Children.Add(CategoryName);
                        layout.Children.Add(ForType);
                        layout.Children.Add(ForCategoryId);
                        layout.Children.Add(Alias);

                        //chk menu with counter value
                        //foreach (Dashboard_UnreadData menu1 in CountItems.data.ToString())
                        //{ 
                        if (menu.alias == "opinion_poll" || menu.alias == "contests" || menu.alias == "events")
                        {
                            //Counter
                            Grid Counter = new Grid();
                            Counter.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                            Counter.HorizontalOptions = LayoutOptions.End;
                            Counter.Margin = new Thickness(0, 0, 20, 0);

                            if (menu.alias == "opinion_poll" && CountItems.data.opinion_poll.ToString() != "0")
                            {
                                Image CounterBackground = new Image
                                {
                                    Source = "counter.png",
                                    HeightRequest = 20,
                                    WidthRequest = 20
                                };
                                Label CounterText = new Label
                                {
                                    Text = CountItems.data.opinion_poll.ToString(),
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Bold,
                                    TextColor = Color.White

                                };
                                Counter.Children.Add(CounterBackground, 0, 0);
                                Counter.Children.Add(CounterText, 0, 0);
                            }
                            if (menu.alias == "contests" && CountItems.data.contests.ToString() != "0")
                            {
                                Image CounterBackground = new Image
                                {
                                    Source = "counter.png",
                                    HeightRequest = 20,
                                    WidthRequest = 20
                                };
                                Label CounterText = new Label
                                {
                                    Text = CountItems.data.contests.ToString(),
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Bold,
                                    TextColor = Color.White

                                };
                                Counter.Children.Add(CounterBackground, 0, 0);
                                Counter.Children.Add(CounterText, 0, 0);
                            }
                            if (menu.alias == "events" && CountItems.data.events.ToString() != "0")
                            {
                                Image CounterBackground = new Image
                                {
                                    Source = "counter.png",
                                    HeightRequest = 20,
                                    WidthRequest = 20
                                };
                                Label CounterText = new Label
                                {
                                    Text = CountItems.data.events.ToString(),
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Bold,
                                    TextColor = Color.White

                                };
                                Counter.Children.Add(CounterBackground, 0, 0);
                                Counter.Children.Add(CounterText, 0, 0);
                            }
                            //Layout
                            Grid MenuGrid = new Grid();
                            MenuGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                            MenuGrid.Children.Add(layout, 0, 0);
                            MenuGrid.Children.Add(Counter, 0, 0);

                            CategoryList.Children.Add(MenuGrid);
                        }

                        else
                        {
                            CategoryList.Children.Add(layout);
                        }
                        //  }

                    }
                    catch (Exception ex)
                    {
                        var x = ex.Message;
                    }
                }
                NoInternet.IsVisible = false;
            }
        }

        private void Menu_Tapped(object sender, EventArgs e)
        {
            MasterDetailsPage.masterPage.IsPresented = true;
        }

        //  Prenium Ad Model Functions

        public async void GetPreniumBannerDetails()
        {
            if (AdCount == 0)
            {
                MultipartFormDataContent formdata = new MultipartFormDataContent
                {
                    { new StringContent(AppData.UserId), "user_id" }
                };

                try
                {
                    string items = await wrapper.PostFormDataAsync(Constant.APIs[(int)Constant.APIName.PreniumUrl], formdata);

                    if (items.ToString() == "NoInternet")
                    {
                        // NoInternet.IsVisible = true;
                    }

                    else
                    {
                        PreniumAdModelObj = JsonConvert.DeserializeObject<PreniumAdModel>(items);
                        AdCount = (PreniumAdModelObj.data.Ad_list).Length;
                        SecondsInterval = PreniumAdModelObj.data.Time_Interval;
                        PopUpAdBanner();
                    }
                }

                catch
                {
                    NoDataPage.IsVisible = true;
                }
            }
        }

        void PopUpAdBanner()    
        {
            try
            {
                Device.StartTimer(TimeSpan.FromSeconds(SecondsInterval), () =>
                {
                    if (PreniumAdModelObj.data.Ad_list.Count() != 0)
                    {
                        if (flag != AdCount)
                        {
                            PreniumImageSrc = PreniumAdModelObj.data.Ad_list[flag].Add_image;
                            PreniumImageWebSource = PreniumAdModelObj.data.Ad_list[flag].Ad_url;

                        }
                        else
                        {
                            flag = 0;
                            PreniumImageSrc = PreniumAdModelObj.data.Ad_list[flag].Add_image;
                            PreniumImageWebSource = PreniumAdModelObj.data.Ad_list[flag].Ad_url;
                        }
                        //if (AppData.IsPreniumAdOpen == true)
                        //{
                        //    Navigation.PopAsync();
                        //}
                        Navigation.PushAsync(new PreniumAdPage(PreniumImageSrc, PreniumImageWebSource));

                        // Do NAvigation
                        flag++;
                    }
                    return true;
                });
            }
            catch(Exception ex)
            {

            }
          
        }

        private async void TaazaCashTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }
    }
}