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
 
namespace TaazaTV.View.Eventpoll
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PollContestListPage : ContentPage
    {
        PollContestList Items = new PollContestList();
        MonkeysViewModel model = new MonkeysViewModel();
        string Type = "";
        string BreakingNewsId = "";
        static bool IsLoad = true;

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
                    loadListdata(Type);
                    IsRefreshing = false;
                });
            }
        }


        public PollContestListPage(string Title, ImageSource Image, string Type)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Contests";
            //BindingContext = model.breaingNews;
            IsLoad = true;
            this.TitleText.Text = Title;
            this.TitleImage.Source = Image;
            this.Type = Type;

            int SlidePosition = 0;

        }

        protected override async void OnAppearing()
        {
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                NoInternet.IsVisible = false;
                NoDataPage.IsVisible = false;
                MainFrame.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);
                IsLoad = false;
                loadListdata(Type);
            }
            base.OnAppearing();
        }

        private async void loadListdata(string Type)
        {


            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("type", Type));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.PollContestQuestionList], parameters);
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
                        Items = JsonConvert.DeserializeObject<PollContestList>(jsonstr);
                    }
                    catch
                    {
                        NoInternet.IsVisible = false;
                        MainFrame.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    if (Items.data.poll_list.Count() <= 0)
                    {
                        NoDataPage.IsVisible = true;
                        MainFrame.IsVisible = false;
                    }
                    MainFrame.IsVisible = true;
                    lstView.ItemsSource = Items.data.poll_list;
                    lstView.HeightRequest = (Items.data.poll_list.Count() * lstView.RowHeight) + 5;

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
            Poll_List model = (Poll_List)e.Item;
            lstView.SelectedItem = null;
            await Navigation.PushAsync(new PollContestDetailsPage(model.poll_id.ToString(), model.question_title.ToString(), TitleImage.Source, Type));
        }

        private async void CarouselZoos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BreaingNews model = (BreaingNews)e.SelectedItem;
            BreakingNewsId = model.Id;
            //await Navigation.PushAsync(new NewsDetailsPage(model.Id));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NewsDetailsPage(BreakingNewsId));
        }

        private void CarouselZoos_PositionSelected(object sender, SelectedPositionChangedEventArgs e)
        {

        } 
        private async void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
            MainFrame.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);
            IsLoad = false;
            loadListdata(Type);
        }
        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}