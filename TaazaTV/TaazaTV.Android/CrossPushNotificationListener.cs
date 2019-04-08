using System;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;
using Android.Util;
using TaazaTV.Helper;

namespace TaazaTV.Droid
{
    public class CrossPushNotificationListener: IPushNotificationListener
    {
        public CrossPushNotificationListener()
        {

        }

        public void OnError(string message, DeviceType deviceType)
        {
            
        }

        public void OnMessage(JObject values, DeviceType deviceType)
        {
            
        }

        public void OnRegistered(string token, DeviceType deviceType)
        {
            Constant.DeviceToken = token;
        }

        public void OnUnregistered(DeviceType deviceType)
        {
            
        }

        public bool ShouldShowNotification()
        {
            return true;
        }
    }
}