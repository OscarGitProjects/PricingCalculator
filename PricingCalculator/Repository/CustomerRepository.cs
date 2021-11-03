using PricingCalculator.Exceptions;
using PricingCalculator.Models;
using System.Collections.Generic;
using System.Linq;

namespace PricingCalculator.Repository
{
    /// <summary>
    /// Repository för customers
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        /// <summary>
        /// List med customers
        /// </summary>
        protected IList<Customer> m_lsCustomers;

        /// <summary>
        /// Property som returnerar antalet customers
        /// </summary>
        public int NumberOfCustomers
        {
            get { return m_lsCustomers != null ? m_lsCustomers.Count : 0; }
        }

        /// <summary>
        /// Metoden lägger till en customer
        /// </summary>
        /// <param name="customer">Referense till customer</param>
        /// <exception cref="CustomerAlreadyExistsException">Kastas om en customer med samma customerId redan finns</exception>
        public void AddCustomer(Customer customer)
        {
            if (m_lsCustomers == null)
                m_lsCustomers = new List<Customer>();

            // Kontrollera om customer redan finns eller ej
            Customer cust = m_lsCustomers.Where(c => c.CustomerId == customer.CustomerId).FirstOrDefault();
            if (cust != null)
                throw new CustomerAlreadyExistsException($"CustomerRepository->AddCustomer(). Customer with id {customer.CustomerId} already excist");

            m_lsCustomers.Add(customer);
        }


        /// <summary>
        /// Metoden hämtar sökt customer
        /// </summary>
        /// <param name="iCustomerId">Id för sökt customer</param>
        /// <returns>Sökt customer eller null</returns>
        /// <exception cref="System.ArgumentNullException">Metoden kan kasta argument null exception</exception>
        public Customer GetCustomer(int iCustomerId)
        {
            if (m_lsCustomers == null || m_lsCustomers.Count <= 0)
                return null;

            return m_lsCustomers.Where(c => c.CustomerId == iCustomerId).FirstOrDefault();
        }
    }
}