using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaazaTV.View.TaazaCash;
using TaazaTV.View.Accounts;
using System.Collections.ObjectModel;
using TaazaTV.Services;

namespace TaazaTV.View.TaazaCash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestaurantListPage : ContentPage
    {
        RestaurantListModel Items = new RestaurantListModel();
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        ObservableCollection<FilterModel> filterModel = new ObservableCollection<FilterModel>();
        int i = 0;

        public RestaurantListPage()
        {
            InitializeComponent();
#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif
            if (AppData.IsLogin == false)
                TaazaCashTab.IsVisible = false;
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
            RestaurantlstView.IsVisible = false;
            CustomToolbar.IsVisible = false;
            Loader.IsVisible = true;
            TaazaCashAmount.Text = AppData.TaazaCash;
            LoadRestaurantList();
            LoadCityListForRestaurants();
        }

        private async void LoadRestaurantList()
        {
            try
            {

                var y = await CommonFunc.FooterAdFunc("restaurant");
                if (y != null && y.Count() > 0)
                {
                    CarouselAd.ItemsSource = y.ToList();
                    CarouselAd.IsVisible = true;
                }
                else
                {
                    CarouselAd.IsVisible = false;
                }

             
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                //parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                //parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                //parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.RestaurantList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<RestaurantListModel>(jsonstr);
                    }
                    catch (Exception ex)
                    {
                        NoDataPage.IsVisible = true;
                    }

                    foreach (var x in Items.data.restaurants.restaurant_data)
                    {
                        x.RestBannerImg = x.restaurant_images.Select(Y => Y.imageURL).FirstOrDefault();
                    }

                    BindingContext = Items.data.restaurants;
                    RestaurantlstView.ItemsSource = Items.data.restaurants.restaurant_data;

                    foreach (var x in Items.data.restaurant_category_cuisine)
                    {
                        filterModel.Add(new FilterModel() { Id = x.cuisine_id, Text = x.cuisine_name, Type = "restaurant_category_cuisine" });
                    }




                    StackLayout layoutall = new StackLayout();
                    layoutall.HeightRequest = 37;
                    layoutall.WidthRequest = (DetectWidth.Width) / 4;
                    var alltapGestureRecognizer = new TapGestureRecognizer();
                    alltapGestureRecognizer.Tapped += async (s, e) =>
                    {
                        StackLayout parent = (StackLayout)s;
                        ;
                        UpdateRestList(((Label)parent.Children[0]).Text, "", "", "");
                    };

                    layoutall.GestureRecognizers.Add(alltapGestureRecognizer);

                    Label AliasAll = new Label()
                    {
                        IsVisible = true,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.White,
                        Text = "All",
                    };

                    Label AllCategoryId = new Label()
                    {
                        IsVisible = false,
                        Text = "",
                    };

                    layoutall.Children.Add(AllCategoryId);
                    layoutall.Children.Add(AliasAll);
                    CategoryList.Children.Add(layoutall);


                    foreach (var x in Items.data.restaurant_category_list)
                    {
                        double Width = (DetectWidth.Width) / 4;
                        StackLayout layout = new StackLayout();
                        layout.HeightRequest = 37;
                        layout.WidthRequest = Width;
                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += async (s, e) =>
                        {
                            StackLayout parent = (StackLayout)s;
                            ;
                            UpdateRestList(((Label)parent.Children[0]).Text, "", "", "");
                        };

                        layout.GestureRecognizers.Add(tapGestureRecognizer);

                        Label Alias = new Label()
                        {
                            IsVisible = true,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            Text = x.category_name.ToString()
                        };

                        Label CategoryId = new Label()
                        {
                            IsVisible = false,
                            Text = x.category_id.ToString()
                        };

                        layout.Children.Add(CategoryId);
                        layout.Children.Add(Alias);
                        CategoryList.Children.Add(layout);
                    }




                        //foreach (var x in Items.data.restaurant_category_list)
                        //{
                        //    i++;
                        //    switch (i)
                        //    {
                        //        case 1:
                        //            Cat1Txt.Text = x.category_name;
                        //            // Cat1Img.Source = x.image_url;
                        //            Cat1ID.Text = x.category_id.ToString();
                        //            break;

                        //        case 2:
                        //            Cat2Txt.Text = x.category_name;
                        //            //   Cat2Img.Source = x.image_url;
                        //            Cat2ID.Text = x.category_id.ToString();
                        //            break;

                        //        case 3:
                        //            Cat3Txt.Text = x.category_name;
                        //            //  Cat3Img.Source = x.image_url;
                        //            Cat3ID.Text = x.category_id.ToString();
                        //            break;

                        //        case 4:
                        //            Cat4Txt.Text = x.category_name;
                        //            //   Cat4Img.Source = x.image_url;
                        //            Cat4ID.Text = x.category_id.ToString();
                        //            break;

                        //        default:
                        //            filterModel.Add(new FilterModel() { Id = x.category_id, Text = x.category_name, Type = "restaurant_category_list" });
                        //            break;
                        //    }

                        //}
                }

                FilterPicker.ItemsSource = filterModel.Select(x => x.Text).ToList();
                NoDataPage.IsVisible = false;
                RestaurantlstView.IsVisible = true;
                CustomToolbar.IsVisible = true;
                Loader.IsVisible = false;
                if (Items.data.restaurants.restaurant_data.Count() == 0)
                {
                    NoDataPage.IsVisible = true;
                }

            }
            catch (Exception ex)
            {
                NoDataPage.IsVisible = true;
            }
        }

        private async void LoadCityListForRestaurants()
        {

            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.RestaurantLoactionList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                   // NoInternet.IsVisible = true;
                   // MainFrame.IsVisible = false;
                    //NewsDetailslbl.IsVisible = false;
                }
                else
                {
                    try
                    {
                        var CityItems = JsonConvert.DeserializeObject<EventRestCityListModel>(jsonstr);
                        LocationFilterPicker.ItemsSource = CityItems.data.city_list.Select(X => X.city).ToList();
                    }

                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private async void NavToRestDetails(object sender, ItemTappedEventArgs e)
        {

            var model = (_Restaurant_Data)e.Item;
            RestaurantlstView.SelectedItem = null;
            await Navigation.PushAsync(new RestaurantDetailPage(model.restaurant_id.ToString()));
        }

        private async void BackButton_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void NavToCityList(object sender, EventArgs e)
        {
            LocationFilterPicker.Focus();
        }

        private void LocationFilterSelectionChanged(object sender, EventArgs e)
        {
            if (LocationFilterPicker.SelectedIndex != -1)
            {
                LocationFilterPicker.SelectedIndex = -1;
                var picker = (Picker)sender;
                var z = picker.SelectedItem.ToString();
                UpdateRestList("", "","", z);
            }
        }

        private async void TaazaCashTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }

        private void CarouselAd_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void CarouselAd_Tapped(object sender, EventArgs e)
        {

        }

        private void DoSomething(object sender, EventArgs e)
        {
            LoadRestaurantList();
        }

        private void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            UpdateRestList("", "", "", "");
        }

        private void OpenFilterSelection(object sender, EventArgs e)
        {
            FilterPicker.Focus();
        }

        private void LoadRestListCategory(object sender, EventArgs e)
        {
            StackLayout parent = (StackLayout)sender;
            var x = ((Label)parent.Children[0]).Text;
            UpdateRestList(x, "", "", "");
        }

        private void FilterSelectionChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var modelfilter = filterModel.Where(X => X.Text == picker.SelectedItem.ToString()).FirstOrDefault();
            if (modelfilter.Type == "restaurant_category_list")
            {
                UpdateRestList((modelfilter.Id).ToString(), "","", "");
            }
            else
            {
                UpdateRestList("",modelfilter.Id.ToString(), "", "");
            }
        }

        private void SearchRestaurantsPressed(object sender, EventArgs e)
        {
            UpdateRestList("", "", RestaurantSearchBar.Text, "");
        }

        public async void UpdateRestList(string CategoryTypeID, string CuisineTypeId, string SearchText, string LocationValue)
        {
            Loader.IsVisible = true;
            NoDataPage.IsVisible = false;
            RestaurantlstView.IsVisible = false;
            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("search_text", SearchText));
                parameters.Add(new KeyValuePair<string, string>("category_id", CategoryTypeID));
                parameters.Add(new KeyValuePair<string, string>("cuisine_id", CuisineTypeId));
                parameters.Add(new KeyValuePair<string, string>("location_search", LocationValue));

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.RestaurantList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                }
                else
                {
                    Items = JsonConvert.DeserializeObject<RestaurantListModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                NoDataPage.IsVisible = true;
            }

            if(Items.data.restaurants.restaurant_data.Count()==0)
            {
                NoDataPage.IsVisible = true;
                Loader.IsVisible = false;
            }
            foreach (var x in Items.data.restaurants.restaurant_data)
            {
                x.RestBannerImg = x.restaurant_images.Select(Y => Y.imageURL).FirstOrDefault();
            }
            RestaurantlstView.ItemsSource = Items.data.restaurants.restaurant_data;
            RestaurantlstView.HeightRequest = (Items.data.restaurants.restaurant_data.Count() * RestaurantlstView.RowHeight) + 2;
            RestaurantlstView.IsVisible = true;
            Loader.IsVisible = false;

        }

       
    }
}

