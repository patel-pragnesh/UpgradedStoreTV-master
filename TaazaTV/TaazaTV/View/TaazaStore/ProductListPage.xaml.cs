using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Controls;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductListPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();

        string slugVal, searchText, sellerId, priceTo, sorting, priceFrom;

        int pageNumber;

        StringBuilder sb = new StringBuilder();

        ObservableCollection<Store_Product_List> ProductListCollection;

        private void SortButtonClicked(object sender, EventArgs e)
        {
            SortingPicker.Focus();
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            InitialLoading(slugVal, searchText, sellerId, "", "", "", "");
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            HeaderView.Content = new StoreHeaderView();
        }

        private void PullToRefreshListView(object sender, EventArgs e)
        {
            pageNumber = 1;
            ProductListCollection = new ObservableCollection<Store_Product_List>();
            InitialLoading(slugVal, searchText, sellerId, priceFrom, priceTo, sb.ToString(), sorting);
            ProductListView.IsRefreshing = false;
        }

        public ProductListPage(string slug_val, string search_text, string seller_ID)
        {
            InitializeComponent();
            HeaderView.Content = new StoreHeaderView();
            ProductListCollection = new ObservableCollection<Store_Product_List>();
            pageNumber = 1;
            slugVal = slug_val;
            searchText = search_text;
            sellerId = seller_ID;

            InitialLoading(slugVal, searchText, sellerId, "", "", "", "");

            MessagingCenter.Subscribe<List<string>>(this, "ApplyFilters", (st) =>
            {
                sb = new StringBuilder();
                for (int i = 2; i < st.Count(); i++)
                {

                    sb.Append(st[i] + ",");
                }

                priceFrom = st[0];
                priceTo = st[1];

                InitialLoading(slugVal, searchText, sellerId, priceFrom, priceTo, sb.ToString(), "");
            });
        }

        private async void InitialLoading(string slug, string search, string sellerid, string pricefrom, string priceto, string brandfilters, string sortBy)
        {
            try
            {
                if(pageNumber == 1)
                {
                    Loader.IsVisible = true;
                }
                
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                    new KeyValuePair<string, string>("category_slug", slug),
                    new KeyValuePair<string, string>("search_text", search),
                    new KeyValuePair<string, string>("seller_id", sellerid),
                    new KeyValuePair<string, string>("price_search_from", pricefrom),
                    new KeyValuePair<string, string>("price_search_to", priceto),
                    new KeyValuePair<string, string>("brand_search", brandfilters),
                    new KeyValuePair<string, string>("sort_by", sortBy),  // Like price_high_to_low or price_low_to_high
                    new KeyValuePair<string, string>("page", pageNumber.ToString()), // call this after lazy loading
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GeneralProductListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                    NoDataPage.IsVisible = true;
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<ProductListModel>(jsonstr);
                    foreach(var item in Items.data.product_list)
                    {
                        ProductListCollection.Add(item);
                    }
                 // ProductListView.ItemsSource = Items.data.product_list.ToList();
                    ProductListView.ItemsSource = ProductListCollection.ToList();
                    Loader.IsVisible = false;
                    if (ProductListCollection.Count() == 0)
                        NoDataPage.IsVisible = true;
                    if(Items.data.hasMoreData == true)
                    {
                        pageNumber++;
                        InitialLoading(slugVal, searchText, sellerId, priceFrom, priceTo, sb.ToString(), sorting);
                    }
                       
                }
            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                NoDataPage.IsVisible = true;
                var x = ex.Message;
            }
        }

        private async void FilterBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FilterPage());
        }

        private void ApplySorting(object sender, EventArgs e)
        {
            if (SortingPicker.SelectedIndex == 0)
            {
                sorting = "price_low_to_high";
                InitialLoading(slugVal, searchText, sellerId, priceFrom, priceTo, sb.ToString(), sorting);
            }

            if (SortingPicker.SelectedIndex == 1)
            {
                sorting = "price_high_to_low";
                InitialLoading(slugVal, searchText, sellerId, priceFrom, priceTo, sb.ToString(), sorting);
            }
        }

        private async void NavToProductDetails(object sender, ItemTappedEventArgs e)
        {
            ProductListView.SelectedItem = null;
            await Navigation.PushAsync(new ProductDetailsPage((e.Item as Store_Product_List).product_slug));
        }

    }
}
