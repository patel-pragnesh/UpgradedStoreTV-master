using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowListPage : ContentPage
    {
        ShowsModel Items = new ShowsModel();
        static bool IsLoad = true;
        string PlaylistId = "";
        bool isLoading;
        string currentPage = "";
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

        public Command PullToRefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    SetData();
                    IsRefreshing = false;
                });
            }
        }


        public ShowListPage(string PlaylistId, string Title, ImageSource Image)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Shows";
            IsLoad = true;
            TitleImage.Source = Image;
            TitleText.Text = Title;
            this.PlaylistId = PlaylistId;
        }

        protected override async void OnAppearing()
        {      
            if (IsLoad)
            {
                NoInternet.IsVisible = false;
                NoDataPage.IsVisible = false;
                MainFrame.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);
                IsLoad = false;
                SetData();
            }

            base.OnAppearing();
        }

        protected async void SetData()
        {
            //MainFrame.IsVisible = false;

            try
            {
                //if (Items.data == null)
                //{
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("playlistId", PlaylistId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ShowsPlaylistVideos], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    lstView.IsVisible = false;
                    MainFrame.IsVisible = false;
                    NoInternet.IsVisible = true;
                    NoDataPage.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<ShowsModel>(jsonstr);
                    }
                   catch
                    {
                        lstView.IsVisible = false;
                        MainFrame.IsVisible = false;
                        NoInternet.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }


                    lstView.ItemsSource = Items.data.playlists.items;

                    lstView.ItemAppearing += (sender, e) =>
                    {
                        if (isLoading || Items.data.playlists.items.Count() == 0)
                            return;
                        var listitem = e.Item.ToString();

                        if (((VideoItem)e.Item).id.ToString() == Items.data.playlists.items[(Items.data.playlists.items.Count() - 1)].id.ToString())
                        {
                            if (!string.IsNullOrEmpty(Items.data.playlists.nextPageToken))
                            {
                                if (currentPage != Items.data.playlists.nextPageToken)
                                {
                                    LoadItems(Items.data.playlists.nextPageToken);
                                    currentPage = Items.data.playlists.nextPageToken;
                                }
                            }
                        }
                    };
                    lstView.IsVisible = true;
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

        protected async void LoadItems(string NextPage)
        {
            //MainFrame.IsVisible = false;
            lasyLoader.IsVisible = true;
            await Task.Delay(1000);
            try
            {
                //if (Items.data == null)
                //{
                ShowsModel NewItems = new ShowsModel();
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("playlistId", PlaylistId));
                parameters.Add(new KeyValuePair<string, string>("pageToken", NextPage));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ShowsPlaylistVideos], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    lstView.IsVisible = false;
                    MainFrame.IsVisible = false;
                    NoInternet.IsVisible = true;
                }
                else
                {
                    NewItems = JsonConvert.DeserializeObject<ShowsModel>(jsonstr);
                }

                List<VideoItem> asd = new List<VideoItem>();
                asd = Items.data.playlists.items.ToList();

                foreach (VideoItem list in NewItems.data.playlists.items)
                {
                    asd.Add(list);
                }

                Items.data.playlists.items = asd.ToArray();
                Items.data.playlists.nextPageToken = NewItems.data.playlists.nextPageToken;
                lstView.ItemsSource = Items.data.playlists.items;


                //}
            }
            catch (Exception ex)
            {

            }

            lasyLoader.IsVisible = true;
        }

        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            VideoItem model = (VideoItem)e.Item;
            lstView.SelectedItem = null;

            await Navigation.PushModalAsync(new VideoViewPage("https://www.youtube.com/embed/" + model.snippet.resourceId.videoId));
        }

        private async void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
            MainFrame.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);
            IsLoad = false;
            SetData();

        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}