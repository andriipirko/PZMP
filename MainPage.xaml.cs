using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdminAccountingApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            AddElements();
		}

        private void AddElements()
        {
            ToolbarItem tb = new ToolbarItem
            {
                Text = "Список клієнтів",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };

            tb.Clicked += async (sender, e) =>
            {
                var clientsPage = new NavigationPage(new ClientsView());
                await Navigation.PushModalAsync(clientsPage);
            };

            ToolbarItem tb1 = new ToolbarItem
            {
                Text = "Пункт меню",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };

            ToolbarItem tb2 = new ToolbarItem
            {
                Text = "Пункт меню",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };

            ToolbarItems.Add(tb);
            ToolbarItems.Add(tb1);
            ToolbarItems.Add(tb2);


            StackLayout stackLayout = new StackLayout();

            Button buttonClients = new Button
            {
                Text = "Список клієнтів",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            buttonClients.Clicked += OnButtonClientsClicked;
            stackLayout.Children.Add(buttonClients);
            this.Content = stackLayout;
        }

        private async void OnButtonClientsClicked(object sender, EventArgs e)
        {
            var clientsPage = new NavigationPage(new ClientsView());
            await Navigation.PushModalAsync(clientsPage);
        }
    }
}
