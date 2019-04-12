using Newtonsoft.Json;
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
	public partial class SubCategoryPage : ContentPage
	{

        HttpRequestWrapper wrapper = new HttpRequestWrapper();

        public SubCategoryPage (string slugVal)
		{
			InitializeComponent ();
            InitialLoading(slugVal);
		}

        private async void InitialLoading(string slug)
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("category_slug", slug),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.FirstLevelCategoryListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {

                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<MainCategoryListModel>(jsonstr);
                    SubCatDetailListView.ItemsSource = Items.data.categorys;
                   //  CategoryListView.ItemsSource = Items.data.categorys;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void SubCategoryTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductListPage(((sender as Grid).Children[0] as Label).Text, ""));
        }
    }
}