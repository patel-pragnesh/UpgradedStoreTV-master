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
    public partial class CategoryListPage : ContentPage
    {
        CategoryAPIResponse Items = new CategoryAPIResponse();
        static bool IsLoad = true;
        bool isLoading;
        Page page;
        //public bool _isRefreshing = false;
        //public bool IsRefreshing
        //{
        //    get { return _isRefreshing; }
        //    set
        //    {
        //        _isRefreshing = value;
        //        OnPropertyChanged(nameof(IsRefreshing));
        //    }
        //}

        //public Command PullToRefreshCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            IsRefreshing = true;
        //            LoadCategoryList();
        //            IsRefreshing = false;
        //        });
        //    }
        //}

        public CategoryListPage()
        {
            InitializeComponent();
            LoadCategoryList();

        }

        protected override async void OnAppearing()
        {
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                NoInternet.IsVisible = false;
                NoDataPage.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);

                LoadCategoryList();
                IsLoad = false;
            }
            else
            {
                //MainFrame.IsVisible = true;
                Loader.IsVisible = false;
            }

            base.OnAppearing();
        }

        private async void LoadCategoryList()
        {
           
            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Category], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    NoDataPage.IsVisible = false;
                    CategoryListView.IsVisible = false;
                    //await DisplayAlert("No Connection", "Please check your internet connection", "OK");
                    //await Navigation.PushModalAsync(new NoInternetPage());
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<CategoryAPIResponse>(jsonstr);
                    }
                    catch
                    {
                        CategoryListView.IsVisible = false;
                        NoInternet.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    ObservableCollection<CategoryListModel> categories = new ObservableCollection<CategoryListModel>();
                    CategoryListView.ItemsSource = categories;
                    int i;
                    foreach (var item in Items.data.category)
                    {
                        for (i = 0; i <= Convert.ToInt32(Items.data.category.Count.ToString()); i++)
                        {
                            categories.Add(new CategoryListModel
                            {

                                CategoryId = Items.data.category[i].category_id,
                                CategoryName = Items.data.category[i].category_name
                            });
                        }
                    }
                    if (categories.Count <= 0)
                    {
                        NoDataPage.IsVisible = true;
                        CategoryListView.IsVisible = false;
                    }
                    CategoryListView.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
            }
            Loader.IsVisible = false;
          
            //CategoryListView.IsPullToRefreshEnabled = true;
            //CategoryListView.RefreshCommand = PullToRefreshCommand;
            //CategoryListView.IsRefreshing = IsRefreshing;
            //CategoryListView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as CategoryListModel;

            TaazaTV.View.News.NewsSubmitPage.CategoryName = item.CategoryName.ToString();
            TaazaTV.View.News.NewsSubmitPage.CategoryId = item.CategoryId.ToString();
            //TaazaTV.View.Accounts.ProfilePage.CityName = item.DisplayName.ToString();
            //TaazaTV.View.Accounts.ProfilePage.CityId = item.DisplayId.ToString();

          //  var item = e.Item as CategoryList;
            await Navigation.PopModalAsync();
        }

        private async void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(100);

            LoadCategoryList();
        }
        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}