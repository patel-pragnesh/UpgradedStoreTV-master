using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        StringBuilder sb = new StringBuilder();

        private void SortButtonClicked(object sender, EventArgs e)
        {
            SortingPicker.Focus();
        }

        public ProductListPage(string slug_val, string search_text, string seller_ID)
        {
            InitializeComponent();

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
                    new KeyValuePair<string, string>("page", ""), // call this after lazy loading
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GeneralProductListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<ProductListModel>(jsonstr);
                    ProductListView.ItemsSource = Items.data.product_list.ToList();
                }
            }
            catch (Exception ex)
            {
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
