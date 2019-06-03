using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationPage : ContentPage
	{

        HttpRequestWrapper wrapper = new HttpRequestWrapper();

		public NotificationPage ()
		{
			InitializeComponent ();
            LoadNotifications();

        }

        private async void LoadNotifications()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.NotificationListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    await DisplayAlert("Alert", "Server Error", "OK");
                }
                else
                {
                    var des = JsonConvert.DeserializeObject<NotificationModel>(jsonstr);
                    if (des.responseText == "Success")
                    {
                        NotificationListView.ItemsSource = des.data.notification_list;
                    }
                    else
                    {
                        await DisplayAlert("Alert", des.responseText, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Server Error", "OK");
            }
        }

        private async void BackBtnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}