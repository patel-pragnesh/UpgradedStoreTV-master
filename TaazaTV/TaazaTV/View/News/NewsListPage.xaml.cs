using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.View.Accounts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsListPage : ContentPage
    {
        NewsListModel Items = new NewsListModel();
        MonkeysViewModel model = new MonkeysViewModel();
        public static string CityName = "All";
        public static string CityId = "";
        string CategoryId = "";
        string BreakingNewsId = "";
        static bool IsLoad = true;
        bool isLoading;
        int currentPage = 1;
        public bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        //  bool IsAllCity = true;
        public Command PullToRefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    loadNewsListdata(CategoryId, CityId);
                    IsRefreshing = false;
                });
            }
        }

        public NewsListPage(string CategoryId = "", string CategoryName = "", ImageSource Image = null)
        {
            InitializeComponent();
            BindingContext = model.breaingNews;
            this.CategoryId = CategoryId;
            //  loadNewsListdata(CategoryId, CityId);
            IsLoad = true;
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (CategoryName.Contains(" "))
                    this.Title = "Taaza TV " + CategoryName.Substring(3) + " News";
                else
                    this.Title = "Taaza TV " + CategoryName + " News";
            }
            TitleText.Text = string.IsNullOrEmpty(CategoryName) ? "News" : CategoryName;
            TitleImage.Source = Image;
            if (CategoryId != "2")
            {
                this.ToolbarItems.Clear();
            }

            int SlidePosition = 0;
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                try
                {
                    SlidePosition++;
                    if (Items != null)
                    {
                        if (SlidePosition == Items.data.featured_news_list.Count())
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

        protected override async void OnAppearing()
        {            
            if (CategoryId != "2")
            {
                this.ToolbarItems.Clear();
            }

            if (CityPopUp.Text != CityName)
            {
                IsLoad = true;
                CityPopUp.Text = CityName;
            }
            else if (CityPopUp.Text.ToString() != "")
            {
                CityPopUp.Text = CityName;
            }
            if (IsLoad)
            {
                var ScreenWidth = MainFrame.Width;
                CarouselZoos.HeightRequest = (ScreenWidth / 16) * 9;

                NoInternet.IsVisible = false;
                NoDataPage.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);
                IsLoad = false;
                loadNewsListdata(CategoryId, CityId);
            }
            base.OnAppearing();
        }

        private async void loadNewsListdata(string CategoryId, string CId)
        {
            //MainFrame.IsVisible = false;

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                if (AppData.UserId != "")
                {
                    parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                }
                parameters.Add(new KeyValuePair<string, string>("category_id", string.IsNullOrEmpty(CategoryId) ? "1" : CategoryId));
                if (CategoryId == "2")
                {
                    parameters.Add(new KeyValuePair<string, string>("city_id", CId));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("city_id", ""));
                }
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewsList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                    CarouselZoos.IsVisible = false;
                    NoDataPage.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<NewsListModel>(jsonstr);
                    }
                    catch
                    {
                        NoInternet.IsVisible = false;
                        MainFrame.IsVisible = false;
                        lstView.IsVisible = false;
                        CarouselZoos.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    if (Items.data.total_news <= 0)
                    {
                        NoDataPage.IsVisible = true;
                        MainFrame.IsVisible = false;
                    }
                    MainFrame.IsVisible = true;
                    //lstView.ItemsSource = Items.data.news_list;
                    ObservableCollection<BreaingNews> featured_news_list = new ObservableCollection<BreaingNews>();
                    if (Items.data.featured_news_list.Count() > 0)
                    {
                        foreach (News_List news in Items.data.featured_news_list)
                        {
                            featured_news_list.Add(new BreaingNews
                            {
                                ImageUrl = news.banner_image,
                                Name = news.news_title,
                                Id = news.news_id.ToString()

                            });
                        }

                        CarouselZoos.IsVisible = true;
                    }
                    else
                    {
                        CarouselZoos.IsVisible = false;
                    }

                    lstView.ItemsSource = Items.data.news_list;
                    //lstView.HeightRequest = (Items.data.news_list.Count() * lstView.RowHeight) + 5;
                    if (Items.data.total_news > 0)
                    {
                        if (featured_news_list.Count() > 0)
                        {
                            model.breaingNews = featured_news_list;
                            BindingContext = model;
                            CarouselZoos.IsVisible = true;
                        }
                    }
                    else
                    {
                        CarouselZoos.IsVisible = false;
                    }

                    lstView.ItemAppearing += (sender, e) =>
                    {
                        if (isLoading || Items.data.news_list.Count() == 0)
                            return;
                        var listitem = e.Item.ToString();

                        if (((News_List)e.Item).news_id.ToString() == Items.data.news_list[(Items.data.news_list.Count() - 1)].news_id.ToString())
                        {
                            if (currentPage == Items.data.current_page)
                            {
                                if (Items.data.total_pages != Items.data.current_page)
                                {
                                    currentPage++;
                                    LoadItems();
                                }
                            }
                        }
                    };
                    MainFrame.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
            }


            Loader.IsVisible = false;
            lstView.IsPullToRefreshEnabled = true;
            lstView.RefreshCommand = PullToRefreshCommand;
            lstView.IsRefreshing = IsRefreshing;
            lstView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");
        }

        private async Task LoadItems()
        {
            lasyLoader.IsVisible = true;
            await Task.Delay(1000);

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                if (AppData.UserId != "")
                {
                    parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                }
                parameters.Add(new KeyValuePair<string, string>("category_id", string.IsNullOrEmpty(CategoryId) ? "1" : CategoryId));
                if (CategoryId == "2")
                {
                    parameters.Add(new KeyValuePair<string, string>("city_id", CityId));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>("city_id", ""));
                }
                // parameters.Add(new KeyValuePair<string, string>("city_id", CityId));
                parameters.Add(new KeyValuePair<string, string>("page", (Items.data.current_page + 1).ToString()));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewsList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    lstView.IsVisible = false;
                    NoInternet.IsVisible = true;
                }
                else
                {
                    NewsListModel NewItems = new NewsListModel();
                    NewItems = JsonConvert.DeserializeObject<NewsListModel>(jsonstr);

                    List<News_List> asd = new List<News_List>();
                    asd = Items.data.news_list.ToList();

                    foreach (News_List list in NewItems.data.news_list)
                    {
                        asd.Add(list);
                    }

                    Items.data.news_list = asd.ToArray();

                    Items.data.current_page = NewItems.data.current_page;
                    lstView.ItemsSource = Items.data.news_list;
                    //lstView.HeightRequest = Items.data.news_list.Count() * lstView.RowHeight;
                    //currentPage= NewItems.data.current_page;
                }
            }
            catch
            {

            }
            lstView.IsVisible = true;
            lasyLoader.IsVisible = false;
        }

        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            News_List model = (News_List)e.Item;
            lstView.SelectedItem = null;
            await Navigation.PushAsync(new NewsDetailsPage(model.news_id.ToString()));
        }

        private async void CarouselZoos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BreaingNews model = (BreaingNews)e.SelectedItem;
            BreakingNewsId = model.Id;
            //await Navigation.PushAsync(new NewsDetailsPage(model.Id));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsDetailsPage(BreakingNewsId));
        }

        private void CarouselZoos_PositionSelected(object sender, SelectedPositionChangedEventArgs e)
        {

        }

        private async void TappedForCity(object sender, ItemTappedEventArgs e)
        {
            //city_data model = (city_data)e.Item;
            //lstView.SelectedItem = null;
            await Navigation.PushModalAsync(new CityListPage(true));
            //OnAppearing();
            // loadNewsListdata(CategoryId, CityId);

        }

        private async void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);
            IsLoad = false;
            loadNewsListdata(CategoryId, CityId);
        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}