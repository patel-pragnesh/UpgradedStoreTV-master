using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public partial class EventsPage : ContentPage
	{
        NewsListModel Items = new NewsListModel();
        MonkeysViewModel model = new MonkeysViewModel();
        string CategoryId = "";
        string BreakingNewsId = "";
        static bool IsLoad = true;

        public EventsPage (string CategoryId = "", string CategoryName = "")
		{
			InitializeComponent ();
            BindingContext = model.breaingNews;
            this.CategoryId = CategoryId;
            IsLoad = true;
            Title = CategoryName;

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
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                IsLoad = false;
                loadNewsListdata(CategoryId);
            }
            base.OnAppearing();
        }
        private async void loadNewsListdata(string CategoryId)
        {
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
            MainFrame.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                parameters.Add(new KeyValuePair<string, string>("category_id", string.IsNullOrEmpty(CategoryId) ? "1" : CategoryId));
                parameters.Add(new KeyValuePair<string, string>("city_id", AppData.UserCityId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewsList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                }
                else
                {
                    Items = JsonConvert.DeserializeObject<NewsListModel>(jsonstr);
                    if (Items.data.total_news <= 0)
                    {
                        NoDataPage.IsVisible = true;
                        MainFrame.IsVisible = false;
                    }
                }


                //lstView.ItemsSource = Items.data.news_list;
                ObservableCollection<BreaingNews> featured_news_list = new ObservableCollection<BreaingNews>();
                foreach (News_List news in Items.data.featured_news_list)
                {
                    featured_news_list.Add(new BreaingNews
                    {
                        ImageUrl = news.banner_image,
                        Name = news.news_title,
                        Id = news.news_id.ToString()

                    });
                }

                lstView.ItemsSource = Items.data.news_list;
                if (featured_news_list.Count() > 0)
                {
                    model.breaingNews = featured_news_list;
                    BindingContext = model;
                    CarouselZoos.IsVisible = true;
                }
                else
                {
                    CarouselZoos.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
            }

            MainFrame.IsVisible = true;
            Loader.IsVisible = false;
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
        private void DoSomething(object sender, EventArgs e)
        {
            loadNewsListdata(CategoryId);
        }
        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}