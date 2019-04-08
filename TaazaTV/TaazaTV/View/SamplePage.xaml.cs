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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SamplePage : ContentPage
    {
        private ObservableCollection<PreniumAdList> Adobj { get; set; }
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        public string pagename = "event";

        public SamplePage()
        {
            InitializeComponent();
        }

        private void xyz(object sender, Model.TaazaStoreModel.EventArgs<double> e)
        {
            if (something.LeftValue == something.RightValue)
                something.LeftValue = something.RightValue - 1;
        }

        private void xyz1(object sender, object e)
        {

        }
    }
}