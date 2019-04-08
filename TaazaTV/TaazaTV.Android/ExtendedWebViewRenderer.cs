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
using Xamarin.Forms;
using TaazaTV.Controls;
using TaazaTV.Droid;


using WebView = Android.Webkit.WebView;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer))]
namespace TaazaTV.Droid
{
    public class ExtendedWebViewRenderer : WebViewRenderer
    {
        static ExtendedWebView _xwebView = null;
        WebView _webView;

        class ExtendedWebViewClient : Android.Webkit.WebViewClient
        {
            public override async void OnPageFinished(WebView view, string url)
            {
                if (_xwebView != null)
                {
                    int i = 10;
                    while (view.ContentHeight == 0 && i-- > 0) // wait here till content is rendered
                        await System.Threading.Tasks.Task.Delay(100);
                    _xwebView.HeightRequest = view.ContentHeight;                    
                }
                base.OnPageFinished(view, url);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
          
                base.OnElementChanged(e);
                _xwebView = e.NewElement as ExtendedWebView;
                _webView = Control;

                if (e.OldElement == null)
                {
                    _webView.SetWebViewClient(new ExtendedWebViewClient());
                }
        }
    }
}