using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Component;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilterPage : ContentPage
	{
		public FilterPage ()
		{
			InitializeComponent ();
		}

        private void xyz(object sender, object e)
        {
           if(something.RightValue == something.LeftValue)
            {
                something.LeftValue = something.LeftValue - 5*(something.Step);
            }
        }

        private void xyz1(object sender, object e)
        {
            if (something.RightValue == something.LeftValue)
            {
                something.RightValue = something.RightValue +  5*(something.Step);
            }
        }
    }
}