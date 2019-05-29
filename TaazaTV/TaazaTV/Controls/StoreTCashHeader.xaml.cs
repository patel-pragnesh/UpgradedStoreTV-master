using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.View.TaazaCash;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StoreTCashHeader : ContentView
	{
		public StoreTCashHeader ()
		{
			InitializeComponent ();
            TaazaCashAmount.Text = AppData.TaazaCash;

        }

        private async void BackBtn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void TaazaCashTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }
    }
}
