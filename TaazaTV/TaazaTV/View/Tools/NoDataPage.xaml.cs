using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.Tools
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoDataPage : StackLayout
    {
        public NoDataPage()
        {
            InitializeComponent();
        }
        public Color TextColor
        {
            get { return this.LabelText.TextColor; }
            set { this.LabelText.TextColor = value; }
        }
        public Label HeadingTextControl
        {
            get { return this.HeadingText; }
            set { this.HeadingText = value; }
        }
        public Label TextControl
        {
            get { return this.LabelText; }
            set { this.LabelText = value; }
        }



        public event EventHandler BackClickedButton;
        //public EventHandler ClickedCommand
        //{
        //    get { return null; }
        //    set { this.buttonItem.Clicked += value; }
        //}


        private void BackbuttonItem_Clicked(object sender, EventArgs e)
        {
            this.BackClickedButton(this, EventArgs.Empty);
        }

    }
}