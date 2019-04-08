using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using TaazaTV.View.Tools;
using TaazaTV.View.News;

namespace TaazaTV.View.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityListPage : ContentPage
    {
        CityModel Items = new CityModel();
        bool _IsAllCity;
        public CityListPage(bool IsAllCity = false)
        {
            InitializeComponent();
            _IsAllCity = IsAllCity;
            LoadCityList(_IsAllCity);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            var keysearch = MainSearchBox.Text;
            var Result = CityList._cityList.Where(x => x.city_name.ToLower().Contains(keysearch.ToLower()));
            ObservableCollection<city_data> SearchResult = new ObservableCollection<city_data>(Result);
            if (SearchResult.Count <= 0)
            {
                NoDataPage.IsVisible = true;
                CityListView.IsVisible = false;
            }
            else
            {
                CityListView.IsVisible = true;
                NoDataPage.IsVisible = false;
            }
            CityListView.ItemsSource = SearchResult;


        }

        private async void LoadCityList(bool IsAllCity)
        {
            Loader.IsVisible = true;
            CityListView.IsVisible = true;
            NoInternet.IsVisible = false;
            bool iscityload = true;
            await Task.Delay(100);

            //if (CityList._cityList.Count() < 1)
            //{

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.Cities], parameters);

                if (jsonstr.ToString() == "NoInternet")
                {
                    CityListView.IsVisible = false;
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                    NoDataPage.IsVisible = false;
                    //await DisplayAlert("No Connection", "Please check your internet connection", "OK");
                    //await Navigation.PushModalAsync(new NoInternetPage());
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<CityModel>(jsonstr);
                    }
                    catch
                    {
                        CityListView.IsVisible = false;
                        NoInternet.IsVisible = false;
                        MainFrame.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                    CityList._cityList = new ObservableCollection<city_data>(Items.data.city_data);
                    MainFrame.IsVisible = true;

                }
            }
            catch (Exception ex)
            {

            }

            // }

            if (CityList._cityList.Count() > 0)
            {
                List<city_data> data = new List<city_data>();
                if (IsAllCity && iscityload)
                {
                    data.Add(new city_data { city_id = "", city_name = "All" });
                    iscityload = false;
                    foreach (var city in CityList._cityList)
                    {
                        data.Add(city);
                    }
                }
                else
                {
                    data = CityList._cityList.ToList();
                }

                CityListView.ItemsSource = data;
            }
            else
            {
                CityListView.IsVisible = false;
                //NoData.IsVisible = true;
            }

            Loader.IsVisible = false;
            MainFrame.IsVisible = true;
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as city_data;

            RegistrationPage.CityName = item.city_name.ToString();
            RegistrationPage.CityId = item.city_id.ToString();
            ProfilePage.CityName = item.city_name.ToString();
            ProfilePage.CityId = item.city_id.ToString();
            NewsListPage.CityName = item.city_name.ToString();
            NewsListPage.CityId = item.city_id.ToString();

            await Navigation.PopModalAsync();
            //await this.Navigation.PushModalAsync(new RegistrationPage(nameofcity));
            // Your code here
        }

        private void DoSomething(object sender, EventArgs e)
        {
            LoadCityList(_IsAllCity);
        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void BackButton(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}