using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Load Dashboard by default
            MainFrame.Navigate(new DashboardPage());
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();

            switch (tag)
            {
                case "Dashboard":
                    MainFrame.Navigate(new DashboardPage());
                    break;
                case "Stocks":
                    MainFrame.Navigate(new StocksPage());
                    break;
                case "Requests":
                    MainFrame.Navigate(new RequestsPage());
                    break;
                case "Returns":
                    MainFrame.Navigate(new ReturnsPage());
                    break;
                case "AddStock":
                    MainFrame.Navigate(new AddStockPage());
                    break;
                case "AddRequest":
                    MainFrame.Navigate(new AddRequestPage());
                    break;
                case "AddReturn":
                    MainFrame.Navigate(new AddReturnPage());
                    break;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}