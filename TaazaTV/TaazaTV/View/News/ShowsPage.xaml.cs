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

namespace TaazaTV.View.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowsPage : ContentPage
    {
        ShowsModel Items = new ShowsModel();
        static bool IsLoad = true;
        public ShowsPage(string Title, ImageSource Image)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Shows";
            IsLoad = true;
            TitleImage.Source = Image;
            TitleText.Text = Title;
        }

        protected override async void OnAppearing()
        {
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                Loader.IsVisible = true;
                await Task.Delay(1000);

                SetData();
                IsLoad = false;
            }
            else
            {
                //MainFrame.IsVisible = true;
                Loader.IsVisible = false;
            }

            base.OnAppearing();
        }

        protected async void SetData()
        {
            MainFrame.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(100);

            if (Items.data == null)
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.ShowsPlaylist], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    //lstView.IsVisible = false;
                    NoInternet.IsVisible = true;
                    NoDataPage.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<ShowsModel>(jsonstr);
                    }
                    catch
                    {
                        NoInternet.IsVisible = false;
                        NoDataPage.IsVisible = true;
                    }
                }

                // Menus
                foreach (VideoItem menu in Items.data.playlists.items)
                {
                    double Width = (MainFrame.Width - 30) / 4;
                    StackLayout layout = new StackLayout();
                    layout.HeightRequest = Width + 40;
                    layout.WidthRequest = Width;
                    //layout.Padding = new Thickness(10);

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += async (s, e) =>
                    {
                        // handle the tap
                        StackLayout parent = (StackLayout)s;
                        await Navigation.PushAsync(new ShowListPage(((Xamarin.Forms.Label)parent.Children[1]).Text, ((Xamarin.Forms.Label)parent.Children[2]).Text, TitleImage.Source));
                    };

                    layout.GestureRecognizers.Add(tapGestureRecognizer);
                    //layout.BackgroundColor = Color.Gray;
                    layout.Margin = new Thickness(1);
                    Image image = new Image();
                    image.WidthRequest = Width - 5;
                    image.HeightRequest = Width - 15;
                    image.Source = menu.snippet.thumbnails.@default.url;

                    Label PlaylistId = new Label()
                    {
                        IsVisible = false,
                        Text = menu.id.ToString()
                    };

                    Label PlaylistName = new Label()
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 12,
                        Text = menu.snippet.title
                    };

                    layout.Children.Add(image);
                    layout.Children.Add(PlaylistId);
                    layout.Children.Add(PlaylistName);
                    CategoryList.Children.Add(layout);
                }
            }

            MainFrame.IsVisible = true;
            Loader.IsVisible = false;
        }

        private async void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private void DoSomething(object sender, EventArgs e)
        {
            SetData();
        }
    }
}