using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Services;
using UserNotifications;
using Firebase.CloudMessaging;
using TaazaTV.Helper;
using XLabs.Forms;
using System.IO;
using XLabs.Forms.Services;
using XLabs.Platform.Mvvm;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.iOS;

namespace TaazaTV.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();

            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(33, 150, 243);
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White });

            //var resolverContainer = new global::XLabs.Ioc.SimpleContainer();
            //resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
            //    .Register<IMediaPicker, MediaPicker>()
            //    .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
            //    .Register<INetwork>(t => t.Resolve<IDevice>().Network);
            //XLabs.Ioc.Resolver.SetResolver(resolverContainer.GetResolver());

            this.SetIoc();


            Firebase.Core.App.Configure();


            LoadApplication(new TaazaTV.App());


            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                //UNUserNotificationCenter.Current.Delegate = (UNUserNotificationCenterDelegate)Self;

                // For iOS 10 data message (sent via FCM)
                //Messaging.SharedInstance.RemoteMessageDelegate = (IMessagingDelegate)Self;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();


            // Firebase component initialize
            ////Firebase.Analytics.App.Configure();
            //var Fireoptions = new Firebase.Core.Options("1:949232771857:ios:e4f29ed3b92c3e0f", "949232771857");
            //.SetApplicationId("1:949232771857:android:c50d31d4999f5a24")
            //.SetApiKey("AIzaSyCdtpHWKCXEoKHSOpcp-2VB3v8gsqFttpc")
            ////.SetDatabaseUrl("Firebase-Database-Url")
            //.SetGcmSenderId(GcmSenderId)
            //.Build();

            //Firebase.Core.App.Configure();

            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {
                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                // if you want to send notification per user, use this token
                System.Diagnostics.Debug.WriteLine("Token:" + newToken);
                AppData.DeviceToken = newToken;

                Task.Run(async () =>
                {
                    try
                    {
                        HttpRequestWrapper wrapper = new HttpRequestWrapper();
                        List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                        parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                        parameters.Add(new KeyValuePair<string, string>("user_id", string.IsNullOrEmpty(AppData.UserId) ? "" : AppData.UserId));
                        parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
                        parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
                        parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));
                        var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UpdateDeviceToken], parameters);
                    }
                    catch { }
                });
                connectFCM();
            });


            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            Messaging.SharedInstance.Disconnect();
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            connectFCM();
            base.OnActivated(uiApplication);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
#if DEBUG
            Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Sandbox);
#endif
#if !DEBUG
            Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Prod);
#endif
        }

        // iOS 9 <=, fire when recieve notification foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

            // Generate custom event
            NSString[] keys = { new NSString("Event_type") };
            NSObject[] values = { new NSString("Recieve_Notification") };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

            // Send custom event
            Firebase.Analytics.Analytics.LogEvent("CustomEvent", parameters);

            if (application.ApplicationState == UIApplicationState.Active)
            {
                System.Diagnostics.Debug.WriteLine(userInfo);
                var aps_d = userInfo["aps"] as NSDictionary;
                var alert_d = aps_d["alert"] as NSDictionary;
                var body = alert_d["body"] as NSString;
                var title = alert_d["title"] as NSString;
                debugAlert(title, body);
            }
        }

        // iOS 10, fire when recieve notification foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var title = notification.Request.Content.Title;
            var body = notification.Request.Content.Body;
            debugAlert(title, body);
        }

        private void connectFCM()
        {
            Messaging.SharedInstance.Connect((error) =>
            {
                if (error == null)
                {
                    Messaging.SharedInstance.Subscribe("/topics/all");
                }
                System.Diagnostics.Debug.WriteLine(error != null ? "error occured" : "connect success");
            });
        }

        private void debugAlert(string title, string message)
        {
            var alert = new UIAlertView(title ?? "Title", message ?? "Message", null, "Cancel", "OK");
            alert.Show();
        }

        private void SetIoc()
        {
            var resolverContainer = new global::XLabs.Ioc.SimpleContainer();

            var app = new XFormsAppiOS();
            app.Init(this);

            var documents = app.AppDataDirectory;
            var pathToDatabase = Path.Combine(documents, "xforms.db");

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IDisplay>(t => t.Resolve<IDevice>().Display)
                .Register<IFontManager>(t => new FontManager(t.Resolve<IDisplay>()))
                //.Register<XLabs.Serialization.IJsonSerializer, XLabs.Serialization.JsonNET.JsonSerializer>()
                //.Register<IJsonSerializer, Services.Serialization.SystemJsonSerializer>()
                //.Register<ITextToSpeechService, TextToSpeechService>()
                //.Register<IEmailService, EmailService>()
                .Register<IMediaPicker, MediaPicker>()
                .Register<INetwork>(t => t.Resolve<IDevice>().Network)
                .Register<IXFormsApp>(app)
                .Register<ISecureStorage, SecureStorage>()
                .Register<global::XLabs.Ioc.IDependencyContainer>(t => resolverContainer);
            //.Register<global::XLabs.Caching.ICacheProvider>(
            //    t => new SQLiteSimpleCache(new SQLitePlatformIOS(),
            //        new SQLiteConnectionString(pathToDatabase, true), t.Resolve<global::XLabs.Serialization.IJsonSerializer>())
            //);

            XLabs.Ioc.Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}
