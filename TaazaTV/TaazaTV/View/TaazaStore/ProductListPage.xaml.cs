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
        public ProductListPage(string slugVal)
        {
            InitializeComponent();
            InitialLoading(slugVal);
        }

        private async void InitialLoading(string slug)
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                  //  new KeyValuePair<string, string>("category_slug", slug),
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
    }
}