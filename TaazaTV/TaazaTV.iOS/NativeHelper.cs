using TaazaTV.Helper;
using System.Diagnostics;
using TaazaTV.iOS;
using UIKit;
using Foundation;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(NativeHelper))]
namespace TaazaTV.iOS
{
    public class NativeHelper : INativeHelper
    {
        public void CloseApp()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }
    }
}