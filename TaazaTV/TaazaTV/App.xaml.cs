using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaazaTV.Helper;
using TaazaTV.View;
using TaazaTV.View.Accounts;
using TaazaTV.View.Navigation;
using TaazaTV.View.TaazaCash;
using TaazaTV.View.TaazaStore;
using Xamarin.Forms;

namespace TaazaTV
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //AppData.IsLogin = true;
            //AppData.UserId = "4";
            //IDevice device = Resolver.Resolve<IDevice>();
            //Constant.DeviceToken = device.Id;

            // MainPage = new ProductDetailsPage();

            #region MyRegion
            if (AppData.IsMerchantLogin)
            {
                MainPage = new MerchantVerification();
            }

            else
            {
                if (AppData.IsLogin || AppData.IsSkip)
                {
                    MainPage = new MasterDetailsPage();
                }

                else
                {
                    MainPage = new RegistrationPage();
                }
            }

            #endregion

            //Background color
            //MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, Color.White);

            //Title color
            //MainPage.SetValue(NavigationPage.BackButtonTitleProperty, Color.Black);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //CrossPushNotification.Current.Register();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
