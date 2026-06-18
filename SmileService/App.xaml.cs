using SmileService.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SmileService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new SmileService.Models.SmileServiceDBContext())
            {

                db.Database.EnsureCreated();

                if (!db.Users.Any())
                {
                    var admin = new SmileService.Models.User
                    {
                        Login = "admin",
                        Password = "admin",
                        Role = "Admin",
                        FullName = "Романов Павел Сергеевич",
                        ContactInfo = "8 (928) 123-45-67"
                    };

                    var master1 = new SmileService.Models.User
                    {
                        Login = "master1",
                        Password = "123",
                        Role = "Master",
                        FullName = "Петров Алексей Сергеевич",
                        ContactInfo = "8 (905) 777-88-99"
                    };

                    var master2 = new SmileService.Models.User
                    {
                        Login = "master2",
                        Password = "123",
                        Role = "Master",
                        FullName = "Иванов Дмитрий Владимирович",
                        ContactInfo = "8 (962) 444-55-66"
                    };

                    var master3 = new SmileService.Models.User
                    {
                        Login = "master3",
                        Password = "123",
                        Role = "Master",
                        FullName = "Сидоров Роман Александрович",
                        ContactInfo = "8 (918) 222-33-44"
                    };

                    var storekeeper1 = new SmileService.Models.User
                    {
                        Login = "sk1",
                        Password = "123",
                        Role = "Storekeeper",
                        FullName = "Зайцев Николай Николаевич",
                        ContactInfo = "8 (909) 111-22-33"
                    };

                    var storekeeper2 = new SmileService.Models.User
                    {
                        Login = "sk2",
                        Password = "123",
                        Role = "Storekeeper",
                        FullName = "Воробьев Михаил Сергеевич",
                        ContactInfo = "8 (951) 444-55-66"
                    };

                    // Приемщик (1 человек)
                    var receiver = new SmileService.Models.User
                    {
                        Login = "receiver",
                        Password = "123",
                        Role = "Receptionist",
                        FullName = "Белова Елена Александровна",
                        ContactInfo = "8 (938) 555-66-77"
                    };

                    // Бухгалтер (1 человек)
                    var accountant = new SmileService.Models.User
                    {
                        Login = "Acc1",
                        Password = "123",
                        Role = "Accountant",
                        FullName = "Краснова Татьяна Васильевна",
                        ContactInfo = "8 (928) 999-00-11"
                    };

                    db.Users.AddRange(admin, master1, master2, master3, storekeeper1, storekeeper2, receiver, accountant);
                    db.SaveChanges();

                    var c1 = new SmileService.Models.Client { FullName = "Шаповалов Игорь Викторович", Phone = "8 (918) 888-11-22", RegistrationDate = DateTime.Now.AddDays(-20) };
                    var c2 = new SmileService.Models.Client { FullName = "Медведева Анна Александровна", Phone = "8 (903) 445-12-34", RegistrationDate = DateTime.Now.AddDays(-18) };
                    var c3 = new SmileService.Models.Client { FullName = "Кузнецов Максим Олегович", Phone = "8 (928) 333-44-55", RegistrationDate = DateTime.Now.AddDays(-15) };
                    var c4 = new SmileService.Models.Client { FullName = "Смирнова Елена Николаевна", Phone = "8 (938) 111-22-33", RegistrationDate = DateTime.Now.AddDays(-12) };
                    var c5 = new SmileService.Models.Client { FullName = "Васильев Сергей Петрович", Phone = "8 (909) 456-78-90", RegistrationDate = DateTime.Now.AddDays(-10) };
                    var c6 = new SmileService.Models.Client { FullName = "Попова Ольга Игоревна", Phone = "8 (961) 999-88-77", RegistrationDate = DateTime.Now.AddDays(-9) };
                    var c7 = new SmileService.Models.Client { FullName = "Соколов Артем Дмитриевич", Phone = "8 (919) 555-44-33", RegistrationDate = DateTime.Now.AddDays(-7) };
                    var c8 = new SmileService.Models.Client { FullName = "Михайлова Дарья Сергеевна", Phone = "8 (904) 222-33-11", RegistrationDate = DateTime.Now.AddDays(-6) };
                    var c9 = new SmileService.Models.Client { FullName = "Новиков Федор Андреевич", Phone = "8 (951) 777-66-55", RegistrationDate = DateTime.Now.AddDays(-5) };
                    var c10 = new SmileService.Models.Client { FullName = "Федорова Наталья Викторовна", Phone = "8 (928) 444-11-22", RegistrationDate = DateTime.Now.AddDays(-4) };
                    var c11 = new SmileService.Models.Client { FullName = "Морозов Никита Юрьевич", Phone = "8 (988) 333-77-88", RegistrationDate = DateTime.Now.AddDays(-2) };
                    var c12 = new SmileService.Models.Client { FullName = "Волкова Ирина Олеговна", Phone = "8 (918) 124-55-66", RegistrationDate = DateTime.Now.AddDays(-1) };

                    db.Clients.AddRange(c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12);
                    db.SaveChanges();

                    var d1 = new SmileService.Models.Device { DeviceType = "Смартфон", Model = "iPhone 14 Pro", SerialNumber = "ASDF123456XYZ", ClientId = c1.Id, DefectDescription = "Разбит экран, тачскрин не реагирует на нажатия." };
                    var d2 = new SmileService.Models.Device { DeviceType = "Ноутбук", Model = "ASUS ROG Strix", SerialNumber = "NS8823910239", ClientId = c2.Id, DefectDescription = "Сильно греется в нагрузке, выключается через 15 минут игры." };
                    var d3 = new SmileService.Models.Device { DeviceType = "Планшет", Model = "iPad Air 2022", SerialNumber = "DMPX99112233", ClientId = c3.Id, DefectDescription = "Попала жидкость (сок), не включается, не реагирует на зарядку." };
                    var d4 = new SmileService.Models.Device { DeviceType = "Телевизор", Model = "Samsung Crystal UHD", SerialNumber = "TV7711223344", ClientId = c4.Id, DefectDescription = "Пропало изображение, звук есть. Подозревается выход из строя подсветки." };
                    var d5 = new SmileService.Models.Device { DeviceType = "Игровая консоль", Model = "PlayStation 5", SerialNumber = "PS5-99882211A", ClientId = c5.Id, DefectDescription = "Привод не затягивает диски, слышен сильный скрежет внутри механизма." };
                    var d6 = new SmileService.Models.Device { DeviceType = "Смартфон", Model = "Samsung Galaxy S23", SerialNumber = "SMG992381293", ClientId = c6.Id, DefectDescription = "Разбита задняя стеклянная крышка, запала кнопка блокировки экрана." };
                    var d7 = new SmileService.Models.Device { DeviceType = "Ноутбук", Model = "MacBook Air M2", SerialNumber = "MACM2023XYZ8", ClientId = c7.Id, DefectDescription = "Залит кофе. Некоторые клавиши нижнего ряда не нажимаются или залипают." };
                    var d8 = new SmileService.Models.Device { DeviceType = "Робот-пылесос", Model = "Xiaomi Roborock S7", SerialNumber = "XIAOS7VACUUM", ClientId = c8.Id, DefectDescription = "Ошибка лидара. Крутится на месте и выдает сбой ориентации в пространстве." };
                    var d9 = new SmileService.Models.Device { DeviceType = "Смартфон", Model = "Xiaomi RedMi Note 12", SerialNumber = "REDMI12NOTE9", ClientId = c9.Id, DefectDescription = "Быстро разряжается, выключается на улице при минусовой температуре." };
                    var d10 = new SmileService.Models.Device { DeviceType = "Монитор", Model = "LG UltraGear 27", SerialNumber = "LGLINE27GEAR", ClientId = c10.Id, DefectDescription = "Сломан разъем DisplayPort из-за резкого выдергивания кабеля." };
                    var d11 = new SmileService.Models.Device { DeviceType = "Смартфон", Model = "iPhone 11", SerialNumber = "IPH11OLD7721", ClientId = c11.Id, DefectDescription = "Плохо слышно собеседника при разговоре, динамик сильно хрипит." };
                    var d12 = new SmileService.Models.Device { DeviceType = "Планшет", Model = "Huawei MatePad", SerialNumber = "HUAWEIPAD001", ClientId = c12.Id, DefectDescription = "Зависает намертво на логотипе при включении, не загружает систему." };

                    db.Devices.AddRange(d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12);
                    db.SaveChanges();

                    db.Orders.AddRange(
                        new SmileService.Models.Order
                        {
                            DeviceId = d1.Id,
                            MasterId = master1.Id,
                            Status = "В работе",
                            TotalPrice = 12500,
                            CreatedDate = DateTime.Now.AddDays(-5)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d2.Id,
                            MasterId = master2.Id,
                            Status = "Новая",
                            TotalPrice = 2500,
                            CreatedDate = DateTime.Now.AddMinutes(-45)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d3.Id,
                            MasterId = master1.Id,
                            Status = "Выдан",
                            TotalPrice = 6800,
                            CreatedDate = DateTime.Now.AddDays(-6),
                            FinishedDate = DateTime.Now.AddDays(-1)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d4.Id,
                            MasterId = master3.Id,
                            Status = "В работе",
                            TotalPrice = 8000,
                            CreatedDate = DateTime.Now.AddDays(-3)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d5.Id,
                            MasterId = master2.Id,
                            Status = "Выдан",
                            TotalPrice = 4500,
                            CreatedDate = DateTime.Now.AddDays(-4),
                            FinishedDate = DateTime.Now.AddDays(-2)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d6.Id,
                            MasterId = master3.Id,
                            Status = "Новая",
                            TotalPrice = 5200,
                            CreatedDate = DateTime.Now.AddHours(-3)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d7.Id,
                            MasterId = master1.Id,
                            Status = "В работе",
                            TotalPrice = 14000,
                            CreatedDate = DateTime.Now.AddDays(-2)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d8.Id,
                            MasterId = master2.Id,
                            Status = "Выдан",
                            TotalPrice = 3900,
                            CreatedDate = DateTime.Now.AddDays(-5),
                            FinishedDate = DateTime.Now.AddHours(-12)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d9.Id,
                            MasterId = master3.Id,
                            Status = "Новая",
                            TotalPrice = 2100,
                            CreatedDate = DateTime.Now.AddMinutes(-15)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d10.Id,
                            MasterId = master1.Id,
                            Status = "В работе",
                            TotalPrice = 3200,
                            CreatedDate = DateTime.Now.AddDays(-1)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d11.Id,
                            MasterId = master2.Id,
                            Status = "Выдан",
                            TotalPrice = 1800,
                            CreatedDate = DateTime.Now.AddDays(-2),
                            FinishedDate = DateTime.Now.AddMinutes(-20)
                        },
                        new SmileService.Models.Order
                        {
                            DeviceId = d12.Id,
                            MasterId = master3.Id,
                            Status = "Новая",
                            TotalPrice = 1500,
                            CreatedDate = DateTime.Now.AddHours(-1)
                        }
                    );

                    db.SaveChanges();

                    if (!db.Parts.Any())
                    {
                        db.Parts.AddRange(
                            new SmileService.Models.Part
                            {
                                PartName = "Дисплей iPhone 14 Pro (Original)",
                                Sku = "DSP-I14P-ORG",
                                PurchasePrice = 8500,
                                SellingPrice = 12000,
                                StockQuantity = 7
                            },
                            new SmileService.Models.Part
                            {
                                PartName = "Аккумулятор iPhone 11 Brait",
                                Sku = "BAT-I11-BRT",
                                PurchasePrice = 1100,
                                SellingPrice = 2500,
                                StockQuantity = 15
                            },
                            new SmileService.Models.Part
                            {
                                PartName = "Термопаста Arctic MX-4 4g",
                                Sku = "TM-MX4-4G",
                                PurchasePrice = 350,
                                SellingPrice = 750,
                                StockQuantity = 40
                            },
                            new SmileService.Models.Part
                            {
                                PartName = "SSD накопитель Kingston NV2 500GB",
                                Sku = "SSD-KNV2-500",
                                PurchasePrice = 2800,
                                SellingPrice = 4200,
                                StockQuantity = 10
                            },
                            new SmileService.Models.Part
                            {
                                PartName = "Разъем питания Type-C универсальный",
                                Sku = "CN-TPC-UNI",
                                PurchasePrice = 50,
                                SellingPrice = 450,
                                StockQuantity = 100
                            }
                        );
                    }

                    db.SaveChanges();
                }
            }
        }
    }
}
