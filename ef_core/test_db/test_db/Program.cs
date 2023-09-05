using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using test_db.Control;
using test_db.Model;

namespace test_db
{
    public enum OrderStatus
    {
        Unknown,
        InProcess,
        Done,
        Cancelled
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new YourDbContext())
            {
                // Проверка и создание таблиц, если их нет
                dbContext.Database.EnsureCreated();

                // Добавление данных в таблицу "statuses", если она пуста
                if (!dbContext.Statuses.Any())
                {
                    var statuses = new[]
                    {
                        new Status { StatusName = "In work..." },
                        new Status { StatusName = "Done!" },
                        new Status { StatusName = "Cancelled." }
                    };

                    dbContext.Statuses.AddRange(statuses);
                    dbContext.SaveChanges();
                }

                while (true)
                {
                    Console.WriteLine("Choose an action:");
                    Console.WriteLine("1. Display the list of orders.");
                    Console.WriteLine("2. Create a new user.");
                    Console.WriteLine("3. Create a new order ( /w new user ).");
                    Console.WriteLine("4. Exit");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            DisplayOrders(dbContext);
                            break;
                        case "2":
                            CreateUser(dbContext);
                            break;
                        case "3":
                            CreateOrderWithUser(dbContext);
                            break;
                        case "4":
                            return; // Выход
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
        }

        static void DisplayOrders(YourDbContext dbContext)
        {
            Console.WriteLine("List of Orders:");
            var orders = dbContext.Orders.Include(o => o.User).Include(o => o.Status).ToList();
            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}");
                Console.WriteLine($"User: {order.User.Username}");
                Console.WriteLine($"Order Date: {order.OrderDate}");
                Console.WriteLine($"Total Amount: {order.TotalAmount:C}");
                Console.WriteLine($"Status: {order.Status.StatusName}");
                Console.WriteLine();
            }
        }

        static User CreateUser(YourDbContext dbContext)
        {
            Console.Write("Enter the username: ");
            var username = Console.ReadLine();

            Console.Write("Enter the city: ");
            var city = Console.ReadLine();

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = username,
                City = city
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            Console.WriteLine($"User {username} has been created.");

            return user;
        }

        static void CreateOrderWithUser(YourDbContext dbContext)
        {
            var user = CreateUser(dbContext);

            var orderDate = DateTime.UtcNow; // UTC

            Console.Write("Enter the total amount: ");
            if (decimal.TryParse(Console.ReadLine(), out var totalAmount))
            {
                var order = new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = user.UserId,
                    OrderDate = orderDate,
                    TotalAmount = totalAmount,
                    StatusId = (int)OrderStatus.InProcess
                };

                dbContext.Orders.Add(order);
                dbContext.SaveChanges();

                Console.WriteLine("Order has been created with status(In work) and current date.");
            }
            else
            {
                Console.WriteLine("Invalid total amount.");
            }
        }
    }
}
