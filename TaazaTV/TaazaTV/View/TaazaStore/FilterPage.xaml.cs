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
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>() { };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.FilterOptionsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {

                }

                else
                {
                    var Items = JsonConvert.DeserializeObject<ProductFilterModel>(jsonstr);
                    this.BindingContext = Items;
                }
            }
            catch (Exception ex)
            {
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

        private void ResetClicked(object sender, EventArgs e)
        {

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
    }
}