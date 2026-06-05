using Microsoft.EntityFrameworkCore;
using SmileService.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SmileService.Views
{
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // 1. Валидация полей (включая новое поле пароля)
            if (string.IsNullOrWhiteSpace(TxtFullName.Text) ||
                string.IsNullOrWhiteSpace(TxtLogin.Text) ||
                string.IsNullOrWhiteSpace(TxtPassword.Password) ||
                ComboRole.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля формы.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 2. Создаем объект сотрудника и передаем введенный админом пароль
                User newUser = new User
                {
                    FullName = TxtFullName.Text.Trim(),
                    Login = TxtLogin.Text.Trim(),
                    Password = TxtPassword.Password.Trim(), // Забираем пароль из PasswordBox
                    Role = (ComboRole.SelectedItem as ComboBoxItem).Content.ToString()
                };

                // 3. Сохраняем стандартным и чистым методом EF Core
                //using (SmileServiceDBContext db = new SmileServiceDBContext())
                //{
                //    db.Users.Add(newUser);
                //    db.SaveChanges();
                //}

                // 4. Всё прошло успешно — закрываем окно
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"Ошибка при сохранении данных: {errorMsg}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}