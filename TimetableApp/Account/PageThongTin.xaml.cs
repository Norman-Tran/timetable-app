﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace TimetableApp.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageThongTin : ContentPage
    {
        HttpClient client;
        public PageThongTin()
        {
            InitializeComponent();
            client = new HttpClient();
            updateInfo();
        }

        private void updateInfo()
        {
            txtWelcome.Text = $"Xin chào, {SinhVien.DangNhap.TenSV}!";
            txtTenSV.Text = SinhVien.DangNhap.TenSV;
            txtMaSV.Text = SinhVien.DangNhap.MaSV;
            txtTenDangNhap.Text = SinhVien.DangNhap.TenDangNhap;
            txtMatKhau.Text = SinhVien.DangNhap.MatKhau;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string uri = $"http://lno-ie307.somee.com/api/TaiKhoan";
            try
            {
                SinhVien capNhapSV = new SinhVien
                {
                    MaSV = SinhVien.DangNhap.MaSV,
                    TenSV = txtTenSV.Text,
                    TenDangNhap = txtTenDangNhap.Text,
                    MatKhau = txtMatKhau.Text,
                    QuyenAdmin = SinhVien.DangNhap.QuyenAdmin
                };

                string jsonSV = JsonConvert.SerializeObject(capNhapSV);
                StringContent httpContent = new StringContent(jsonSV, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(uri, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    if (content == "1")
                    {
                        await DisplayAlert("Thành công", "Cập nhập thông tin thành công", "OK");
                        SinhVien.DangNhap = capNhapSV;
                        txtWelcome.Text = $"Xin chào, {SinhVien.DangNhap.TenSV}!";
                    }
                    else
                    {
                        await DisplayAlert("Thất bại", "Không thể cập nhập thông tin", "Thử lại");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", ex.Message, "Thoát");
                // Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            bool logout = await DisplayAlert("Đăng xuất", "Bạn có muốn đăng xuất khỏi tải khoản này?", "Đồng ý", "Huỷ");
            if (logout)
            {
                SinhVien.DangNhap = new SinhVien();
                Shell.Current.GoToAsync("//login");
            }
        }
    }
}