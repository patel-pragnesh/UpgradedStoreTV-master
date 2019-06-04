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
        string slug;

        public SubCategoryPage (string slugVal)
		{
			InitializeComponent ();
            slug = slugVal;
            InitialLoading(slug);
		}

        private async void InitialLoading(string slug)
        {
            try
            {
                Loader.IsVisible = true;
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("category_slug", slug),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.FirstLevelCategoryListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                    NoDataPage.IsVisible = true;
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<MainCategoryListModel>(jsonstr);
                    SubCatDetailListView.ItemsSource = Items.data.categorys;
                    //  CategoryListView.ItemsSource = Items.data.categorys;

                    Loader.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                NoDataPage.IsVisible = true;
            }
        }

        private async void SubCategoryTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductListPage(((sender as Grid).Children[0] as Label).Text, "", ""));
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            InitialLoading(slug);
        }
    }
}