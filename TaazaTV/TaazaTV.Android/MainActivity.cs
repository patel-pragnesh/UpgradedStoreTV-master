using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Media;
using Android.Content;
using Android.Gms.Common;
using Android.Util;
using Firebase;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Droid;

namespace TaazaTV.Droid
{
	[Activity (Label = "TaazaTV", Icon = "@drawable/launcher", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        public static Context AppContext;
        protected override void OnCreate (Bundle bundle)
		{
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
           
            try
            {

                //if (Intent.Extras != null)
                //{
                //    foreach (var key in Intent.Extras.KeySet())
                //    {
                //        var value = Intent.Extras.GetString(key);
                //        Log.Debug("FCM", "Key: {0} Value: {1}", key, value);
                //    }
                //}
                StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
                StrictMode.SetVmPolicy(builder.Build());
                base.OnCreate(bundle);
                var container = new SimpleContainer();
                //container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
                container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
                container.Register<INetwork>(t => t.Resolve<IDevice>().Network);
                container.Register<IMediaPicker, MediaPickerdroid>().Register<IDependencyContainer>(t => container);

                Resolver.SetResolver(container.GetResolver());
                Rg.Plugins.Popup.Popup.Init(this, bundle);
                global::Xamarin.Forms.Forms.Init(this, bundle);
                ImageCircleRenderer.Init();

            }
            catch { }
            

            IsPlayServicesAvailable();

            try
            {
                string GcmSenderId = "949232771857";
                var options = new FirebaseOptions.Builder()
            .SetApplicationId("1:949232771857:android:c50d31d4999f5a24")
            .SetApiKey("AIzaSyCdtpHWKCXEoKHSOpcp-2VB3v8gsqFttpc")
            //.SetDatabaseUrl("Firebase-Database-Url")
            .SetGcmSenderId(GcmSenderId)
            .Build();

                var firebaseApp = FirebaseApp.InitializeApp(this, options);
                // Generate token in background thread
            }
            catch (Exception e)
            {

            }

            LoadApplication(new TaazaTV.App());
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug("FCM",GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug("FCM", "This device is not supported");
                    Finish();
                }
                return false;
            }
            else
            {
                Log.Debug("FCM", "Google Play Services is available.");
                return true;
            }
        }
    }
}

