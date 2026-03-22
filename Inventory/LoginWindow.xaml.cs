using System.Windows;

namespace Inventory
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            //key press enter to login

            txtPassword.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                    btnLogin_Click(s, e);
            };
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (username == "admin" && password == "admin123")
            {
                MainWindow dashboard = new MainWindow();
                dashboard.Show();
                this.Close();
            }
            else
            {
                txtError.Text = "Invalid username or password.";
            }
        }
    }
}