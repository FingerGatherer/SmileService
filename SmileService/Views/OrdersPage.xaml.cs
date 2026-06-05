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
            //CheckAndSeedDatabase();
            LoadMastersFilter();
            LoadOrders();
        }

        //private void CheckAndSeedDatabase()
        //{
        //    try
        //    {
        //        using (SmileServiceDBContext db = new SmileServiceDBContext())
        //        {
        //            // Проверяем, пустая ли база по заказам. Если нет — запускаем тотальный SQL-перезапуск
        //            if (db.Orders.Any() || db.Users.Count() > 2)
        //            {
        //                // Формируем единый SQL-скрипт на чистку и заливку данных
        //                string sqlScript = @"
        //            -- 1. Чистим таблицы в правильном порядке из-за связей (Внешних ключей)
        //            DELETE FROM Orders;
        //            DELETE FROM Devices;
        //            DELETE FROM Clients;
        //            DELETE FROM Users;

        //            -- 2. Сбрасываем автоинкременты (ID), чтобы они снова начинались с 1
        //            DBCC CHECKIDENT ('Orders', RESEED, 0);
        //            DBCC CHECKIDENT ('Devices', RESEED, 0);
        //            DBCC CHECKIDENT ('Clients', RESEED, 0);
        //            DBCC CHECKIDENT ('Users', RESEED, 0);

        //            -- 3. Вставляем сотрудников строго по твоему списку
        //            -- Пароли ставим '123'
        //            INSERT INTO Users (Login, Password, FullName, Role, ContactInfo) VALUES 
        //            ('admin', '123', N'Романов Сергей Владимирович', 'Admin', '+7 (999) 111-22-33'),
        //            ('petrov', '123', N'Петров Игорь Иванович', 'Master', '+7 (999) 555-44-33'),
        //            ('ivanov', '123', N'Иванов Алексей Андреевич', 'Master', '+7 (900) 555-00-11'),
        //            ('sidorov', '123', N'Сидоров Николай Петрович', 'Master', '+7 (911) 444-11-22'),
        //            ('smirnova', '123', N'Смирнова Анна Петровна', 'Receptionist', '+7 (911) 222-33-44'),
        //            ('klimov', '123', N'Климов Олег Николаевич', 'Storekeeper', '+7 (922) 333-44-55'),
        //            ('fedorov', '123', N'Федоров Артем Игоревич', 'Storekeeper', '+7 (933) 444-55-66'),
        //            ('johnson', '123', N'Попова Елена Васильевна', 'Accountant', '+7 (944) 555-66-77');

        //            -- 4. Вставляем 20 разнообразных клиентов, устройств и заказов
        //            -- Заказ 1
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Голицын И.В.', '+7 918 456 35 73');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) 
        //            VALUES (1, N'Техника', 'iPhone 14', 'SN-53421', N'В чехле, без коробки', N'Разбился в сопли');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) 
        //            VALUES (1, N'В работе', '2026-06-01', 10000, 3); -- На мастере Иванове (ID=3)

        //            -- Заказ 2
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Васильев А.С.', '+7 920 111 22 33');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) 
        //            VALUES (2, N'Техника', 'Samsung Galaxy S23', 'SN-99821', N'Полный комплект', N'Не заряжается');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) 
        //            VALUES (2, N'Принят', '2026-06-02', 4000, NULL);

        //            -- Заказ 3
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Морозова Е.А.', '+7 905 444 55 66');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) 
        //            VALUES (3, N'Техника', 'MacBook Pro 13', 'SN-11204', N'Только ноутбук', N'Залит кофе');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) 
        //            VALUES (3, N'В работе', '2026-06-03', 15000, 2); -- На мастере Петрове (ID=2)

        //            -- Заказ 4
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Кузнецов Д.М.', '+7 916 777 88 99');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) 
        //            VALUES (4, N'Техника', 'Asus ROG Strix', 'SN-44512', N'С бп питания', N'Артефакты на видеокарте');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) 
        //            VALUES (4, N'Выдан', '2026-05-25', 22000, 4);

        //            -- Заказ 5
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Павлова О.И.', '+7 999 888 77 66');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) 
        //            VALUES (5, N'Техника', 'iPad Air', 'SN-88731', N'В чехле', N'Быстро разряжается');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) 
        //            VALUES (5, N'Архив', '2026-05-20', 5000, 3);

        //            -- Заказ 6
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Новиков С.П.', '+7 903 222 33 44');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) 
        //            VALUES (6, N'Техника', 'Xiaomi Mi 13', 'SN-66712', N'Только устройство', N'Не ловит сеть');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) 
        //            VALUES (6, N'Принят', '2026-06-04', 3500, NULL);

        //            -- Заказ 7
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Федорова М.В.', '+7 911 333 44 55');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) 
        //            VALUES (7, N'Техника', 'HP Pavilion', 'SN-33219', N'С зарядкой', N'Шумит кулер');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) 
        //            VALUES (7, N'В работе', '2026-06-04', 2500, 2);

        //            -- Заказ 8
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Соколов К.Е.', '+7 985 666 55 44');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) 
        //            VALUES (8, N'Техника', 'AirPods Pro', 'SN-10923', N'В коробке', N'Тихий звук в левом наушнике');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) 
        //            VALUES (8, N'Выдан', '2026-05-29', 1500, 4);

        //            -- Генерируем еще 12 типовых заказов для ровного счета в 20 штук
        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Смирнов Д.А.', '+7 900 123 45 67');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (9, N'Техника', 'iPhone 13', 'SN-001', N'Устройство', N'Разбит экран');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (9, N'Принят', '2026-06-01', 8000, NULL);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Петрова Т.В.', '+7 900 765 43 21');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (10, N'Техника', 'Samsung A54', 'SN-002', N'В чехле', N'Зависает');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (10, N'В работе', '2026-06-02', 3000, 2);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Иванов К.М.', '+7 911 111 22 33');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (11, N'Техника', 'Redmi Note 12', 'SN-003', N'Коробка', N'Сломан разъем');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (11, N'Выдан', '2026-06-01', 2000, 3);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Семенов А.А.', '+7 922 222 33 44');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (12, N'Техника', 'PlayStation 5', 'SN-004', N'Кабель, геймпад', N'Греется');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (12, N'Архив', '2026-05-15', 6000, 4);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Дмитриев С.В.', '+7 933 333 44 55');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (13, N'Техника', 'Nintendo Switch', 'SN-005', N'Чехол', N'Дрифт стиков');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (13, N'Принят', '2026-06-03', 2500, NULL);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Михайлов Н.О.', '+7 944 444 55 66');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (14, N'Техника', 'iPhone 11', 'SN-006', N'Без комплекта', N'Замена АКБ');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (14, N'В работе', '2026-06-03', 4500, 3);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Федоров В.А.', '+7 955 555 66 77');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (15, N'Техника', 'Honor 90', 'SN-007', N'Зарядка', N'Не включается');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (15, N'Выдан', '2026-05-28', 5500, 2);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Андреев Р.С.', '+7 966 666 77 88');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (16, N'Техника', 'Huawei MateBook', 'SN-008', N'Блок питания', N'Не работает тачпад');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (16, N'Архив', '2026-05-10', 4000, 4);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Николаев И.К.', '+7 977 777 88 99');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (17, N'Техника', 'Lenovo Legion', 'SN-009', N'Коробка, мышь', N'Чистка от пыли');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (17, N'Принят', '2026-06-04', 2000, NULL);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Алексеев М.Е.', '+7 988 888 99 00');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (18, N'Техника', 'Apple Watch 8', 'SN-010', N'Ремешок', N'Не синхронизируется');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (18, N'В работе', '2026-06-04', 3000, 3);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Сергеев П.Д.', '+7 999 999 00 11');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (19, N'Техника', 'Xbox Series X', 'SN-011', N'Провод', N'Не читает диски');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (19, N'Выдан', '2026-05-26', 7000, 4);

        //            INSERT INTO Clients (FullName, Phone) VALUES (N'Романов В.В.', '+7 901 000 11 22');
        //            INSERT INTO Devices (ClientId, DeviceType, Model, SerialNumber, Equipment, DefectDescription) VALUES (20, N'Техника', 'Yandex Station 2', 'SN-012', N'Блок питания', N'Хрипит динамик');
        //            INSERT INTO Orders (DeviceId, Status, CreatedDate, TotalPrice, MasterId) VALUES (20, N'Принят', '2026-06-05', 3500, NULL);
        //        ";

        //                // Выполняем сырой SQL запрос в СУБД
        //                db.Database.ExecuteSqlRaw(sqlScript);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка выполнения SQL-скрипта: {ex.Message}");
        //    }
        //}
        public void LoadMastersFilter()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    // Берем только мастеров из БД
                    var masters = db.Users
                        .Where(u => u.Role == "Master")
                        .Select(u => new { u.Id, u.FullName })
                        .ToList();

                    // Привязываем данные к комбобоксу фильтра
                    MasterFilterComboBox.ItemsSource = masters;
                    MasterFilterComboBox.DisplayMemberPath = "FullName"; // Что видит пользователь
                    MasterFilterComboBox.SelectedValuePath = "Id";       // Что считывает код (int)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка заполнения фильтра мастеров: {ex.Message}");
            }

        }

        // Загрузка и фильтрация заказов в DataGrid
        private void LoadOrders()
        {
            try
            {
                using (SmileServiceDBContext db = new SmileServiceDBContext())
                {
                    // 1. Начинаем строить базовый запрос со всеми связями
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

                    // 3. ПРИМЕНЯЕМ ФИЛЬТР ПО СТАТУСУ
                    // Замени ComboStatusFilter на имя своего ComboBox фильтра статусов
                    if (StatusFilterComboBox.SelectedItem is ComboBoxItem selectedStatusItem)
                    {
                        string statusValue = selectedStatusItem.Content.ToString();
                        if (statusValue != "Все" && statusValue != "Все статусы")
                        {
                            query = query.Where(o => o.Status == statusValue);
                        }
                    }

                    // 4. ПРИМЕНЯЕМ ФИЛЬТР ПО МАСТЕРАМ
                    // Замени ComboMasterFilter на имя своего ComboBox фильтра мастеров
                    if (MasterFilterComboBox.SelectedValue != null && MasterFilterComboBox.SelectedValue is int masterId)
                    {
                        query = query.Where(o => o.MasterId == masterId);
                    }

                    // 5. СЧИТАЕМ КОЛИЧЕСТВО ЗАПИСЕЙ (уже отфильтрованных!)
                    totalOrdersCount = query.Count();
                    totalPages = (int)Math.Ceiling((double)totalOrdersCount / pageSize);
                    if (totalPages < 1) totalPages = 1;

                    // Если из-за фильтра текущая страница улетела за пределы максимума
                    if (currentPage > totalPages) currentPage = totalPages;

                    // 6. СОРТИРОВКА И ПАГИНАЦИЯ
                    var ordersPageList = query
                        .OrderByDescending(o => o.Id) // Сначала новые
                        .Skip((currentPage - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    OrdersGrid.ItemsSource = ordersPageList;

                    // 7. ОБНОВЛЕНИЕ ИНТЕРФЕЙСА ПАГИНАЦИИ
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
            // Отключаем стрелку назад, если мы на первой странице
            BtnPrev.IsEnabled = (currentPage > 1);
            // Отключаем стрелку вперед, если мы на последней странице
            BtnNext.IsEnabled = (currentPage < totalPages);

            // Сбрасываем стили всех числовых кнопок на дефолтные FlatButton
            var flatButtonStyle = (Style)FindResource("MaterialDesignFlatButton");
            var activeButtonStyle = (Style)FindResource("MaterialDesignFlatMidBgButton");

            // Создаем кисти для текста и фона
            Brush grayTextBrush = (Brush)new BrushConverter().ConvertFrom("#777777");
            Brush activeBgBrush = (Brush)new BrushConverter().ConvertFrom("#1976D2");

            // Сброс первой кнопки
            BtnPage1.Style = flatButtonStyle;
            BtnPage1.Background = Brushes.Transparent; // Используем Brushes.Transparent напрямую
            BtnPage1.Foreground = grayTextBrush;

            // Сброс второй кнопки
            BtnPage2.Style = flatButtonStyle;
            BtnPage2.Background = Brushes.Transparent;
            BtnPage2.Foreground = grayTextBrush;

            // Сброс третьей кнопки
            BtnPage3.Style = flatButtonStyle;
            BtnPage3.Background = Brushes.Transparent;
            BtnPage3.Foreground = grayTextBrush;

            // Подсвечиваем синим фоном текущую активную кнопку страницы
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

        // Обработчики событий для интерфейса XAML
        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        public void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadOrders();
        }

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

        public void ApplyFilters()
        {
            LoadOrders();
        }

        // Кнопка открытия окна добавления нового заказа
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addWindow = new AddOrderWindow();
            addWindow.Owner = Window.GetWindow(this);

            if (addWindow.ShowDialog() == true)
            {
                LoadOrders(); // Перерисовываем таблицу после сохранения
            }
        }

        private void OrdersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // 1. Проверяем, что в таблице выделена строка и это именно Заказ
            var selectedOrder = OrdersGrid.SelectedItem as Order;
            if (selectedOrder == null) return;

            // 2. Создаем наше новое окно деталей и передаем туда этот заказ
            OrderDetailsWindow detailsWindow = new OrderDetailsWindow(selectedOrder);

            // Указываем владельцем главное окно приложения, чтобы оно красиво центрировалось сверху
            detailsWindow.Owner = Window.GetWindow(this);

            // 3. Открываем окно как модальное (ShowDialog)
            if (detailsWindow.ShowDialog() == true)
            {
                // Если внутри окна нажали "Сохранить", обновляем список заказов на странице
                LoadOrders();
            }
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