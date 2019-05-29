﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.View;
using TaazaTV.View.TaazaStore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoreHeaderView : ContentView
    {
        public StoreHeaderView()
        {
            InitializeComponent();

            if(AppData.CartCount == String.Empty)
            {
                CartFrame.IsVisible = false;
            }

            else
            {
                CartFrame.IsVisible = true;
                CartLabel.Text = AppData.CartCount;
            }
        }

        private async void Search_Btn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async void BackBtn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Bell_Btn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrdersPage());
        }

        private async void Cart_Btn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }
    }
}