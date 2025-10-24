using FitnesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FitnesApp
{
    internal class Program
    {
        private static DbContextOptions<Db27595Context> GetDbContextOptions()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<Db27595Context>();
            return optionsBuilder.UseSqlServer(connectionString).Options;
        }

        public static async Task Main(string[] args)
        {
            Console.WriteLine("--- Лабораторная работа №2: Entity Framework и LINQ ---");
            Console.WriteLine("Каждый шаг будет выполняться после нажатия любой клавиши.\n");

            Console.ReadKey();
            await SelectAllFromOneSideTable();
            Console.ReadKey();
            await SelectFilteredFromOneSideTable();
            Console.ReadKey();
            await GroupDataFromManySideTable();
            Console.ReadKey();
            await SelectFromTwoRelatedTables();
            Console.ReadKey();
            await SelectFromTwoRelatedTablesWithFilter();
            Console.ReadKey();
            await InsertIntoOneSideTable();
            Console.ReadKey();
            await InsertIntoManySideTable();
            Console.ReadKey();
            await DeleteFromOneSideTable();
            Console.ReadKey();
            await DeleteFromManySideTable();
            Console.ReadKey();
            await UpdateRecordByCondition();

            Console.WriteLine("\n--- Все шаги выполнены. Работа программы завершена. ---");
            Console.ReadKey();
        }

        // 2.1. Выборка всех данных из таблицы «один» (MembershipTypes)
        private static async Task SelectAllFromOneSideTable()
        {
            Console.WriteLine("--- 2.1: Все типы абонементов (таблица MembershipTypes) ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var membershipTypes = await db.MembershipTypes.AsNoTracking().ToListAsync();
                Console.WriteLine($"Найдено {membershipTypes.Count} типов абонементов.");
                foreach (var mt in membershipTypes.Take(5))
                {
                    Console.WriteLine($"ID: {mt.MembershipTypeId}, Название: {mt.Name}, Цена: {mt.Cost}");
                }
            }
            Console.WriteLine("----------------------------------------------------------\n");
        }

        // 2.2. Выборка данных из таблицы «один» с фильтрацией (Trainers)
        private static async Task SelectFilteredFromOneSideTable()
        {
            Console.WriteLine("--- 2.2: Тренеры со специализацией 'Кроссфит' ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var trainers = await db.Trainers.AsNoTracking()
                    .Where(t => t.Specialization == "Кроссфит")
                    .ToListAsync();
                Console.WriteLine($"Найдено {trainers.Count} тренеров.");
                foreach (var t in trainers.Take(5))
                {
                    Console.WriteLine($"ID: {t.TrainerId}, ФИО: {t.FullName}");
                }
            }
            Console.WriteLine("-------------------------------------------------\n");
        }

        // 2.3. Группировка данных в таблице «многие» (ClientMemberships)
        private static async Task GroupDataFromManySideTable()
        {
            Console.WriteLine("--- 2.3: Количество покупок по каждому типу абонемента (топ-5) ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var salesCounts = await db.ClientMemberships
                    .Include(cm => cm.MembershipType)
                    .GroupBy(cm => cm.MembershipType.Name)
                    .Select(g => new { MembershipName = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToListAsync();

                Console.WriteLine("Топ-5 самых продаваемых абонементов:");
                foreach (var item in salesCounts)
                {
                    Console.WriteLine($"Абонемент: '{item.MembershipName}', Продано: {item.Count} шт.");
                }
            }
            Console.WriteLine("---------------------------------------------------------------------\n");
        }

        // 2.4. Выборка из двух полей двух связанных таблиц
        private static async Task SelectFromTwoRelatedTables()
        {
            Console.WriteLine("--- 2.4: Клиенты и названия их абонементов (первые 5) ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var clientMemberships = await db.ClientMemberships
                    .Include(cm => cm.Client)
                    .Include(cm => cm.MembershipType)
                    .Select(cm => new { ClientName = cm.Client.FirstName + " " + cm.Client.LastName, MembershipName = cm.MembershipType.Name })
                    .Take(5)
                    .ToListAsync();

                foreach (var item in clientMemberships)
                {
                    Console.WriteLine($"Клиент: {item.ClientName}, Абонемент: {item.MembershipName}");
                }
            }
            Console.WriteLine("----------------------------------------------------------\n");
        }

        // 2.5. Выборка из двух таблиц с фильтром
        private static async Task SelectFromTwoRelatedTablesWithFilter()
        {
            Console.WriteLine("--- 2.5: Клиенты, чей абонемент истекает в течение 7 дней ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                var weekFromNow = today.AddDays(7);

                var expiringSoon = await db.ClientMemberships
                    .Include(cm => cm.Client)
                    .Where(cm => cm.EndDate > today && cm.EndDate <= weekFromNow)
                    .Select(cm => new { ClientName = cm.Client.FirstName + " " + cm.Client.LastName, EndDate = cm.EndDate })
                    .ToListAsync();

                Console.WriteLine($"Найдено клиентов с истекающим абонементом: {expiringSoon.Count}");
                foreach (var item in expiringSoon.Take(5))
                {
                    Console.WriteLine($"Клиент: {item.ClientName}, Истекает: {item.EndDate:yyyy-MM-dd}");
                }
            }
            Console.WriteLine("---------------------------------------------------------------\n");
        }

        // 2.6. Вставка данных в таблицу «один»
        private static async Task InsertIntoOneSideTable()
        {
            Console.WriteLine("--- 2.6: Добавление нового типа абонемента ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var newMembershipType = new MembershipType
                {
                    Name = "Тестовый абонемент",
                    Type = "Time",
                    DurationDays = 14, // ИСПРАВЛЕНО
                    VisitCount = null,  // Явно указываем null для другого типа
                    Cost = 35.00m,
                    AvailableZones = "Только зал"
                };
                db.MembershipTypes.Add(newMembershipType);
                await db.SaveChangesAsync();
                Console.WriteLine($"Добавлен тип абонемента с ID: {newMembershipType.MembershipTypeId}");
            }
            Console.WriteLine("----------------------------------------------\n");
        }

        // 2.7. Вставка данных в таблицу «многие»
        private static async Task InsertIntoManySideTable()
        {
            Console.WriteLine("--- 2.7: Продажа абонемента клиенту ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var client = await db.Clients.FirstOrDefaultAsync();
                var membershipType = await db.MembershipTypes.FirstOrDefaultAsync(mt => mt.Type == "Время"); // Берем абонемент по времени
                var employee = await db.Employees.FirstOrDefaultAsync();

                if (client != null && membershipType != null && employee != null)
                {
                    var today = DateOnly.FromDateTime(DateTime.Now);
                    var newSale = new ClientMembership
                    {
                        ClientId = client.ClientId,
                        MembershipTypeId = membershipType.MembershipTypeId,
                        EmployeeId = employee.EmployeeId,
                        PurchaseDate = DateTime.Now,
                        StartDate = today, // ИСПРАВЛЕНО
                        EndDate = today.AddDays(membershipType.DurationDays ?? 0) // ИСПРАВЛЕНО
                    };
                    db.ClientMemberships.Add(newSale);
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Оформлена продажа с ID: {newSale.ClientMembershipId} для клиента {client.FirstName} {client.LastName}");
                }
                else
                {
                    Console.WriteLine("Не удалось найти клиента, тип абонемента или сотрудника для оформления продажи.");
                }
            }
            Console.WriteLine("---------------------------------------\n");
        }

        // 2.8. Удаление данных из таблицы «один»
        private static async Task DeleteFromOneSideTable()
        {
            Console.WriteLine("--- 2.8: Удаление тестового типа абонемента ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var toDelete = await db.MembershipTypes
                    .FirstOrDefaultAsync(s => s.Name == "Тестовый абонемент");

                if (toDelete != null)
                {
                    // Сначала удаляем все связанные продажи, чтобы не нарушить целостность данных
                    var relatedSales = db.ClientMemberships.Where(cm => cm.MembershipTypeId == toDelete.MembershipTypeId);
                    if (relatedSales.Any())
                    {
                        db.ClientMemberships.RemoveRange(relatedSales);
                        await db.SaveChangesAsync();
                    }

                    db.MembershipTypes.Remove(toDelete);
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Тип абонемента с ID {toDelete.MembershipTypeId} и все его продажи успешно удалены.");
                }
                else
                {
                    Console.WriteLine("Тестовый тип абонемента для удаления не найден.");
                }
            }
            Console.WriteLine("-----------------------------------------------\n");
        }

        // 2.9. Удаление данных из таблицы «многие»
        private static async Task DeleteFromManySideTable()
        {
            Console.WriteLine("--- 2.9: Удаление (отмена) последней продажи ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var saleToDelete = await db.ClientMemberships
                    .OrderByDescending(s => s.ClientMembershipId)
                    .FirstOrDefaultAsync();

                if (saleToDelete != null)
                {
                    db.ClientMemberships.Remove(saleToDelete);
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Продажа с ID {saleToDelete.ClientMembershipId} успешно удалена.");
                }
                else
                {
                    Console.WriteLine("Продажа для удаления не найдена.");
                }
            }
            Console.WriteLine("------------------------------------------------\n");
        }

        // 2.10. Обновление записи по условию
        private static async Task UpdateRecordByCondition()
        {
            Console.WriteLine("--- 2.10: Обновление цены для 'Месячный безлимит' ---");
            using (var db = new Db27595Context(GetDbContextOptions()))
            {
                var toUpdate = await db.MembershipTypes
                    .FirstOrDefaultAsync(s => s.Name == "Месячный безлимит");

                if (toUpdate != null)
                {
                    var oldPrice = toUpdate.Cost;
                    toUpdate.Cost *= 1.10m; // Увеличиваем цену на 10%
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Цена для абонемента '{toUpdate.Name}' обновлена.");
                    Console.WriteLine($"Старая цена: {oldPrice:C}, Новая цена: {toUpdate.Cost:C}");
                }
                else
                {
                    Console.WriteLine("Абонемент для обновления не найден.");
                }
            }
            Console.WriteLine("--------------------------------------------------------\n");
        }
    }
}