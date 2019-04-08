using Xamarin.Forms;
using TaazaTV.Droid;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms.Platform.Android;
using System.Reflection;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomMapRenderer))]
namespace TaazaTV.Droid
{
    public class CustomMapRenderer : NavigationPageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
            var bar = (Android.Support.V7.Widget.Toolbar)typeof(NavigationPageRenderer)
            .GetField("_toolbar", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);
            bar.SetLogo(Resource.Drawable.logos);            
        }
    }
}