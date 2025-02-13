﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Plugin.Calendar.Models;

namespace TimetableApp
{
    public class PageTKBSV_VM
    {
        public EventCollection Events { get; set; }
        HttpClient client;

        public PageTKBSV_VM()
        {
            client = new HttpClient();
            Events = new EventCollection { };
            updateEvent();
        }

        private async void updateEvent()
        {
            List<LopHoc> studentClasses = await GetClassList();
            foreach (LopHoc studentClass in studentClasses)
            {
                // Get start and end date of each class from dB
                DateTime startDate = studentClass.NgayBD;
                DateTime endDate = studentClass.NgayKT;

                // Find the weekdate that the class actualy start
                DateTime classDate = startDate;
                while ((int)classDate.DayOfWeek != int.Parse(studentClass.Thu) - 1)
                {
                    classDate = classDate.AddDays(1);
                }

                // Loop through every week to add the class to the date
                for (; classDate < endDate; classDate = classDate.AddDays(7))
                {
                    if (!Events.ContainsKey(classDate))
                    {
                        Events.Add(classDate, new List<LopHoc> { studentClass });
                    }
                    else
                    {
                        // Handle multiple class a day
                        List<LopHoc> classesOnThisDay = (List<LopHoc>)Events[classDate];
                        classesOnThisDay.Add(studentClass);
                        Events[classDate] = classesOnThisDay;
                    }
                }
            }
        }

        private async Task<List<LopHoc>> GetClassList()
        {
            try
            {
                string MaSV = SinhVien.DangNhap.MaSV;
                string uri = $"http://lno-ie307.somee.com/api/LopHoc?MaSV={MaSV}";
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<LopHoc>>(content);
                }
                return new List<LopHoc>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
                return new List<LopHoc>();
            }
        }
    }
}
