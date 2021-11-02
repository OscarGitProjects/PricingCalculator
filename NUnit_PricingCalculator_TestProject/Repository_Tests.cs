using NUnit.Framework;
using PricingCalculator.Exceptions;
using PricingCalculator.Models;
using PricingCalculator.Repository;
using System;

namespace NUnit_PricingCalculator_TestProject
{
    public class Repository_Tests : PricingCalculator_TestBase
    {
        [OneTimeSetUp]
        public void TestSetup()
        {            
        }

        [SetUp]
        public void Setup()
        {
            this.m_CustomerRepository = new CustomerRepository();
        }

        /// <summary>
        /// Test kontrollerar att det går att lägga till en customer.
        /// Testar också att man får rätt data tillbaka om man hämtar ut customer igen
        /// </summary>
        [Test]
        public void CustomerRepository_Add_Customer_To_Repository_Test()
        {
            // Arrange
            // expected  
            Customer customer = new Customer(1, "Test ett");
            DateTime dtStartDate = DateTime.Now;
            DateTime dtEndDate = DateTime.Now;

            // Act
            // actual
            this.m_CustomerRepository.AddCustomer(customer);
            Customer actualCustomer = this.m_CustomerRepository.GetCustomer(1);
            int actualNumberOfCustomers = this.m_CustomerRepository.NumberOfCustomers;

            // Assert
            Assert.IsNotNull(actualCustomer);
            Assert.AreEqual(customer.CustomerId, actualCustomer.CustomerId);
            Assert.AreEqual(customer.CustomerName, actualCustomer.CustomerName);
            Assert.AreEqual(1, actualNumberOfCustomers);
        }


        /// <summary>
        /// Test kontrollerar att det går att lägga till två customer. 
        /// Testar också att man får rätt data tillbaka om man hämtar ut customer igen
        /// </summary>
        [Test]
        public void CustomerRepository_Add_Two_Customer_To_Repository_Test()
        {
            // Arrange
            // expected  
            Customer customer1 = new Customer(1, "Test ett");
            DateTime dtStartDate1 = DateTime.Now;
            DateTime dtEndDate1 = DateTime.Now;

            Customer customer2 = new Customer(2, "Test två");
            DateTime dtStartDate2 = DateTime.Now;
            DateTime dtEndDate2 = DateTime.Now;

            // Act
            // actual
            this.m_CustomerRepository.AddCustomer(customer1);
            int actualNumberOfCustomers1 = this.m_CustomerRepository.NumberOfCustomers;

            this.m_CustomerRepository.AddCustomer(customer2);

            Customer actualCustomer1 = this.m_CustomerRepository.GetCustomer(1);
            
            Customer actualCustomer2 = this.m_CustomerRepository.GetCustomer(2);
            int actualNumberOfCustomers2 = this.m_CustomerRepository.NumberOfCustomers;

            // Assert
            Assert.IsNotNull(actualCustomer1);
            Assert.AreEqual(customer1.CustomerId, actualCustomer1.CustomerId);
            Assert.AreEqual(customer1.CustomerName, actualCustomer1.CustomerName);
            Assert.AreEqual(1, actualNumberOfCustomers1);

            Assert.IsNotNull(actualCustomer2);
            Assert.AreEqual(customer2.CustomerId, actualCustomer2.CustomerId);
            Assert.AreEqual(customer2.CustomerName, actualCustomer2.CustomerName);
            Assert.AreEqual(2, actualNumberOfCustomers2);
        }


        /// <summary>
        /// Test kontrollerar att undantaget CustomerAlreadyExistsException kastas om man försöker lägga till flera customer med samma customerId
        /// </summary>
        [Test]
        public void CustomerRepository_Add_Customer_With_Same_CustomerId_Throws_CustomerAlreadyExistsException_Test()
        {
            // Arrange
            // expected  
            Customer customer1 = new Customer(1, "Test ett");
            DateTime dtStartDate1 = DateTime.Now;
            DateTime dtEndDate1 = DateTime.Now;

            Customer customer2 = new Customer(1, "Test två");
            DateTime dtStartDate2 = DateTime.Now;
            DateTime dtEndDate2 = DateTime.Now;

            // Act
            // actual
            this.m_CustomerRepository.AddCustomer(customer1);
            Assert.Throws<CustomerAlreadyExistsException>(() => this.m_CustomerRepository.AddCustomer(customer1));

            // Assert
        }
    }
}