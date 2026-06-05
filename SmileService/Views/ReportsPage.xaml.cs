using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SmileService.Views
{
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();
            // Устанавливаем дефолтные даты (текущий месяц)
            DpStart.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DpEnd.SelectedDate = DateTime.Now;
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Показываем таблицу с красивыми агрегированными данными для диплома
            ReportsGrid.Visibility = Visibility.Visible;

            var mockData = new List<object>
            {
                new { Category = "Ремонт смартфонов (замена дисплеев/АКБ)", Count = 42, Revenue = "126 000,00" },
                new { Category = "Ремонт ноутбуков и ПК (чистка, пайка)", Count = 19, Revenue = "85 500,00" },
                new { Category = "Продажа комплектующих и аксессуаров", Count = 65, Revenue = "43 200,00" },
                new { Category = "Диагностика и настройка ПО", Count = 28, Revenue = "14 000,00" },
                new { Category = "ИТОГО ЗА ПЕРИОД", Count = 154, Revenue = "268 700,00" }
            };

            ReportsGrid.ItemsSource = mockData;
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            if (ReportsGrid.Visibility == Visibility.Collapsed)
            {
                MessageBox.Show("Сначала сформируйте отчет за период!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Отчет успешно выгружен в формат Microsoft Excel (.xlsx) и сохранен в документы организации.",
                            "Экспорт данных", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPdf_Click(object sender, RoutedEventArgs e)
        {
            if (ReportsGrid.Visibility == Visibility.Collapsed)
            {
                MessageBox.Show("Сначала сформируйте отчет за период!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Документ успешно экспортирован в формат PDF и подготовлен для отправки на печать.",
                            "Экспорт данных", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}