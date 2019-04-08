using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.Model;
using TaazaTV.Helper;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BreakingNewsPage : ContentPage
    {
        BreakingNewsModel Items = new BreakingNewsModel();

        static bool IsLoad = true;
        bool isLoading;
        Page page;
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

        public BreakingNewsPage(string Title, ImageSource Image)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Breaking News";
            IsLoad = true;
            TitleImage.Source = Image;
            TitleText.Text = Title;

        }

        protected override async void OnAppearing()
        {
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                NoInternet.IsVisible = false;
                NoDataPage.IsVisible = false;
                MainContainer.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);
                IsLoad = false;
                SetData();

            }
            else
            {
                //MainFrame.IsVisible = true;
                Loader.IsVisible = false;
            }

            base.OnAppearing();
        }

        protected async void SetData()
        {
            try
            {
                //if (Items.data == null)
                //{
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                //  parameters.Add(new KeyValuePair<string, string>("city_id", AppData.UserCityId));
                parameters.Add(new KeyValuePair<string, string>("breaking_news", "1"));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewsList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    lstView.IsVisible = false;
                    NoInternet.IsVisible = true;
                    NoDataPage.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<BreakingNewsModel>(jsonstr);
                    }
                    catch
                    {
                        lstView.IsVisible = false;
                        NoInternet.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }

                    if (Items.data.total_news <= 0)
                    {
                        NoDataPage.IsVisible = true;
                        MainContainer.IsVisible = false;
                    }
                    else
                    {
                        lstView.ItemsSource = Items.data.news_list;
                        lstView.HeightRequest = Items.data.news_list.Count() * lstView.RowHeight;

                        lstView.ItemAppearing += (sender, e) =>
                        {
                            if (isLoading || Items.data.news_list.Count() == 0)
                                return;
                            var listitem = e.Item.ToString();

                            if (((News_List_Bnews)e.Item).news_id.ToString() == Items.data.news_list[(Items.data.news_list.Count() - 1)].news_id.ToString())
                            {
                                if (Items.data.total_pages != Items.data.current_page)
                                {
                                    LoadItems();
                                }
                            }
                        };

                    }
                    lstView.IsVisible = true;
                    MainContainer.IsVisible = true;
                }
                //}
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
                //   parameters.Add(new KeyValuePair<string, string>("city_id", AppData.UserCityId));
                //  parameters.Add(new KeyValuePair<string, string>("breaking_news", "1"));
                parameters.Add(new KeyValuePair<string, string>("page", (Items.data.current_page + 1).ToString()));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewsList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    lstView.IsVisible = false;
                    MainContainer.IsVisible = false;
                    NoInternet.IsVisible = true;

                }
                else
                {
                    BreakingNewsModel NewItems = new BreakingNewsModel();
                    NewItems = JsonConvert.DeserializeObject<BreakingNewsModel>(jsonstr);

                    List<News_List_Bnews> asd = new List<News_List_Bnews>();
                    asd = Items.data.news_list.ToList();

                    foreach (News_List_Bnews list in NewItems.data.news_list)
                    {
                        asd.Add(list);
                    }

                    Items.data.news_list = asd.ToArray();

                    Items.data.current_page = NewItems.data.current_page;
                    lstView.ItemsSource = Items.data.news_list;
                    lstView.HeightRequest = Items.data.news_list.Count() * lstView.RowHeight;

                    lstView.IsVisible = true;
                    MainContainer.IsVisible = true;
                }
            }
            catch (Exception ex)
            {

            }

            lasyLoader.IsVisible = false;
            lstView.IsVisible = true;
            MainContainer.IsVisible = true;
        }

        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            News_List_Bnews model = (News_List_Bnews)e.Item;
            lstView.SelectedItem = null;
            await Navigation.PushAsync(new NewsDetailsPage(model.news_id.ToString()));
        }

        private async void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
            MainContainer.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);
            SetData();
            IsLoad = false;

        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}