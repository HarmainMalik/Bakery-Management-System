using System;
using System.Windows.Forms;
using SweetBakery.BusinessLogic;
using SweetBakery.DataAccess;

namespace UI
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            CustomerRepository customerRepo = new CustomerRepository();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var productRepo = new ProductRepository();
            var salesRepo = new SalesRepository();
            var adminRepo = new AdminRepository();
            var custRepo = new CustomerRepository();

            var authService = new AuthService();
            var productService = new ProductService(productRepo);
            var adminService = new AdminService(adminRepo);
            var customerService = new CustomerService(customerRepo);
            var salesService = new SalesService(salesRepo);

            // This is the FIRST form that opens — the main login chooser
            Application.Run(new LoginForm(
                authService,
                productService,
                adminService,
                customerService,
                salesService));
        }
    }
}