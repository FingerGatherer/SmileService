using Microsoft.EntityFrameworkCore;
using SmileService.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmileService.Views
{
    public partial class OrdersPage : Page
    {
        private string _searchText = "";
        private int currentPage = 1;
        private int pageSize = 7;
        private int totalPages = 1;
        private int totalOrdersCount = 0;

        public OrdersPage()
        {
            InitializeComponent();
            LoadMastersFilter();
            LoadOrders();
        }
        public void LoadMastersFilter()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    var masters = db.Users
                        .Where(u => u.Role == "Master")
                        .Select(u => new { u.Id, u.FullName })
                        .ToList();

                    MasterFilterComboBox.ItemsSource = masters;
                    MasterFilterComboBox.DisplayMemberPath = "FullName";
                    MasterFilterComboBox.SelectedValuePath = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка заполнения фильтра мастеров: {ex.Message}");
            }

        }
        private void LoadOrders()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    var query = db.Orders
                        .Include(o => o.Device)
                            .ThenInclude(d => d.Client)
                        .Include(o => o.Master)
                        .AsQueryable();

                    if (!string.IsNullOrWhiteSpace(_searchText))
                    {
                        string search = _searchText.Trim().ToLower();
                        query = query.Where(o => o.Device.Model.ToLower().Contains(search) ||
                                                 o.Device.Client.FullName.ToLower().Contains(search) ||
                                                 o.Device.Client.Phone.Contains(search));
                    }

                    if (StatusFilterComboBox.SelectedItem is ComboBoxItem selectedStatusItem)
                    {
                        string statusValue = selectedStatusItem.Content.ToString();
                        if (statusValue != "Все" && statusValue != "Все статусы")
                        {
                            query = query.Where(o => o.Status == statusValue);
                        }
                    }

                    if (MasterFilterComboBox.SelectedValue != null && MasterFilterComboBox.SelectedValue is int masterId)
                    {
                        query = query.Where(o => o.MasterId == masterId);
                    }

                    totalOrdersCount = query.Count();
                    totalPages = (int)Math.Ceiling((double)totalOrdersCount / pageSize);
                    if (totalPages < 1) totalPages = 1;

                    if (currentPage > totalPages) currentPage = totalPages;

                    var ordersPageList = query
                        .OrderByDescending(o => o.Id)
                        .Skip((currentPage - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    OrdersGrid.ItemsSource = ordersPageList;

                    int startRecord = totalOrdersCount == 0 ? 0 : (currentPage - 1) * pageSize + 1;
                    int endRecord = Math.Min(currentPage * pageSize, totalOrdersCount);

                    TxtPageInfo.Text = $"Показаны {startRecord}–{endRecord} из {totalOrdersCount} заявок";

                    UpdatePaginationButtonsVisuals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации и загрузки данных: {ex.Message}");
            }
        }

        private void UpdatePaginationButtonsVisuals()
        {
            BtnPrev.IsEnabled = (currentPage > 1);
            BtnNext.IsEnabled = (currentPage < totalPages);

            var flatButtonStyle = (Style)FindResource("MaterialDesignFlatButton");
            var activeButtonStyle = (Style)FindResource("MaterialDesignFlatMidBgButton");

            Brush grayTextBrush = (Brush)new BrushConverter().ConvertFrom("#777777");
            Brush activeBgBrush = (Brush)new BrushConverter().ConvertFrom("#1976D2");

            BtnPage1.Style = flatButtonStyle;
            BtnPage1.Background = Brushes.Transparent;
            BtnPage1.Foreground = grayTextBrush;

            BtnPage2.Style = flatButtonStyle;
            BtnPage2.Background = Brushes.Transparent;
            BtnPage2.Foreground = grayTextBrush;

            BtnPage3.Style = flatButtonStyle;
            BtnPage3.Background = Brushes.Transparent;
            BtnPage3.Foreground = grayTextBrush;

            if (currentPage == 1)
            {
                BtnPage1.Style = activeButtonStyle;
                BtnPage1.Background = activeBgBrush;
                BtnPage1.Foreground = Brushes.White;
            }
            else if (currentPage == 2)
            {
                BtnPage2.Style = activeButtonStyle;
                BtnPage2.Background = activeBgBrush;
                BtnPage2.Foreground = Brushes.White;
            }
            else if (currentPage == 3)
            {
                BtnPage3.Style = activeButtonStyle;
                BtnPage3.Background = activeBgBrush;
                BtnPage3.Foreground = Brushes.White;
            }
        }
        public void Page_Loaded(object sender, RoutedEventArgs e) => LoadOrders();
        public void FilterChanged(object sender, SelectionChangedEventArgs e) => LoadOrders();
        public void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            _searchText = "";
            if (StatusFilterComboBox != null) StatusFilterComboBox.SelectedIndex = 0;
            if (MasterFilterComboBox != null) MasterFilterComboBox.SelectedItem = null;
            LoadOrders();
        }

        public void SetSearchText(string text)
        {
            _searchText = text;
            currentPage = 1;
            LoadOrders();
        }

        public void ApplyFilters() => LoadOrders();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addWindow = new AddOrderWindow();
            addWindow.Owner = Window.GetWindow(this);

            if (addWindow.ShowDialog() == true)
                LoadOrders();
        }

        private void OrdersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedOrder = OrdersGrid.SelectedItem as Order;
            if (selectedOrder == null) return;

            OrderDetailsWindow detailsWindow = new OrderDetailsWindow(selectedOrder);

            detailsWindow.Owner = Window.GetWindow(this);

            if (detailsWindow.ShowDialog() == true)
                LoadOrders();
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadOrders();
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadOrders();
            }
        }

        private void BtnPageNum_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (int.TryParse(btn.Content.ToString(), out int targetPage))
                {
                    if (targetPage <= totalPages)
                    {
                        currentPage = targetPage;
                        LoadOrders();
                    }
                }
            }
        }
    }
}