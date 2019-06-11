using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.View.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.View.TaazaCash;
using System.Net.Http;
using TaazaTV.Services;
using TaazaTV.View.Accounts;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDetailsPage : ContentPage
    {
        EventDetailsResponseModel Items = new EventDetailsResponseModel();
        // List<ImageList> imageList = new List<ImageList>();
        string EventHeading, EventID, TopSubHeading, BannerImage = "";
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        HtmlWebViewSource html;

        public EventDetailsPage(string EventId)
        {
            InitializeComponent();
            if (AppData.IsLogin == false)
                TaazaCashTab.IsVisible = false;
            TaazaCashAmount.Text = AppData.TaazaCash;
            EventID = EventId;
            MainFrame.IsVisible = false;
            loadEventdata(EventId);
#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0, 20, 0, 0);
#endif
        }

        private async void loadEventdata(string Id)
        {
            var x = await CommonFunc.FooterAdFunc("event-details");
            if (x != null && x.Count() > 0)
            {
                CarouselAd.ItemsSource = x.ToList();
                CarouselAd.IsVisible = true;
            }
            else
            {
                CarouselAd.IsVisible = false;
            }

            MainFrame.IsVisible = false;
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;

            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("event_id", Id));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.EventDetails], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                    Bannerimg.IsVisible = false;
                    NoDataPage.IsVisible = false;

                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<EventDetailsResponseModel>(jsonstr);

                        if (Items.data.event_details.event_prices.Count() > 0)
                        {
                            PricelstView.HeightRequest = ((Items.data.event_details.event_prices.Count()) * 50) + 15;
                            PricelstView.ItemsSource = Items.data.event_details.event_prices;
                            PricesPicker.ItemsSource = Items.data.event_details.event_prices.Select(X => X.ticket_type).ToList();
                        }
                        else
                        {
                            BookNowButton.IsVisible = false;
                            BookNowCategory.IsVisible = false;
                            PriceListView.IsVisible = false;
                        }
                    }

                    catch
                    {
                        MainFrame.IsVisible = false;
                        Bannerimg.IsVisible = false;
                        NoInternet.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    html = new HtmlWebViewSource
                    {

                        Html = "<html><head><style type=\"text/css\">" +
                       " body {" +
                       "font-family: Arial;" +
                       "font-size: 15;" +
                       //"text-align: justify;" +
                       "}" +
                       "</style></head><body>" +
                       Items.data.event_details.event_description + "</body></html>"

                        //  Html = Items.data.event_details.event_description
                    };
                    EventDesc.Source = html;
                    EventHeading = Items.data.event_details.event_name;
                    TopSubHeading = Items.data.event_details.event_tag_line;

                    var html1 = new HtmlWebViewSource
                    {
                        Html = "<iframe width=\"100%\" height=\"200\" frameborder=\"0\" scrolling=\"no\" marginheight=\"0\" marginwidth=\"0\" src = \"https://maps.google.com/maps?q=" + Items.data.event_details.event_vanue.latitude + "," + Items.data.event_details.event_vanue.longitude + "&hl=es;z=14&amp;output=embed\" ></ iframe > "
                    };
                    Map.Source = html1;
                    BannerImage = Items.data.event_details.banner_image;
                    BindingContext = Items.data.event_details;
                }
            }
            catch (Exception ex)
            {
                NoDataPage.IsVisible = true;
            }

            MainFrame.IsVisible = true;
            Loader.IsVisible = false;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            loadEventdata(EventID);
        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Banner_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ZoomImagePage(BannerImage));
        }

        private async void NavToPayment(object sender, EventArgs e)
        {
            if (!AppData.PickerProblem)
            {
                PricesPicker.SelectedIndex = -1;
                var picker = (Picker)sender;
                var modelfilter = Items.data.event_details.event_prices.Where(X => X.ticket_type == picker.SelectedItem.ToString()).FirstOrDefault();
                AppData.PickerProblem = true;
                await Navigation.PushAsync(new PayConfirmPage(EventID, BannerImage, EventHeading, TopSubHeading, modelfilter.price_id, Convert.ToInt32(modelfilter.max_taazacash_can_use)));
            }
        }

        private void PopUpPicker(object sender, EventArgs e)
        {
            if (AppData.UserId == "")
            {
                Navigation.PushAsync(new RegistrationPage());
            }
            else
                PricesPicker.Focus();
        }

        private async void BackBtn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void NavToTransactionDetails(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }

        string AdURL;
        private void CarouselAd_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                PreniumAdModel model = (PreniumAdModel)e.SelectedItem;
                AdURL = (model.data.Ad_list.Select(x => x.Add_image)).ToString();
            }
            catch (Exception ex)
            {
                Logging.Write(ex, "CarouselAd_ItemSelected");
            }
        }

        private void CarouselAd_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(AdURL))
                    Device.OpenUri(new Uri(AdURL));
            }
            catch (Exception ex)
            {
                Logging.Write(ex, "CarouselAd_Tapped");
            }
        }
    }
}
