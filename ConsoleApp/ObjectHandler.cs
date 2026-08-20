using SweetBakery.DataAccess;
using SweetBakery.BusinessLogic;
using SweetBakery.Models;

namespace ConsoleApp
{
    public static class ObjectHandler
    {
        public static IAdminDL GetAdminDL() => new AdminRepository();
        public static ICustomerDL GetCustomerDL() => new CustomerRepository();
        public static IProductDL GetProductDL() => new ProductRepository();
        public static ISaleDL GetSaleDL() => new SalesRepository();
    }
}