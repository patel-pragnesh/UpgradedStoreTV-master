using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.Services;
using TaazaTV.View.Accounts;
using TaazaTV.View.TaazaStore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaCash
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestaurantDetailPage : ContentPage
	{
        RestaurantDetailsData Items = new RestaurantDetailsData();
        // List<ImageList> imageList = new List<ImageList>();
        string RestaurantHeading, RestaurantID, BannerImage = "";

       
        HttpRequestWrapper wrapper = new HttpRequestWrapper();

        public RestaurantDetailPage (string RestaurantId)
		{
			InitializeComponent ();

#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif
            if (AppData.IsLogin == false)
                TaazaCashTab.IsVisible = false;
            MainFrame.IsVisible = false;
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            TaazaCashAmount.Text = AppData.TaazaCash;
            RestaurantID = RestaurantId;
            LoadRestaurantDetails(RestaurantID);
		}

        private async void BackBtn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void NavToTransactionDetails(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }

        private async void LoadRestaurantDetails(string Id)
        {
            //var x = await CommonFunc.FooterAdFunc("restaurant-details");
            //if (x != null && x.Count() > 0)
            //{
            //    CarouselAd.ItemsSource = x.ToList();
            //    CarouselAd.IsVisible = true;
            //}
            //else
            //{
            //    CarouselAd.IsVisible = false;
            //}

            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("restaurant_id", Id));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.RestaurantDetails], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    Loader.IsVisible = false;
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                    Bannerimg.IsVisible = false;
                    NoDataPage.IsVisible = false;
                }
                else
                {
                    try
                    {
                       var Items = JsonConvert.DeserializeObject<RestaurantDetailModel>(jsonstr);

                        var html = new HtmlWebViewSource
                        {
                            Html = Items.data.restaurant_details.description
                        };
                        RestDesc.Source = html;
                        RestaurantHeading = Items.data.restaurant_details.restaurant_name;
                        BannerImage = Items.data.restaurant_details.restaurant_images.Select(X => X.imageURL).FirstOrDefault();

                        var html1 = new HtmlWebViewSource
                        {
                            Html = "<iframe width=\"100%\" height=\"200\" frameborder=\"0\" scrolling=\"no\" marginheight=\"0\" marginwidth=\"0\" src = \"https://maps.google.com/maps?q=" + Items.data.restaurant_details.restaurant_vanue.latitude + "," + Items.data.restaurant_details.restaurant_vanue.longitude + "&hl=es;z=14&amp;output=embed\" ></ iframe > "
                        };
                        Map.Source = html1;
                        Bannerimg.Source = BannerImage;
                        BindingContext = Items.data.restaurant_details;
                        MainFrame.IsVisible = true;
                        Loader.IsVisible = false;
                    }
                    catch(Exception ex)
                    {
                        MainFrame.IsVisible = false;
                        Bannerimg.IsVisible = false;
                        NoInternet.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                NoDataPage.IsVisible = true;
            }
        }

        private void NavToPayment(object sender, EventArgs e)
        {
            if(AppData.UserId == "")
            {
                Navigation.PushAsync(new RegistrationPage());
            }
            else
            Navigation.PushAsync(new PayConfirmPage(RestaurantID, BannerImage, RestaurantHeading));
        }

        private void DoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            NoInternet.IsVisible = false;
            LoadRestaurantDetails(RestaurantID);
        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            NoInternet.IsVisible = false;
            await Navigation.PopAsync();
        }

        private async void PopUpPaymentRestaurant(object sender, EventArgs e)
        {
            await PopupNavigation.PushAsync(new PopUpTaskView(RestaurantID,"2"), true);
        }

        //private void CarouselAd_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    try
        //    {
        //        PreniumAdModel model = (PreniumAdModel)e.SelectedItem;
        //        AdURL = (model.data.Ad_list.Select(x => x.Add_image)).ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Write(ex, "CarouselAd_ItemSelected");
        //    }
        //}

        //private void CarouselAd_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(AdURL))
        //            Device.OpenUri(new Uri(AdURL));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Write(ex, "CarouselAd_Tapped");
        //    }
        //}
    }
}