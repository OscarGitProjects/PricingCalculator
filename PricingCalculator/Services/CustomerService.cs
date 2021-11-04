using PricingCalculator.Models;
using PricingCalculator.Repository;

namespace PricingCalculator.Services
{
    /// <summary>
    /// Service för att hämta customer
    /// </summary>
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// Referens till en service där man hämtar information om en customer från repository
        /// </summary>
        private readonly ICustomerRepository m_CustomerRepository;


        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="CustomerRepository">Referens till en service där man hämtar information om en customer från repository</param>
        public CustomerService(ICustomerRepository CustomerRepository)
        {
            this.m_CustomerRepository = CustomerRepository;
        }

        /// <summary>
        /// Metoden skapar önskat antal kunder
        /// </summary>
        /// <param name="numberOfCostumers">Antal kunder som skall skapas</param>
        public void CreateCustomers(int iNumberOfCustomers)
        {
            Customer newCustomer = null;

            for(int i = 1; i <= iNumberOfCustomers; i++)
            {
                newCustomer = new Customer(i, "Customer " + i);

                // TODO
                // Man ska kunna ha rabatt mellan datum. Ändra i koden

                this.m_CustomerRepository.AddCustomer(newCustomer);
            }
        }


        /// <summary>
        /// Metoden hämtar sökt customer från repository
        /// </summary>
        /// <param name="iCustomerId">Id för sökt customer</param>
        /// <returns>Sökt customer eller null</returns>
        public Customer GetCustomer(int iCustomerId)
        {
            return this.m_CustomerRepository.GetCustomer(iCustomerId);
        }
    }
}