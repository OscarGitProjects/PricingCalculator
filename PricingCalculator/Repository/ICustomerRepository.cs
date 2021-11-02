using PricingCalculator.Models;

namespace PricingCalculator.Repository
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Property som returnerar antalet customers
        /// </summary>
        int NumberOfCustomers { get; }

        /// <summary>
        /// Metoden lägger till en customer
        /// </summary>
        /// <param name="customer">Referense till customer</param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Metoden hämtar sökt customer
        /// </summary>
        /// <param name="iCustomerId">Id för sökt customer</param>
        /// <returns>Sökt customer eller null</returns>
        Customer GetCustomer(int iCustomerId);
    }
}