using NUnit.Framework;
using PricingCalculator.Handlers;
using PricingCalculator.Models;
using PricingCalculator.Services;
using System;

namespace NUnit_PricingCalculator_TestProject
{
    public class PriceCalculator_CalculateNumberOfDiscountedDaysInPeriodForService_Tests : PricingCalculator_TestBase
    {
        [OneTimeSetUp]
        public void TestSetup()
        {
            var test = this.BuildConfiguration(TestContext.CurrentContext.TestDirectory);
            this.m_PriceCalculateService = new PriceCalculateService(this.BuildConfiguration(TestContext.CurrentContext.TestDirectory), new CustomerHandler());
        }

        [SetUp]
        public void Setup()
        {
        }


        #region Test av metoden CalculateNumberOfDiscountedDaysInPeriodForService

        /// <summary>
        /// Test kontrollerar att ArgumentNullException kastas om referensen till Customer objektet är null
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfDiscountedDaysInPeriodForService_Customer_Referense_null_Test()
        {
            // Arrange
            // expected
            Customer customer = null;
            DateTime dtStartDate = DateTime.Now.AddDays(1);
            DateTime dtEndDate = DateTime.Now;

            // Act
            // actual
            Assert.Throws<ArgumentNullException>(() => this.m_PriceCalculateService.CalculateNumberOfDiscountedDaysInPeriodForService(CallingService.SERVICE_A, customer, dtStartDate, dtEndDate));

            // Assert
        }


        /// <summary>
        /// Test kontrollerar att ArgumentException kastas om startdatum är efter slutdatum
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfDiscountedDaysInPeriodForService_StartDate_After_EndDate_Test()
        {
            // Arrange
            // expected
            Customer customer = new Customer(1, "Test 1");
            DateTime dtStartDate = DateTime.Now.AddDays(1);
            DateTime dtEndDate = DateTime.Now;

            // Act
            // actual
            Assert.Throws<ArgumentException>(() => this.m_PriceCalculateService.CalculateNumberOfDiscountedDaysInPeriodForService(CallingService.SERVICE_A, customer, dtStartDate, dtEndDate));

            // Assert
        }


        /// <summary>
        /// Test kontrollerar att vi får tillbaka 0 dagar i perioden om vi inte har rabatter
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfDiscountedDaysInPeriodForService_NumberOfDays_0_When_No_Discount_Test()
        {
            // Arrange
            // expected
            Customer customer = new Customer(1, "Test 1");
            customer.DiscountForServiceA.HasDiscount = false;
            customer.DiscountForServiceA.HasDiscountForAPeriod = false;
            DateTime dtStartDate = DateTime.Now.AddDays(-1);
            DateTime dtEndDate = DateTime.Now;

            int iExpectedNumberOfDays = 0;

            // Act
            // actual
            int iActualNumberOfDays = this.m_PriceCalculateService.CalculateNumberOfDiscountedDaysInPeriodForService(CallingService.SERVICE_A, customer, dtStartDate, dtEndDate);

            // Assert
            Assert.AreEqual(iExpectedNumberOfDays, iActualNumberOfDays);
        }


        /// <summary>
        /// Test kontrollerar att vi får tillbaka korrekt antal dagar för en rabatt period. Då räknar vi alla dagar i veckan
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfDiscountedDaysInPeriodForService_Discount_NumberOfDays_7_When_Counting_All_Days_In_Period_Test()
        {
            // Arrange
            // expected
            Customer customer = new Customer(1, "Test 1");
            customer.DiscountForServiceA.HasDiscount = true;
            customer.DiscountForServiceA.HasDiscountForAPeriod = true;
            customer.DiscountForServiceA.StartDate = new DateTime(2021, 11, 06);
            customer.DiscountForServiceA.EndDate = new DateTime(2021, 11, 12);

            DateTime dtStartDate = new DateTime(2021, 11, 01);
            DateTime dtEndDate = new DateTime(2021, 11, 12);

            int iExpectedNumberOfDays = 7;

            // Act
            // actual
            int iActualNumberOfDays = this.m_PriceCalculateService.CalculateNumberOfDiscountedDaysInPeriodForService(CallingService.SERVICE_A, customer, dtStartDate, dtEndDate, false);

            // Assert
            Assert.AreEqual(iExpectedNumberOfDays, iActualNumberOfDays);
        }


        /// <summary>
        /// Test kontrollerar att vi får tillbaka korrekt antal dagar för en rabatt period. Då räknar vi bara arbetsdagar dvs ej lördag och söndag
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfDiscountedDaysInPeriodForService_Discount_NumberOfDays_7_When_Counting_Working_Days_In_Period_Test()
        {
            // Arrange
            // expected
            Customer customer = new Customer(1, "Test 1");
            customer.DiscountForServiceA.HasDiscount = true;
            customer.DiscountForServiceA.HasDiscountForAPeriod = true;
            customer.DiscountForServiceA.StartDate = new DateTime(2021, 11, 06);
            customer.DiscountForServiceA.EndDate = new DateTime(2021, 11, 12);

            DateTime dtStartDate = new DateTime(2021, 11, 01);
            DateTime dtEndDate = new DateTime(2021, 11, 12);

            int iExpectedNumberOfDays = 5;

            // Act
            // actual
            int iActualNumberOfDays = this.m_PriceCalculateService.CalculateNumberOfDiscountedDaysInPeriodForService(CallingService.SERVICE_A, customer, dtStartDate, dtEndDate, true);

            // Assert
            Assert.AreEqual(iExpectedNumberOfDays, iActualNumberOfDays);
        }


        /// <summary>
        /// Test kontrollerar att vi får tillbaka korrekt antal dagar för en rabatt period. Då räknar vi bara arbetsdagar dvs ej lördag och söndag
        /// </summary>
        [Test]
        public void PriceCalculateService_CalculateNumberOfDiscountedDaysInPeriodForService_Discount_NumberOfDays_2_When_Counting_Working_Days_In_Period_Test()
        {
            // Arrange
            // expected
            Customer customer = new Customer(1, "Test 1");
            customer.DiscountForServiceA.HasDiscount = true;
            customer.DiscountForServiceA.HasDiscountForAPeriod = true;
            customer.DiscountForServiceA.StartDate = new DateTime(2021, 11, 06);
            customer.DiscountForServiceA.EndDate = new DateTime(2021, 11, 12);

            DateTime dtStartDate = new DateTime(2021, 11, 11);
            DateTime dtEndDate = new DateTime(2021, 11, 12);

            int iExpectedNumberOfDays = 2;

            // Act
            // actual
            int iActualNumberOfDays = this.m_PriceCalculateService.CalculateNumberOfDiscountedDaysInPeriodForService(CallingService.SERVICE_A, customer, dtStartDate, dtEndDate, true);

            // Assert
            Assert.AreEqual(iExpectedNumberOfDays, iActualNumberOfDays);
        }

        #endregion // End of region Test av metoden CalculateNumberOfDiscountedDaysInPeriodForService
    }
}
