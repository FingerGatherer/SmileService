using System.Windows;
using System.Windows.Controls;
using SmileService.Models;

namespace SmileService.Views
{
    public partial class MainWindow : Window
    {
        private HomePage _homePage;
        private OrdersPage _ordersPage;
        private UsersPage _usersPage;
        private ClientsPage _clientsPage;
        private ReportsPage _reportsPage;

        public MainWindow()
        {
            using (var db = new SmileServiceDBContext())
            {
                db.Database.EnsureCreated();
            }

            InitializeComponent();

            _homePage = new HomePage();
            _ordersPage = new OrdersPage();
            _usersPage = new UsersPage();
            _clientsPage = new ClientsPage();
            _reportsPage = new ReportsPage();

            MainFrame.Navigate(_homePage);

            SearchTextBox.Visibility = Visibility.Collapsed;
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;

            ApplyPermissions();

            BtnHome.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1976D2"));
            BtnHome.FontWeight = FontWeights.Bold;

            BtnOrders.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#555555"));
            BtnOrders.FontWeight = FontWeights.Normal;
        }

        private void ApplyPermissions()
        {
            if (UserSession.CurrentUser == null)
            {
                Application.Current.Shutdown();
                return;
            }

            if (TxtUserName != null)
            {
                TxtUserName.Text = UserSession.CurrentUser.FullName;
            }

            string role = UserSession.CurrentUser.Role;

            BtnHome.Visibility = Visibility.Visible;
            BtnClients.Visibility = Visibility.Visible;
            BtnOrders.Visibility = Visibility.Visible;
            BtnUsers.Visibility = Visibility.Visible;
            BtnReports.Visibility = Visibility.Visible;

            if (FindName("BtnAddOrder") is Button btnAdd) btnAdd.Visibility = Visibility.Visible;

            switch (role)
            {
                case "Admin":
                    break;

                case "Receptionist":
                    BtnUsers.Visibility = Visibility.Collapsed;
                    BtnReports.Visibility = Visibility.Collapsed;
                    break;

                case "Master":
                    BtnUsers.Visibility = Visibility.Collapsed;
                    BtnClients.Visibility = Visibility.Collapsed;
                    BtnReports.Visibility = Visibility.Collapsed;

                    if (FindName("BtnAddOrder") is Button btnAddM) btnAddM.Visibility = Visibility.Collapsed;
                    break;

                case "Storekeeper":
                    BtnUsers.Visibility = Visibility.Collapsed;
                    BtnClients.Visibility = Visibility.Collapsed;
                    BtnOrders.Visibility = Visibility.Collapsed;

                    MainFrame.Content = null;
                    MessageBox.Show("Доступ ограничен.", "SmileService", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;

                case "Accountant":
                    BtnUsers.Visibility = Visibility.Collapsed;
                    BtnClients.Visibility = Visibility.Collapsed;

                    if (FindName("BtnAddOrder") is Button btnAddA) btnAddA.Visibility = Visibility.Collapsed;
                    break;

                default:
                    BtnUsers.Visibility = Visibility.Collapsed;
                    BtnClients.Visibility = Visibility.Collapsed;
                    BtnOrders.Visibility = Visibility.Collapsed;
                    BtnReports.Visibility = Visibility.Collapsed;
                    MainFrame.Content = null;
                    break;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти из учетной записи?",
                                                      "Выход из системы",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                UserSession.CurrentUser = null;

                SmileService.Views.LoginWindow loginWindow = new SmileService.Views.LoginWindow();
                loginWindow.Show();

                this.Close();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MainFrame.Content is OrdersPage)
            {
                _ordersPage.SetSearchText(SearchTextBox.Text);
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            if (clickedButton == BtnOrders)
            {
                SearchTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                SearchTextBox.Visibility = Visibility.Collapsed;
                SearchTextBox.Text = string.Empty;
            }

            BtnHome.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#555555"));
            BtnHome.FontWeight = FontWeights.Normal;

            BtnClients.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#555555"));
            BtnClients.FontWeight = FontWeights.Normal;

            BtnOrders.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#555555"));
            BtnOrders.FontWeight = FontWeights.Normal;

            BtnUsers.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#555555"));
            BtnUsers.FontWeight = FontWeights.Normal;

            BtnReports.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#555555"));
            BtnReports.FontWeight = FontWeights.Normal;

            clickedButton.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1976D2"));
            clickedButton.FontWeight = FontWeights.Bold;

            if (clickedButton == BtnOrders)
            {
                MainFrame.Navigate(_ordersPage);
            }
            else if (clickedButton == BtnHome)
            {
                MainFrame.Navigate(_homePage);
            }
            else if (clickedButton == BtnClients)
            {
                MainFrame.Navigate(_clientsPage);
            }
            else if (clickedButton == BtnReports)
            {
                MainFrame.Navigate(_reportsPage);
            }
            else if (clickedButton == BtnUsers)
            {
                _usersPage.LoadUsers();
                MainFrame.Navigate(_usersPage);
            }
            else
            {
                MessageBox.Show($"Страница '{clickedButton.Content}' находится на этапе верстки по макету интерфейса.", "SmileService", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addWindow = new AddOrderWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true)
            {
                _ordersPage.ApplyFilters();
            }
        }
    }
}