﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTaoThongBao : ContentPage
    {
        ThongBao _ThongBao;
        public PageTaoThongBao()
        {
            InitializeComponent();
            AddMaSV.Focus();
            AddNoiDung.Focus();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            ThongBao _ThongBao = new ThongBao();
            _ThongBao.ID = 1;
            _ThongBao.MaSV = AddMaSV.Text;
            _ThongBao.NoiDung= AddNoiDung.Text;

            HttpClient httpClient = new HttpClient();
            string json = JsonConvert.SerializeObject(_ThongBao);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage kq;

            kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/Notification?NoiDung=" + AddNoiDung.Text, stringContent);
            var kqthem = await kq.Content.ReadAsStringAsync();
            if (int.Parse(kqthem.ToString()) > 0)
            {
                await DisplayAlert("Thông báo", "Thêm thông báo thành công", "OK");
            }
            else
                await DisplayAlert("Thông báo", "Thêm thông báo không thành công", "Thử lại");

            await Navigation.PopAsync();
        }
    }
}