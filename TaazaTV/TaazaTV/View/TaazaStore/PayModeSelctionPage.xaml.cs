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
	public partial class PayModeSelctionPage : ContentPage
	{
		public PayModeSelctionPage ()
		{
			InitializeComponent ();
		}

        private void TaazaPayableChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void CallPAymentApI(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Payment Gateway Not Configured", "OK");
        }

        private void TaazaPaymentChanged(object sender, FocusEventArgs e)
        {
            try
            {
                if(Convert.ToInt32(Amount.Text) - Convert.ToInt32(((sender as Entry).Text))  != 0)
                {
                    Amount.Text = (Convert.ToInt32(Amount.Text) - Convert.ToInt32((sender as Entry).Text)).ToString();
                }
            }
            catch
            {

            }
        }
    }
}