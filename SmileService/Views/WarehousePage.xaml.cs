using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmileService.Models;

namespace SmileService.Views
{
    public partial class WarehousePage : Page
    {
        public WarehousePage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new SmileServiceDBContext())
            {
                PartsGrid.ItemsSource = db.Parts.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text) || string.IsNullOrWhiteSpace(TxtSku.Text))
            {
                MessageBox.Show("Заполните название детали и артикул!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtPurchase.Text, out decimal purchase) ||
                !decimal.TryParse(TxtSelling.Text, out decimal selling) ||
                !int.TryParse(TxtQty.Text, out int qty))
            {
                MessageBox.Show("Цены и количество должны быть числами!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (qty < 0 || purchase < 0 || selling < 0)
            {
                MessageBox.Show("Значения не могут быть отрицательными!", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new SmileServiceDBContext())
                {
                    var newPart = new Part
                    {
                        PartName = TxtName.Text.Trim(),
                        Sku = TxtSku.Text.Trim(),
                        PurchasePrice = purchase,
                        SellingPrice = selling,
                        StockQuantity = qty
                    };

                    db.Parts.Add(newPart);
                    db.SaveChanges();
                }

                LoadData();
                ClearFields();
                MessageBox.Show("Деталь успешно добавлена на склад!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с БД: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PartsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PartsGrid.SelectedItem is SmileService.Models.Part selectedPart)
            {
                TxtName.Text = selectedPart.PartName;
                TxtSku.Text = selectedPart.Sku;
                TxtPurchase.Text = selectedPart.PurchasePrice.ToString();
                TxtSelling.Text = selectedPart.SellingPrice.ToString();
                TxtQty.Text = selectedPart.StockQuantity.ToString();
            }
        }

        private void BtnWriteOff_Click(object sender, RoutedEventArgs e)
        {
            if (!(PartsGrid.SelectedItem is SmileService.Models.Part selectedPart))
            {
                MessageBox.Show("Выберите деталь в таблице для списания!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new SmileServiceDBContext())
                {
                    var partInDb = db.Parts.FirstOrDefault(p => p.Id == selectedPart.Id);

                    if (partInDb != null)
                    {
                        if (partInDb.StockQuantity <= 0)
                        {
                            MessageBox.Show("Невозможно списать деталь! Остаток на складе уже равен 0.", "Ошибка списания", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        partInDb.StockQuantity -= 1;
                        db.SaveChanges();
                    }
                }

                LoadData();

                ClearFields();

                MessageBox.Show("Компонент успешно списан со склада!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при списании: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFields()
        {
            TxtName.Clear();
            TxtSku.Clear();
            TxtPurchase.Clear();
            TxtSelling.Clear();
            TxtQty.Clear();
        }
    }
}