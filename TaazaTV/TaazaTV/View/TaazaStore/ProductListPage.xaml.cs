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
        public ProductListPage()
        {
            InitializeComponent();
            InitialLoading();

        }

        private async void InitialLoading()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),

                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GeneralProductListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<ProductListModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }
    }
}