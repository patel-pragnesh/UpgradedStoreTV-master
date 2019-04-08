using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.Social
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SocialFeedDetailsPage : ContentPage
	{
        

        Feed_Data Items = new Feed_Data();
        string feedId = "";
        public SocialFeedDetailsPage (Feed_Data Items)
		{
			InitializeComponent ();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Social Network";
            //TitleImage.Source = Items.feed_image;
            //TitleText.Text = Items.title;
            //shortDesc.Text = Items.content;
            BindingContext = Items;
        }

        private void Share_Tapped(object sender, EventArgs e)
        {

        }

        private void VideoView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void DoSomething(object sender, EventArgs e)
        {
            //loadNewsdata(feedId);
        }

        private void loadNewsdata(object feedId)
        {
            throw new NotImplementedException();
        }

        private async Task NoDataDoSomethingAsync(object sender, EventArgs e)
        {
           await Navigation.PopAsync();
        }
    }
}