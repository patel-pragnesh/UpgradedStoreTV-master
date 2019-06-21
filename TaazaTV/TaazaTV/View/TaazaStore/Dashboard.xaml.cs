using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaazaTV.Controls;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();

        public Dashboard()
        {
            InitializeComponent();
            AppData.PopUpProblem = false;
            HeaderView.Content = new StoreHeaderView();
            InitialLoading();
            PartialLoading();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            HeaderView.Content = new StoreHeaderView();
        }

        private async void PartialLoading()
        {
            Loader.IsVisible = true;
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                //{
                //    new KeyValuePair<string, string>("category_slug", Constant.CompanyID),
                //    new KeyValuePair<string, string>("user_id", AppData.UserId)
                //};

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetStoreDashboardDataAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoDataPage.IsVisible = true;
                    Loader.IsVisible = false;
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<StoreDashModel>(jsonstr);
                    StoreFeaturedCarousel.ItemsSource = Items.data.features;
                    OffersListView.ItemsSource = Items.data.offers;
                    TopSellersListView.ItemsSource = Items.data.top_sellers;
                    TopSellersListView.HeightRequest = (Items.data.top_sellers.Count() * 65) + 15;
                    Loader.IsVisible = false;
                }
            }
            catch(Exception ex)
            {
                Loader.IsVisible = false;
                NoDataPage.IsVisible = true;
            }
        }

        private async void InitialLoading()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                //{
                //    new KeyValuePair<string, string>("category_slug", Constant.CompanyID),
                //    new KeyValuePair<string, string>("user_id", AppData.UserId)
                //};

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.FirstLevelCategoryListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<MainCategoryListModel>(jsonstr);

                    CategoryListView.ItemsSource = Items.data.categorys;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void StoreFeatured_Tapped(object sender, EventArgs e)
        {
            if(((sender as Grid).Children[0] as Label).Text.ToString() != string.Empty)
            {
                await Navigation.PushAsync(new ProductDetailsPage(((sender as Grid).Children[0] as Label).Text.ToString()));
            }
        }

        private async void NavToSellerList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SellerListPage());
        }

        private async void CategoryListTapped(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new SubCategoryPage(((sender as StackLayout).Children[0] as Label).Text));
        }

        private  void SellerDeatilsNav(object sender, SelectedItemChangedEventArgs e)
        {
            
            //(sender as ListView). = -1;

            //var model = ;
            //BookingsList.SelectedItem = null;
            //await Navigation.PushAsync(new BookingDetailsPage(model));
        }

        private async void NavSellerDeatils(object sender, ItemTappedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
            await Navigation.PushAsync(new SellerDetailsPage((e.Item as TopSeller).seller_id.ToString()));
        }


        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            InitialLoading();
            PartialLoading();
        }

        private async void OffersListTapped(object sender, EventArgs e)
        {
            if (((sender as Grid).Children[0] as Label).Text.ToString() != string.Empty)
            {
                await Navigation.PushAsync(new ProductListPage(((sender as Grid).Children[0] as Label).Text.ToString(), "", ""));
            }
        }
    }
}

 