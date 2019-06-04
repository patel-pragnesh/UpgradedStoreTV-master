using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Component;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using TaazaTV.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static TaazaTV.ViewModel.ProductDetailsViewModel;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailsPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        List<int> SelectedVariants = new List<int>();
        ProductDetailsModel Items = new ProductDetailsModel();
        ProductDetailsViewModel vm = new ProductDetailsViewModel();
        int remAttr;
        string slug;
        string ErrorMsg = "Please select one option!!";
        public ProductDetailsPage(string pro_slug)
        {
            InitializeComponent();
            slug = pro_slug;
            InitialLoading(slug);
            this.BindingContext = vm;
        }

        private async void InitialLoading(string proSlug)
        {
            try
            {
                Loader.IsVisible = true;
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("device_type", "ANDROID"),
                    new KeyValuePair<string, string>("app_version", "2.0"),
                    new KeyValuePair<string, string>("product_slug", proSlug),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ProductDetailsAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                    NoDataPage.IsVisible = true;
                }
                else
                {
                    Items = JsonConvert.DeserializeObject<ProductDetailsModel>(jsonstr);
                    vm.ProductID = Items.data.product_details.product_id;
                    vm.Name = Items.data.product_details.product_name;
                    vm.Description = Items.data.product_details.product_description;
                    vm.Seller = Items.data.product_details.seller_name;
                    vm.SellerDesc = Items.data.product_details.seller_details;
                    vm.PriceRange = Items.data.product_details.price_range;
                    vm.CarImages = new List<Store_Product_Images>(Items.data.product_details.images);

                    
                    if (Items.data.product_options.Count() != 0)
                    {
                        vm.ProductOptions = new List<Product_Sku_Options>(Items.data.product_options);
                        VariantsListView.ItemsSource = vm.ProductOptions;
                        VariantsListView.HeightRequest = (Items.data.product_options.Count()*60) + 5;
                    }
                    Loader.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                NoDataPage.IsVisible = true;
                var x = ex.Message;
            }
        }

        private void VariantsSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as HorizontalList).SelectedItem != null)
                {
                    List<Product_Variant_Options> itemList = new List<Product_Variant_Options>((sender as HorizontalList).ItemsSource as List<Product_Variant_Options>);

                    foreach (var x in itemList)
                    {
                        if (x.IsSelected == true)
                        {
                            remAttr = x.variant_option_id;
                            SelectedVariants.Remove(remAttr);
                        }
                    }

                    vm.ProductOptions.SelectMany(a => a.variant_options.Where(b => b.variant_option_id == remAttr)
                             .Select(d => { d.background_color = "White"; d.IsSelected = false; return d; })).ToList();

                    Product_Variant_Options item = (sender as HorizontalList).SelectedItem as Product_Variant_Options;

                    vm.ProductOptions.SelectMany(a => a.variant_options.Where(b => b.variant_option_id == item.variant_option_id)
                             .Select(d => { d.background_color = "#2196F3"; d.IsSelected = true; return d; })).ToList();

                    SelectedVariants.Add(item.variant_option_id);

                    if (SelectedVariants.Count() == Items.data.product_options.Count())
                    {
                        Loader.IsVisible = true;
                        var NewBindingContext = Items.data.product_details.sku_variants.Where(z => z.variant_option_ids.Intersect(SelectedVariants).Count() == Items.data.product_options.Count()).FirstOrDefault();
                        vm.CarImages = NewBindingContext.images.ToList();
                        vm.Description = NewBindingContext.description;
                        vm.OfferPrice = NewBindingContext.sale_price;
                        vm.Price = NewBindingContext.regular_price;
                        vm.SkuID = NewBindingContext.sku_id;
                        vm.ProductID = NewBindingContext.product_id;
                        vm.SkuID = NewBindingContext.sku_id;

                        PriceStack.IsVisible = false;
                        OfferStack.IsVisible = true;
                        Loader.IsVisible = false;
                    }
                }
            }
         
            catch
            {
                Loader.IsVisible = false;
                ErrorMsg = "This product is  not availbale in stock!!!";
            }
        }

        private async void AddToCartClicked(object sender, EventArgs e)
        {
            if (vm.SkuID != null)
            {
                try
                {
                    Loader.IsVisible = true;
                    List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                    {
                     new KeyValuePair<string, string>("user_id", AppData.UserId),
                     new KeyValuePair<string, string>("mode", "add"),
                     new KeyValuePair<string, string>("product_id", vm.ProductID),
                     new KeyValuePair<string, string>("product_sku_id", vm.SkuID),
                     new KeyValuePair<string, string>("quantity", "1"),
                    };

                    var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.AddRemoveCartAPI], parameters);
                    if (jsonstr.ToString() == "NoInternet")
                    {
                        Loader.IsVisible = false;
                        await DisplayAlert("Alert", "Some Error Occured!!", "OK");
                    }

                    else
                    {
                        var Items = JsonConvert.DeserializeObject<SuccessResponseModel>(jsonstr);
                        if(Items.responseText == "Success")
                        {
                            Loader.IsVisible = false;
                            await DisplayAlert("Alert", "Product Added To Cart!!", "OK");
                        }
                        Loader.IsVisible = false;
                    }
                }
                catch (Exception ex)
                {
                    Loader.IsVisible = false;
                    await DisplayAlert("Alert", "Some Error Occured!!", "OK");
                }
            }
            else
            {
                await DisplayAlert("Alert", ErrorMsg, "OK");
            }
        }

        private void ListViewTapped(object sender, ItemTappedEventArgs e)
        {
            VariantsListView.SelectedItem = null;
        }

        private async void BuyNowClicked(object sender, EventArgs e)
        {
            if (vm.SkuID != null)
            {
                try
                {
                    Loader.IsVisible = true;
                    List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                    {
                     new KeyValuePair<string, string>("user_id", AppData.UserId),
                     new KeyValuePair<string, string>("mode", "add"),
                     new KeyValuePair<string, string>("product_id", vm.ProductID),
                     new KeyValuePair<string, string>("product_sku_id", vm.SkuID),
                     new KeyValuePair<string, string>("quantity", "1"),
                    };

                    var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.AddRemoveCartAPI], parameters);
                    if (jsonstr.ToString() == "NoInternet")
                    {
                        Loader.IsVisible = false;
                        await DisplayAlert("Alert", "Some Error Occured!!", "OK");
                    }

                    else
                    {
                        var Items = JsonConvert.DeserializeObject<SuccessResponseModel>(jsonstr);
                        if (Items.responseText == "Success")
                        {
                            Loader.IsVisible = false;
                            await Navigation.PushAsync(new CartPage());
                        }
                        Loader.IsVisible = false;
                    }
                }
                catch (Exception ex)
                {
                    Loader.IsVisible = false;
                    await DisplayAlert("Alert", "Some Error Occured!!", "OK");
                }
            }
            else
            {
                await DisplayAlert("Alert", ErrorMsg, "OK");
            }
           
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            InitialLoading(slug);
        }
    }
}