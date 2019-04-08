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
    public partial class NoInternetPage : StackLayout
    {
        public NoInternetPage()
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


        public event EventHandler ClickedButton;

        //  public event EventHandler BackClickedButton; 
        //public EventHandler ClickedCommand
        //{
        //    get { return null; }
        //    set { this.buttonItem.Clicked += value; }
        //}

        private void buttonItem_Clicked(object sender, EventArgs e)
        {
            this.ClickedButton(this, EventArgs.Empty);
        }
        //private void BackbuttonItem_Clicked(object sender, EventArgs e)
        //{ 
        //    this.BackClickedButton(this, EventArgs.Empty);
        //}

    }
}