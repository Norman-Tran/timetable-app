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

namespace TimetableApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageAdDeadline : ContentPage
	{
		Deadline _dl;
		public PageAdDeadline()
		{
			InitializeComponent();
		}

		public PageAdDeadline(Deadline deadline)
		{
			InitializeComponent();
			Title = "Sửa Deadline";
			_dl = deadline;
			AddMaLop.Text = deadline.MaLop;
			AddNoiDung.Text = deadline.NoiDung;
			AddNoiDung.Focus();
		}

        private async void Save_Clicked(object sender, EventArgs e)
        {
			if(_dl!= null)
			{
				_dl.MaSV = SinhVien.DangNhap.MaSV.ToString();
				_dl.MaLop = AddMaLop.Text;
				_dl.NoiDung = AddNoiDung.Text;
				_dl.HoanThanh = "false";

                HttpClient httpClient = new HttpClient();
                string jsonup = JsonConvert.SerializeObject(_dl);
                StringContent stringContent = new StringContent(jsonup, Encoding.UTF8, "application/json");
                HttpResponseMessage kq;

				kq = await httpClient.PutAsync("http://www.lno-ie307.somee.com/api/Homework?ID=" + _dl.ID, stringContent);
                var kqthem = await kq.Content.ReadAsStringAsync();

                if (int.Parse(kqthem.ToString()) > 0)
                {
                    await DisplayAlert("Thông báo", "Sửa deadline thành công", "OK");
                }
                else
                    await DisplayAlert("Thông báo", "Sửa deadline không thành công", "Thử lại");
            }
			else
			{
				Deadline _deadline = new Deadline();
				_deadline.MaSV = SinhVien.DangNhap.MaSV.ToString();
				_deadline.MaLop = AddMaLop.Text;
				_deadline.NoiDung = AddNoiDung.Text;
				_deadline.HoanThanh = "false";

                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(_deadline);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage kq;

                kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/Homework?MaSV=" + SinhVien.DangNhap.MaSV.ToString() + "&MaLop=" + _deadline.MaLop + "&NoiDung=" + _deadline.NoiDung, stringContent);
                var kqthem = await kq.Content.ReadAsStringAsync();
                if (int.Parse(kqthem.ToString()) > 0)
                {
                    await DisplayAlert("Thông báo", "Thêm deadline thành công", "OK");
                }
                else
                    await DisplayAlert("Thông báo", "Thêm deadline không thành công", "Thử lại");
            }
            await Navigation.PopAsync();
        }
    }
}