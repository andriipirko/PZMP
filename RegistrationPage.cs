using Xamarin.Forms;

namespace AdminAccountingApp
{
    class RegistrationPage : ContentPage
    {
        RestService _restService;

        public RegistrationPage()
        {
            _restService = new RestService();
            AddComponents();
        }

        private void AddComponents()
        {
            EntryCell loginField = new EntryCell();
            loginField.Placeholder = "Email";

            EntryCell passwordField = new EntryCell();
            passwordField.Placeholder = "Пароль";

            TableView table = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("Авторизація в системі")
                {
                    new TableSection("Реєстрація нового користувача")
                    {
                        loginField,
                        passwordField
                    }
                }
            };

            Button enterButton = new Button();
            enterButton.Text = "Зареєструвати";
            enterButton.BackgroundColor = Color.Accent;
            enterButton.Clicked += async (sender, e) => await AuthorizeAsync(loginField.Text, passwordField.Text);

            StackLayout sl = new StackLayout();
            sl.Children.Add(table);
            sl.Children.Add(enterButton);
            Content = sl;
        }

        private async System.Threading.Tasks.Task AuthorizeAsync(string login, string password)
        {
            bool authResult = await _restService.RegisterNewCustomer(login, password);
            if (authResult)
            {
                await Navigation.PopModalAsync();
            }

        }
    }
}
