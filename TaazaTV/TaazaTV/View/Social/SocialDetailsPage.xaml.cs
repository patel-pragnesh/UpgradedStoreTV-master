using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TaazaTV.View.Social
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocialDetailsPage : ContentPage
    {
        SocialDetailsModel Items = new SocialDetailsModel();
        List<image> imageList = new List<image>();
        static bool IsLoad = true;
        bool isLoading;
        Page page;
        string group_id = "";

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

        protected override async void OnAppearing()
        {
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                NoInternet.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(100);

                loadNewsdata(group_id);
                IsLoad = false;
            }


            base.OnAppearing();
        }

        public Command PullToRefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    loadNewsdata(group_id);
                    IsRefreshing = false;
                });
            }
        }


        public SocialDetailsPage(string group_id, ImageSource Image, String Title)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Social Network";
            loadNewsdata(group_id);
            TitleImage.Source = Image;
            TitleText.Text = Title;
        }

        private async void loadNewsdata(string group_id)
        {

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                parameters.Add(new KeyValuePair<string, string>("group_id", group_id));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.FeedList], parameters);
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
                        Items = JsonConvert.DeserializeObject<SocialDetailsModel>(jsonstr);
                    }
                    catch
                    {
                        NoInternet.IsVisible = false;
                        MainFrame.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    lstView.ItemsSource = Items.data.group_feed.feed_data;
                    lstView.HeightRequest = Items.data.group_feed.feed_data.Count() * lstView.RowHeight + 2;
                    lstView.ItemAppearing += (sender, e) =>
                    {
                        if (isLoading || Items.data.group_feed.feed_data.Count() == 0)
                            return;
                        var listitem = e.Item.ToString();

                        if (((Feed_Data)e.Item).title.ToString() == Items.data.group_feed.feed_data[(Items.data.group_feed.feed_data.Count() - 1)].title.ToString())
                        {
                            if (Items.data.group_feed.total_pages != Items.data.group_feed.current_page)
                            {
                                LoadItems();
                            }
                        }
                    };
                    Bannerimg.Source = Items.data.group_details.group_banner;
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
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                parameters.Add(new KeyValuePair<string, string>("group_id", group_id));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.FeedList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    lstView.IsVisible = false;
                    NoInternet.IsVisible = true;
                }
                else
                {
                    SocialDetailsModel NewItems = new SocialDetailsModel();
                    NewItems = JsonConvert.DeserializeObject<SocialDetailsModel>(jsonstr);

                    List<Feed_Data> asd = new List<Feed_Data>();
                    asd = Items.data.group_feed.feed_data.ToList();

                    foreach (Feed_Data list in NewItems.data.group_feed.feed_data)
                    {
                        asd.Add(list);
                    }

                    Items.data.group_feed.feed_data = asd.ToArray();

                    Items.data.group_feed.total_pages = NewItems.data.group_feed.current_page;
                    lstView.ItemsSource = Items.data.group_feed.feed_data;
                    lstView.HeightRequest = Items.data.group_feed.feed_data.Count() * lstView.RowHeight + 2;
                }
            }
            catch
            {

            }

            lasyLoader.IsVisible = false;
        }

        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new SocialFeedDetailsPage((Feed_Data)e.Item));
        }

        private async void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(100);

            loadNewsdata(group_id);
            IsLoad = false;
        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void lstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstView.SelectedItem = null;
            //await Navigation.PushAsync(new SocialFeedDetailsPage((Feed_Data)e.SelectedItem));
        }
    }
}