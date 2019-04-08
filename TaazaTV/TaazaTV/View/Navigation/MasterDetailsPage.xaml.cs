using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.View.Accounts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailsPage : MasterDetailPage
    {
        public static MasterDetailPage masterPage;
        public MasterDetailsPage()
        {
            InitializeComponent();
            Icon = "menu.png";
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            masterPage = this;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterDetailsPageMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);

            if (Device.RuntimePlatform == Device.iOS)
            {
                page.Title = item.Title;
            }

            //await Navigation.PushAsync(page);
            //MainPage= 
            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}