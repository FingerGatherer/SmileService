using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmileService.Models;

namespace SmileService.Views
{
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
            LockFieldsForMaster();
            LoadMasters();
        }

        private void LockFieldsForMaster()
        {
            // Если окно открыл сотрудник с ролью Мастер
            if (UserSession.CurrentUser != null && UserSession.CurrentUser.Role == "Master")
            {
                // Находим поле стоимости услуги (замени "TxtPrice" на имя твоего TextBox в XAML)
                if (FindName("PriceBox") is TextBox priceBox)
                {
                    priceBox.IsReadOnly = true; // Запрещаем ввод
                    priceBox.Background = System.Windows.Media.Brushes.LightGray; // Визуально делаем серым
                    priceBox.ToolTip = "Мастер не имеет прав на изменение стоимости услуг.";
                }
            }
        }

        private void LoadMasters()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    var masters = db.Users.Where(u => u.Role == "Master").ToList();
                    ComboMaster.ItemsSource = masters;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сотрудников: {ex.Message}");
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация полей
            if (string.IsNullOrWhiteSpace(TxtClientName.Text) ||
                string.IsNullOrWhiteSpace(TxtClientPhone.Text) ||
                string.IsNullOrWhiteSpace(TxtDeviceModel.Text))
            {
                MessageBox.Show("Пожалуйста, заполните ФИО клиента, телефон и модель устройства!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ComboMaster.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника, ответственного за заказ!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal price = 0;
            decimal.TryParse(PriceBox.Text, out price);

            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    // 1. Создаем или находим клиента
                    var client = db.Clients.FirstOrDefault(c => c.Phone == TxtClientPhone.Text.Trim());
                    if (client == null)
                    {
                        client = new Client
                        {
                            FullName = TxtClientName.Text.Trim(),
                            Phone = TxtClientPhone.Text.Trim(),
                            RegistrationDate = DateTime.Now
                        };
                        db.Clients.Add(client);
                        db.SaveChanges(); // Сохраняем, чтобы получить ClientId
                    }

                    // 2. Создаем устройство
                    var device = new Device
                    {
                        ClientId = client.Id,
                        DeviceType = "Техника",
                        Model = TxtDeviceModel.Text.Trim(),
                        DefectDescription = TxtDefect.Text.Trim()
                    };
                    db.Devices.Add(device);
                    db.SaveChanges(); // Сохраняем, чтобы получить DeviceId

                    // 3. Создаем сам заказ
                    var selectedUser = ComboMaster.SelectedItem as User;
                    var order = new Order
                    {
                        DeviceId = device.Id,
                        Status = "В обработке",
                        CreatedDate = DateTime.Now,
                        MasterId = selectedUser.Id,
                        TotalPrice = price
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    MessageBox.Show("Заказ успешно зарегистрирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.DialogResult = true; // Закрываем окно и передаем сигнал на обновление страницы
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"Ошибка при сохранении заказа: {errorMsg}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}