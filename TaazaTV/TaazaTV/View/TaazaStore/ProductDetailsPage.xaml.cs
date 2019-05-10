using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using TaazaTV.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailsPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        List<int> SelectedVariants = new List<int>();
        ProductDetailsModel Items = new ProductDetailsModel();
        ProductDetailsViewModel vm = new ProductDetailsViewModel();
        public ProductDetailsPage(string pro_slug)
        {
            InitializeComponent();
            InitialLoading(pro_slug);
            this.BindingContext = vm;
        }

        private async void InitialLoading(string proSlug)
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                    new KeyValuePair<string, string>("product_slug", proSlug),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ProductDetailsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {

                }
                else
                {
                    Items = JsonConvert.DeserializeObject<ProductDetailsModel>(jsonstr);
                    vm.Name = Items.data.product_details.product_name;
                    vm.Description = Items.data.product_details.product_description;
                    vm.Seller = Items.data.product_details.product_name;
                    vm.SellerDesc = Items.data.product_details.product_name;
                    vm.PriceRange = Items.data.product_details.price_range;
                    vm.CarImages = new List<Store_Product_Images>(Items.data.product_details.images);

                    if(Items.data.product_options.Count() != 0)
                    {
                        VariantsListView.ItemsSource = Items.data.product_options;
                        VariantsListView.HeightRequest = (Items.data.product_options.Count()*60) + 5;
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if(SelectedVariants.Count() == Items.data.product_options.Count())
            {
                var NewBindingContext = Items.data.product_details.sku_variants.Where(z => z.variant_option_ids.Intersect(SelectedVariants).Count() == Items.data.product_options.Count()).FirstOrDefault();

                vm.CarImages = NewBindingContext.images.ToList();
                vm.Description = NewBindingContext.description;
                vm.OfferPrice = NewBindingContext.sale_price;
                vm.Price = NewBindingContext.regular_price;
                vm.ProductID = NewBindingContext.product_id;
                vm.SkuID = NewBindingContext.sku_id;
            }
           
        }

        private void AddCartClicked(object sender, EventArgs e)
        {

        }

        //private void OnVariantSelected(object sender, EventArgs e)
        //{
        //    (sender as Frame).BackgroundColor = Color.Purple;
        //}

        private void VariantsSelectionChanged(object sender, EventArgs e)
        {

        }
    }
}