using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using TaazaTV.View.Accounts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsCategoryPage : ContentPage
    {
        static NewsCategoryListModel Items;
        static bool IsLoad = true;
        HttpRequestWrapper wrapper = new HttpRequestWrapper();

        //public bool _isRefreshing = false;
        //public bool IsRefreshing
        //{
        //    get { return _isRefreshing; }
        //    set
        //    {
        //        _isRefreshing = value;
        //        OnPropertyChanged(nameof(IsRefreshing));
        //    }
        //}

        //public Command PullToRefreshCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            IsRefreshing = true;
        //            SetData();
        //            IsRefreshing = false;
        //        });
        //    }
        //}

        public NewsCategoryPage(string Title, ImageSource Image)
        {
            InitializeComponent();
            FooterAdFunc();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV News";
            IsLoad = true;
            TitleImage.Source = Image;
            TitleText.Text = Title;
        }

        protected override async void OnAppearing()
        {
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                NoInternet.IsVisible = false;

                MainFrame.IsVisible = false;
                Loader.IsVisible = true;
                await Task.Delay(1000);
                IsLoad = false;
                SetData();
            }
            base.OnAppearing();
        }

        private async void SetData()
        {
            //MainFrame.IsVisible = false;
            //Loader.IsVisible = true;
            //await Task.Delay(1000);
            try
            {
                //if (Items == null)
                //{
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NewsCategory], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    lstView.IsVisible = false;
                    //NoDataPage.IsVisible = false;
                    NoInternet.IsVisible = true;
                    MainFrame.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<NewsCategoryListModel>(jsonstr);
                    }
                    catch
                    {
                        NoInternet.IsVisible = false;
                        MainFrame.IsVisible = false;
                        //NoDataPage.IsVisible = true;
                    }
                    //}

                    // Menus
                    foreach (CategoryList menu in Items.data.category)
                    {
                        double Width = (MainFrame.Width - 30) / 4;
                        StackLayout layout = new StackLayout();
                        layout.HeightRequest = Width + 20;
                        layout.WidthRequest = Width;
                        //layout.Padding = new Thickness(10);

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += async (s, e) =>
                        {
                            // handle the tap
                            StackLayout parent = (StackLayout)s;
                            await Navigation.PushAsync(new NewsListPage(((Xamarin.Forms.Label)parent.Children[1]).Text, ((Xamarin.Forms.Label)parent.Children[2]).Text, ((Xamarin.Forms.Image)parent.Children[0]).Source));
                        };

                        layout.GestureRecognizers.Add(tapGestureRecognizer);
                        //layout.BackgroundColor = Color.Gray;
                        layout.Margin = new Thickness(1);
                        Image image = new Image();
                        image.WidthRequest = Width - 20;
                        image.WidthRequest = Width - 20;
                        image.Source = menu.category_image;

                        Label CategoryId = new Label()
                        {
                            IsVisible = false,
                            Text = menu.category_id.ToString()
                        };

                        Label CategoryName = new Label()
                        {
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 12,
                            Text = menu.category_name.ToString()
                        };

                        layout.Children.Add(image);
                        layout.Children.Add(CategoryId);
                        layout.Children.Add(CategoryName);
                        CategoryList.Children.Add(layout);
                    }

                    lstView.ItemsSource = Items.data.top_news;
                    lstView.HeightRequest = (Items.data.top_news.Count() * lstView.RowHeight) + 1;
                    MainFrame.IsVisible = true;
                    lstView.IsVisible = true;
                }
            }
            catch (Exception ex)
            {

            }

            Loader.IsVisible = false;
            //lstView.IsPullToRefreshEnabled = true;
            //lstView.RefreshCommand = PullToRefreshCommand;
            //lstView.IsRefreshing = IsRefreshing;
            //lstView.SetBinding(ListView.IsRefreshingProperty, "IsRefreshing");
        }

        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Top_News model = (Top_News)e.Item;
            lstView.SelectedItem = null;
            await Navigation.PushAsync(new NewsDetailsPage(model.news_id.ToString()));



        }

        private async void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            //   NoDataPage.IsVisible = false;
            MainFrame.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);
            IsLoad = false;
            SetData();

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



        public async void FooterAdFunc()
        {
            try
            {
                MultipartFormDataContent formdata = new MultipartFormDataContent();
                formdata.Add(new StringContent("event"), "page_name");
                formdata.Add(new StringContent(AppData.UserId), "user_id");

                string items = await wrapper.PostFormDataAsync(Constant.APIs[(int)Constant.APIName.GeneralUrl], formdata);

                if (items.ToString() == "NoInternet")
                {
                    // NoInternet.IsVisible = true;
                }

                else
                {
                    var deobj = JsonConvert.DeserializeObject<PreniumAdModel>(items);
                    var footersrc = deobj.data.Ad_list.ToList();
                    if (footersrc != null)
                    {
                        if (footersrc.Count() > 0)
                        {
                            CarouselAd.ItemsSource = footersrc;
                            CarouselAd.IsVisible = true;
                        }
                        else
                        {
                            CarouselAd.IsVisible = false;
                        }
                    }
                    else
                    {
                        CarouselAd.IsVisible = false;
                    }
                }
            }

            catch
            {
                throw new NotImplementedException();
            }
        }


    }
}