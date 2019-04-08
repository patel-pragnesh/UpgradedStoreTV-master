using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.View.News;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace TaazaTV.View.Social
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocialNewsPage : ContentPage
    {

        SocialNewsModel Items = new SocialNewsModel();

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

        public SocialNewsPage(String Title, ImageSource Image)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Social Network";
            IsLoad = true;
            TitleImage.Source = Image;
            TitleText.Text = Title;
        }

        protected override async void OnAppearing()
        {
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                Loader.IsVisible = true;
                MainFrame.IsVisible = false;
                NoInternet.IsVisible = false;
                NoDataPage.IsVisible = false;
                await Task.Delay(1000);

                SetData();
                IsLoad = false;
            }
           

            base.OnAppearing();
        }

        protected async void SetData()
        {
          
            //if (Items.data == null)
            //{
            HttpRequestWrapper wrapper = new HttpRequestWrapper();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
            parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));

            var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GroupList], parameters);
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
                    Items = JsonConvert.DeserializeObject<SocialNewsModel>(jsonstr);
                }
                catch
                {
                    lstView.IsVisible = false;
                    MainFrame.IsVisible = false;
                    NoInternet.IsVisible = false;
                    NoDataPage.IsVisible = true;
                }
                if (Items.data.groups.Count() <= 0)
                {
                    NoDataPage.IsVisible = true;
                    MainFrame.IsVisible = false;
                }
                else
                {
                    MainFrame.IsVisible = true;
                    lstView.ItemsSource = Items.data.groups;
                    lstView.HeightRequest = Items.data.groups.Count() * lstView.RowHeight;
                }
            }

            //}
           
            Loader.IsVisible = false;
            lstView.IsPullToRefreshEnabled = true;
            lstView.RefreshCommand = PullToRefreshCommand;
            lstView.IsRefreshing = IsRefreshing;
            lstView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");
        }

        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Group model = (Group)e.Item;
            lstView.SelectedItem = null;
            await Navigation.PushAsync(new SocialDetailsPage(model.group_id.ToString(), TitleImage.Source, TitleText.Text));

        }

        private async void DoSomething(object sender, EventArgs e)
        {
            Loader.IsVisible = true;
            MainFrame.IsVisible = false;
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
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