using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.Services;
using TaazaTV.View.Accounts;
using TaazaTV.View.TaazaCash;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventListPage : ContentPage
    {
        EventListModel Items = new EventListModel();
        BreakingEventsViewModel model = new BreakingEventsViewModel();
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        ObservableCollection<FilterModel> filterModel = new ObservableCollection<FilterModel>();
        int i = 0;
        string CategoryId = "";
        string EventsID = "";
        static bool IsLoad = true;

        public bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public Command PullToRefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    loadEventListdata(CategoryId);
                    IsRefreshing = false;
                });
            }
        }


        public EventListPage(string CategoryId, string Title, ImageSource Image)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Events";
            if (AppData.IsLogin == false)
                TaazaCashTab.IsVisible = false;
            BindingContext = model.breaingEvents;
            this.CategoryId = CategoryId;
            IsLoad = true;
            //Title = CategoryName;
            int SlidePosition = 0;

#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif

            TaazaCashAmount.Text = AppData.TaazaCash;
            LoadCityListForEvents();

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                try
                {
                    SlidePosition++;
                    if (Items != null)
                    {
                        if (SlidePosition == Items.data.featured_event.Count())
                            SlidePosition = 0;
                    }
                    else
                    {
                        SlidePosition = 0;
                    }

                    CarouselZoos.Position = SlidePosition;

                }
                catch { }
                return true;
            });
        }

        private async void LoadCityListForEvents()
        {

            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.EventLocationList], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
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
            catch(Exception ex)
            {

            }
        }

        protected override async void OnAppearing()
        {

            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                var ScreenWidth = MainFrame.Width;
                CarouselZoos.HeightRequest = (ScreenWidth / 16) * 9;
                NoInternet.IsVisible = false;
                NoDataPage.IsVisible = false;
                MainFrame.IsVisible = false;
                CustomToolbar.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);
                IsLoad = false;
                var x = await CommonFunc.FooterAdFunc("event");
                if (x != null && x.Count() > 0)
                {
                    CarouselAd.ItemsSource = x.ToList();
                    CarouselAd.IsVisible = true;
                }
                else
                {
                    CarouselAd.IsVisible = false;
                }
                loadEventListdata(CategoryId);
            }
            base.OnAppearing();
        }

        private async void loadEventListdata(string CategoryId)
        {

            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                if (!string.IsNullOrEmpty(this.CategoryId))
                {
                    parameters.Add(new KeyValuePair<string, string>("category_id", CategoryId));
                }
                //  parameters.Add(new KeyValuePair<string, string>("page ", "1"));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Eventlist], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                    //NewsDetailslbl.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<EventListModel>(jsonstr);
                    }
                    catch
                    {
                        lstView.IsVisible = false;
                        NoInternet.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    BindingContext = Items.data.events.event_data;
                    if (Items.data.events.total_events <= 0 && Items.data.featured_event.Count() <= 0)
                    {
                        NoDataPage.IsVisible = true;
                        MainFrame.IsVisible = false;
                    }

                    Cat1Txt.Text = "All";
                    ////   Cat1Img.Source = x.image_url;
                    Cat1ID.Text = "";

                    filterModel.Clear();
                    foreach (var x in Items.data.event_category_list)
                    {
                        i++;
                        switch (i)
                        {
                            //case 1:
                            //    //Cat1Txt.Text = x.category_name;
                            //    ////   Cat1Img.Source = x.image_url;
                            //    //Cat1ID.Text = x.category_id.ToString();
                            //    break;

                            case 1:
                                Cat2Txt.Text = x.category_name;
                                //  Cat2Img.Source = x.image_url;
                                Cat2ID.Text = x.category_id.ToString();
                                break;

                            case 2:
                                Cat3Txt.Text = x.category_name;
                                //Cat3Img.Source = x.image_url;
                                Cat3ID.Text = x.category_id.ToString();
                                break;

                            case 3:
                                Cat4Txt.Text = x.category_name;
                                //  Cat4Img.Source = x.image_url;
                                Cat4ID.Text = x.category_id.ToString();
                                break;

                            default:
                                filterModel.Add(new FilterModel() { Id = x.category_id, Text = x.category_name, Type = "restaurant_category_list" });
                                break;
                        }

                    }

                    FilterPicker.ItemsSource = filterModel.Select(x => x.Text).ToList();

                    lstView.ItemsSource = Items.data.events.event_data;

                    ObservableCollection<BreaingEvents> featured_event = new ObservableCollection<BreaingEvents>();
                    foreach (_Event_Data Events in Items.data.featured_event)
                    {
                        featured_event.Add(new BreaingEvents
                        {
                            ImageUrl = Events.banner_image,
                            Name = Events.event_name,
                            Id = Events.event_id.ToString()
                        });
                    }
                    lstView.ItemsSource = Items.data.events.event_data;
                    lstView.HeightRequest = (Items.data.events.event_data.Count() * lstView.RowHeight) + 2;

                    if (featured_event.Count() > 0)
                    {
                        model.breaingEvents = featured_event;
                        BindingContext = model;
                        CarouselZoos.IsVisible = true;
                    }
                    else
                    {
                        CarouselZoos.IsVisible = false;
                    }

                    MainFrame.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
            }

            CustomToolbar.IsVisible = true;
            Loader.IsVisible = false;
            //lstView.IsPullToRefreshEnabled = true;
            //lstView.RefreshCommand = PullToRefreshCommand;
            //lstView.IsRefreshing = IsRefreshing;
            //lstView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");
        }

        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _Event_Data model = (_Event_Data)e.Item;
            lstView.SelectedItem = null;
            await Navigation.PushAsync(new EventDetailsPage(model.event_id.ToString()));
        }

        private void CarouselZoos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BreaingEvents model = (BreaingEvents)e.SelectedItem;
            EventsID = model.Id;
        }

        private void CarouselZoos_PositionSelected(object sender, SelectedPositionChangedEventArgs e)
        {

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EventDetailsPage(EventsID));
        }

        private async void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            NoDataPage.IsVisible = false;
            MainFrame.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);
            IsLoad = false;
            loadEventListdata(CategoryId);

        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            NoDataPage.IsVisible = false;
            UpdateRestList("", "", "");
        }

        private void CarouselAd_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void CarouselAd_Tapped(object sender, EventArgs e)
        {

        }

        private async void BackButton_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void TaazaCashTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }

        private void NavToCityList(object sender, EventArgs e)
        {
            LocationFilterPicker.Focus();
        }

        private void LocationFilterSelectionChanged(object sender, EventArgs e)
        {
            if (LocationFilterPicker.SelectedIndex != -1)
            {
                if(Device.RuntimePlatform == Device.Android)
                {
                    LocationFilterPicker.SelectedIndex = -1;
                }
                var picker = (Picker)sender;
                var z = picker.SelectedItem.ToString();
                UpdateRestList("", "", z);
            }
        }

        private void FilterSelectionChanged(object sender, EventArgs e)
        {
            if (FilterPicker.SelectedIndex != -1)
            {
                FilterPicker.SelectedIndex = -1;
                var picker = (Picker)sender;
                var modelfilter = filterModel.Where(X => X.Text == picker.SelectedItem.ToString()).FirstOrDefault();
                UpdateRestList(modelfilter.Id.ToString(), "", "");
            }
        }

        private void OpenFilterSelection(object sender, EventArgs e)
        {
            if(filterModel.Count > 0)
            FilterPicker.Focus();
        }

        private void LoadEventListCategory(object sender, EventArgs e)
        {
            StackLayout parent = (StackLayout)sender;
            var x = ((Label)parent.Children[0]).Text;
            UpdateRestList(x, "", "");
        }

        private void SearchEventsPressed(object sender, EventArgs e)
        {
            UpdateRestList("", EventSearchBar.Text, "");
        }

        public async void UpdateRestList(string CategoryTypeID, string SearchText, string LocationValue)
        {
            Loader.IsVisible = true;
            NoDataPage.IsVisible = false;
            lstView.IsVisible = false;
            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("search_text", SearchText));
                parameters.Add(new KeyValuePair<string, string>("category_id", CategoryTypeID));
                parameters.Add(new KeyValuePair<string, string>("location_search", LocationValue));

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Eventlist], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                }
                else
                {
                    Items = JsonConvert.DeserializeObject<EventListModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                NoDataPage.IsVisible = true;
            }

            if (Items.data.events.event_data.Count() == 0)
            {
                NoDataPage.IsVisible = true;
                lstView.IsVisible = false;
                Loader.IsVisible = false;
            }

            lstView.ItemsSource = Items.data.events.event_data;
            lstView.HeightRequest = (Items.data.events.event_data.Count() * lstView.RowHeight) + 2;
            lstView.IsVisible = true;
            Loader.IsVisible = false;
        }

    }
}