using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmileService.Models;

namespace SmileService.Views
{
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        public void LoadUsers()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    var usersList = db.Users.ToList();

                    if (UsersGrid != null)
                    {
                        UsersGrid.ItemsSource = usersList;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"Ошибка загрузки сотрудников: {errorMsg}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        public void AddUser_Click(object sender, RoutedEventArgs e)
        {
            
            AddUserWindow addUserWin = new AddUserWindow();
            addUserWin.Owner = Window.GetWindow(this);
            if (addUserWin.ShowDialog() == true)
            {
                LoadUsers();
            }
        }

        public void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }
    }
}