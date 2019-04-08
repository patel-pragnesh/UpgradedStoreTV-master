using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoreHeaderView : ContentView
    {
        public StoreHeaderView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SamplePage());
        }

        private void Search_Btn_Tapped(object sender, EventArgs e)
        {

        }

        private void BackBtn_Tapped(object sender, EventArgs e)
        {

        }

        private void Bell_Btn_Tapped(object sender, EventArgs e)
        {

        }

        private void Cart_Btn_Tapped(object sender, EventArgs e)
        {

        }
    }
}