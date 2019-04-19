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

namespace TaazaTV.View.TaazaStore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilterPage : ContentPage
	{
        HttpRequestWrapper wrapper = new HttpRequestWrapper();

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
    }
}