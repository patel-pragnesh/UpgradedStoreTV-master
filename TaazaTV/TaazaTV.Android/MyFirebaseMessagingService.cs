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
using Firebase.Messaging;
using Android.Util;
using Android.Media;

namespace TaazaTV.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            SendNotification(message.GetNotification().Body, message.GetNotification().Title);
        }

        void SendNotification(string messageBody, string title)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            //if (title.ToLower().Trim().Equals("taazatv") || title.ToLower().Trim().Equals(""))
            //{
            //    var notificationBuilder = new Notification.Builder(this)
            //   .SetSmallIcon(Resource.Drawable.launcher)
            //   .SetContentText(messageBody)
            //   .SetSound(defaultSoundUri)
            //   .SetAutoCancel(true)
            //   .SetContentIntent(pendingIntent);

            //    var notificationManager = NotificationManager.FromContext(this);
            //    notificationManager.Notify(0, notificationBuilder.Build());
            //}
            //else
            //{

            //}

            var notificationBuilder = new Notification.Builder(this)
               .SetSmallIcon(Resource.Drawable.launcher)
               .SetContentTitle(title)
               .SetContentText(messageBody)
               .SetSound(defaultSoundUri)
               .SetAutoCancel(true)
               .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());


        }

        private String Contentformatter(String messagebody)
        {
            try
            {
                if (messagebody.Substring(0, 8).ToLower().Equals("taazatv"))
                    messagebody= messagebody.Remove(0,8);
                return messagebody;
            }
            catch (Exception e)
            {
                return messagebody;
                throw;
            }
        }
    }
}