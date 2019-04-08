using Newtonsoft.Json;
using Plugin.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.View.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubmitedNewsPage : ContentPage
    {
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
                    loadSubmiteddata();
                    IsRefreshing = false;
                });
            }
        }

        protected override async void OnAppearing()
        {
                        
            //if (IsLoad)
            //{
                MainFrame.IsVisible = false;
                NoDataPage.IsVisible = false;
                NoInternet.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);
              //  IsLoad = false;
                loadSubmiteddata();
               
            //}
            //else
            //{
            //    //MainFrame.IsVisible = true;
            //    Loader.IsVisible = false;
            //}

            base.OnAppearing();
        }
        UserWiseSubmitNewsModel Items = new UserWiseSubmitNewsModel();
        public SubmitedNewsPage()
        {
            InitializeComponent();
            loadSubmiteddata();
        }
        private async void loadSubmiteddata()
        {
          
            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                //parameters.Add(new KeyValuePair<string, string>("category_id", "1"));
                parameters.Add(new KeyValuePair<string, string>("city_id", AppData.UserCityId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UserwiseNewsList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                    Addnewssign.IsVisible = false;
                    NoDataPage.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<UserWiseSubmitNewsModel>(jsonstr);
                    }
                    catch
                    {
                        NoInternet.IsVisible = false;
                        MainFrame.IsVisible = false;
                        Addnewssign.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    Items = JsonConvert.DeserializeObject<UserWiseSubmitNewsModel>(jsonstr);
                    lstView.ItemsSource = Items.data.news_list;
                    if (Items.data.totla_news <= 0)
                    {
                        NoDataPage.IsVisible = true;
                        MainFrame.IsVisible = false;

                    }
                    else
                    {
                        MainFrame.IsVisible = true;
                        Addnewssign.IsVisible = true;
                    }
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



        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            UserwiseNews_List model = (UserwiseNews_List)e.Item;
            lstView.SelectedItem = null;
            await Navigation.PushAsync(new NewsDetailsPage(model.news_id.ToString()));
        }

        private void Share_Tapped(object sender, EventArgs e)
        {
            CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage
            {
                Title = "Motz Cod.es",
                Text = "Checkout Motz Cod.es! for all sorts of goodies",
                Url = "http://motzcod.es"
            });
        }

        //private async void SubmitNews_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new NewsSubmitPage());
        //}

        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                App.Current.MainPage = new MasterDetailsPage();
            });

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }

        private async void NewsForms_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsSubmitPage());
        }
        private async void DoSomething(object sender, EventArgs e)
        {
            MainFrame.IsVisible = false;
            NoDataPage.IsVisible = false;
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);

            loadSubmiteddata();
            IsLoad = false;
                 
        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            //await Navigation.PopAsync();
            App.Current.MainPage = new MasterDetailsPage();
        }

        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MasterDetailsPage();
        }
    }
}