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
using TaazaTV.Helper;
using TaazaTV.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(NativeHelper))]
namespace TaazaTV.Droid
{
    public class NativeHelper : INativeHelper
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }        
    }
}