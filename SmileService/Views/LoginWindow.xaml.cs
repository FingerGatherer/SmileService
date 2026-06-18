using System;
using System.Linq;
using System.Windows;
using SmileService.Models;

namespace SmileService.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string password = TxtPassword.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля для входа!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

                    if (user != null)
                    {
                        UserSession.CurrentUser = user;

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки пользователя: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}