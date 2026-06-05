using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmileService.Models;

namespace SmileService.Views
{
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    // Берем клиентов и динамически подтягиваем к каждому дату его последнего заказа
                    var clientsWithDates = db.Clients.Select(c => new
                    {
                        c.Id,
                        c.FullName,
                        c.Phone,
                        c.Email,
                        // Ищем в таблице Orders все заявки этого клиента через связку устройств 
                        // и выбираем максимальную (последнюю) дату. Если заявок нет — будет null.
                        LastOrderDate = db.Orders
                            .Where(o => o.Device.ClientId == c.Id)
                            .Max(o => (DateTime?)o.CreatedDate)
                    }).ToList();

                    // Запихиваем этот красивый результат в таблицу
                    ClientsGrid.ItemsSource = clientsWithDates;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании истории обращений клиентов: {ex.Message}",
                                "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtClientSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtClientSearch.Text == "Поиск по ФИО или телефону...")
            {
                TxtClientSearch.Text = "";
                TxtClientSearch.Foreground = System.Windows.Media.Brushes.Black;
            }
        }
    }
}