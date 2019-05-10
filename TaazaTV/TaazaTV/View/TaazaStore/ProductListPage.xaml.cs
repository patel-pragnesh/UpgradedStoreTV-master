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
        public ProductListPage(string slugVal, string searchText, string sellerID)
        {
            InitializeComponent();
            InitialLoading(slugVal, searchText, sellerID);
            LoadFilterOptions();
        }

        private async void LoadFilterOptions()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                  //  new KeyValuePair<string, string>("category_slug", slug),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.FilterOptionsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<ProductFilterModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void InitialLoading(string slug, string search, string sellerid)
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

        private async void NavToProductDetails(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new ProductDetailsPage((e.Item as Store_Product_List).product_slug));
        }
    }
}