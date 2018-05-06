using Xamarin.Forms;
using System.Threading.Tasks;
using System;

namespace AdminAccountingApp
{
    class AddNewCustomer : ContentPage
    {
        RestService _restService;

        public AddNewCustomer()
        {
            _restService = new RestService();
            AddElements();
        }

        private void AddElements()
        {
            EntryCell newCustomer = new EntryCell();
            newCustomer.Placeholder = "Ім'я користувача";

            TableView table = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("Добавлення нового користувача")
                {
                    new TableSection("Введіть дані користувача")
                    {
                        newCustomer
                    }
                }
            };

            Button createCustomerButton = new Button();
            createCustomerButton.Text = "Створити";
            createCustomerButton.BackgroundColor = Color.Aqua;
            createCustomerButton.Clicked += async (sender, e) => await CreateNewCustomer(newCustomer.Text);

            StackLayout sl = new StackLayout();
            sl.Children.Add(table);
            sl.Children.Add(createCustomerButton);

            Content = sl;
        }

        private async Task CreateNewCustomer(string name)
        {
            await _restService.CreateNewCustomer(name);
            await Navigation.PopModalAsync();
        }
    }
}
