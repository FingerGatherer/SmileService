using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmileService.Models;

namespace SmileService.Views
{
    // Класс называется строго UsersPage, как просит MainWindow
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers(); // Загружаем при открытии страницы
        }

        // 1. Метод LoadUsers (исправляет ошибку в MainWindow.xaml.cs)
        public void LoadUsers()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    // Вычитываем абсолютно всех пользователей
                    var usersList = db.Users.ToList();

                    // Находим твой DataGrid. 
                    // Посмотри в UsersPage.xaml, как он называется (EmployeesGrid, UsersGrid или DataGridUsers).
                    // Если имя отличается, поменяй имя переменной ниже:
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

        // 2. Событие загрузки страницы из XAML
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        // 3. Обработчик кнопки добавления нового пользователя (исправляет ошибку AddUser_Click)
        public void AddUser_Click(object sender, RoutedEventArgs e)
        {
            // Здесь в будущем откроется окно создания сотрудника, а пока заглушка:
            MessageBox.Show("Форма добавления нового сотрудника находится на этапе верстки.", "SmileService", MessageBoxButton.OK, MessageBoxImage.Information);

            // Если у тебя уже есть окно добавления (например, AddUserWindow), раскомментируй код ниже:
            
            AddUserWindow addUserWin = new AddUserWindow();
            addUserWin.Owner = Window.GetWindow(this);
            if (addUserWin.ShowDialog() == true)
            {
                LoadUsers();
            }
        }

        // 4. Обработчик кнопки обновить (исправляет ошибку Refresh_Click)
        public void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }
    }
}