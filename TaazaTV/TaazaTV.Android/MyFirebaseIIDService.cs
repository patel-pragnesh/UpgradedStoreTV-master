using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Android.Util;
using TaazaTV.Helper;

namespace TaazaTV.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }
        async void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
            AppData.DeviceToken = token;

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
        }
    }
}