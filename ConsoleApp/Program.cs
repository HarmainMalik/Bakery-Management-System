using System;
using SweetBakery.BusinessLogic;
using SweetBakery.DataAccess;
using SweetBakery.BusinessLogic;
using SweetBakery.Models;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var authService = new AuthService();
            var productService = new ProductService(ObjectHandler.GetProductDL());
            var adminService = new AdminService(ObjectHandler.GetAdminDL());
            var customerService = new CustomerService(ObjectHandler.GetCustomerDL());
            var salesService = new SalesService(ObjectHandler.GetSaleDL());

            Console.WriteLine("Sweet Bakery Console App");
            Console.WriteLine("1. Show all products");
            Console.WriteLine("2. Show total revenue");
            Console.Write("Choice: ");
            string c = Console.ReadLine();

            if (c == "1")
                foreach (var p in productService.GetAll())
                    Console.WriteLine(p.Name + " | Stock: " + p.Stock + " | Price: " + p.Price);
            else if (c == "2")
                Console.WriteLine("Total Revenue: Rs. " + salesService.TotalRevenue());

            Console.ReadKey();
        }
    }
}