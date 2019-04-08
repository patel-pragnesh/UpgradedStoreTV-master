using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
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
    public partial class Dashboard : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        public Dashboard()
        {
            InitializeComponent();
            HeaderView.Content = new StoreHeaderView();
            InitialLoading();
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

        private void StoreFeatured_Tapped(object sender, EventArgs e)
        {

        }

        private async void NavToSellerList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SellerListPage());
        }
    }
}