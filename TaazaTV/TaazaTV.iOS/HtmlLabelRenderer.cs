using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace TaazaTV.iOS
{    
    class HtmlLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            var attr = new NSAttributedStringDocumentAttributes();
            var nsError = new NSError();
            attr.DocumentType = NSDocumentType.HTML;

            var text = e.NewElement.Text;

            //I wrap the text here with the default font and size
            text = "<style>body{font-family: '" + this.Control.Font.Name + "'; font-size:" + this.Control.Font.PointSize + "px;}</style>" + text;

            var myHtmlData = NSData.FromString(text, NSStringEncoding.Unicode);
            this.Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Control == null)
            {
                return;
            }

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                var attr = new NSAttributedStringDocumentAttributes();
                var nsError = new NSError();
                attr.DocumentType = NSDocumentType.HTML;

                var myHtmlData = NSData.FromString(this.Control.Text, NSStringEncoding.Unicode);
                this.Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
            }
        }
    }
}