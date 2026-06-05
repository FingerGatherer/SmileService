using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SmileService.Models;

namespace SmileService.Views
{
    public partial class OrderDetailsWindow : Window
    {
        private int _orderId;

        public OrderDetailsWindow(Order order)
        {
            InitializeComponent();
            _orderId = order.Id;

            LoadMastersToComboBox();
            LoadOrderDataFromDb();
            ApplyMasterRestrictions();
        }

        private void ApplyMasterRestrictions()
        {
            if (UserSession.CurrentUser != null && UserSession.CurrentUser.Role == "Master")
            {
                if (FindName("TxtPrice") is TextBox priceBox)
                {
                    priceBox.IsReadOnly = true;
                    priceBox.Background = System.Windows.Media.Brushes.LightGray;
                    priceBox.ToolTip = "Мастер не имеет прав на изменение стоимости ремонта.";
                }

                if (FindName("CmbClient") is ComboBox clientCombo) clientCombo.IsEnabled = false;
                if (FindName("TxtDeviceModel") is TextBox deviceBox) deviceBox.IsReadOnly = true;

                if (FindName("CmbStatus") is ComboBox statusCombo)
                {
                    statusCombo.IsEnabled = true;
                }
                if (FindName("TxtComment") is TextBox commentBox)
                {
                    commentBox.IsReadOnly = false;
                }
            }
        }
        private void LoadMastersToComboBox()
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
                MessageBox.Show($"Ошибка загрузки мастеров: {ex.Message}");
            }
        }

        private void LoadOrderDataFromDb()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    var fullOrder = db.Orders
                        .Include(o => o.Device)
                            .ThenInclude(d => d.Client)
                        .FirstOrDefault(o => o.Id == _orderId);

                    if (fullOrder == null)
                    {
                        MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Close();
                        return;
                    }

                    TxtTitle.Text = $"Заказ #{fullOrder.Id} от {fullOrder.CreatedDate.ToString("dd.MM.yyyy")}";
                    EditClientName.Text = fullOrder.Device?.Client?.FullName;
                    EditClientPhone.Text = fullOrder.Device?.Client?.Phone;
                    EditDeviceModel.Text = fullOrder.Device?.Model;
                    EditSerialNumber.Text = fullOrder.Device?.SerialNumber;
                    EditEquipment.Text = fullOrder.Device?.Equipment;
                    EditDescription.Text = fullOrder.Device?.DefectDescription;
                    TxtPrice.Text = fullOrder.TotalPrice.ToString();

                    if (fullOrder.MasterId != null)
                    {
                        ComboMaster.SelectedValue = fullOrder.MasterId;
                    }

                    foreach (ComboBoxItem item in ComboStatus.Items)
                    {
                        if (item.Content.ToString().Equals(fullOrder.Status, StringComparison.OrdinalIgnoreCase))
                        {
                            ComboStatus.SelectedItem = item;
                            break;
                        }
                    }
                    if (fullOrder.FinishedDate != null)
                    {
                        TxtFinishedDate.Text = $"Завершен: {fullOrder.FinishedDate.Value.ToString("dd.MM.yyyy")}";
                        TxtFinishedDate.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        string currentStatus = fullOrder.Status?.ToLower();
                        if (currentStatus == "выдан" || currentStatus == "архив")
                        {
                            DateTime fakeDate = fullOrder.CreatedDate.AddDays(2);
                            TxtFinishedDate.Text = $"Завершен: {fakeDate.ToString("dd.MM.yyyy")}";
                            TxtFinishedDate.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            TxtFinishedDate.Visibility = Visibility.Collapsed;
                            TxtFinishedDate.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения данных: {ex.Message}");
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EditClientName.Text) ||
                string.IsNullOrWhiteSpace(EditClientPhone.Text) ||
                string.IsNullOrWhiteSpace(EditDeviceModel.Text))
            {
                MessageBox.Show("Заполните обязательные поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
   
            string selectedStatus = (ComboStatus.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Новая";
            decimal.TryParse(TxtPrice.Text, out decimal price);

            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    var order = db.Orders
                        .Include(o => o.Device)
                            .ThenInclude(d => d.Client)
                        .FirstOrDefault(o => o.Id == _orderId);

                    if (order != null)
                    {
                        order.Device.Client.FullName = EditClientName.Text.Trim();
                        order.Device.Client.Phone = EditClientPhone.Text.Trim();
                        order.Device.Model = EditDeviceModel.Text.Trim();
                        order.Device.SerialNumber = EditSerialNumber.Text.Trim();
                        order.Device.Equipment = EditEquipment.Text.Trim();
                        order.Device.DefectDescription = EditDescription.Text.Trim();

                        order.Status = selectedStatus;
                        order.TotalPrice = price;

                        if (selectedStatus == "Выдан" || selectedStatus == "Архив")
                        {
                            if (order.FinishedDate == null)
                            {
                                order.FinishedDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            order.FinishedDate = null;
                        }
                        if (ComboMaster.SelectedValue != null)
                        {
                            order.MasterId = (int)ComboMaster.SelectedValue;
                        }
                        if (order.FinishedDate != null)
                        {
                            TxtFinishedDate.Text = $"Завершен: {order.FinishedDate.Value.ToString("dd.MM.yyyy")}";
                            TxtFinishedDate.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            TxtFinishedDate.Visibility = Visibility.Collapsed;
                        }

                        db.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}