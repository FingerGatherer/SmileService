using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmileService.Models;

namespace SmileService.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    int activeOrdersCount = db.Orders.Count(o => o.Status == "В работе");
                    TxtActiveOrders.Text = $"{activeOrdersCount} {GetOrderWordForm(activeOrdersCount)}";

                    DateTime today = DateTime.Today;
                    int newOrdersTodayCount = db.Orders.Count(o => o.Status == "Новая" && o.CreatedDate >= today);
                    TxtNewOrders.Text = $"{newOrdersTodayCount} {GetOrderWordForm(newOrdersTodayCount)}";

                    DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    decimal monthlyRevenue = db.Orders
                        .Where(o => (o.Status == "Выдан" || o.Status == "Архив") && o.CreatedDate >= firstDayOfMonth)
                        .Sum(o => (decimal?)o.TotalPrice) ?? 0;

                    TxtRevenue.Text = $"{monthlyRevenue:N0} ₽";
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show($"Ошибка обновления: {errorMsg}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private string GetOrderWordForm(int count)
        {
            int n = Math.Abs(count) % 100;
            int n1 = n % 10;
            if (n > 10 && n < 20) return "заявок";
            if (n1 > 11 && n1 < 15) return "заявок";
            if (n1 == 1) return "заявка";
            if (n1 > 1 && n1 < 5) return "заявки";
            return "заявок";
        }
    }
}