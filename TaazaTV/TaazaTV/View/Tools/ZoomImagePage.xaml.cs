﻿using PinchGesture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.Tools
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZoomImagePage : ContentPage
    {
        public ZoomImagePage(string Image)
        {
            InitializeComponent();

            Images.Source = Image;
        }
    }
}