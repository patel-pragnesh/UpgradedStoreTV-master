using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductListPage("",(sender as SearchBar).Text, ""));
        }
    }
}