﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PageDangNhap());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}