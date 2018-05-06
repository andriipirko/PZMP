using Xamarin.Forms;

namespace AdminAccountingApp
{
    class AuthorizationPage : ContentPage
    {
        RestService _restService;

        public AuthorizationPage()
        {
            _restService = new RestService();
            AddComponents();
        }

        private void AddComponents()
        {
            SwitchCell saveConfig = new SwitchCell();
            saveConfig.Text = "Зберегти дані";
            saveConfig.On = true;

            EntryCell loginField = new EntryCell();
            loginField.Placeholder = "Логін";

            PasswordEntryCell passwordField = new PasswordEntryCell();
            passwordField.Placeholder = "Пароль";

            TableView table = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot("Авторизація в системі")
                {
                    new TableSection("Авторизація в системі")
                    {
                        loginField,
                        passwordField
                    },
                    new TableSection("Додаткові налаштування")
                    {
                        saveConfig
                    }
                }
            };

            Button enterButton = new Button();
            enterButton.Text = "Ввійти";
            enterButton.BackgroundColor = Color.Accent;
            enterButton.Clicked += async (sender, e) => await AuthorizeAsync(loginField.Text, passwordField.Value);

            StackLayout sl = new StackLayout();
            sl.Children.Add(table);
            sl.Children.Add(enterButton);
            Content = sl;
        }

        private async System.Threading.Tasks.Task AuthorizeAsync(string login, string password)
        {
            bool authResult = await _restService.Authorize(login, password);
            if (authResult)
            {
                await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
            }

        }
    }

    public class PasswordEntryCell : EntryCell
    {
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                setStars();
            }
        }

        private string starFiller(int count)
        {
            var output = "";
            for (; count > 0; count--, output += "●")
                ;
            return output;
        }

        private void setStars()
        {
            this.Text = starFiller(this.Value.Length);
        }

        public PasswordEntryCell()
        {
            this.Value = "";
            this.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
                if (e.PropertyName == "Text")
                {
                    var txtLen = this.Text.Length;
                    var txtVal = this.Text;
                    var mdlLen = this.Value.Length;
                    if (txtLen > mdlLen)
                    {
                        this.Value += txtVal.Substring(txtLen - 1);
                    }
                    else
                    {
                        this.Value = this.Value.Substring(0, txtLen);
                    }
                    setStars();
                }
            };
        }
    }
}
