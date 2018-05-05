﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using System.Linq;
using System.Threading;
using Android.App;
using Android.Util;

namespace AdminAccountingApp
{
    class ClientsView : ContentPage
    {
        private RestService _restService;
        private bool _activityIsActive;

        public ClientsView()
        {
            Title = "Список клієнтів";
            _activityIsActive = false;
            _restService = new RestService();
            GetClientsInfo();
        }

        public async Task GetClientsInfo()
        {
            Title = "Завантаження даних";

            ActivityIndicator indicator = new ActivityIndicator
            {
                IsVisible = true,
                IsRunning = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            Content = indicator;

            var res = await _restService.GetCustomersAsync();
            Title = "Cписок клієнтів";
            var activeCustomers = res.Where(c => c.active == 1);
            var disabledCustomers = res.Where(c => c.active == 0);

            List<SwitchCell> switchesActive = new List<SwitchCell>();
            foreach (var item in activeCustomers)
            {
                var sw = new SwitchCell();
                sw.Text = item.companyName;
                sw.On = await _restService.ServerWasStarted(item.port);
                sw.OnChanged += new EventHandler<Xamarin.Forms.ToggledEventArgs>(async (sender, e) => await SendRequest(item.cid, item.port, sw.On));
                switchesActive.Add(sw);
            }

            List<SwitchCell> switchesDisabled = new List<SwitchCell>();
            foreach (var item in disabledCustomers)
            {
                var sw = new SwitchCell();
                sw.Text = item.companyName;
                sw.On = await _restService.ServerWasStarted(item.port);
                sw.OnChanged += new EventHandler<Xamarin.Forms.ToggledEventArgs>(async (sender, e) => await SendRequest(item.cid, item.port, sw.On));
                switchesDisabled.Add(sw);
            }

            TableView table = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("Ввід даних")
                {
                    new TableSection("Активні сервери")
                    {
                        switchesActive
                    },
                    new TableSection("Неактивні сервери")
                    {
                        switchesDisabled
                    }
                }
            };

            DisplayMetrics displayMetrics = new DisplayMetrics();

            Button createNewCustomerButtom = new Button();
            createNewCustomerButtom.Text = "+";
            createNewCustomerButtom.TextColor = Color.White;
            createNewCustomerButtom.BackgroundColor = Color.Accent;
            createNewCustomerButtom.FontSize = 20;
            createNewCustomerButtom.WidthRequest = 30;
            createNewCustomerButtom.HeightRequest = 30;
            createNewCustomerButtom.BorderRadius = (int)createNewCustomerButtom.WidthRequest; 

            AbsoluteLayout l = new AbsoluteLayout();
            l.Children.Add(table);
            l.Children.Add(createNewCustomerButtom, new Point(displayMetrics.WidthPixels - 35, displayMetrics.HeightPixels - 35));

            ScrollView scroll = new ScrollView();
            scroll.Content = l;

            Content = scroll;
        }

        private async Task SendRequest(int id, int port, bool serverState)
        {
            Title = "Завантаження даних";

            ActivityIndicator indicator = new ActivityIndicator
            {
                IsVisible = true,
                IsRunning = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            Content = indicator;

            if (!serverState)
            {
                await _restService.StopServer(port);
            }
            else
            {
                var startResult = await _restService.StartServer(id);

                if (startResult)
                {
                    while ((await _restService.ServerWasStarted(port)) == false) { }
                }
            }            

            await GetClientsInfo();
        }
    }


}