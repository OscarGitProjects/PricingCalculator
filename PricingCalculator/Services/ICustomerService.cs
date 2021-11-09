using PricingCalculator.Models;

namespace PricingCalculator.Services
{
    public interface ICustomerService
    {
        Customer GetCustomer(int iCustomerId);
        void CreateCustomers();
    }
}