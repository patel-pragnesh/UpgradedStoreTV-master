using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Component;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace TaazaTV.View.TaazaStore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilterPage : ContentPage
	{
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        List<string> retParams = new List<string>();


        public FilterPage ()
		{
			InitializeComponent ();
            InitialDataLoading();
		}

        private async void InitialDataLoading()
        {
            try
            {
                Loader.IsVisible = true;
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>() { };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.FilterOptionsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoDataPage.IsVisible = true;
                    Loader.IsVisible = false;
                }

                else
                {
                    var Items = JsonConvert.DeserializeObject<ProductFilterModel>(jsonstr);
                    this.BindingContext = Items;
                    Loader.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                NoDataPage.IsVisible = true;
                Loader.IsVisible = false;
                var x = ex.Message;
            }
        }

        private async void BackBtnTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void FilterListTapped(object sender, ItemTappedEventArgs e)
        {
            BrandListView.SelectedItem = null;
        }

        private async void ResetClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void ApplyClicked(object sender, EventArgs e)
        {
            retParams.Insert(0, RangePrice.LeftValue.ToString());
            retParams.Insert(1, RangePrice.RightValue.ToString());
            MessagingCenter.Send<List<string>>(retParams, "ApplyFilters");
            await Navigation.PopModalAsync();
        }

        private void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            if((sender as CheckBox).Checked == true)
            {
                retParams.Add((((sender as CheckBox).Parent as StackLayout).Children[0] as Label).Text);
            }
            else
            {
                retParams.Remove((((sender as CheckBox).Parent as StackLayout).Children[0] as Label).Text);
            }
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            InitialDataLoading();
        }
    }
}